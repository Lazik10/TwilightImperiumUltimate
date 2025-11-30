namespace TwilightImperiumUltimate.DataAccess.Configurations.Relationships;

public class TiglUserPrestigueRankConfiguration : IEntityTypeConfiguration<TiglUserPrestigeRank>
{
    public void Configure(EntityTypeBuilder<TiglUserPrestigeRank> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(TiglUserPrestigeRank), Schema.Relationships);

        builder.HasKey(x => x.Id);

        builder.HasIndex(tupr => new { tupr.TiglUserId, tupr.PrestigeRankId, })
            .HasDatabaseName("IX_TiglUserPrestigeRank_TiglUserId_PrestigeRankId");

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(e => e.TiglUserId)
            .IsRequired()
            .HasColumnName(nameof(TiglUserPrestigeRank.TiglUserId))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(1);

        builder.Property(e => e.PrestigeRankId)
            .IsRequired()
            .HasColumnName(nameof(TiglUserPrestigeRank.PrestigeRankId))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(2);

        builder.Property(x => x.Rank)
            .IsRequired()
            .HasColumnName(nameof(TiglUserPrestigeRank.Rank))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(3);

        builder.Property(x => x.AchievedAt)
            .IsRequired()
            .HasColumnName(nameof(TiglUserPrestigeRank.AchievedAt))
            .HasConversion<long>()
            .HasColumnType("bigint")
            .HasColumnOrder(4);

        builder.HasOne(tupr => tupr.TiglUser)
            .WithMany(tu => tu.PrestigeRanks)
            .HasForeignKey(tupr => tupr.TiglUserId);

        builder.HasOne(tupr => tupr.PrestigeRank)
            .WithMany(a => a.PrestigeRanks)
            .HasForeignKey(tupr => tupr.PrestigeRankId);
    }
}
