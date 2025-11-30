namespace TwilightImperiumUltimate.DataAccess.Configurations.Relationships;

internal class TiglUserAchievementConfiguration : IEntityTypeConfiguration<TiglUserAchievement>
{
    public void Configure(EntityTypeBuilder<TiglUserAchievement> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(TiglUserAchievement), Schema.Relationships);

        builder.HasKey(tua => new { tua.TiglUserId, tua.AchievementId, tua.Faction });

        builder.Property(e => e.TiglUserId)
            .IsRequired()
            .HasColumnName(nameof(TiglUserAchievement.TiglUserId))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(0);

        builder.Property(e => e.AchievementId)
            .IsRequired()
            .HasColumnName(nameof(TiglUserAchievement.AchievementId))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(1);

        builder.Property(e => e.AchievementName)
            .IsRequired()
            .HasColumnName(nameof(TiglUserAchievement.AchievementName))
            .HasConversion<string>()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)")
            .HasColumnOrder(2);

        builder.Property(x => x.Rank)
            .IsRequired()
            .HasColumnName(nameof(TiglUserAchievement.Rank))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(3);

        builder.Property(x => x.Faction)
            .IsRequired()
            .HasColumnName(nameof(TiglUserAchievement.Faction))
            .HasConversion<string>()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)")
            .HasColumnOrder(4);

        builder.Property(x => x.AchievedAt)
            .IsRequired()
            .HasColumnName(nameof(TiglUserAchievement.AchievedAt))
            .HasConversion<long>()
            .HasColumnType("bigint")
            .HasColumnOrder(5);

        builder.Property(x => x.MatchId)
            .IsRequired()
            .HasColumnName(nameof(TiglUserAchievement.MatchId))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(6);

        builder.Property(x => x.MatchName)
            .IsRequired()
            .HasColumnName(nameof(TiglUserAchievement.MatchName))
            .HasMaxLength(200)
            .HasColumnType("varchar(200)")
            .HasColumnOrder(7);

        builder.HasOne(tua => tua.TiglUser)
            .WithMany(tu => tu.Achievements)
            .HasForeignKey(tua => tua.TiglUserId);

        builder.HasOne(tua => tua.Achievement)
            .WithMany(a => a.Achievements)
            .HasForeignKey(tua => tua.AchievementId);
    }
}
