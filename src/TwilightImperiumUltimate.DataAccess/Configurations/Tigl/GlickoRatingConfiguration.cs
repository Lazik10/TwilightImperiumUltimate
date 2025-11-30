using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class GlickoRatingConfiguration : IEntityTypeConfiguration<GlickoRating>
{
    public void Configure(EntityTypeBuilder<GlickoRating> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.GlickoRatings, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.HasIndex(e => e.GlickoStatsId)
            .IsUnique()
            .HasDatabaseName("IX_GlickoRatings_GlickoStatsId");

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.GlickoStatsId)
            .IsRequired()
            .HasColumnName(nameof(GlickoRating.GlickoStatsId))
            .HasColumnType("int")
            .HasColumnOrder(1);

        builder.Property(x => x.Rating)
            .IsRequired()
            .HasColumnName(nameof(GlickoRating.Rating))
            .HasColumnType("float")
            .HasColumnOrder(2);

        builder.Property(x => x.Volatility)
            .IsRequired()
            .HasColumnName(nameof(GlickoRating.Volatility))
            .HasColumnType("float")
            .HasColumnOrder(3);

        builder.Property(x => x.Rd)
            .IsRequired()
            .HasColumnName(nameof(GlickoRating.Rd))
            .HasColumnType("float")
            .HasColumnOrder(4);

        builder.HasOne(a => a.GlickoStats)
            .WithOne(u => u.Rating)
            .HasForeignKey<GlickoRating>(a => a.GlickoStatsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
