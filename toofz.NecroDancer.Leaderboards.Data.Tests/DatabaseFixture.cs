using System;
using System.Data.SqlClient;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public sealed class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            var connectionString = DatabaseHelper.GetConnectionString();
            Connection = new SqlConnection(connectionString);
            Db = new LeaderboardsContext(Connection);
        }

        public SqlConnection Connection { get; }
        public LeaderboardsContext Db { get; }

        public void Dispose()
        {
            Db.Dispose();
            Connection.Dispose();
        }
    }
}
