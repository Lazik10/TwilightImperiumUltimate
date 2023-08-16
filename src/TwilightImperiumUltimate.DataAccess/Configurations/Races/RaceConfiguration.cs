using TwilightImperiumUltimate.DataAccess.RelationshipEntities;
using TwilightImperiumUltimate.DataAccess.Tables.Races;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Races;

public class RaceConfiguration : IEntityTypeConfiguration<Race>
{
    public void Configure(EntityTypeBuilder<Race> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Races, Schema.Race);

        builder.HasKey(e => e.RaceName);

        builder.Property(e => e.RaceName)
            .IsRequired()
            .HasColumnName(nameof(Race.RaceName))
            .HasConversion<int>()
            .HasColumnType("integer");
    }
}
