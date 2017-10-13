using System.Data.Entity.ModelConfiguration;

namespace toofz.NecroDancer.Leaderboards
{
    sealed class DailyEntryConfiguration : EntityTypeConfiguration<DailyEntry>
    {
        public DailyEntryConfiguration()
        {
            this.HasKey(e => new { e.LeaderboardId, e.Rank });
            this.Property(e => e.LeaderboardId)
                .HasColumnOrder(0);
            this.Property(e => e.Rank)
                .HasColumnOrder(1);
        }
    }
}
