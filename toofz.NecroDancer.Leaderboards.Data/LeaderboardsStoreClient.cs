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

            await connection.OpenIfClosedAsync(cancellationToken).ConfigureAwait(false);
            using (var db = new LeaderboardsContext(connection))
            using (var transaction = connection.BeginTransaction())
            {
                db.Database.UseTransaction(transaction);
                try
                {
                    var mappingFragment = db.GetMappingFragment<TEntity>();
                    var tableName = mappingFragment.GetTableName();
                    var viewName = tableName;

                    var stagingTableName = $"{viewName}_A";
                    var activeTableName = $"{viewName}_B";
                    var count = await db.Set<TEntity>().CountAsync(cancellationToken).ConfigureAwait(false);
                    if (count != 0)
                    {
                        stagingTableName = $"{viewName}_B";
                        activeTableName = $"{viewName}_A";
                    }

                    await connection.DisableNonclusteredIndexesAsync(stagingTableName, transaction, cancellationToken).ConfigureAwait(false);
                    // Cannot assume that the staging table is empty even though it's truncated afterwards.
                    // This can happen when initially working with a database that was modified by legacy code. Legacy code 
                    // truncated at the beginning instead of after.
                    await connection.TruncateTableAsync(stagingTableName, transaction, cancellationToken).ConfigureAwait(false);
                    await BulkCopyAsync(items, stagingTableName, mappingFragment, transaction, cancellationToken).ConfigureAwait(false);
                    await connection.RebuildNonclusteredIndexesAsync(stagingTableName, transaction, cancellationToken).ConfigureAwait(false);
                    await connection.SwitchTableAsync(
                        viewName,
                        stagingTableName,
                        mappingFragment.GetColumnNames(),
                        transaction,
                        cancellationToken)
                        .ConfigureAwait(false);
                    // Active table is now the new staging table
                    await connection.TruncateTableAsync(activeTableName, transaction, cancellationToken).ConfigureAwait(false);

                    transaction.Commit();

                    return items.Count();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
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

            options = options ?? new BulkUpsertOptions();

            items = items.ToList();
            if (!items.Any()) { return 0; }

            await connection.OpenIfClosedAsync(cancellationToken).ConfigureAwait(false);
            using (var db = new LeaderboardsContext(connection))
            using (var transaction = connection.BeginTransaction())
            {
                db.Database.UseTransaction(transaction);
                try
                {
                    var mappingFragment = db.GetMappingFragment<TEntity>();
                    var tableName = mappingFragment.GetTableName();
                    var tempTableName = $"#{tableName}";

                    await connection.SelectIntoTemporaryTableAsync(tableName, tempTableName, transaction, cancellationToken).ConfigureAwait(false);
                    await BulkCopyAsync(items, tempTableName, mappingFragment, transaction, cancellationToken).ConfigureAwait(false);

                    var rowsAffected = await connection.MergeAsync(
                        tableName,
                        tempTableName,
                        mappingFragment.GetColumnNames(),
                        mappingFragment.GetPrimaryKeyColumnNames(),
                        options.UpdateWhenMatched,
                        transaction,
                        cancellationToken)
                        .ConfigureAwait(false);

                    transaction.Commit();

                    return rowsAffected;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private async Task BulkCopyAsync<TEntity>(
            IEnumerable<TEntity> items,
            string destinationTableName,
            MappingFragment mappingFragment,
            SqlTransaction transaction,
            CancellationToken cancellationToken)
            where TEntity : class
        {
            using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock & SqlBulkCopyOptions.KeepNulls, transaction))
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