using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    internal static class SqlConnectionExtensions
    {
        #region SwitchTable

        public static async Task SwitchTableAsync(
            this SqlConnection connection,
            string viewName,
            string tableName,
            IEnumerable<string> columns,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            using (var command = GetSwitchTableCommand(connection, viewName, tableName, columns, transaction))
            {
                await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        internal static SqlCommandAdapter GetSwitchTableCommand(
            SqlConnection connection,
            string viewName,
            string tableName,
            IEnumerable<string> columns,
            SqlTransaction transaction)
        {
            var command = SqlCommandAdapter.FromConnection(connection, transaction);

            command.CommandText = $@"ALTER VIEW {Quote(viewName)}
AS

SELECT {string.Join(", ", columns.Select(Quote))}
FROM {Quote(tableName)};";

            return command;
        }

        #endregion

        #region TruncateTable

        public static async Task TruncateTableAsync(
            this SqlConnection connection,
            string tableName,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            using (var command = GetTruncateTableCommand(connection, tableName, transaction))
            {
                await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        internal static SqlCommandAdapter GetTruncateTableCommand(
            SqlConnection connection,
            string tableName,
            SqlTransaction transaction)
        {
            var command = SqlCommandAdapter.FromConnection(connection, transaction);

            command.CommandText = $"TRUNCATE TABLE {Quote(tableName)};";

            return command;
        }

        #endregion

        #region SelectIntoTemporaryTable

        public static async Task SelectIntoTemporaryTableAsync(
            this SqlConnection connection,
            string baseTableName,
            string tempTableName,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            using (var command = GetSelectIntoTemporaryTableCommand(connection, baseTableName, tempTableName, transaction))
            {
                await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        internal static SqlCommandAdapter GetSelectIntoTemporaryTableCommand(
            SqlConnection connection,
            string baseTableName,
            string tempTableName,
            SqlTransaction transaction)
        {
            var command = SqlCommandAdapter.FromConnection(connection, transaction);

            command.CommandText = $"SELECT TOP 0 * INTO {Quote(tempTableName)} FROM {Quote(baseTableName)};";

            return command;
        }

        #endregion

        #region Merge

        public static async Task<int> MergeAsync(
            this SqlConnection connection,
            string targetTable,
            string tableSource,
            IEnumerable<string> columns,
            IEnumerable<string> primaryKeyColumns,
            bool updateWhenMatched,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            using (var command = GetMergeCommand(connection, targetTable, tableSource, columns, primaryKeyColumns, updateWhenMatched, transaction))
            {
                return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        internal static SqlCommandAdapter GetMergeCommand(
            SqlConnection connection,
            string targetTable,
            string tableSource,
            IEnumerable<string> columns,
            IEnumerable<string> primaryKeyColumns,
            bool updateWhenMatched,
            SqlTransaction transaction)
        {
            var command = SqlCommandAdapter.FromConnection(connection, transaction);

            var mergeSearchCondition = GetMergeSearchCondition();
            var setClause = GetSetClause();
            var columnList = GetValuesList();
            var valuesList = GetValuesList();

            var sb = new StringBuilder();

            sb.AppendLine($"MERGE INTO {Quote(targetTable)} AS {Quote("Target")}");
            sb.AppendLine($"USING {Quote(tableSource)} AS {Quote("Source")}");
            sb.AppendLine($"    ON ({mergeSearchCondition})");

            if (updateWhenMatched)
            {
                sb.AppendLine($"WHEN MATCHED");
                sb.AppendLine($"    THEN");
                sb.AppendLine($"        UPDATE");
                sb.AppendLine($"        SET {setClause}");
            }

            sb.AppendLine($"WHEN NOT MATCHED");
            sb.AppendLine($"    THEN");
            sb.AppendLine($"        INSERT ({columnList})");
            sb.AppendLine($"        VALUES ({valuesList});");

            command.CommandText = sb.ToString();

            return command;

            string GetMergeSearchCondition()
            {
                var cols = from c in primaryKeyColumns
                           select $"{Quote("Target")}.{Quote(c)} = {Quote("Source")}.{Quote(c)}";

                return string.Join(" AND ", cols);
            }

            string GetSetClause()
            {
                // Exclude primary key columns
                var cols = from c in columns.Except(primaryKeyColumns)
                           select $"{Quote("Target")}.{Quote(c)} = {Quote("Source")}.{Quote(c)}";

                return string.Join(",\r\n            ", cols);
            }

            string GetValuesList()
            {
                return string.Join(", ", columns.Select(Quote));
            }
        }

        #endregion

        #region AlterNonclusteredIndexes

        public static Task DisableNonclusteredIndexesAsync(
            this SqlConnection connection,
            string tableName,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            return AlterNonclusteredIndexesAsync(connection, tableName, "DISABLE", transaction, cancellationToken);
        }

        public static Task RebuildNonclusteredIndexesAsync(
            this SqlConnection connection,
            string tableName,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            return AlterNonclusteredIndexesAsync(connection, tableName, "REBUILD", transaction, cancellationToken);
        }

        private static async Task AlterNonclusteredIndexesAsync(
            this SqlConnection connection,
            string tableName,
            string action,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
        {
            using (var command = GetAlterNonclusteredIndexesCommand(connection, tableName, action, transaction))
            {
                await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        internal static SqlCommandAdapter GetAlterNonclusteredIndexesCommand(
            SqlConnection connection,
            string tableName,
            string action,
            SqlTransaction transaction)
        {
            var command = SqlCommandAdapter.FromConnection(connection, transaction);

            command.CommandText = $@"DECLARE @sql AS VARCHAR(MAX)='';

SELECT @sql = @sql + 'ALTER INDEX ' + sys.indexes.name + ' ON ' + sys.objects.name + ' {action};' + CHAR(13) + CHAR(10)
FROM sys.indexes
JOIN sys.objects ON sys.indexes.object_id = sys.objects.object_id
WHERE sys.indexes.type_desc = 'NONCLUSTERED'
  AND sys.objects.type_desc = 'USER_TABLE'
  AND sys.objects.name = @tableName;

EXEC(@sql);";
            command.Parameters.Add("@tableName", SqlDbType.VarChar).Value = tableName;

            return command;
        }

        #endregion

        private static string Quote(string name)
        {
            return $"[{name}]";
        }
    }
}
