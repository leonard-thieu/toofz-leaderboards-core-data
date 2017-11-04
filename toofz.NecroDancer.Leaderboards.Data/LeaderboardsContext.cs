using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;

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

        #region https://romiller.com/2014/04/08/ef6-1-mapping-between-types-tables/

        internal MappingFragment GetMappingFragment<TEntity>()
            where TEntity : class
        {
            var metadata = ((IObjectContextAdapter)this).ObjectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            var type = typeof(TEntity);
            var entityType = metadata
                .GetItems<EntityType>(DataSpace.OSpace)
                .Single(e => objectItemCollection.GetClrType(e) == type);

            // Get the entity set that uses this entity type
            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(s => s.ElementType.Name == entityType.Name);

            // Find the mapping between conceptual and storage model for this entity set
            var mapping = metadata
                .GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                .Single()
                .EntitySetMappings
                .Single(s => s.EntitySet == entitySet);

            return mapping
                .EntityTypeMappings
                .Single()
                .Fragments
                .Single();
        }

        internal string GetTableName(MappingFragment mappingFragment)
        {
            // Find the storage entity set (table) that the entity is mapped
            var table = mappingFragment.StoreEntitySet;

            // Return the table name from the storage entity set
            return (string)table.MetadataProperties["Table"].Value ?? table.Name;
        }

        internal IEnumerable<ScalarPropertyMapping> GetPropertyMappings(MappingFragment mappingFragment)
        {
            return mappingFragment
                .PropertyMappings
                .OfType<ScalarPropertyMapping>()
                .ToList();
        }

        internal IEnumerable<string> GetColumnNames(MappingFragment mappingFragment)
        {
            return GetPropertyMappings(mappingFragment)
                .Select(m => m.Column.Name)
                .ToList();
        }

        internal IEnumerable<string> GetPrimaryKeyColumnNames(MappingFragment mappingFragment)
        {
            return mappingFragment
                .StoreEntitySet
                .ElementType
                .KeyProperties // What's the difference between KeyMembers and KeyProperties?
                .Select(c => c.Name)
                .ToList();
        }

        #endregion
    }
}
