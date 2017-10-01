using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace toofz.NecroDancer.Leaderboards
{
    sealed class CharacterConfiguration : EntityTypeConfiguration<Character>
    {
        public CharacterConfiguration()
        {
            this.HasKey(c => c.CharacterId);
            this.Property(c => c.CharacterId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(c => c.Name)
                .IsRequired();
            this.Property(c => c.DisplayName)
                .IsRequired();
        }
    }
}
