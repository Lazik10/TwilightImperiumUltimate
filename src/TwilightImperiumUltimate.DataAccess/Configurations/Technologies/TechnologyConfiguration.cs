using TwilightImperiumUltimate.DataAccess.RelationshipEntities;
using TwilightImperiumUltimate.DataAccess.Tables.Technologies;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Technologies;

public class TechnologyConfiguration : IEntityTypeConfiguration<Technology>
{
    public void Configure(EntityTypeBuilder<Technology> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Technologies, Schema.Technology);

        builder.HasKey(e => e.TechnologyName);

        builder.Property(e => e.TechnologyName)
            .IsRequired()
            .HasColumnName(nameof(Technology.TechnologyName))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Type)
            .IsRequired()
            .HasColumnName(nameof(Technology.Type))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Level)
            .IsRequired()
            .HasColumnName(nameof(Technology.Level))
            .HasConversion<int>()
            .HasColumnType("integer");

        builder.Property(e => e.Text)
            .IsRequired()
            .HasColumnName(nameof(Technology.Text))
            .HasColumnType("nvarchar(50)");

        builder.Property(e => e.GameVersion)
            .IsRequired()
            .HasColumnName(nameof(Technology.GameVersion))
            .HasConversion<int>()
            .HasColumnType("integer");
    }
}
