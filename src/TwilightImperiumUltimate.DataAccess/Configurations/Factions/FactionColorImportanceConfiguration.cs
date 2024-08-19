namespace TwilightImperiumUltimate.DataAccess.Configurations.Factions;

internal sealed class FactionColorImportanceConfiguration : IEntityTypeConfiguration<FactionColorImportance>
{
    public void Configure(EntityTypeBuilder<FactionColorImportance> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.FactionColorImportance, Schema.Faction);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnOrder(0);

        builder.Property(e => e.FactionName)
            .IsRequired()
            .HasColumnName(nameof(FactionColorImportance.FactionName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.Color)
            .IsRequired()
            .HasColumnName(nameof(FactionColorImportance.Color))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(2);
    }
}
