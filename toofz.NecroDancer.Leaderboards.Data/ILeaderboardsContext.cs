﻿using System;
using System.Data.Entity;

namespace toofz.NecroDancer.Leaderboards
{
    public interface ILeaderboardsContext : IDisposable
    {
        DbSet<Leaderboard> Leaderboards { get; set; }
        DbSet<Entry> Entries { get; set; }
        DbSet<DailyLeaderboard> DailyLeaderboards { get; set; }
        DbSet<DailyEntry> DailyEntries { get; set; }
        DbSet<Player> Players { get; set; }
        DbSet<Replay> Replays { get; set; }
    }
}