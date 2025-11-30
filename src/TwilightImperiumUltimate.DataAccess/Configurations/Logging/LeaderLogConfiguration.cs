using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwilightImperiumUltimate.Core.Entities.Logging;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Logging;

public class LeaderLogConfiguration : IEntityTypeConfiguration<LeaderLog>
{
    public void Configure(EntityTypeBuilder<LeaderLog> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.LeaderLogs, Schema.Log);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.NewOwnerId, x.NewOwnerDiscordId, x.PreviousOwnerId, x.PreviousOwnerDiscordId })
            .HasDatabaseName("IX_LeaderLogs_NewOwnerId_NewOwnerDiscordId_PreviousOwnerId_PreviousOwnerDiscordId");

        builder.HasIndex(x => new { x.League, x.Faction, x.Name, x.Timestamp, x.Duration })
            .HasDatabaseName("IX_LeaderLogs_League_Faction_Name_Timestamp_AfterDuration");

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.Timestamp)
            .IsRequired()
            .HasColumnName(nameof(LeaderLog.Timestamp))
            .HasColumnType("bigint")
            .HasColumnOrder(1);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName(nameof(LeaderLog.Name))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(2);

        builder.Property(x => x.Faction)
            .IsRequired()
            .HasColumnName(nameof(LeaderLog.Faction))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(3);

        builder.Property(x => x.League)
            .IsRequired()
            .HasColumnName(nameof(LeaderLog.League))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(4);

        builder.Property(x => x.PreviousOwnerId)
            .HasColumnName(nameof(LeaderLog.PreviousOwnerId))
            .HasColumnType("integer")
            .HasColumnOrder(5);

        builder.Property(x => x.PreviousOwnerDiscordId)
            .HasColumnName(nameof(LeaderLog.PreviousOwnerDiscordId))
            .HasConversion<long>()
            .HasColumnType("bigint")
            .HasColumnOrder(6);

        builder.Property(x => x.PreviousOwner)
            .IsRequired()
            .HasColumnName(nameof(LeaderLog.PreviousOwner))
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(7);

        builder.Property(x => x.NewOwnerDiscordId)
            .HasColumnName(nameof(LeaderLog.NewOwnerDiscordId))
            .HasConversion<long>()
            .HasColumnType("bigint")
            .HasColumnOrder(8);

        builder.Property(x => x.NewOwnerId)
            .HasColumnName(nameof(LeaderLog.NewOwnerId))
            .HasColumnType("integer")
            .HasColumnOrder(9);

        builder.Property(x => x.NewOwner)
            .IsRequired()
            .HasColumnName(nameof(LeaderLog.NewOwner))
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(10);

        builder.Property(x => x.Duration)
            .IsRequired()
            .HasColumnName(nameof(LeaderLog.Duration))
            .HasColumnType("bigint")
            .HasColumnOrder(11);

        builder.Property(x => x.MatchId)
            .HasColumnName(nameof(LeaderLog.MatchId))
            .HasColumnType("int")
            .HasColumnOrder(12);

        builder.Property(x => x.Published)
            .IsRequired()
            .HasColumnName(nameof(LeaderLog.Published))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasColumnOrder(13);
    }
}
