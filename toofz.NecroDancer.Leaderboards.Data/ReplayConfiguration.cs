using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace toofz.NecroDancer.Leaderboards
{
    internal sealed class ReplayConfiguration : IEntityTypeConfiguration<Replay>
    {
        public void Configure(EntityTypeBuilder<Replay> builder)
        {
            builder.HasKey(e => e.ReplayId);
            builder.Property(e => e.ReplayId)
                   .ValueGeneratedNever();
        }
    }
}
