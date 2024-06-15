using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.DataAccess.Tables.Cards;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

internal class PromissaryNoteCardConfiguration : IEntityTypeConfiguration<PromissaryNoteCard>
{
    public void Configure(EntityTypeBuilder<PromissaryNoteCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.PromissaryNoteCards, Schema.Card);

        builder.HasKey(x => x.PromissaryNoteCardName);

        builder.Property(e => e.PromissaryNoteCardName)
            .IsRequired()
            .HasColumnName(nameof(PromissaryNoteCard.PromissaryNoteCardName))
            .HasConversion<int>()
            .HasColumnType("integer");
    }
}
