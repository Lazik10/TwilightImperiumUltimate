using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class AsyncPlayerMatchStatsConfiguration : IEntityTypeConfiguration<AsyncPlayerMatchStats>
{
    public void Configure(EntityTypeBuilder<AsyncPlayerMatchStats> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.AsyncPlayerMatchStats, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.HasIndex(x => x.DiscordUserId)
            .HasDatabaseName("IX_AsyncPlayerMatchStats_DiscordUserId");

        builder.HasIndex(x => x.MatchId)
            .HasDatabaseName("IX_AsyncPlayerMatchStats_MatchId");

        builder.HasIndex(x => x.Faction)
            .HasDatabaseName("IX_AsyncPlayerMatchStats_Faction");

        builder.HasIndex(x => x.Season)
            .HasDatabaseName("IX_AsyncPlayerMatchStats_Season");

        builder.HasIndex(x => x.IsRankUpGame)
            .HasDatabaseName("IX_AsyncPlayerMatchStats_IsRankUpGame");

        builder.HasIndex(x => x.Winner)
            .HasDatabaseName("IX_AsyncPlayerMatchStats_Winner");

        builder.HasIndex(x => x.StartTimestamp)
            .HasDatabaseName("IX_AsyncPlayerMatchStats_StartTimestamp");

        builder.Property(x => x.AsyncStatsId)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.AsyncStatsId))
            .HasColumnType("int")
            .HasColumnOrder(1);

        builder.Property(x => x.MatchId)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.MatchId))
            .HasColumnType("int")
            .HasColumnOrder(2);

        builder.Property(x => x.DiscordUserId)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.DiscordUserId))
            .HasColumnType("bigint")
            .HasColumnOrder(3);

        builder.Property(x => x.Winner)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.Winner))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasColumnOrder(4);

        builder.Property(x => x.Score)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.Score))
            .HasColumnType("int")
            .HasColumnOrder(5);

        builder.Property(x => x.Placement)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.Placement))
            .HasColumnType("int")
            .HasColumnOrder(6);

        builder.Property(x => x.ExpectedWinPercentage)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.ExpectedWinPercentage))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(7);

        builder.Property(x => x.RatingOld)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.RatingOld))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(8);

        builder.Property(x => x.RatingNew)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.RatingNew))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(9);

        builder.Property(x => x.AussieScoreOld)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.AussieScoreOld))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(10);

        builder.Property(x => x.AussieScoreNew)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.AussieScoreNew))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(11);

        builder.Property(x => x.OpponentAvgRating)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.OpponentAvgRating))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(12);

        builder.Property(x => x.Season)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.Season))
            .HasColumnType("int")
            .HasColumnOrder(13);

        builder.Property(X => X.Faction)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.Faction))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(14);

        builder.Property(x => x.StartTimestamp)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.StartTimestamp))
            .HasColumnType("bigint")
            .HasColumnOrder(15);

        builder.Property(x => x.EndTimestamp)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.EndTimestamp))
            .HasColumnType("bigint")
            .HasColumnOrder(16);

        builder.Property(x => x.IsRankUpGame)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.IsRankUpGame))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasMaxLength(10)
            .HasColumnOrder(17);

        builder.Property(x => x.OldRank)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.OldRank))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(18);

        builder.Property(x => x.NewRank)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.NewRank))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(19);

        builder.Property(x => x.ForcedReset)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerMatchStats.ForcedReset))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasMaxLength(10)
            .HasColumnOrder(20);

        builder.HasOne(x => x.AsyncStats)
            .WithMany(x => x.MatchStats)
            .HasForeignKey(x => x.AsyncStatsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
