using System;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    // This class handles database initialization and cleanup for ALL database-related tests (including tests that 
    // don't use LeaderboardsContext).
    [Collection("Uses SQL Server")]
    public abstract class DatabaseTestsBase : IDisposable
    {
        public DatabaseTestsBase()
        {
            connectionString = DatabaseHelper.GetConnectionString();
            db = new LeaderboardsContext(connectionString);

            db.Database.Delete();  // Make sure it really dropped - needed for dirty database
            db.Database.Initialize(force: true);
        }

        protected readonly string connectionString;
        protected readonly LeaderboardsContext db;

        public void Dispose()
        {
            db.Database.Delete();
        }
    }
}
