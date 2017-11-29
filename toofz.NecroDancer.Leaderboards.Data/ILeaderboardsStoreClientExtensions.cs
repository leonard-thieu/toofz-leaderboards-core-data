using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Contains extension methods for <see cref="ILeaderboardsStoreClient"/>.
    /// </summary>
    public static class ILeaderboardsStoreClientExtensions
    {
        public static Task<int> BulkUpsertAsync<TEntity>(
            this ILeaderboardsStoreClient storeClient,
            IEnumerable<TEntity> items,
            CancellationToken cancellationToken)
            where TEntity : class
        {
            if (storeClient == null)
                throw new ArgumentNullException(nameof(storeClient));

            return storeClient.BulkUpsertAsync(items, null, cancellationToken);
        }
    }
}
