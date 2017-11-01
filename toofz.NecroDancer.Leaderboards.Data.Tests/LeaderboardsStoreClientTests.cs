using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using SqlBulkUpsert;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class LeaderboardsStoreClientTests
    {
        public class GetLeaderboardMappingsMethod
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
                var storeClient = new LeaderboardsStoreClient(connection);

                // Act
                var mappings = storeClient.GetLeaderboardMappings();

                // Assert
                Assert.IsAssignableFrom<ColumnMappings<Leaderboard>>(mappings);
            }
        }

        public class SaveChangesAsyncMethod_Leaderboards
        {
            [Fact]
            public async Task UpsertsLeaderboards()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);
                var mockUpserter = new Mock<ITypedUpserter<Leaderboard>>();
                var upserter = mockUpserter.Object;
                var leaderboards = new List<Leaderboard>();

                // Act
                await storeClient.SaveChangesAsync(upserter, leaderboards);

                // Assert
                mockUpserter.Verify(u => u.UpsertAsync(connection, leaderboards, true, It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task ReturnsRowsAffected()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);
                var mockUpserter = new Mock<ITypedUpserter<Leaderboard>>();
                var upserter = mockUpserter.Object;
                var leaderboards = new List<Leaderboard>();
                mockUpserter
                    .Setup(u => u.UpsertAsync(connection, leaderboards, true, It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(400));

                // Act
                var rowsAffected = await storeClient.SaveChangesAsync(upserter, leaderboards);

                // Assert
                Assert.Equal(400, rowsAffected);
            }
        }

        public class GetEntryMappingsMethod
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);

                // Act
                var mappings = storeClient.GetEntryMappings();

                // Assert
                Assert.IsAssignableFrom<ColumnMappings<Entry>>(mappings);
            }
        }

        public class SaveChangesAsyncMethod_Entries
        {
            [Fact]
            public async Task InsertsEntries()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);
                var mockUpserter = new Mock<ITypedUpserter<Entry>>();
                var upserter = mockUpserter.Object;
                var entries = new List<Entry>();

                // Act
                await storeClient.SaveChangesAsync(upserter, entries);

                // Assert
                mockUpserter.Verify(u => u.InsertAsync(connection, entries, It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task ReturnsRowsAffected()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);
                var mockUpserter = new Mock<ITypedUpserter<Entry>>();
                var upserter = mockUpserter.Object;
                var entries = new List<Entry>();
                mockUpserter
                    .Setup(u => u.InsertAsync(connection, entries, It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(20000));

                // Act
                var rowsAffected = await storeClient.SaveChangesAsync(upserter, entries);

                // Assert
                Assert.Equal(20000, rowsAffected);
            }
        }

        public class GetDailyLeaderboardMappingsMethod
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);

                // Act
                var mappings = storeClient.GetDailyLeaderboardMappings();

                // Assert
                Assert.IsAssignableFrom<ColumnMappings<DailyLeaderboard>>(mappings);
            }
        }

        public class SaveChangesAsyncMethod_DailyLeaderboards
        {
            [Fact]
            public async Task UpsertsDailyLeaderboards()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);
                var mockUpserter = new Mock<ITypedUpserter<DailyLeaderboard>>();
                var upserter = mockUpserter.Object;
                var dailyLeaderboards = new List<DailyLeaderboard>();

                // Act
                await storeClient.SaveChangesAsync(upserter, dailyLeaderboards);

                // Assert
                mockUpserter.Verify(u => u.UpsertAsync(connection, dailyLeaderboards, true, It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task ReturnsRowsAffected()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);
                var mockUpserter = new Mock<ITypedUpserter<DailyLeaderboard>>();
                var upserter = mockUpserter.Object;
                var dailyLeaderboards = new List<DailyLeaderboard>();
                mockUpserter
                    .Setup(u => u.UpsertAsync(connection, dailyLeaderboards, true, It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(400));

                // Act
                var rowsAffected = await storeClient.SaveChangesAsync(upserter, dailyLeaderboards);

                // Assert
                Assert.Equal(400, rowsAffected);
            }
        }

        public class GetDailyEntryMappingsMethod
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);

                // Act
                var mappings = storeClient.GetDailyEntryMappings();

                // Assert
                Assert.IsAssignableFrom<ColumnMappings<DailyEntry>>(mappings);
            }
        }

        public class SaveChangesAsyncMethod_DailyEntries
        {
            [Fact]
            public async Task UpsertsDailyEntries()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);
                var mockUpserter = new Mock<ITypedUpserter<DailyEntry>>();
                var upserter = mockUpserter.Object;
                var dailyEntries = new List<DailyEntry>();

                // Act
                await storeClient.SaveChangesAsync(upserter, dailyEntries);

                // Assert
                mockUpserter.Verify(u => u.UpsertAsync(connection, dailyEntries, true, It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task ReturnsRowsAffected()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);
                var mockUpserter = new Mock<ITypedUpserter<DailyEntry>>();
                var upserter = mockUpserter.Object;
                var dailyEntries = new List<DailyEntry>();
                mockUpserter
                    .Setup(u => u.UpsertAsync(connection, dailyEntries, true, It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(400));

                // Act
                var rowsAffected = await storeClient.SaveChangesAsync(upserter, dailyEntries);

                // Assert
                Assert.Equal(400, rowsAffected);
            }
        }

        public class GetPlayerMappingsMethod
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);

                // Act
                var mappings = storeClient.GetPlayerMappings();

                // Assert
                Assert.IsAssignableFrom<ColumnMappings<Player>>(mappings);
            }
        }

        public class SaveChangesAsyncMethod_Players
        {
            [Fact]
            public async Task UpsertsPlayers()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);
                var mockUpserter = new Mock<ITypedUpserter<Player>>();
                var upserter = mockUpserter.Object;
                var players = new List<Player>();

                // Act
                await storeClient.SaveChangesAsync(upserter, players, true);

                // Assert
                mockUpserter.Verify(u => u.UpsertAsync(connection, players, true, It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task ReturnsRowsAffected()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);
                var mockUpserter = new Mock<ITypedUpserter<Player>>();
                var upserter = mockUpserter.Object;
                var players = new List<Player>();
                mockUpserter
                    .Setup(u => u.UpsertAsync(connection, players, true, It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(400));

                // Act
                var rowsAffected = await storeClient.SaveChangesAsync(upserter, players, true);

                // Assert
                Assert.Equal(400, rowsAffected);
            }
        }

        public class GetReplayMappingsMethod
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);

                // Act
                var mappings = storeClient.GetReplayMappings();

                // Assert
                Assert.IsAssignableFrom<ColumnMappings<Replay>>(mappings);
            }
        }

        public class SaveChangesAsyncMethod_Replays
        {
            [Fact]
            public async Task UpsertsReplays()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);
                var mockUpserter = new Mock<ITypedUpserter<Replay>>();
                var upserter = mockUpserter.Object;
                var replays = new List<Replay>();

                // Act
                await storeClient.SaveChangesAsync(upserter, replays, true);

                // Assert
                mockUpserter.Verify(u => u.UpsertAsync(connection, replays, true, It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task ReturnsRowsAffected()
            {
                // Arrange
                var connection = new SqlConnection();
                var storeClient = new LeaderboardsStoreClient(connection);
                var mockUpserter = new Mock<ITypedUpserter<Replay>>();
                var upserter = mockUpserter.Object;
                var replays = new List<Replay>();
                mockUpserter
                    .Setup(u => u.UpsertAsync(connection, replays, true, It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(400));

                // Act
                var rowsAffected = await storeClient.SaveChangesAsync(upserter, replays, true);

                // Assert
                Assert.Equal(400, rowsAffected);
            }
        }
    }
}
