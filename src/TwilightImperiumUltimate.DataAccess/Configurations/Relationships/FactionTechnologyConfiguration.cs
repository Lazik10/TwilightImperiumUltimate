using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Relationships;

public class FactionTechnologyConfiguration : IEntityTypeConfiguration<FactionTechnology>
{
    public void Configure(EntityTypeBuilder<FactionTechnology> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(FactionTechnology), Schema.Realationships);

        // Configure the composite key for the intermediary table
        builder.HasKey(ft => new { ft.FactionName, ft.TechnologyName });

        builder.Property(e => e.FactionName)
            .IsRequired()
            .HasColumnName(nameof(FactionTechnology.FactionName))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.TechnologyName)
            .IsRequired()
            .HasColumnName(nameof(FactionTechnology.TechnologyName))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.StartingTechnology)
            .IsRequired()
            .HasColumnName(nameof(FactionTechnology.StartingTechnology))
            .HasConversion<int>()
            .HasColumnType("integer");
    }
}
