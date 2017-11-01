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

        private void Initialize()
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Leaderboard> Leaderboards { get => Set<Leaderboard>(); }
        public DbSet<Entry> Entries { get => Set<Entry>(); }
        public DbSet<DailyLeaderboard> DailyLeaderboards { get => Set<DailyLeaderboard>(); }
        public DbSet<DailyEntry> DailyEntries { get => Set<DailyEntry>(); }
        public DbSet<Player> Players { get => Set<Player>(); }
        public DbSet<Replay> Replays { get => Set<Replay>(); }
        public DbSet<Product> Products { get => Set<Product>(); }
        public DbSet<Mode> Modes { get => Set<Mode>(); }
        public DbSet<Run> Runs { get => Set<Run>(); }
        public DbSet<Character> Characters { get => Set<Character>(); }

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
