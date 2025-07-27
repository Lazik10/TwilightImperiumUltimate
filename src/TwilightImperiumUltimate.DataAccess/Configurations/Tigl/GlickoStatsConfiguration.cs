using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;
using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class GlickoStatsConfiguration : IEntityTypeConfiguration<GlickoStats>
{
    public void Configure(EntityTypeBuilder<GlickoStats> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.GlickoStats, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.TiglUserId)
            .IsRequired()
            .HasColumnName(nameof(GlickoStats.TiglUserId))
            .HasColumnType("integer")
            .HasColumnOrder(1);

        builder.Property(e => e.GlickoRatingId)
            .IsRequired()
            .HasColumnName(nameof(GlickoStats.GlickoRatingId))
            .HasColumnType("integer")
            .HasColumnOrder(2);

        builder.Property(e => e.League)
            .IsRequired()
            .HasColumnName(nameof(GlickoStats.League))
            .HasConversion<string>()
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasColumnOrder(3);

        builder.Property(x => x.Rank)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillStats.Rank))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasColumnOrder(4);

        builder.HasOne(a => a.TiglUser)
            .WithMany(u => u.GlickoStats)
            .HasForeignKey(p => p.TiglUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Rating)
            .WithOne(u => u.GlickoStats)
            .HasForeignKey<GlickoStats>(a => a.GlickoRatingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
