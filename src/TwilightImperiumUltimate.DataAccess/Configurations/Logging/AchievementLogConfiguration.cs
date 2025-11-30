using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwilightImperiumUltimate.Core.Entities.Logging;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Logging;

internal class AchievementLogConfiguration : IEntityTypeConfiguration<AchievementLog>
{
    public void Configure(EntityTypeBuilder<AchievementLog> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.AchievementLogs, Schema.Log);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.Timestamp)
            .IsRequired()
            .HasColumnName(nameof(AchievementLog.Timestamp))
            .HasColumnType("bigint")
            .HasColumnOrder(1);

        builder.Property(x => x.AchievementName)
            .IsRequired()
            .HasColumnName(nameof(AchievementLog.AchievementName))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(2);

        builder.Property(x => x.TiglUserId)
            .IsRequired()
            .HasColumnName(nameof(AchievementLog.TiglUserId))
            .HasColumnType("int")
            .HasColumnOrder(3);

        builder.Property(x => x.TiglUserName)
            .IsRequired()
            .HasColumnName(nameof(AchievementLog.TiglUserName))
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(4);

        builder.Property(x => x.TiglUserDiscordId)
            .IsRequired()
            .HasColumnName(nameof(AchievementLog.TiglUserDiscordId))
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(5);

        builder.Property(x => x.MatchId)
            .HasColumnName(nameof(AchievementLog.MatchId))
            .HasColumnType("int")
            .HasColumnOrder(6);

        builder.Property(x => x.Published)
            .IsRequired()
            .HasColumnName(nameof(AchievementLog.Published))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasColumnOrder(7);
    }
}
