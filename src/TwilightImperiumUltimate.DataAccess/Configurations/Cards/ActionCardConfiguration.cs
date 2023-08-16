using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.DataAccess.Tables.Cards;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

public class ActionCardConfiguration : IEntityTypeConfiguration<ActionCard>
{
    public void Configure(EntityTypeBuilder<ActionCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.ActionCards, Schema.Card);

        builder.HasKey(x => x.ActionCardName);

        builder.Property(e => e.ActionCardName)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.ActionCardName))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.Name))
            .HasColumnType("text");

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.Type))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.Text))
            .HasColumnType("text");

        builder.Property(e => e.ActionCardWindow)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.ActionCardWindow))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.ImagePath)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.ImagePath))
            .HasColumnType("text");

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.GameVersion))
            .HasConversion<int>()
            .HasColumnType("integer");
    }
}
