using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace toofz.NecroDancer.Leaderboards
{
    sealed class LeaderboardConfiguration : EntityTypeConfiguration<Leaderboard>
    {
        public LeaderboardConfiguration()
        {
            this.HasKey(c => c.LeaderboardId);
            this.Property(c => c.LeaderboardId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(c => c.DisplayName)
                .IsRequired();
            this.Property(c => c.Name)
                .IsRequired();
            this.HasIndex(c => new
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
