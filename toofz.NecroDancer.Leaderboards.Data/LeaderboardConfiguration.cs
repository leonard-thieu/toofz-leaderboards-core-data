using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace toofz.NecroDancer.Leaderboards
{
    internal sealed class LeaderboardConfiguration : IEntityTypeConfiguration<Leaderboard>
    {
        public void Configure(EntityTypeBuilder<Leaderboard> builder)
        {
            builder.HasKey(c => c.LeaderboardId);
            builder.Property(c => c.LeaderboardId)
                   .ValueGeneratedNever();
            builder.Property(c => c.DisplayName)
                   .IsRequired();
            builder.Property(c => c.Name)
                   .IsRequired();
            builder.HasIndex(c => new
            {
                c.CharacterId,
                c.RunId,
                c.ModeId,
                c.ProductId,
                c.IsProduction,
                c.IsCoOp,
                c.IsCustomMusic,
            })
            .HasName("IX_Leaderboards")
            .IsUnique();
        }
    }
}
