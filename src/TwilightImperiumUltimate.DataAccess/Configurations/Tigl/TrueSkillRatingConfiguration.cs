using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class TrueSkillRatingConfiguration : IEntityTypeConfiguration<TrueSkillRating>
{
    public void Configure(EntityTypeBuilder<TrueSkillRating> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.TrueSkillRatings, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.TrueSkillStatsId)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillRating.TrueSkillStatsId))
            .HasColumnType("int")
            .HasColumnOrder(1);

        builder.Property(x => x.Mu)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillRating.Mu))
            .HasColumnType("float")
            .HasColumnOrder(2);

        builder.Property(x => x.Sigma)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillRating.Sigma))
            .HasColumnType("float")
            .HasColumnOrder(3);

        builder.HasOne(x => x.TrueSkillStats)
            .WithOne(x => x.TrueSkillRating)
            .HasForeignKey<TrueSkillRating>(x => x.TrueSkillStatsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
