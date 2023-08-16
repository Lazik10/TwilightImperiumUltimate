using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.DataAccess.Tables.Cards;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

internal class ObjectiveCardConfiguration : IEntityTypeConfiguration<ObjectiveCard>
{
    public void Configure(EntityTypeBuilder<ObjectiveCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.ObjectiveCards, Schema.Card);

        builder.HasKey(x => x.ObjectiveCardName);

        builder.Property(e => e.ObjectiveCardName)
            .IsRequired()
            .HasColumnName(nameof(ObjectiveCard.ObjectiveCardName))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.ObjectiveCardType)
            .IsRequired()
            .HasColumnName(nameof(ObjectiveCard.ObjectiveCardType))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasColumnName(nameof(ObjectiveCard.Name))
            .HasColumnType("text");

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(ObjectiveCard.Type))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(ObjectiveCard.Text))
            .HasColumnType("text");

        builder.Property(e => e.ImagePath)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.ImagePath))
            .HasColumnType("text");

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(ObjectiveCard.GameVersion))
            .HasConversion<int>()
            .HasColumnType("integer");
    }
}
