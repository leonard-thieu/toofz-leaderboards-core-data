using System;
using System.Data.Entity;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public sealed class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<LeaderboardsContext>());
            var connectionString = DatabaseHelper.GetConnectionString();
            Db = new LeaderboardsContext(connectionString);
            Db.Database.Delete();  // Make sure it really dropped - needed for dirty database
            Db.Database.Initialize(force: true);
        }

        public LeaderboardsContext Db { get; }

        public void Dispose()
        {
            Db.Database.Delete();
            Db.Dispose();
        }
    }
}
