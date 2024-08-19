namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

internal sealed class StrategyCardConfiguration : IEntityTypeConfiguration<StrategyCard>
{
    public void Configure(EntityTypeBuilder<StrategyCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.StrategyCards, Schema.Card);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(e => e.EnumName)
            .IsRequired()
            .HasColumnName(nameof(StrategyCard.EnumName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(StrategyCard.Type))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(2);

        builder.Property(e => e.InitiativeOrder)
            .IsRequired()
            .HasColumnName(nameof(StrategyCard.InitiativeOrder))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(3);

        builder.Property(e => e.PrimaryAbilityText)
            .IsRequired()
            .HasColumnName(nameof(StrategyCard.PrimaryAbilityText))
            .HasColumnType("text")
            .HasColumnOrder(4);

        builder.Property(e => e.SecondaryAbilityText)
            .IsRequired()
            .HasColumnName(nameof(StrategyCard.SecondaryAbilityText))
            .HasColumnType("text")
            .HasColumnOrder(5);

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(StrategyCard.Text))
            .HasColumnType("text")
            .HasColumnOrder(6);

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(StrategyCard.GameVersion))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(7);
    }
}
