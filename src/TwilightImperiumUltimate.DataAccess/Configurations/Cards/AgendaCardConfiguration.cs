using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.DataAccess.Tables.Cards;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

public class AgendaCardConfiguration : IEntityTypeConfiguration<AgendaCard>
{
    public void Configure(EntityTypeBuilder<AgendaCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.AgendaCards, Schema.Card);

        builder.HasKey(x => x.AgendaCardName);

        builder.Property(e => e.AgendaCardName)
            .IsRequired()
            .HasColumnName(nameof(AgendaCard.AgendaCardName))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasColumnName(nameof(AgendaCard.Name))
            .HasColumnType("text");

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(AgendaCard.Text))
            .HasColumnType("text");

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(AgendaCard.Type))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.AgendaCardType)
            .IsRequired()
            .HasColumnName(nameof(AgendaCard.AgendaCardType))
            .HasConversion<int>()
            .HasColumnType("integer");

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
