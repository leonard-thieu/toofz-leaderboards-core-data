using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace toofz.NecroDancer.Leaderboards
{
    internal sealed class ModeConfiguration : IEntityTypeConfiguration<Mode>
    {
        public void Configure(EntityTypeBuilder<Mode> builder)
        {
            builder.HasKey(c => c.ModeId);
            builder.Property(c => c.ModeId)
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
