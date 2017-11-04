using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class LeaderboardsStoreClient
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

            using (var db = new LeaderboardsContext(connection))
            using (var transaction = connection.BeginTransaction())
            {
                db.Database.UseTransaction(transaction);
                try
                {
                    var mappingFragment = db.GetMappingFragment<TEntity>();
                    var tableName = mappingFragment.GetTableName();
                    var viewName = tableName;
                    var columnNames = mappingFragment.GetColumnNames();

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

                    using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock & SqlBulkCopyOptions.KeepNulls, transaction))
                    {
                        sqlBulkCopy.BulkCopyTimeout = 0;
                        sqlBulkCopy.DestinationTableName = tableName;

                        foreach (var columnName in columnNames)
                        {
                            sqlBulkCopy.ColumnMappings.Add(columnName, columnName);
                        }

                        using (var reader = new TypedDataReader<TEntity>(mappingFragment.GetScalarPropertyMappings(), items))
                        {
                            await sqlBulkCopy.WriteToServerAsync(reader, cancellationToken).ConfigureAwait(false);
                        }
                    }
                    await connection.RebuildNonclusteredIndexesAsync(stagingTableName, transaction, cancellationToken).ConfigureAwait(false);

                    await connection.SwitchTableAsync(viewName, stagingTableName, columnNames, transaction, cancellationToken).ConfigureAwait(false);
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

        public async Task<int> UpsertAsync<TEntity>(
            IEnumerable<TEntity> items,
            bool updateWhenMatched,
            CancellationToken cancellationToken)
            where TEntity : class
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (!items.Any()) { return 0; }

            // LeaderboardsContext does not use the connection/transaction. If changes are made where LeaderboardsContext does use the 
            // connection/transaction, ensure that the connection/transaction is passed to LeaderboardsContext.
            using (var db = new LeaderboardsContext())
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var mappingFragment = db.GetMappingFragment<TEntity>();
                    var tableName = mappingFragment.GetTableName();
                    var tempTableName = $"#{tableName}";
                    var columnNames = mappingFragment.GetColumnNames();
                    var primaryKeyColumnNames = mappingFragment.GetPrimaryKeyColumnNames();

                    await connection.SelectIntoTemporaryTableAsync(tableName, tempTableName, transaction, cancellationToken).ConfigureAwait(false);

                    using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock & SqlBulkCopyOptions.KeepNulls, transaction))
                    {
                        sqlBulkCopy.BulkCopyTimeout = 0;
                        sqlBulkCopy.DestinationTableName = tableName;

                        foreach (var columnName in columnNames)
                        {
                            sqlBulkCopy.ColumnMappings.Add(columnName, columnName);
                        }

                        using (var reader = new TypedDataReader<TEntity>(mappingFragment.GetScalarPropertyMappings(), items))
                        {
                            await sqlBulkCopy.WriteToServerAsync(reader, cancellationToken).ConfigureAwait(false);
                        }
                    }

                    var rowsAffected = await connection.MergeAsync(
                        tableName,
                        tempTableName,
                        columnNames,
                        primaryKeyColumnNames,
                        updateWhenMatched,
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
    }
}