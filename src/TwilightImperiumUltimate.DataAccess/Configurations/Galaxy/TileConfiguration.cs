using TwilightImperiumUltimate.DataAccess.Tables.Galaxy;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Galaxy;

public class TileConfiguration : IEntityTypeConfiguration<SystemTile>
{
    public void Configure(EntityTypeBuilder<SystemTile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.SystemTiles, Schema.Galaxy);

        builder.HasKey(e => e.SystemTileName)
            .IsClustered();

        builder.Property(e => e.Id)
            .IsRequired()
            .UseIdentityColumn()
            .HasColumnType("integer");

        builder.Property(e => e.SystemTileName)
            .IsRequired()
            .HasColumnName(nameof(SystemTile.SystemTileName))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.TileCategory)
            .IsRequired()
            .HasColumnName(nameof(SystemTile.TileCategory))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.FactionName)
                .IsRequired()
                .HasColumnName(nameof(SystemTile.FactionName))
                .HasConversion<int>()
                .HasColumnType("integer");

        builder.HasMany(e => e.Planets)
            .WithOne(b => b.SystemTile)
            .HasForeignKey(p => p.SystemTileName);

        builder.HasMany(e => e.Wormholes)
            .WithOne(b => b.SystemTile)
            .HasForeignKey(b => b.SystemTileName);

        builder.Property(e => e.Anomaly)
            .IsRequired()
            .HasColumnName(nameof(SystemTile.Anomaly))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.GameVersion)
                .IsRequired()
                .HasColumnName(nameof(Planet.GameVersion))
                .HasConversion<int>()
                .HasColumnType("integer");
    }
}
