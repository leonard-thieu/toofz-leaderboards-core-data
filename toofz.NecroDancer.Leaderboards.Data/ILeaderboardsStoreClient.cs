using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    public interface ILeaderboardsStoreClient : IDisposable
    {
        Task<int> BulkInsertAsync<TEntity>(
            IEnumerable<TEntity> items,
            CancellationToken cancellationToken)
            where TEntity : class;
        Task<int> BulkUpsertAsync<TEntity>(
            IEnumerable<TEntity> items,
            BulkUpsertOptions options,
            CancellationToken cancellationToken)
            where TEntity : class;
    }
}