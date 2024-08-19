namespace TwilightImperiumUltimate.DataAccess.Configurations.Factions;

internal sealed class FactionConfiguration : IEntityTypeConfiguration<Faction>
{
    public void Configure(EntityTypeBuilder<Faction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Factions, Schema.Faction);

        builder.HasKey(x => x.FactionName);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(e => e.FactionName)
            .IsRequired()
            .HasColumnName(nameof(Faction.FactionName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.ComplexityRating)
            .IsRequired()
            .HasColumnName(nameof(Faction.ComplexityRating))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(2);

        builder.Property(e => e.HomeSystem)
            .IsRequired()
            .HasColumnName(nameof(Faction.HomeSystem))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(3);

        builder.Property(x => x.Commodities)
            .IsRequired()
            .HasColumnName(nameof(Faction.Commodities))
            .HasColumnType("integer")
            .HasColumnOrder(4);

        builder.Property(x => x.Action)
            .IsRequired()
            .HasColumnName(nameof(Faction.Action))
            .HasColumnType("nvarchar(MAX)")
            .HasColumnOrder(5);

        builder.Property(x => x.History)
            .IsRequired()
            .HasColumnName(nameof(Faction.History))
            .HasColumnType("nvarchar(MAX)")
            .HasColumnOrder(6);

        builder.Property(x => x.Quote)
            .IsRequired()
            .HasColumnName(nameof(Faction.Quote))
            .HasColumnType("nvarchar(MAX)")
            .HasColumnOrder(7);

        builder.Property(x => x.SystemStats)
            .IsRequired()
            .HasColumnName(nameof(Faction.SystemStats))
            .HasColumnType("nvarchar(MAX)")
            .HasColumnOrder(8);

        builder.Property(x => x.SystemInfo)
            .IsRequired()
            .HasColumnName(nameof(Faction.SystemInfo))
            .HasColumnType("nvarchar(MAX)")
            .HasColumnOrder(9);

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(Faction.GameVersion))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(10);
    }
}
