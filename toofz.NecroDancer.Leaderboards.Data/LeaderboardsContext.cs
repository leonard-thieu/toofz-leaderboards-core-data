using System.Data.Common;
using System.Data.Entity;

namespace toofz.NecroDancer.Leaderboards
{
    public class LeaderboardsContext : DbContext, ILeaderboardsContext
    {
        public LeaderboardsContext()
        {
            Initialize();
        }

        public LeaderboardsContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Initialize();
        }

        public LeaderboardsContext(DbConnection existingConnection) : base(existingConnection, contextOwnsConnection: false)
        {
            Initialize();
        }

        private void Initialize()
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Leaderboard> Leaderboards => Set<Leaderboard>();
        public DbSet<Entry> Entries => Set<Entry>();
        public DbSet<DailyLeaderboard> DailyLeaderboards => Set<DailyLeaderboard>();
        public DbSet<DailyEntry> DailyEntries => Set<DailyEntry>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Replay> Replays => Set<Replay>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Mode> Modes => Set<Mode>();
        public DbSet<Run> Runs => Set<Run>();
        public DbSet<Character> Characters => Set<Character>();

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var configs = modelBuilder.Configurations;

            configs.Add(new LeaderboardConfiguration());
            configs.Add(new EntryConfiguration());
            configs.Add(new DailyLeaderboardConfiguration());
            configs.Add(new DailyEntryConfiguration());
            configs.Add(new PlayerConfiguration());
            configs.Add(new ReplayConfiguration());
            configs.Add(new ProductConfiguration());
            configs.Add(new ModeConfiguration());
            configs.Add(new RunConfiguration());
            configs.Add(new CharacterConfiguration());
        }
    }
}
