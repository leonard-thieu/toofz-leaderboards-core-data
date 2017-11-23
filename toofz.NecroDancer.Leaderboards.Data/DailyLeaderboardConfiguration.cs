using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace toofz.NecroDancer.Leaderboards
{
    internal sealed class DailyLeaderboardConfiguration : IEntityTypeConfiguration<DailyLeaderboard>
    {
        public void Configure(EntityTypeBuilder<DailyLeaderboard> builder)
        {
            builder.HasKey(c => c.LeaderboardId);
            builder.Property(c => c.LeaderboardId)
                   .ValueGeneratedNever();
            builder.Property(c => c.DisplayName)
                   .IsRequired();
            builder.Property(c => c.Name)
                   .IsRequired();
            builder.Property(c => c.Date)
                   .HasColumnType("date");
            builder.HasIndex(c => new
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
