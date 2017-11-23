using System;
using Microsoft.EntityFrameworkCore;

namespace toofz.NecroDancer.Leaderboards
{
    public interface ILeaderboardsContext : IDisposable
    {
        DbSet<Leaderboard> Leaderboards { get; }
        DbSet<Entry> Entries { get; }
        DbSet<DailyLeaderboard> DailyLeaderboards { get; }
        DbSet<DailyEntry> DailyEntries { get; }
        DbSet<Player> Players { get; }
        DbSet<Replay> Replays { get; }
        DbSet<Product> Products { get; }
        DbSet<Mode> Modes { get; }
        DbSet<Run> Runs { get; }
        DbSet<Character> Characters { get; }
    }
}