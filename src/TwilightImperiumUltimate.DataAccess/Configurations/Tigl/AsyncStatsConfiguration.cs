using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;
using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class AsyncStatsConfiguration : IEntityTypeConfiguration<AsyncStats>
{
    public void Configure(EntityTypeBuilder<AsyncStats> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.AsyncStats, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.TiglUserId)
            .IsRequired()
            .HasColumnName(nameof(AsyncStats.TiglUserId))
            .HasColumnType("integer")
            .HasColumnOrder(1);

        builder.Property(e => e.AsyncRatingId)
            .IsRequired()
            .HasColumnName(nameof(AsyncStats.AsyncRatingId))
            .HasColumnType("integer")
            .HasColumnOrder(2);

        builder.Property(e => e.League)
            .IsRequired()
            .HasColumnName(nameof(AsyncStats.League))
            .HasConversion<string>()
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasColumnOrder(3);

        builder.Property(e => e.Rank)
            .IsRequired()
            .HasColumnName(nameof(AsyncStats.Rank))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(4);

        builder.HasOne(a => a.TiglUser)
            .WithMany(u => u.AsyncStats)
            .HasForeignKey(p => p.TiglUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Rating)
            .WithOne(u => u.AsyncStats)
            .HasForeignKey<AsyncRating>(a => a.AsyncStatsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
