using System;
using System.Data.SqlClient;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public abstract class DatabaseTestsBase : IDisposable
    {
        public DatabaseTestsBase(DatabaseFixture fixture)
        {
            Connection = fixture.Connection;
            Db = fixture.Db;

            Db.Database.Delete();  // Make sure it really dropped - needed for dirty database
            Db.Database.Initialize(force: true);
        }

        public SqlConnection Connection { get; }
        public LeaderboardsContext Db { get; }

        public void Dispose()
        {
            Connection.Close();
            Db.Database.Delete();
        }
    }
}
