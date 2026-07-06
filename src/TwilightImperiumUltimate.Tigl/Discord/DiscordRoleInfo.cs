namespace TwilightImperiumUltimate.Tigl.Discord;

public sealed class DiscordRoleInfo
{
    public required string RoleName { get; init; }

    public required string RoleId { get; init; }

    public required string ColorHex { get; init; }

    public int ColorRgb { get; init; }

    public required string EmojiId { get; init; }
}
