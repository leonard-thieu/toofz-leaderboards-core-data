using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class LeaderboardsContextTests
    {
        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var db = new LeaderboardsContext();

                // Assert
                Assert.IsAssignableFrom<LeaderboardsContext>(db);
            }
        }

        public class LeaderboardsProperty
        {
            [Fact]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var leaderboards = db.Leaderboards;

                // Assert
                Assert.IsAssignableFrom<DbSet<Leaderboard>>(leaderboards);
            }
        }

        public class EntriesProperty
        {
            [Fact]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var entries = db.Entries;

                // Assert
                Assert.IsAssignableFrom<DbSet<Entry>>(entries);
            }
        }

        public class DailyLeaderboardsProperty
        {
            [Fact]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var leaderboards = db.DailyLeaderboards;

                // Assert
                Assert.IsAssignableFrom<DbSet<DailyLeaderboard>>(leaderboards);
            }
        }

        public class DailyEntriesProperty
        {
            [Fact]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var entries = db.DailyEntries;

                // Assert
                Assert.IsAssignableFrom<DbSet<DailyEntry>>(entries);
            }
        }

        public class PlayersProperty
        {
            [Fact]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var players = db.Players;

                // Assert
                Assert.IsAssignableFrom<DbSet<Player>>(players);
            }
        }

        public class ReplaysProperty
        {
            [Fact]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var replays = db.Replays;

                // Assert
                Assert.IsAssignableFrom<DbSet<Replay>>(replays);
            }
        }

        public class ProductsProperty
        {
            [Fact]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var products = db.Products;

                // Assert
                Assert.IsAssignableFrom<DbSet<Product>>(products);
            }
        }

        public class ModesProperty
        {
            [Fact]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var modes = db.Modes;

                // Assert
                Assert.IsAssignableFrom<DbSet<Mode>>(modes);
            }
        }

        public class RunsProperty
        {
            [Fact]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var runs = db.Runs;

                // Assert
                Assert.IsAssignableFrom<DbSet<Run>>(runs);
            }
        }

        public class CharactersProperty
        {
            [Fact]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var characters = db.Characters;

                // Assert
                Assert.IsAssignableFrom<DbSet<Character>>(characters);
            }
        }

        [Trait("Category", "Uses SQL Server")]
        public class IntegrationTests : DatabaseTestsBase
        {
            [Fact]
            public void PreGeneratedMappingViewsIsUpToDate()
            {
                db.Leaderboards.FirstOrDefault();
                db.Entries.FirstOrDefault();
                db.DailyLeaderboards.FirstOrDefault();
                db.DailyEntries.FirstOrDefault();
                db.Players.FirstOrDefault();
                db.Replays.FirstOrDefault();
                db.Products.FirstOrDefault();
                db.Modes.FirstOrDefault();
                db.Runs.FirstOrDefault();
                db.Characters.FirstOrDefault();
            }
        }
    }
}
