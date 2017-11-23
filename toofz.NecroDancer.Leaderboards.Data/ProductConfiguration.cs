using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace toofz.NecroDancer.Leaderboards
{
    internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(c => c.ProductId);
            builder.Property(c => c.ProductId)
                   .ValueGeneratedNever();

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(32);
            builder.HasIndex(c => c.Name)
                   .IsUnique();
            builder.Property(c => c.DisplayName)
                   .IsRequired();
        }
    }
}
