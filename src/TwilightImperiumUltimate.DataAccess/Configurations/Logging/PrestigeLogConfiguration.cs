using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwilightImperiumUltimate.Core.Entities.Logging;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Logging;

internal class PrestigeLogConfiguration : IEntityTypeConfiguration<PrestigeLog>
{
    public void Configure(EntityTypeBuilder<PrestigeLog> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.PrestigeLogs, Schema.Log);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.League, x.TiglUserDiscordId, x.FactionName, x.Name, x.Timestamp, x.TiglUserId })
            .HasDatabaseName("IX_PrestigeLogs_League_TiglUserDiscordId_Faction_Name_Timestamp_TiglUserId");

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.Timestamp)
            .IsRequired()
            .HasColumnName(nameof(PrestigeLog.Timestamp))
            .HasColumnType("bigint")
            .HasColumnOrder(1);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName(nameof(PrestigeLog.Name))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(2);

        builder.Property(x => x.FactionName)
            .IsRequired()
            .HasColumnName(nameof(PrestigeLog.FactionName))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(3);

        builder.Property(x => x.League)
            .IsRequired()
            .HasColumnName(nameof(PrestigeLog.League))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(4);

        builder.Property(x => x.Rank)
            .IsRequired()
            .HasColumnName(nameof(PrestigeLog.Rank))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(5);

        builder.Property(x => x.TiglUserId)
            .HasColumnName(nameof(PrestigeLog.TiglUserId))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(6);

        builder.Property(x => x.TiglUserDiscordId)
            .HasColumnName(nameof(PrestigeLog.TiglUserDiscordId))
            .HasConversion<long>()
            .HasColumnType("bigint")
            .HasColumnOrder(7);

        builder.Property(x => x.TiglUserName)
            .IsRequired()
            .HasColumnName(nameof(PrestigeLog.TiglUserName))
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(8);

        builder.Property(x => x.MatchId)
            .HasColumnName(nameof(PrestigeLog.MatchId))
            .HasColumnType("int")
            .HasColumnOrder(9);

        builder.Property(x => x.Published)
            .IsRequired()
            .HasColumnName(nameof(PrestigeLog.Published))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasColumnOrder(10);
    }
}
