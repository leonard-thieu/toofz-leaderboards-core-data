using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class LeaderboardsStoreClient : ILeaderboardsStoreClient
    {
        public LeaderboardsStoreClient(SqlConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
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

            await connection.OpenIsClosedAsync(cancellationToken).ConfigureAwait(false);

            MappingFragment mappingFragment;
            string tableName;
            string viewName;
            string stagingTableName;
            string activeTableName;

            using (var db = new LeaderboardsContext(connection))
            {
                mappingFragment = db.GetMappingFragment<TEntity>();
                tableName = mappingFragment.GetTableName();
                viewName = tableName;

                stagingTableName = $"{viewName}_A";
                activeTableName = $"{viewName}_B";
                var count = await db.Set<TEntity>().CountAsync(cancellationToken).ConfigureAwait(false);
                if (count != 0)
                {
                    stagingTableName = $"{viewName}_B";
                    activeTableName = $"{viewName}_A";
                }
            }

            await connection.DisableNonclusteredIndexesAsync(stagingTableName, cancellationToken).ConfigureAwait(false);
            // Cannot assume that the staging table is empty even though it's truncated afterwards.
            // This can happen when initially working with a database that was modified by legacy code. Legacy code 
            // truncated at the beginning instead of after.
            await connection.TruncateTableAsync(stagingTableName, cancellationToken).ConfigureAwait(false);
            await BulkCopyAsync(items, stagingTableName, mappingFragment, cancellationToken).ConfigureAwait(false);
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

            await connection.OpenIsClosedAsync(cancellationToken).ConfigureAwait(false);

            MappingFragment mappingFragment;

            using (var db = new LeaderboardsContext(connection))
            {
                mappingFragment = db.GetMappingFragment<TEntity>();
            }

            var tableName = mappingFragment.GetTableName();
            var tempTableName = $"#{tableName}";

            await connection.SelectIntoTemporaryTableAsync(tableName, tempTableName, cancellationToken).ConfigureAwait(false);
            await BulkCopyAsync(items, tempTableName, mappingFragment, cancellationToken).ConfigureAwait(false);

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
            CancellationToken cancellationToken)
            where TEntity : class
        {
            using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock & SqlBulkCopyOptions.KeepNulls, null))
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
    }
}