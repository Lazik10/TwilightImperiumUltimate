using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Galaxy;

internal sealed class PlanetConfiguration : IEntityTypeConfiguration<Planet>
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
            .HasColumnType("integer")
            .HasColumnOrder(0);

        builder.Property(e => e.PlanetName)
            .IsRequired()
            .HasColumnName(nameof(Planet.PlanetName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.Resources)
            .IsRequired()
            .HasColumnName(nameof(Planet.Resources))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(2);

        builder.Property(e => e.Influence)
            .IsRequired()
            .HasColumnName(nameof(Planet.Influence))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(3);

        builder.Property(e => e.IsLegendary)
            .IsRequired()
            .HasColumnName(nameof(Planet.IsLegendary))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(4);

        builder.Property(e => e.TechnologySkip)
            .IsRequired()
            .HasColumnName(nameof(Planet.TechnologySkip))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(5);

        builder.Property(e => e.PlanetTrait)
            .IsRequired()
            .HasColumnName(nameof(Planet.PlanetTrait))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(6);

        builder.Property(x => x.SystemTileName)
            .IsRequired()
            .HasColumnName(nameof(Planet.SystemTileName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(7);

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(Planet.GameVersion))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(8);

        builder.HasOne(e => e.SystemTile)
            .WithMany(b => b.Planets)
            .HasForeignKey(b => b.PlanetName);
    }
}
