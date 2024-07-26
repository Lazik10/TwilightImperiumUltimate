namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

internal sealed class ActionCardConfiguration : IEntityTypeConfiguration<ActionCard>
{
    public void Configure(EntityTypeBuilder<ActionCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.ActionCards, Schema.Card);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(e => e.EnumName)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.EnumName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.Type))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(2);

        builder.Property(e => e.TimingWindow)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.TimingWindow))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(3);

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.Text))
            .HasColumnType("text")
            .HasColumnOrder(4);

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.GameVersion))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasColumnOrder(5);
    }
}
