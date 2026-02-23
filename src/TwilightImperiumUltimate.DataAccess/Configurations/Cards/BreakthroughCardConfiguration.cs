namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

internal sealed class BreakthroughCardConfiguration : IEntityTypeConfiguration<BreakthroughCard>
{
    public void Configure(EntityTypeBuilder<BreakthroughCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.BreakthroughCards, Schema.Card);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(e => e.EnumName)
            .IsRequired()
            .HasColumnName(nameof(BreakthroughCard.EnumName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.FactionName)
            .IsRequired()
            .HasColumnName(nameof(BreakthroughCard.FactionName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(2);

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(BreakthroughCard.Type))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(3);

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(BreakthroughCard.Text))
            .HasColumnType("text")
            .HasColumnOrder(4);

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(BreakthroughCard.GameVersion))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasColumnOrder(5);
    }
}
