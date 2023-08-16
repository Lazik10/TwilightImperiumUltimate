using TwilightImperiumUltimate.DataAccess.RelationshipEntities;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Relationships;

public class RaceTechnologyConfiguration : IEntityTypeConfiguration<RaceTechnology>
{
    public void Configure(EntityTypeBuilder<RaceTechnology> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(RaceTechnology), Schema.Realationships);

        builder.HasKey(e => new { e.RaceName, e.TechnologyName });

        builder.Property(e => e.RaceName)
            .IsRequired()
            .HasColumnName(nameof(RaceTechnology.RaceName))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.TechnologyName)
            .IsRequired()
            .HasColumnName(nameof(RaceTechnology.TechnologyName))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.StartingTechnology)
            .IsRequired()
            .HasColumnName(nameof(RaceTechnology.StartingTechnology))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.HasOne(rt => rt.Race)
            .WithMany(r => r.RaceTechnologies)
            .HasForeignKey(rt => rt.RaceName);

        builder.HasOne(rt => rt.Technology)
            .WithMany(t => t.RaceTechnologies)
            .HasForeignKey(rt => rt.TechnologyName);
    }
}
