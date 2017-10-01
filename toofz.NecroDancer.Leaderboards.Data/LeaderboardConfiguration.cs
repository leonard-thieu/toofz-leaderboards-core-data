using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.CodeAnalysis;

namespace toofz.NecroDancer.Leaderboards
{
    [ExcludeFromCodeCoverage]
    sealed class LeaderboardConfiguration : EntityTypeConfiguration<Leaderboard>
    {
        public LeaderboardConfiguration()
        {
            this.HasKey(c => c.LeaderboardId);
            this.Property(c => c.LeaderboardId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(c => c.DisplayName)
                .IsRequired();
            this.Property(c => c.CharacterId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_Leaderboards", 1) { IsUnique = true }));
            this.Property(c => c.RunId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_Leaderboards", 2) { IsUnique = true }));
            this.Property(c => c.ModeId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_Leaderboards", 3) { IsUnique = true }));
            this.Property(c => c.ProductId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_Leaderboards", 4) { IsUnique = true }));
            this.Property(c => c.IsProduction)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_Leaderboards", 5) { IsUnique = true }));
        }
    }
}
