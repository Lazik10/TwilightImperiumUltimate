namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

internal sealed class FlagshipCardConfiguration : IEntityTypeConfiguration<FlagshipCard>
{
    public void Configure(EntityTypeBuilder<FlagshipCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.FlagshipCards, Schema.Card);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(e => e.EnumName)
            .IsRequired()
            .HasColumnName(nameof(FlagshipCard.EnumName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.FactionName)
            .IsRequired()
            .HasColumnName(nameof(FlagshipCard.FactionName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(2);

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(FlagshipCard.Type))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(3);

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(FlagshipCard.Text))
            .HasColumnType("text")
            .HasColumnOrder(4);

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(FlagshipCard.GameVersion))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasColumnOrder(5);
    }
}
