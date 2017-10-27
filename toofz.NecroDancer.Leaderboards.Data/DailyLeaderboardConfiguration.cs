using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace toofz.NecroDancer.Leaderboards
{
    sealed class DailyLeaderboardConfiguration : EntityTypeConfiguration<DailyLeaderboard>
    {
        public DailyLeaderboardConfiguration()
        {
            this.HasKey(c => c.LeaderboardId);
            this.Property(c => c.LeaderboardId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(c => c.DisplayName)
                .IsRequired();
            this.Property(c => c.Name)
                .IsRequired();
            this.Property(c => c.Date)
                .HasColumnType("date");
            this.HasIndex(c => new
            {
                c.Date,
                c.ProductId,
                c.IsProduction,
            })
            .HasName("IX_DailyLeaderboards")
            .IsUnique();
        }
    }
}
