using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class TrueSkillPlayerMatchStatsConfiguration : IEntityTypeConfiguration<TrueSkillPlayerMatchStats>
{
    public void Configure(EntityTypeBuilder<TrueSkillPlayerMatchStats> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.TrueSkillPlayerMatchStats, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.HasIndex(x => x.DiscordUserId)
            .HasDatabaseName("IX_TrueSkillPlayerMatchStats_DiscordUserId");

        builder.HasIndex(x => x.MatchId)
            .HasDatabaseName("IX_TrueSkillPlayerMatchStats_MatchId");

        builder.HasIndex(x => x.Faction)
            .HasDatabaseName("IX_TrueSkillPlayerMatchStats_Faction");

        builder.HasIndex(x => x.Season)
            .HasDatabaseName("IX_TrueSkillPlayerMatchStats_Season");

        builder.HasIndex(x => x.IsRankUpGame)
            .HasDatabaseName("IX_TrueSkillPlayerMatchStats_IsRankUpGame");

        builder.HasIndex(x => x.Winner)
            .HasDatabaseName("IX_TrueSkillPlayerMatchStats_Winner");

        builder.HasIndex(x => x.StartTimestamp)
            .HasDatabaseName("IX_TrueSkillPlayerMatchStats_StartTimestamp");

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.TrueSkillStatsId)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.TrueSkillStatsId))
            .HasColumnType("int")
            .HasColumnOrder(1);

        builder.Property(x => x.MatchId)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.MatchId))
            .HasColumnType("int")
            .HasColumnOrder(2);

        builder.Property(x => x.DiscordUserId)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.DiscordUserId))
            .HasColumnType("bigint")
            .HasColumnOrder(3);

        builder.Property(x => x.Winner)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.Winner))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasColumnOrder(4);

        builder.Property(x => x.Score)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.Score))
            .HasColumnType("int")
            .HasColumnOrder(5);

        builder.Property(x => x.Placement)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.Placement))
            .HasColumnType("int")
            .HasColumnOrder(6);

        builder.Property(x => x.MuOld)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.MuOld))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(7);

        builder.Property(x => x.MuNew)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.MuNew))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(8);

        builder.Property(x => x.SigmaOld)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.SigmaOld))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(9);

        builder.Property(x => x.SigmaNew)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.SigmaNew))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(10);

        builder.Property(x => x.OpponentAvgRating)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.OpponentAvgRating))
            .HasColumnType("decimal(18,10)")
            .HasColumnOrder(11);

        builder.Property(x => x.Season)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.Season))
            .HasColumnType("int")
            .HasColumnOrder(12);

        builder.Property(x => x.Faction)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.Faction))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(13);

        builder.Property(x => x.StartTimestamp)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.StartTimestamp))
            .HasColumnType("bigint")
            .HasColumnOrder(14);

        builder.Property(x => x.EndTimestamp)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.EndTimestamp))
            .HasColumnType("bigint")
            .HasColumnOrder(15);

        builder.Property(x => x.IsRankUpGame)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.IsRankUpGame))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasMaxLength(10)
            .HasColumnOrder(16);

        builder.Property(x => x.OldRank)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.OldRank))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(17);

        builder.Property(x => x.NewRank)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.NewRank))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(18);

        builder.Property(x => x.ForcedReset)
            .IsRequired()
            .HasColumnName(nameof(TrueSkillPlayerMatchStats.ForcedReset))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasMaxLength(10)
            .HasColumnOrder(19);

        builder.HasOne(x => x.TrueSkillStats)
            .WithMany(x => x.MatchStats)
            .HasForeignKey(x => x.TrueSkillStatsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
