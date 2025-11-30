using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;
using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class TrueSkillStatsConfiguration : IEntityTypeConfiguration<TrueSkillStats>
{
    public void Configure(EntityTypeBuilder<TrueSkillStats> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.TrueSkillStats, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.HasIndex(e => new { e.TiglUserId, e.League })
            .HasDatabaseName("IX_TrueSkillStats_TiglUserId_League");

        builder.Property(x => x.TiglUserId)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillStats.TiglUserId))
            .HasColumnType("integer")
            .HasColumnOrder(1);

        builder.Property(e => e.TrueSkillRatingId)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillStats.TrueSkillRatingId))
            .HasColumnType("integer")
            .HasColumnOrder(2);

        builder.Property(x => x.League)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillStats.League))
            .HasConversion<string>()
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasColumnOrder(3);

        builder.HasOne(a => a.TiglUser)
            .WithMany(u => u.TrueSkillStats)
            .HasForeignKey(p => p.TiglUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.TrueSkillRating)
            .WithOne(u => u.TrueSkillStats)
            .HasForeignKey<TrueSkillRating>(a => a.TrueSkillStatsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
