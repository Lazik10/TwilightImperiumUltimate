namespace TwilightImperiumUltimate.DataAccess.Configurations.Units;

internal sealed class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Units, Schema.Unit);

        builder.HasKey(e => e.UnitName);

        builder.Property(x => x.Id)
            .IsRequired()
            .UseIdentityColumn()
            .HasColumnOrder(0);

        builder.Property(e => e.UnitName)
            .IsRequired()
            .HasColumnName(nameof(Unit.UnitName))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(1);

        builder.Property(e => e.UnitType)
            .IsRequired()
            .HasColumnName(nameof(Unit.UnitType))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(2);

        builder.Property(e => e.Cost)
            .IsRequired()
            .HasColumnName(nameof(Unit.Cost))
            .HasColumnType("integer")
            .HasColumnOrder(3);

        builder.Property(e => e.Combat)
            .IsRequired()
            .HasColumnName(nameof(Unit.Combat))
            .HasColumnType("integer")
            .HasColumnOrder(4);

        builder.Property(e => e.Move)
            .IsRequired()
            .HasColumnName(nameof(Unit.Move))
            .HasColumnType("integer")
            .HasColumnOrder(5);

        builder.Property(e => e.Capacity)
            .IsRequired()
            .HasColumnName(nameof(Unit.Capacity))
            .HasColumnType("integer")
            .HasColumnOrder(6);
    }
}
