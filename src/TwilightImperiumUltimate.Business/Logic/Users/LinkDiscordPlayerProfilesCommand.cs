namespace TwilightImperiumUltimate.Business.Logic.Users;

public sealed class LinkDiscordPlayerProfilesCommand(
    string userId,
    string discordUsername,
    int? asyncPlayerProfileId,
    int? tiglPlayerProfileId) : IRequest<bool>
{
    public string UserId { get; } = userId;

    public string DiscordUsername { get; } = discordUsername;

    public int? AsyncPlayerProfileId { get; } = asyncPlayerProfileId;

    public int? TiglPlayerProfileId { get; } = tiglPlayerProfileId;
}
