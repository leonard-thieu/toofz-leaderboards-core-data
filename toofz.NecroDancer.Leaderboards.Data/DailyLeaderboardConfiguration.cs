using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
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
                .HasColumnType("date")
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_DailyLeaderboards", 1) { IsUnique = true }));
            this.Property(c => c.ProductId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_DailyLeaderboards", 2) { IsUnique = true }));
            this.Property(c => c.IsProduction)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_DailyLeaderboards", 3) { IsUnique = true }));
        }
    }
}
