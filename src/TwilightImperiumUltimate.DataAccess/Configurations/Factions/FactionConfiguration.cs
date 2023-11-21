using TwilightImperiumUltimate.DataAccess.Tables.Factions;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Factions;

public class FactionConfiguration : IEntityTypeConfiguration<Faction>
{
    public void Configure(EntityTypeBuilder<Faction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Factions, Schema.Faction);

        builder.HasKey(x => x.FactionName);

        builder.Property(e => e.FactionName)
            .IsRequired()
            .HasColumnName(nameof(Faction.FactionName))
            .HasConversion<int>()
            .HasColumnType("integer");
    }
}
