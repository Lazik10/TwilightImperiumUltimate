using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class AsyncRatingConfiguration : IEntityTypeConfiguration<AsyncRating>
{
    public void Configure(EntityTypeBuilder<AsyncRating> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.AsyncRatings, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.AsyncStatsId)
            .IsUnique()
            .HasDatabaseName("IX_AsyncRatings_AsyncStatsId");

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.AsyncStatsId)
            .IsRequired()
            .HasColumnName(nameof(AsyncRating.AsyncStatsId))
            .HasColumnType("int")
            .HasColumnOrder(1);

        builder.Property(x => x.Rating)
            .IsRequired()
            .HasColumnName(nameof(AsyncRating.Rating))
            .HasColumnType("float")
            .HasColumnOrder(2);

        builder.Property(x => x.AussieScore)
            .IsRequired()
            .HasColumnName(nameof(AsyncRating.AussieScore))
            .HasColumnType("float")
            .HasColumnOrder(3);

        builder.HasOne(a => a.AsyncStats)
            .WithOne(u => u.Rating)
            .HasForeignKey<AsyncRating>(a => a.AsyncStatsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
