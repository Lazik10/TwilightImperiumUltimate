using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.DataAccess.Tables.Cards;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

internal class ExplorationCardConfiguration : IEntityTypeConfiguration<ExplorationCard>
{
    public void Configure(EntityTypeBuilder<ExplorationCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.ExplorationCards, Schema.Card);

        builder.HasKey(x => x.ExplorationCardName);

        builder.Property(e => e.ExplorationCardName)
            .IsRequired()
            .HasColumnName(nameof(ExplorationCard.ExplorationCardName))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasColumnName(nameof(ExplorationCard.Name))
            .HasColumnType("text");

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(ExplorationCard.Type))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.ExplorationPlanetTrait)
            .IsRequired()
            .HasColumnName(nameof(ExplorationCard.ExplorationPlanetTrait))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(AgendaCard.Text))
            .HasColumnType("text");

        builder.Property(e => e.ImagePath)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.ImagePath))
            .HasColumnType("text");

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(AgendaCard.GameVersion))
            .HasConversion<int>()
            .HasColumnType("integer");
    }
}
