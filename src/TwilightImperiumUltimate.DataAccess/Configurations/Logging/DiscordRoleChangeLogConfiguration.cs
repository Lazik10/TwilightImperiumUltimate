using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwilightImperiumUltimate.Core.Entities.Logging;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Logging;

public class DiscordRoleChangeLogConfiguration : IEntityTypeConfiguration<DiscordRoleChangeLog>
{
    public void Configure(EntityTypeBuilder<DiscordRoleChangeLog> builder)
    {
        builder.ToTable(TableName.DiscordRoleChangeLogs, Schema.Log);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.RoleId)
            .IsRequired()
            .HasColumnName(nameof(DiscordRoleChangeLog.RoleId))
            .HasColumnType("bigint")
            .HasColumnOrder(1);

        builder.Property(x => x.RoleName)
            .IsRequired()
            .HasColumnName(nameof(DiscordRoleChangeLog.RoleName))
            .HasColumnType("varchar(100)")
            .HasColumnOrder(2);

        builder.Property(x => x.Operation)
            .IsRequired()
            .HasColumnName(nameof(DiscordRoleChangeLog.Operation))
            .HasConversion(new EnumToStringConverter<DiscordRoleChangeOperation>())
            .HasColumnType("varchar(50)")
            .HasColumnOrder(3);

        builder.Property(x => x.Timestamp)
            .IsRequired()
            .HasColumnName(nameof(DiscordRoleChangeLog.Timestamp))
            .HasColumnType("bigint")
            .HasColumnOrder(4);

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnName(nameof(DiscordRoleChangeLog.UserId))
            .HasColumnType("bigint")
            .HasColumnOrder(5);

        builder.Property(x => x.GameId)
            .IsRequired()
            .HasColumnName(nameof(DiscordRoleChangeLog.GameId))
            .HasColumnType("int")
            .HasColumnOrder(6);

        builder.Property(x => x.Result)
            .IsRequired()
            .HasColumnName(nameof(DiscordRoleChangeLog.Result))
            .HasConversion(new EnumToStringConverter<DiscordRoleChangeStatus>())
            .HasColumnType("varchar(50)")
            .HasColumnOrder(7);
    }
}
