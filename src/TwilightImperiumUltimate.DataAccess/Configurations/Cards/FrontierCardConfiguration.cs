using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;
using TwilightImperiumUltimate.DataAccess.Tables.Cards;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

public class FrontierCardConfiguration : IEntityTypeConfiguration<FrontierCard>
{
    public void Configure(EntityTypeBuilder<FrontierCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.FrontierCards, Schema.Card);

        builder.HasKey(x => x.FrontierCardName);

        builder.Property(e => e.FrontierCardName)
            .IsRequired()
            .HasColumnName(nameof(FrontierCard.FrontierCardName))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasColumnName(nameof(FrontierCard.Name))
            .HasColumnType("text");

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(FrontierCard.Type))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(FrontierCard.Text))
            .HasColumnType("text");

        builder.Property(e => e.ImagePath)
            .IsRequired()
            .HasColumnName(nameof(ActionCard.ImagePath))
            .HasColumnType("text");

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(FrontierCard.GameVersion))
            .HasConversion<int>()
            .HasColumnType("integer");
    }
}
