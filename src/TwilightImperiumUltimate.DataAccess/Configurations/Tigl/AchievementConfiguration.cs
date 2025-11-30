using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
{
    public void Configure(EntityTypeBuilder<Achievement> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Achievements, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.HasIndex(e => new { e.Faction, e.Category })
            .HasDatabaseName("IX_Achievements_Faction_Category");

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName(nameof(Achievement.Name))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(1);

        builder.Property(x => x.Faction)
            .IsRequired()
            .HasColumnName(nameof(Achievement.Faction))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(2);

        builder.Property(x => x.Category)
            .IsRequired()
            .HasColumnName(nameof(Achievement.Category))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(3);
    }
}
