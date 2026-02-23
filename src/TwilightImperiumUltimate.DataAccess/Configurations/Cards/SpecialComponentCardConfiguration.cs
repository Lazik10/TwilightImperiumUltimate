namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

internal sealed class SpecialComponentCardConfiguration : IEntityTypeConfiguration<SpecialComponentCard>
{
    public void Configure(EntityTypeBuilder<SpecialComponentCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.SpecialComponentCards, Schema.Card);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(e => e.EnumName)
            .IsRequired()
            .HasColumnName(nameof(SpecialComponentCard.EnumName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.FactionName)
            .IsRequired()
            .HasColumnName(nameof(SpecialComponentCard.FactionName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(2);

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(SpecialComponentCard.Type))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(3);

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(SpecialComponentCard.Text))
            .HasColumnType("text")
            .HasColumnOrder(4);

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(SpecialComponentCard.GameVersion))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasColumnOrder(5);

        builder.Property(e => e.Count)
            .IsRequired()
            .HasColumnName(nameof(SpecialComponentCard.Count))
            .HasColumnOrder(6);

        builder.Property(e => e.SpecialType)
            .IsRequired()
            .HasColumnName(nameof(SpecialComponentCard.SpecialType))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(7);
    }
}
