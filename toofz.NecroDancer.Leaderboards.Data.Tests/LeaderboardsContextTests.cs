using System.Data.Entity;
using System.Threading.Tasks;
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

        public class Constructor_String
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var nameOrConnectionString = "Data Source=localhost;Integrated Security=SSPI";

                // Act
                var db = new LeaderboardsContext(nameOrConnectionString);

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
        public class IntegrationTests
        {
            [Fact]
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
