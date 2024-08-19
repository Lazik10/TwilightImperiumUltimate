namespace TwilightImperiumUltimate.DataAccess.Configurations.Galaxy;

internal sealed class SystemTileConfiguration : IEntityTypeConfiguration<SystemTile>
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
            .HasColumnType("integer")
            .HasColumnOrder(0);

        builder.Property(e => e.SystemTileName)
            .IsRequired()
            .HasColumnName(nameof(SystemTile.SystemTileName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.SystemTileCode)
            .IsRequired()
            .HasColumnName(nameof(SystemTile.SystemTileCode))
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(2);

        builder.Property(e => e.TileCategory)
            .IsRequired()
            .HasColumnName(nameof(SystemTile.TileCategory))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(3);

        builder.Property(e => e.FactionName)
            .IsRequired()
            .HasColumnName(nameof(SystemTile.FactionName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(4);

        builder.Property(e => e.Anomaly)
            .IsRequired()
            .HasColumnName(nameof(SystemTile.Anomaly))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(5);

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(Planet.GameVersion))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(6);

        builder.HasMany(e => e.Planets)
            .WithOne(b => b.SystemTile)
            .HasForeignKey(p => p.SystemTileName);

        builder.HasMany(e => e.Wormholes)
            .WithOne(b => b.SystemTile)
            .HasForeignKey(b => b.SystemTileName);
    }
}
