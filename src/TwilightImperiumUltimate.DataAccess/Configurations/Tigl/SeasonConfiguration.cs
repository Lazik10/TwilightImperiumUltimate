using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

internal class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Seasons, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnName(nameof(Season.Id))
            .HasColumnType("integer")
            .HasColumnOrder(0);

        builder.Property(x => x.SeasonNumber)
            .IsRequired()
            .HasColumnName(nameof(Season.SeasonNumber))
            .HasColumnType("integer")
            .HasColumnOrder(1);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName(nameof(Season.Name))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(2);

        builder.Property(x => x.StartDate)
            .IsRequired()
            .HasColumnName(nameof(Season.StartDate))
            .HasColumnType("date")
            .HasColumnOrder(3);

        builder.Property(x => x.EndDate)
            .IsRequired()
            .HasColumnName(nameof(Season.EndDate))
            .HasColumnType("date")
            .HasColumnOrder(4);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasColumnName(nameof(Season.IsActive))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasMaxLength(10)
            .HasColumnOrder(5);
    }
}
