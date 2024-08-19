namespace TwilightImperiumUltimate.DataAccess.Configurations.Cards;

internal sealed class FrontierCardConfiguration : IEntityTypeConfiguration<FrontierCard>
{
    public void Configure(EntityTypeBuilder<FrontierCard> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.FrontierCards, Schema.Card);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(e => e.EnumName)
            .IsRequired()
            .HasColumnName(nameof(FrontierCard.EnumName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(FrontierCard.Type))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(2);

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(FrontierCard.Text))
            .HasColumnType("text")
            .HasColumnOrder(3);

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(FrontierCard.GameVersion))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(4);
    }
}
