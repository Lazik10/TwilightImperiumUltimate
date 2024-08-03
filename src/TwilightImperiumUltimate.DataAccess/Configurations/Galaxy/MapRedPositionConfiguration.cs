namespace TwilightImperiumUltimate.DataAccess.Configurations.Galaxy;

internal class MapRedPositionConfiguration : IEntityTypeConfiguration<MapRedPosition>
{
    public void Configure(EntityTypeBuilder<MapRedPosition> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.MapRedPositions, Schema.Galaxy);

        builder.HasKey(e => e.Id)
            .IsClustered();

        builder.Property(e => e.Id)
            .IsRequired()
            .UseIdentityColumn()
            .HasColumnType("integer")
            .HasColumnOrder(0);

        builder.Property(e => e.MapTemplate)
            .IsRequired()
            .HasColumnName(nameof(MapRedPosition.MapTemplate))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.Count)
            .IsRequired()
            .HasColumnName(nameof(MapRedPosition.Count))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(2);

        builder.Property(e => e.Positions)
            .IsRequired()
            .HasColumnName(nameof(MapRedPosition.Positions))
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(3);
    }
}
