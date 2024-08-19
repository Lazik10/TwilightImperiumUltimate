using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Technologies;

internal sealed class TechnologyConfiguration : IEntityTypeConfiguration<Technology>
{
    public void Configure(EntityTypeBuilder<Technology> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Technologies, Schema.Technology);

        builder.HasKey(e => e.TechnologyName);

        builder.Property(x => x.Id)
            .IsRequired()
            .UseIdentityColumn()
            .HasColumnOrder(0);

        builder.Property(e => e.TechnologyName)
            .IsRequired()
            .HasColumnName(nameof(Technology.TechnologyName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(Technology.Type))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(2);

        builder.Property(e => e.Level)
            .IsRequired()
            .HasColumnName(nameof(Technology.Level))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(3);

        builder.Property(e => e.FactionName)
            .IsRequired()
            .HasColumnName(nameof(Technology.FactionName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(4);

        builder.Property(e => e.IsFactionTechnology)
            .IsRequired()
            .HasColumnName(nameof(Technology.IsFactionTechnology))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(5);

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(Technology.Text))
            .HasColumnType("nvarchar(MAX)")
            .HasColumnOrder(6);

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(Technology.GameVersion))
            .HasConversion<string>()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnOrder(7);
    }
}
