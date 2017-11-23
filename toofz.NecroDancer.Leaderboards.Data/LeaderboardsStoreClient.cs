using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class LeaderboardsStoreClient : ILeaderboardsStoreClient
    {
        /// <summary>
        /// Initializes an instance of the <see cref="LeaderboardsStoreClient"/> class.
        /// </summary>
        /// <param name="connectionString">The connection used to open the SQL Server database.</param>
        public LeaderboardsStoreClient(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }

        private readonly SqlConnection connection;

        public async Task<int> BulkInsertAsync<TEntity>(
            IEnumerable<TEntity> items,
            CancellationToken cancellationToken)
            where TEntity : class
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            items = items.ToList();

            await connection.OpenIfClosedAsync(cancellationToken).ConfigureAwait(false);

            MappingFragment mappingFragment;
            using (var db = new LeaderboardsContext(connection))
            {
                mappingFragment = db.GetMappingFragment<TEntity>();
            }

            var schemaName = mappingFragment.GetSchemaName();
            var tableName = mappingFragment.GetTableName();
            var viewName = tableName;

            var activeTableName = await connection.GetReferencedTableNameAsync(schemaName, viewName, cancellationToken).ConfigureAwait(false);
            var stagingTableName = activeTableName.EndsWith("_A") ?
                $"{viewName}_B" :
                $"{viewName}_A";

            await connection.DisableNonclusteredIndexesAsync(stagingTableName, cancellationToken).ConfigureAwait(false);
#if FEATURE_DROP_PRIMARY_KEYS
            await connection.DropPrimaryKeyAsync(schemaName, stagingTableName, cancellationToken).ConfigureAwait(false);
#endif
            // Cannot assume that the staging table is empty even though it's truncated afterwards.
            // This can happen when initially working with a database that was modified by legacy code. Legacy code 
            // truncated at the beginning instead of after.
            await connection.TruncateTableAsync(stagingTableName, cancellationToken).ConfigureAwait(false);
            await BulkCopyAsync(items, stagingTableName, mappingFragment, true, cancellationToken).ConfigureAwait(false);
#if FEATURE_DROP_PRIMARY_KEYS
            var primaryKeyColumnNames = mappingFragment.GetPrimaryKeyColumnNames();
            await connection.AddPrimaryKeyAsync(schemaName, stagingTableName, primaryKeyColumnNames, cancellationToken).ConfigureAwait(false);
#endif
            await connection.RebuildNonclusteredIndexesAsync(stagingTableName, cancellationToken).ConfigureAwait(false);
            await connection.SwitchTableAsync(
                viewName,
                stagingTableName,
                mappingFragment.GetColumnNames(),
                cancellationToken)
                .ConfigureAwait(false);
            // Active table is now the new staging table
            await connection.TruncateTableAsync(activeTableName, cancellationToken).ConfigureAwait(false);

            return items.Count();
        }

        public Task<int> BulkUpsertAsync<TEntity>(
            IEnumerable<TEntity> items,
            CancellationToken cancellationToken)
            where TEntity : class
        {
            return BulkUpsertAsync(items, null, cancellationToken);
        }

        public async Task<int> BulkUpsertAsync<TEntity>(
            IEnumerable<TEntity> items,
            BulkUpsertOptions options,
            CancellationToken cancellationToken)
            where TEntity : class
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            items = items.ToList();
            options = options ?? new BulkUpsertOptions();

            if (!items.Any()) { return 0; }

            await connection.OpenIfClosedAsync(cancellationToken).ConfigureAwait(false);

            MappingFragment mappingFragment;

            using (var db = new LeaderboardsContext(connection))
            {
                mappingFragment = db.GetMappingFragment<TEntity>();
            }

            var tableName = mappingFragment.GetTableName();
            var tempTableName = $"#{tableName}";

            await connection.SelectIntoTemporaryTableAsync(tableName, tempTableName, cancellationToken).ConfigureAwait(false);
            await BulkCopyAsync(items, tempTableName, mappingFragment, false, cancellationToken).ConfigureAwait(false);

            return await connection.MergeAsync(
                tableName,
                tempTableName,
                mappingFragment.GetColumnNames(),
                mappingFragment.GetPrimaryKeyColumnNames(),
                options.UpdateWhenMatched,
                cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task BulkCopyAsync<TEntity>(
            IEnumerable<TEntity> items,
            string destinationTableName,
            MappingFragment mappingFragment,
            bool useTableLock,
            CancellationToken cancellationToken)
            where TEntity : class
        {
            var options = SqlBulkCopyOptions.KeepNulls;
            if (useTableLock) { options &= SqlBulkCopyOptions.TableLock; }

            using (var sqlBulkCopy = new SqlBulkCopy(connection, options, externalTransaction: null))
            {
                sqlBulkCopy.BulkCopyTimeout = 0;
                sqlBulkCopy.DestinationTableName = destinationTableName;

                foreach (var columnName in mappingFragment.GetColumnNames())
                {
                    sqlBulkCopy.ColumnMappings.Add(columnName, columnName);
                }

                using (var reader = new TypedDataReader<TEntity>(mappingFragment.GetScalarPropertyMappings(), items))
                {
                    await sqlBulkCopy.WriteToServerAsync(reader, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        #region IDisposable Members

        private bool disposed;

        /// <summary>
        /// Disposes of resources used by <see cref="LeaderboardsStoreClient"/>.
        /// </summary>
        public void Dispose()
        {
            if (disposed) { return; }

            connection.Dispose();

            disposed = true;
        }

        #endregion
    }
}