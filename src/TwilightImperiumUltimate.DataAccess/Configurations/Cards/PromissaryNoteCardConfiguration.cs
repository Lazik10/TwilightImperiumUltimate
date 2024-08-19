namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

internal sealed class PromissaryNoteCardConfiguration : IEntityTypeConfiguration<PromissoryNoteCard>
{
    public void Configure(EntityTypeBuilder<PromissoryNoteCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.PromissaryNoteCards, Schema.Card);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(e => e.EnumName)
            .IsRequired()
            .HasColumnName(nameof(PromissoryNoteCard.EnumName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(PromissoryNoteCard.Type))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(2);

        builder.Property(e => e.Faction)
            .IsRequired()
            .HasColumnName(nameof(PromissoryNoteCard.Faction))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(3);

        builder.Property(x => x.Text)
            .IsRequired()
            .HasColumnName(nameof(PromissoryNoteCard.Text))
            .HasColumnType("varchar(MAX)")
            .HasColumnOrder(4);

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(PromissoryNoteCard.GameVersion))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(5);
    }
}
