namespace TwilightImperiumUltimate.DataAccess.Configurations.Galaxy;

internal sealed class WormholeConfiguration : IEntityTypeConfiguration<Wormhole>
{
    public void Configure(EntityTypeBuilder<Wormhole> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Wormholes, Schema.Galaxy);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnOrder(0);

        builder.Property(e => e.WormholeName)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnName(nameof(Wormhole.WormholeName))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(1);

        builder.Property(e => e.SystemTileName)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnName(nameof(Wormhole.SystemTileName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(2);

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnName(nameof(Wormhole.GameVersion))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(3);

        builder.HasOne(e => e.SystemTile)
            .WithMany(b => b.Wormholes)
            .HasForeignKey(b => b.SystemTileName);
    }
}
