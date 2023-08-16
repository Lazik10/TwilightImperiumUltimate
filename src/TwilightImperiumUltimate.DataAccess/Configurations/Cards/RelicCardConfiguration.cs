using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.DataAccess.Tables.Cards;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

public class RelicCardConfiguration : IEntityTypeConfiguration<RelicCard>
{
    public void Configure(EntityTypeBuilder<RelicCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.RelicCards, Schema.Card);

        builder.HasKey(x => x.RelicCardName);

        builder.Property(e => e.RelicCardName)
            .IsRequired()
            .HasColumnName(nameof(RelicCard.RelicCardName))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasColumnName(nameof(RelicCard.Name))
            .HasColumnType("text");

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(RelicCard.Type))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(RelicCard.Text))
            .HasColumnType("text");

        builder.Property(e => e.ImagePath)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.ImagePath))
            .HasColumnType("text");

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(RelicCard.GameVersion))
            .HasConversion<int>()
            .HasColumnType("integer");
    }
}
