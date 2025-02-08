using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Async;

public class PlayerStatsConfiguration : IEntityTypeConfiguration<PlayerStats>
{
    public void Configure(EntityTypeBuilder<PlayerStats> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.AsyncPlayerStats, Schema.Statistics);

        builder.HasKey(p => p.Id);

        builder.HasIndex(x => x.DiscordUserID)
            .IsClustered(false);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(e => e.DiscordUserID)
            .IsRequired()
            .HasColumnName(nameof(PlayerStats.DiscordUserID))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.Property(e => e.FactionName)
            .IsRequired()
            .HasColumnName(nameof(PlayerStats.FactionName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(2);

        builder.Property(x => x.Score)
            .IsRequired()
            .HasColumnName(nameof(PlayerStats.Score))
            .HasColumnType("integer")
            .HasColumnOrder(3);

        builder.Property(e => e.Color)
            .IsRequired()
            .HasColumnName(nameof(PlayerStats.Color))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(4);

        builder.Property(x => x.TotalNumberOfTurns)
            .IsRequired()
            .HasColumnName(nameof(PlayerStats.TotalNumberOfTurns))
            .HasColumnType("integer")
            .HasColumnOrder(5);

        builder.Property(x => x.TotalTurnTime)
            .IsRequired()
            .HasColumnName(nameof(PlayerStats.TotalTurnTime))
            .HasColumnOrder(6);

        builder.Property(x => x.ExpectedHits)
            .IsRequired()
            .HasColumnName(nameof(PlayerStats.ExpectedHits))
            .HasColumnOrder(7);

        builder.Property(x => x.ActualHits)
            .IsRequired()
            .HasColumnName(nameof(PlayerStats.ActualHits))
            .HasColumnOrder(8);

        builder.Property(x => x.Winner)
            .IsRequired()
            .HasColumnName(nameof(PlayerStats.Winner))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(9);

        builder.Property(x => x.GameStatsId)
            .IsRequired()
            .HasColumnName(nameof(PlayerStats.GameStatsId))
            .HasColumnType("integer")
            .HasColumnOrder(10);

        builder.HasOne(p => p.GameStatistics)
            .WithMany(g => g.PlayerStatistics)
            .HasForeignKey(p => p.GameStatsId);
    }
}