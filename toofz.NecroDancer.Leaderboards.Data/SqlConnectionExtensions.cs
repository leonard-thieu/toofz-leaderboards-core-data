﻿using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    internal static class SqlConnectionExtensions
    {
        internal static async Task OpenIsClosedAsync(this SqlConnection connection, CancellationToken cancellationToken)
        {
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        #region SwitchTable

        public static async Task SwitchTableAsync(
            this SqlConnection connection,
            string viewName,
            string tableName,
            IEnumerable<string> columns,
            CancellationToken cancellationToken)
        {
            using (var command = GetSwitchTableCommand(connection, viewName, tableName, columns))
            {
                await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        internal static SqlCommandAdapter GetSwitchTableCommand(
            SqlConnection connection,
            string viewName,
            string tableName,
            IEnumerable<string> columns)
        {
            var command = SqlCommandAdapter.FromConnection(connection);

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
            CancellationToken cancellationToken)
        {
            using (var command = GetTruncateTableCommand(connection, tableName))
            {
                await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        internal static SqlCommandAdapter GetTruncateTableCommand(
            SqlConnection connection,
            string tableName)
        {
            var command = SqlCommandAdapter.FromConnection(connection);

            command.CommandText = $"TRUNCATE TABLE {Quote(tableName)};";

            return command;
        }

        #endregion

        #region SelectIntoTemporaryTable

        public static async Task SelectIntoTemporaryTableAsync(
            this SqlConnection connection,
            string baseTableName,
            string tempTableName,
            CancellationToken cancellationToken)
        {
            using (var command = GetSelectIntoTemporaryTableCommand(connection, baseTableName, tempTableName))
            {
                await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        internal static SqlCommandAdapter GetSelectIntoTemporaryTableCommand(
            SqlConnection connection,
            string baseTableName,
            string tempTableName)
        {
            var command = SqlCommandAdapter.FromConnection(connection);

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
            CancellationToken cancellationToken)
        {
            using (var command = GetMergeCommand(connection, targetTable, tableSource, columns, primaryKeyColumns, updateWhenMatched))
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
            bool updateWhenMatched)
        {
            const string Target = "[Target]";
            const string Source = "[Source]";

            var command = SqlCommandAdapter.FromConnection(connection);

            var mergeSearchCondition = GetMergeSearchCondition();
            var valuesList = GetValuesList();

            var sb = new StringBuilder();

            sb.AppendLine($"MERGE INTO {Quote(targetTable)} AS {Target}");
            sb.AppendLine($"USING {Quote(tableSource)} AS {Source}");
            sb.AppendLine($"    ON ({mergeSearchCondition})");

            if (updateWhenMatched)
            {
                sb.AppendLine($"WHEN MATCHED");
                sb.AppendLine($"    THEN");
                sb.AppendLine($"        UPDATE");
                sb.AppendLine($"        SET {GetSetClause()}");
            }

            sb.AppendLine($"WHEN NOT MATCHED");
            sb.AppendLine($"    THEN");
            sb.AppendLine($"        INSERT ({valuesList})");
            sb.AppendLine($"        VALUES ({valuesList});");

            command.CommandText = sb.ToString();

            return command;

            string GetMergeSearchCondition()
            {
                var cols = from c in primaryKeyColumns
                           select $"{Target}.{Quote(c)} = {Source}.{Quote(c)}";

                return string.Join(" AND ", cols);
            }

            string GetSetClause()
            {
                // Exclude primary key columns
                var cols = from c in columns.Except(primaryKeyColumns)
                           select $"{Target}.{Quote(c)} = {Source}.{Quote(c)}";

                return string.Join(",\r\n            ", cols);
            }

            string GetValuesList()
            {
                return string.Join(", ", columns.Select(Quote));
            }
        }

        #endregion

        #region AlterPrimaryKey

        public static async Task DropPrimaryKeyAsync(
            this SqlConnection connection,
            string tableName,
            CancellationToken cancellationToken)
        {
            var operation = new DropPrimaryKeyOperation { Table = $"dbo.{tableName}" };

            using (var command = GetAlterPrimaryKeyCommand(connection, operation))
            {
                await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public static async Task AddPrimaryKeyAsync(
            this SqlConnection connection,
            string tableName,
            IEnumerable<string> primaryKeyColumnNames,
            CancellationToken cancellationToken)
        {
            var operation = new AddPrimaryKeyOperation { Table = $"dbo.{tableName}" };
            operation.IsClustered = true;
            foreach (var primaryKeyColumnName in primaryKeyColumnNames)
            {
                operation.Columns.Add(primaryKeyColumnName);
            }

            using (var command = GetAlterPrimaryKeyCommand(connection, operation))
            {
                await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        internal static SqlCommandAdapter GetAlterPrimaryKeyCommand(
            this SqlConnection connection,
            PrimaryKeyOperation operation)
        {
            var command = SqlCommandAdapter.FromConnection(connection);

            var generator = new SqlServerMigrationSqlGenerator();
            command.CommandText = generator.Generate(new[] { operation }, "2008").Single().Sql;

            return command;
        }

        #endregion

        #region AlterNonclusteredIndexes

        public static async Task DisableNonclusteredIndexesAsync(
            this SqlConnection connection,
            string tableName,
            CancellationToken cancellationToken)
        {
            using (var command = GetAlterNonclusteredIndexesCommand(connection, tableName, "DISABLE"))
            {
                await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public static async Task RebuildNonclusteredIndexesAsync(
            this SqlConnection connection,
            string tableName,
            CancellationToken cancellationToken)
        {
            using (var command = GetAlterNonclusteredIndexesCommand(connection, tableName, "REBUILD"))
            {
                await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        internal static SqlCommandAdapter GetAlterNonclusteredIndexesCommand(
            SqlConnection connection,
            string tableName,
            string action)
        {
            var command = SqlCommandAdapter.FromConnection(connection);

            command.CommandText = @"DECLARE @sql AS VARCHAR(MAX)='';

SELECT @sql = @sql + 'ALTER INDEX ' + quotename(sys.indexes.name, '""') + ' ON ' + quotename(sys.objects.name, '""') + ' ' + @action + ';' + CHAR(13) + CHAR(10)
FROM sys.indexes
JOIN sys.objects ON sys.indexes.object_id = sys.objects.object_id
WHERE sys.indexes.type_desc = 'NONCLUSTERED'
  AND sys.objects.type_desc = 'USER_TABLE'
  AND sys.objects.name = @tableName;

EXEC(@sql);";
            command.Parameters.Add("@action", SqlDbType.VarChar).Value = action;
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
