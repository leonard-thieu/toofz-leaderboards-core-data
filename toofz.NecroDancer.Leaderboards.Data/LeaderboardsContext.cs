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

        void Initialize()
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<Leaderboard> Leaderboards { get; set; }
        public virtual DbSet<Entry> Entries { get; set; }
        public virtual DbSet<DailyLeaderboard> DailyLeaderboards { get; set; }
        public virtual DbSet<DailyEntry> DailyEntries { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Replay> Replays { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Mode> Modes { get; set; }
        public virtual DbSet<Run> Runs { get; set; }
        public virtual DbSet<Character> Characters { get; set; }

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
