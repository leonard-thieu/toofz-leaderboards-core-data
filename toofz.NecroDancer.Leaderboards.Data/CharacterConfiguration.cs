using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace toofz.NecroDancer.Leaderboards
{
    internal sealed class CharacterConfiguration : IEntityTypeConfiguration<Character>
    {
        public void Configure(EntityTypeBuilder<Character> builder)
        {
            builder.HasKey(c => c.CharacterId);
            builder.Property(c => c.CharacterId)
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
