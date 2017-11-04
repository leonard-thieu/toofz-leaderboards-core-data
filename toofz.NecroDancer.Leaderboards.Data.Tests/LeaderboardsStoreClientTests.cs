using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class LeaderboardsStoreClientTests
    {
        public class Constructor
        {
            [Fact]
            public void ConnectionIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                SqlConnection connection = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new LeaderboardsStoreClient(connection);
                });
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var connection = new SqlConnection();

                // Act
                var storeClient = new LeaderboardsStoreClient(connection);

                // Assert
                Assert.IsAssignableFrom<LeaderboardsStoreClient>(storeClient);
            }
        }

        public class InsertAsyncMethod
        {
            [Fact]
            public async Task ItemsIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);
                IEnumerable<Entry> items = null;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return storeClient.BulkInsertAsync(items, default);
                });
            }

            [Collection(DatabaseCollection.Name)]
            [Trait("Category", "Uses SQL Server")]
            public class IntegrationTests : DatabaseTestsBase
            {
                public IntegrationTests(DatabaseFixture fixture) : base(fixture) { }

                [Fact]
                public async Task BulkInsertsItems()
                {
                    // Arrange
                    var storeClient = new LeaderboardsStoreClient(Connection);
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

        public class UpsertAsyncMethod
        {
            [Fact]
            public async Task ItemsIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);
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
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);
                var items = new List<Replay>();

                // Act
                var rowsAffected = await storeClient.BulkUpsertAsync(items, default);

                // Assert
                Assert.Equal(0, rowsAffected);
            }

            [Collection(DatabaseCollection.Name)]
            [Trait("Category", "Uses SQL Server")]
            public class IntegrationTests : DatabaseTestsBase
            {
                public IntegrationTests(DatabaseFixture fixture) : base(fixture) { }

                [Fact]
                public async Task UpsertsItems()
                {
                    // Arrange
                    var storeClient = new LeaderboardsStoreClient(Connection);
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
