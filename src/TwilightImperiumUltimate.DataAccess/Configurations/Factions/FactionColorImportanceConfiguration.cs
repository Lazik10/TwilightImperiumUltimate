using TwilightImperiumUltimate.DataAccess.Tables.Factions;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Factions;

public class FactionColorImportanceConfiguration : IEntityTypeConfiguration<FactionColorImportance>
{
    public void Configure(EntityTypeBuilder<FactionColorImportance> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.FactionColorImportance, Schema.Faction);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FactionName)
            .IsRequired()
            .HasColumnName(nameof(FactionColorImportance.FactionName))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Color)
            .IsRequired()
            .HasColumnName(nameof(FactionColorImportance.Color))
            .HasConversion<int>()
            .HasColumnType("integer");
    }
}
