using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Async;

public class GameStatsConfiguration : IEntityTypeConfiguration<GameStats>
{
    public void Configure(EntityTypeBuilder<GameStats> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.AsyncGameStats, Schema.Statistics);

        builder.HasKey(g => g.Id);

        builder.HasIndex(x => x.AsyncGameID)
            .IsClustered(false);

        builder.HasIndex(x => x.AsyncFunGameName);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(e => e.AsyncGameID)
            .IsRequired()
            .HasColumnName(nameof(GameStats.AsyncGameID))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(1);

        builder.Property(e => e.AsyncFunGameName)
            .IsRequired()
            .HasColumnName(nameof(GameStats.AsyncFunGameName))
            .HasConversion<string>()
            .HasColumnType("varchar(200)")
            .HasMaxLength(200)
            .HasColumnOrder(2);

        builder.Property(e => e.Platform)
            .IsRequired()
            .HasColumnName(nameof(GameStats.Platform))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(3);

        builder.Property(e => e.Timestamp)
            .IsRequired()
            .HasColumnName(nameof(GameStats.Timestamp))
            .HasColumnOrder(4);

        builder.Property(e => e.SetupTimestamp)
            .IsRequired()
            .HasColumnName(nameof(GameStats.SetupTimestamp))
            .HasColumnOrder(5);

        builder.Property(e => e.EndedTimestamp)
            .HasColumnName(nameof(GameStats.EndedTimestamp))
            .HasColumnOrder(6);

        builder.Property(e => e.HasWinner)
            .IsRequired()
            .HasColumnName(nameof(GameStats.HasWinner))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnOrder(7);

        builder.Property(x => x.NumberOfPlayers)
            .IsRequired()
            .HasColumnName(nameof(GameStats.NumberOfPlayers))
            .HasColumnType("integer")
            .HasColumnOrder(8);

        builder.Property(x => x.Round)
            .IsRequired()
            .HasColumnName(nameof(GameStats.Round))
            .HasColumnType("integer")
            .HasColumnOrder(9);

        builder.Property(x => x.Turn)
            .IsRequired(false)
            .HasColumnName(nameof(GameStats.Turn))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(10);

        builder.Property(x => x.Scoreboard)
            .IsRequired()
            .HasColumnName(nameof(GameStats.Scoreboard))
            .HasColumnType("integer")
            .HasColumnOrder(11);

        builder.Property(e => e.MapString)
            .IsRequired()
            .HasColumnName(nameof(GameStats.MapString))
            .HasConversion<string>()
            .HasColumnType("varchar(MAX)")
            .HasColumnOrder(12);

        builder.Property(e => e.AbsolMode)
            .IsRequired()
            .HasColumnName(nameof(GameStats.AbsolMode))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(13);

        builder.Property(e => e.DiscordantStarsMode)
            .IsRequired()
            .HasColumnName(nameof(GameStats.DiscordantStarsMode))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(14);

        builder.Property(e => e.FrankenGame)
            .IsRequired()
            .HasColumnName(nameof(GameStats.FrankenGame))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(15);

        builder.Property(e => e.Homebrew)
            .IsRequired()
            .HasColumnName(nameof(GameStats.Homebrew))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(16);

        builder.Property(e => e.IsPoK)
            .IsRequired()
            .HasColumnName(nameof(GameStats.IsPoK))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(17);

        builder.HasMany(g => g.GameStatistics)
            .WithOne(gs => gs.GameStats)
            .HasForeignKey(gs => gs.GameStatsId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.PlayerStatistics)
            .WithOne(ps => ps.GameStatistics)
            .HasForeignKey(ps => ps.GameStatsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}