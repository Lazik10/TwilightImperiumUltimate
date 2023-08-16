using TwilightImperiumUltimate.DataAccess.Tables.Galaxy;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Galaxy;

public class WormholeConfiguration : IEntityTypeConfiguration<Wormhole>
{
    public void Configure(EntityTypeBuilder<Wormhole> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Wormholes, Schema.Galaxy);

        builder.Property(e => e.WormholeName)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnName(nameof(Wormhole.WormholeName))
            .HasColumnType("int");

        builder.Property(e => e.SystemTileName)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnName(nameof(Wormhole.SystemTileName))
            .HasColumnType("int");

        builder.HasOne(e => e.SystemTile)
            .WithMany(b => b.Wormholes)
            .HasForeignKey(b => b.SystemTileName);

        builder.Property(e => e.GameVersion).IsRequired()
            .HasConversion<int>()
            .HasColumnName(nameof(Wormhole.GameVersion))
            .HasColumnType("int");
    }
}
