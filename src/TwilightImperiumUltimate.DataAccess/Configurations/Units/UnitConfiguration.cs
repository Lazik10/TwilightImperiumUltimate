using TwilightImperiumUltimate.Core.Entities.Units;
using TwilightImperiumUltimate.DataAccess.Tables.Units;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Units;

internal class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Units, Schema.Unit);

        builder.HasKey(e => e.UnitName);

        builder.Property(e => e.UnitName)
            .IsRequired()
            .HasColumnName(nameof(Unit.UnitName))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.UnitType)
            .IsRequired()
            .HasColumnName(nameof(Unit.UnitType))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Cost)
            .IsRequired()
            .HasColumnName(nameof(Unit.Cost))
            .HasColumnType("integer");

        builder.Property(e => e.Combat)
            .IsRequired()
            .HasColumnName(nameof(Unit.Combat))
            .HasColumnType("integer");

        builder.Property(e => e.Move)
            .IsRequired()
            .HasColumnName(nameof(Unit.Move))
            .HasColumnType("integer");

        builder.Property(e => e.Capacity)
            .IsRequired()
            .HasColumnName(nameof(Unit.Capacity))
            .HasColumnType("integer");
    }
}
