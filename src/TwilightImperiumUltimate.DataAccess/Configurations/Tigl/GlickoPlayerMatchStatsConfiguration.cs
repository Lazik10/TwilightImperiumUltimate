using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class GlickoPlayerMatchStatsConfiguration : IEntityTypeConfiguration<GlickoPlayerMatchStats>
{
    public void Configure(EntityTypeBuilder<GlickoPlayerMatchStats> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.GlickoPlayerMatchStats, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.HasIndex(x => x.DiscordUserId)
            .HasDatabaseName("IX_GlickoPlayerMatchStats_DiscordUserId");

        builder.HasIndex(x => x.MatchId)
            .HasDatabaseName("IX_GlickoPlayerMatchStats_MatchId");

        builder.HasIndex(x => x.Faction)
            .HasDatabaseName("IX_GlickoPlayerMatchStats_Faction");

        builder.HasIndex(x => x.Season)
            .HasDatabaseName("IX_GlickoPlayerMatchStats_Season");

        builder.HasIndex(x => x.IsRankUpGame)
            .HasDatabaseName("IX_GlickoPlayerMatchStats_IsRankUpGame");

        builder.HasIndex(x => x.Winner)
            .HasDatabaseName("IX_GlickoPlayerMatchStats_Winner");

        builder.HasIndex(x => x.StartTimestamp)
            .HasDatabaseName("IX_GlickoPlayerMatchStats_StartTimestamp");

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.GlickoStatsId)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.GlickoStatsId))
            .HasColumnType("int")
            .HasColumnOrder(1);

        builder.Property(x => x.MatchId)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.MatchId))
            .HasColumnType("int")
            .HasColumnOrder(2);

        builder.Property(x => x.DiscordUserId)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.DiscordUserId))
            .HasColumnType("bigint")
            .HasColumnOrder(3);

        builder.Property(x => x.Winner)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.Winner))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasColumnOrder(4);

        builder.Property(x => x.Score)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.Score))
            .HasColumnType("int")
            .HasColumnOrder(5);

        builder.Property(x => x.Placement)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.Placement))
            .HasColumnType("int")
            .HasColumnOrder(6);

        builder.Property(x => x.RatingOld)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.RatingOld))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(7);

        builder.Property(x => x.RatingNew)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.RatingNew))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(8);

        builder.Property(x => x.RdOld)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.RdOld))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(9);

        builder.Property(x => x.RdNew)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.RdNew))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(10);

        builder.Property(x => x.VolatilityOld)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.VolatilityOld))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(11);

        builder.Property(x => x.VolatilityNew)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.VolatilityNew))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(12);

        builder.Property(x => x.OpponentAvgRating)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.OpponentAvgRating))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(13);

        builder.Property(x => x.Season)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.Season))
            .HasColumnType("int")
            .HasColumnOrder(14);

        builder.Property(x => x.Faction)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.Faction))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(15);

        builder.Property(x => x.StartTimestamp)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.StartTimestamp))
            .HasColumnType("bigint")
            .HasColumnOrder(16);

        builder.Property(x => x.EndTimestamp)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.EndTimestamp))
            .HasColumnType("bigint")
            .HasColumnOrder(17);

        builder.Property(x => x.IsRankUpGame)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.IsRankUpGame))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasMaxLength(10)
            .HasColumnOrder(18);

        builder.Property(x => x.OldRank)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.OldRank))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(19);

        builder.Property(x => x.NewRank)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.NewRank))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(20);

        builder.Property(x => x.ForcedReset)
            .IsRequired()
            .HasColumnName(nameof(GlickoPlayerMatchStats.ForcedReset))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasMaxLength(10)
            .HasColumnOrder(21);

        builder.HasOne(x => x.GlickoStats)
            .WithMany(x => x.MatchStats)
            .HasForeignKey(x => x.GlickoStatsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
