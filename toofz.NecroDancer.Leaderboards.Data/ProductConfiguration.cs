using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace toofz.NecroDancer.Leaderboards
{
    sealed class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            this.HasKey(c => c.ProductId);
            this.Property(c => c.ProductId)
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
