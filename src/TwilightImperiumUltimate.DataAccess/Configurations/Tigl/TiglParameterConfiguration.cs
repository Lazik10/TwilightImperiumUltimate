using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class TiglParameterConfiguration : IEntityTypeConfiguration<TiglParameter>
{
    public void Configure(EntityTypeBuilder<TiglParameter> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Parameters, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnName(nameof(TiglParameter.Id))
            .HasColumnType("int")
            .HasColumnOrder(1);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName(nameof(TiglParameter.Name))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(2);

        builder.Property(x => x.Enabled)
            .IsRequired()
            .HasColumnName(nameof(TiglParameter.Enabled))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasMaxLength(10)
            .HasColumnOrder(3);
    }
}
