using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    public interface ILeaderboardsStoreClient
    {
        Task<int> BulkInsertAsync<TEntity>(
            IEnumerable<TEntity> items,
            CancellationToken cancellationToken)
            where TEntity : class;
        Task<int> UpsertAsync<TEntity>(
            IEnumerable<TEntity> items,
            bool updateWhenMatched,
            CancellationToken cancellationToken)
            where TEntity : class;
    }
}