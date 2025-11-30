namespace TwilightImperiumUltimate.Tigl.Discord;

public sealed class DiscordRoleInfo
{
    required public string RoleName { get; init; }

    required public string RoleId { get; init; }

    required public string ColorHex { get; init; }

    public int ColorRgb { get; init; }

    required public string EmojiId { get; init; }
}
