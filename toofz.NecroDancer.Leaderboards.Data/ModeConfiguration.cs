using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace toofz.NecroDancer.Leaderboards
{
    sealed class ModeConfiguration : EntityTypeConfiguration<Mode>
    {
        public ModeConfiguration()
        {
            this.HasKey(c => c.ModeId);
            this.Property(c => c.ModeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute { IsUnique = true }));
            this.Property(c => c.DisplayName)
                .IsRequired();
        }
    }
}
