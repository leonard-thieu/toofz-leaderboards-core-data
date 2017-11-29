using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class ILeaderboardsStoreClientExtensionsTests
    {
        public class BulkUpsertAsyncMethod
        {
            [Fact]
            public async Task StoreClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                ILeaderboardsStoreClient storeClient = null;
                var items = new List<object>();
                var cancellationToken = CancellationToken.None;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return storeClient.BulkUpsertAsync(items, cancellationToken);
                });
            }

            [Fact]
            public async Task CallsBulkUpsertAsyncWithNullOptions()
            {
                // Arrange
                var mockStoreClient = new Mock<ILeaderboardsStoreClient>();
                var storeClient = mockStoreClient.Object;
                var items = new List<object>();
                var cancellationToken = CancellationToken.None;

                // Act
                await storeClient.BulkUpsertAsync(items, cancellationToken);

                // Assert
                mockStoreClient.Verify(s => s.BulkUpsertAsync(items, null, cancellationToken), Times.Once);
            }
        }
    }
}
