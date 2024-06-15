using TwilightImperiumUltimate.DataAccess.Tables.Galaxy;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Galaxy;

public class PlanetConfiguration : IEntityTypeConfiguration<Planet>
{
    public void Configure(EntityTypeBuilder<Planet> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Planets, Schema.Galaxy);

        builder.HasKey(e => e.PlanetName)
            .IsClustered();

        builder.Property(e => e.Id)
            .IsRequired()
            .UseIdentityColumn()
            .HasColumnType("integer");

        builder.Property(e => e.PlanetName)
            .IsRequired()
            .HasColumnName(nameof(Planet.PlanetName))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Resources)
            .IsRequired()
            .HasColumnName(nameof(Planet.Resources))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Influence)
            .IsRequired()
            .HasColumnName(nameof(Planet.Influence))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.IsLegendary)
            .IsRequired()
            .HasColumnName(nameof(Planet.IsLegendary))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.TechnologySkip)
            .IsRequired()
            .HasColumnName(nameof(Planet.TechnologySkip))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.PlanetTrait)
            .IsRequired()
            .HasColumnName(nameof(Planet.PlanetTrait))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.HasOne(e => e.SystemTile)
            .WithMany(b => b.Planets)
            .HasForeignKey(b => b.PlanetName);

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(Planet.GameVersion))
            .HasConversion<int>()
            .HasColumnType("integer");
    }
}
