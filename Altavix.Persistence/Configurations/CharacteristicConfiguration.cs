using Altavix.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Altavix.Persistence.Configurations;

public class CharacteristicConfiguration : IEntityTypeConfiguration<CharacteristicEntity>
{
    public void Configure(EntityTypeBuilder<CharacteristicEntity> builder)
    {
        builder.ToTable("tbCharacteristics");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Enabled)
            .IsRequired()
            .HasDefaultValue(true);
    }
}
