using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Logging;

public class DiscordRoleChangeLog : IEntity
{
    public int Id { get; set; }

    public ulong RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public DiscordRoleChangeOperation Operation { get; set; }

    public long UserId { get; set; }

    public string UserTag { get; set; } = string.Empty;

    public int GameId { get; set; }

    public long Timestamp { get; set; }

    public DiscordRoleChangeStatus Result { get; set; }

}
