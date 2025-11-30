using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwilightImperiumUltimate.Core.Entities.Logging;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Logging;

public class GamePublishLogConfiguration : IEntityTypeConfiguration<GamePublishLog>
{
    public void Configure(EntityTypeBuilder<GamePublishLog> builder)
    {
        builder.ToTable(TableName.GamePublishLogs, Schema.Log);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.Published, x.CreatedAt })
            .HasDatabaseName("IX_GamePublish_Published_CreatedAt");

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.MatchId)
            .IsRequired()
            .HasColumnName(nameof(GamePublishLog.MatchId))
            .HasColumnType("int")
            .HasColumnOrder(1);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnName(nameof(GamePublishLog.CreatedAt))
            .HasColumnType("bigint")
            .HasColumnOrder(2);

        builder.Property(x => x.RankUpPublish)
            .IsRequired()
            .HasColumnName(nameof(GamePublishLog.RankUpPublish))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasColumnOrder(3);

        builder.Property(x => x.PrestigePublish)
            .IsRequired()
            .HasColumnName(nameof(GamePublishLog.PrestigePublish))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasColumnOrder(4);

        builder.Property(x => x.LeaderPublish)
            .IsRequired()
            .HasColumnName(nameof(GamePublishLog.LeaderPublish))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasColumnOrder(5);

        builder.Property(x => x.AchievementPublish)
            .IsRequired()
            .HasColumnName(nameof(GamePublishLog.AchievementPublish))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasColumnOrder(6);

        builder.Property(x => x.AchievementsEvaluated)
            .IsRequired()
            .HasColumnName(nameof(GamePublishLog.AchievementsEvaluated))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasColumnOrder(7);

        builder.Property(x => x.PublishingInProgress)
            .IsRequired()
            .HasColumnName(nameof(GamePublishLog.PublishingInProgress))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasColumnOrder(8);

        builder.Property(x => x.Published)
            .IsRequired()
            .HasColumnName(nameof(GamePublishLog.Published))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasColumnOrder(9);
    }
}
