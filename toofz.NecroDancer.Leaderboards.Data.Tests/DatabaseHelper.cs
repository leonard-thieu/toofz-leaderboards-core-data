using System;
using System.Configuration;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    internal static class DatabaseHelper
    {
        public static string GetConnectionString()
        {
            return Environment.GetEnvironmentVariable("LeaderboardsContextTestConnectionString", EnvironmentVariableTarget.Machine) ??
                "Data Source=localhost;Initial Catalog=LeaderboardsTestDb;Integrated Security=SSPI";
        }
    }
}
