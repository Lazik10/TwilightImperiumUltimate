namespace TwilightImperiumUltimate.DataAccess.Configurations.Relationships;

public class AsyncPlayerProfileGameStatsConfiguration : IEntityTypeConfiguration<AsyncPlayerProfileGameStats>
{
    public void Configure(EntityTypeBuilder<AsyncPlayerProfileGameStats> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(AsyncPlayerProfileGameStats), Schema.Relationships);

        builder.HasKey(pg => new { pg.AsyncPlayerProfileId, pg.GameStatsId });

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(e => e.AsyncPlayerProfileId)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerProfileGameStats.AsyncPlayerProfileId))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(1);

        builder.Property(e => e.GameStatsId)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerProfileGameStats.GameStatsId))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(2);

        builder.HasOne(pg => pg.AsyncPlayerProfile)
            .WithMany(p => p.GameStatistics)
            .HasForeignKey(pg => pg.AsyncPlayerProfileId);

        builder.HasOne(pg => pg.GameStats)
            .WithMany(g => g.GameStatistics)
            .HasForeignKey(pg => pg.GameStatsId);
    }
}