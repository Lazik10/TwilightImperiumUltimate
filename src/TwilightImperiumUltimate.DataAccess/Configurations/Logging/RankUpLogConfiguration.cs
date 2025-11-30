using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwilightImperiumUltimate.Core.Entities.Logging;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Logging;

public class RankUpLogConfiguration : IEntityTypeConfiguration<RankUpLog>
{
    public void Configure(EntityTypeBuilder<RankUpLog> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.RankUpLogs, Schema.Log);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.League, x.TiglUserDiscordId, x.OldRank, x.NewRank, x.Timestamp, x.Duration })
            .HasDatabaseName("IX_RankUpLogs_League_TiglUserDiscordId_OldRank_NewRank_Timestamp_Duration");

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.Timestamp)
            .IsRequired()
            .HasColumnName(nameof(RankUpLog.Timestamp))
            .HasColumnType("bigint")
            .HasColumnOrder(1);

        builder.Property(x => x.League)
            .IsRequired()
            .HasColumnName(nameof(RankUpLog.League))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(2);

        builder.Property(x => x.OldRank)
            .IsRequired()
            .HasColumnName(nameof(RankUpLog.OldRank))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(3);

        builder.Property(x => x.NewRank)
            .IsRequired()
            .HasColumnName(nameof(RankUpLog.NewRank))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(4);

        builder.Property(x => x.TiglUserId)
            .HasColumnName(nameof(RankUpLog.TiglUserId))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(5);

        builder.Property(x => x.TiglUserDiscordId)
            .HasColumnName(nameof(RankUpLog.TiglUserDiscordId))
            .HasConversion<long>()
            .HasColumnType("bigint")
            .HasColumnOrder(6);

        builder.Property(x => x.TiglUserName)
            .IsRequired()
            .HasColumnName(nameof(RankUpLog.TiglUserName))
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(7);

        builder.Property(x => x.Duration)
            .IsRequired()
            .HasColumnName(nameof(RankUpLog.Duration))
            .HasColumnType("bigint")
            .HasColumnOrder(8);

        builder.Property(x => x.MatchId)
            .HasColumnName(nameof(RankUpLog.MatchId))
            .HasColumnType("int")
            .HasColumnOrder(9);

        builder.Property(x => x.Published)
            .IsRequired()
            .HasColumnName(nameof(RankUpLog.Published))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasColumnOrder(10);
    }
}