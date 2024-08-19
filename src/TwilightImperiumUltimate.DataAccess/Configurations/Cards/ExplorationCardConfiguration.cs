namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

internal sealed class ExplorationCardConfiguration : IEntityTypeConfiguration<ExplorationCard>
{
    public void Configure(EntityTypeBuilder<ExplorationCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.ExplorationCards, Schema.Card);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(e => e.EnumName)
            .IsRequired()
            .HasColumnName(nameof(ExplorationCard.EnumName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(ExplorationCard.Type))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(2);

        builder.Property(e => e.ExplorationPlanetTrait)
            .IsRequired()
            .HasColumnName(nameof(ExplorationCard.ExplorationPlanetTrait))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(3);

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(AgendaCard.Text))
            .HasColumnType("text")
            .HasColumnOrder(4);

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(AgendaCard.GameVersion))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(5);
    }
}
