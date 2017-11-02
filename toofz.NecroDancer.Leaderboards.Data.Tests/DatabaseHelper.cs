﻿using System;
using System.Configuration;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    internal static class DatabaseHelper
    {
        public static string GetConnectionString()
        {
            return Environment.GetEnvironmentVariable("LeaderboardsContextTestConnectionString", EnvironmentVariableTarget.Machine) ??
                ConfigurationManager.ConnectionStrings[nameof(LeaderboardsContext)].ConnectionString;
        }
    }
}
