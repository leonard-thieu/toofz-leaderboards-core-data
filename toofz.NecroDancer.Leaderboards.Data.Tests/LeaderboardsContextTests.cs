using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class LeaderboardsContextTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var db = new LeaderboardsContext();

                // Assert
                Assert.IsInstanceOfType(db, typeof(LeaderboardsContext));
            }
        }

        [TestClass]
        public class Constructor_String
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var nameOrConnectionString = "Data Source=localhost;Integrated Security=SSPI";

                // Act
                var db = new LeaderboardsContext(nameOrConnectionString);

                // Assert
                Assert.IsInstanceOfType(db, typeof(LeaderboardsContext));
            }
        }

        [TestClass]
        public class LeaderboardsProperty
        {
            [TestMethod]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var leaderboards = db.Leaderboards;

                // Assert
                Assert.IsInstanceOfType(leaderboards, typeof(DbSet<Leaderboard>));
            }
        }

        [TestClass]
        public class EntriesProperty
        {
            [TestMethod]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var entries = db.Entries;

                // Assert
                Assert.IsInstanceOfType(entries, typeof(DbSet<Entry>));
            }
        }

        [TestClass]
        public class DailyLeaderboardsProperty
        {
            [TestMethod]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var leaderboards = db.DailyLeaderboards;

                // Assert
                Assert.IsInstanceOfType(leaderboards, typeof(DbSet<DailyLeaderboard>));
            }
        }

        [TestClass]
        public class DailyEntriesProperty
        {
            [TestMethod]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var entries = db.DailyEntries;

                // Assert
                Assert.IsInstanceOfType(entries, typeof(DbSet<DailyEntry>));
            }
        }

        [TestClass]
        public class PlayersProperty
        {
            [TestMethod]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var players = db.Players;

                // Assert
                Assert.IsInstanceOfType(players, typeof(DbSet<Player>));
            }
        }

        [TestClass]
        public class ReplaysProperty
        {
            [TestMethod]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var replays = db.Replays;

                // Assert
                Assert.IsInstanceOfType(replays, typeof(DbSet<Replay>));
            }
        }

        [TestClass]
        public class ProductsProperty
        {
            [TestMethod]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var products = db.Products;

                // Assert
                Assert.IsInstanceOfType(products, typeof(DbSet<Product>));
            }
        }

        [TestClass]
        public class ModesProperty
        {
            [TestMethod]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var modes = db.Modes;

                // Assert
                Assert.IsInstanceOfType(modes, typeof(DbSet<Mode>));
            }
        }

        [TestClass]
        public class RunsProperty
        {
            [TestMethod]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var runs = db.Runs;

                // Assert
                Assert.IsInstanceOfType(runs, typeof(DbSet<Run>));
            }
        }

        [TestClass]
        public class CharactersProperty
        {
            [TestMethod]
            public void ReturnsDbSet()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var characters = db.Characters;

                // Assert
                Assert.IsInstanceOfType(characters, typeof(DbSet<Character>));
            }
        }

        [TestClass]
        [TestCategory("Uses SQL Server")]
        public class IntegrationTests
        {
            [TestMethod]
            public async Task PreGeneratedMappingViewsIsUpToDate()
            {
                var connectionString = DatabaseHelper.GetConnectionString();
                using (var context = new LeaderboardsContext(connectionString))
                {
                    await context.Leaderboards.FirstOrDefaultAsync();
                    await context.Entries.FirstOrDefaultAsync();
                    await context.DailyLeaderboards.FirstOrDefaultAsync();
                    await context.DailyEntries.FirstOrDefaultAsync();
                    await context.Players.FirstOrDefaultAsync();
                    await context.Replays.FirstOrDefaultAsync();
                    await context.Products.FirstOrDefaultAsync();
                    await context.Modes.FirstOrDefaultAsync();
                    await context.Runs.FirstOrDefaultAsync();
                    await context.Characters.FirstOrDefaultAsync();

                    context.Database.Delete();
                }
            }
        }
    }
}
