using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.DataAccess.Tables.Cards;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

public class StrategyCardConfiguration : IEntityTypeConfiguration<StrategyCard>
{
    public void Configure(EntityTypeBuilder<StrategyCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.StrategyCards, Schema.Card);

        builder.HasKey(x => x.StrategyCardName);

        builder.Property(e => e.StrategyCardName)
            .IsRequired()
            .HasColumnName(nameof(StrategyCard.StrategyCardName))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasColumnName(nameof(StrategyCard.Name))
            .HasColumnType("text");

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(StrategyCard.Type))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(StrategyCard.Text))
            .HasColumnType("text");

        builder.Property(e => e.InitiativeOrder)
            .IsRequired()
            .HasColumnName(nameof(StrategyCard.InitiativeOrder))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.PrimaryAbilityText)
            .IsRequired()
            .HasColumnName(nameof(StrategyCard.PrimaryAbilityText))
            .HasColumnType("text");

        builder.Property(e => e.SecondaryAbilityText)
            .IsRequired()
            .HasColumnName(nameof(StrategyCard.SecondaryAbilityText))
            .HasColumnType("text");

        builder.Property(e => e.ImagePath)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.ImagePath))
            .HasColumnType("text");

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(StrategyCard.GameVersion))
            .HasConversion<int>()
            .HasColumnType("integer");
    }
}
