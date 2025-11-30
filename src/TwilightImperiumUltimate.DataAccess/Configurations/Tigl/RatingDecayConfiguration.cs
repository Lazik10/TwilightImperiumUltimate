using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class RatingDecayConfiguration : IEntityTypeConfiguration<RatingDecay>
{
    public void Configure(EntityTypeBuilder<RatingDecay> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.RatingDecays, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnName(nameof(RatingDecay.Amount))
            .HasColumnType("float")
            .HasColumnOrder(1);

        builder.Property(x => x.TiglUserId)
            .IsRequired()
            .HasColumnName(nameof(RatingDecay.TiglUserId))
            .HasColumnType("int")
            .HasColumnOrder(2);

        builder.Property(x => x.Timestamp)
            .IsRequired()
            .HasColumnName(nameof(RatingDecay.Timestamp))
            .HasColumnType("bigint")
            .HasColumnOrder(3);

        builder.Property(x => x.League)
            .IsRequired()
            .HasColumnName(nameof(RatingDecay.League))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(4);

        builder.Property(x => x.Season)
            .IsRequired()
            .HasColumnName(nameof(RatingDecay.Season))
            .HasColumnType("int")
            .HasColumnOrder(5);

        builder.Property(x => x.RatingSystem)
            .IsRequired()
            .HasColumnName(nameof(RatingDecay.RatingSystem))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(6);
    }
}
