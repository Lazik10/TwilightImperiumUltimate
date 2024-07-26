namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

internal sealed class ObjectiveCardConfiguration : IEntityTypeConfiguration<ObjectiveCard>
{
    public void Configure(EntityTypeBuilder<ObjectiveCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.ObjectiveCards, Schema.Card);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(e => e.EnumName)
            .IsRequired()
            .HasColumnName(nameof(ObjectiveCard.EnumName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(ObjectiveCard.Type))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(2);

        builder.Property(e => e.ObjectiveCardType)
            .IsRequired()
            .HasColumnName(nameof(ObjectiveCard.ObjectiveCardType))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(3);

        builder.Property(x => x.TimingWindow)
            .IsRequired()
            .HasColumnName(nameof(ObjectiveCard.TimingWindow))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(4);

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(ObjectiveCard.Text))
            .HasColumnType("text")
            .HasColumnOrder(5);

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(ObjectiveCard.GameVersion))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(6);
    }
}
