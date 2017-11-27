using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class LeaderboardsStoreClientTests : IDisposable
    {
        public LeaderboardsStoreClientTests()
        {
            var connectionString = StorageHelper.GetDatabaseConnectionString();
            storeClient = new LeaderboardsStoreClient(connectionString);
        }

        private readonly LeaderboardsStoreClient storeClient;

        public void Dispose()
        {
            storeClient.Dispose();
        }

        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var connectionString = StorageHelper.GetDatabaseConnectionString();

                // Act
                var storeClient = new LeaderboardsStoreClient(connectionString);

                // Assert
                Assert.IsAssignableFrom<LeaderboardsStoreClient>(storeClient);
            }
        }

        public class InsertAsyncMethod : LeaderboardsStoreClientTests
        {
            [Fact]
            public async Task ItemsIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                IEnumerable<Entry> items = null;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return storeClient.BulkInsertAsync(items, default);
                });
            }

            [Trait("Category", "Uses SQL Server")]
            public class IntegrationTests : DatabaseTestsBase
            {
                [Fact]
                public async Task BulkInsertsItems()
                {
                    // Arrange
                    using (var storeClient = new LeaderboardsStoreClient(connectionString))
                    {
                        var items = new[]
                        {
                            new Entry(),
                        };

                        // Act
                        var rowsAffected = await storeClient.BulkInsertAsync(items, default);

                        // Assert
                        Assert.Equal(items.Length, rowsAffected);
                    }
                }
            }
        }

        public class UpsertAsyncMethod : LeaderboardsStoreClientTests
        {
            [Fact]
            public async Task ItemsIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                IEnumerable<Player> items = null;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return storeClient.BulkUpsertAsync(items, default);
                });
            }

            [Fact]
            public async Task ItemsIsEmpty_ShortCircuits()
            {
                // Arrange
                var items = new List<Replay>();

                // Act
                var rowsAffected = await storeClient.BulkUpsertAsync(items, default);

                // Assert
                Assert.Equal(0, rowsAffected);
            }

            [Trait("Category", "Uses SQL Server")]
            public class IntegrationTests : DatabaseTestsBase
            {
                [Fact]
                public async Task UpsertsItems()
                {
                    // Arrange
                    using (var storeClient = new LeaderboardsStoreClient(connectionString))
                    {
                        var items = new[]
                        {
                            new Player(),
                        };

                        // Act
                        var rowsAffected = await storeClient.BulkUpsertAsync(items, default);

                        // Assert
                        Assert.Equal(1, rowsAffected);
                    }
                }
            }
        }
    }
}
