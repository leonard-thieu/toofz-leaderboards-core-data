using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace toofz.NecroDancer.Leaderboards
{
    internal sealed class DailyEntryConfiguration : IEntityTypeConfiguration<DailyEntry>
    {
        public void Configure(EntityTypeBuilder<DailyEntry> builder)
        {
            builder.HasKey(e => new { e.LeaderboardId, e.Rank });
        }
    }
}
