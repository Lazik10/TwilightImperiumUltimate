namespace TwilightImperiumUltimate.API.Options;

public class DiscordOptions
{
    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string RedirectUri { get; set; } = string.Empty;

    public string DiscordAuthorizeUrl { get; set; } = "https://discord.com/api/oauth2/authorize";

    public string DiscordTokenUrl { get; set; } = "https://discord.com/api/oauth2/token";

    public string DiscordMeUrl { get; set; } = "https://discord.com/api/users/@me";

    // Discord.Net bot configuration
    public string BotToken { get; set; } = string.Empty;

    public ulong GuildId { get; set; }

    public string RankUpStandardChannelId { get; set; } = string.Empty;

    public string RankUpFracturedChannelId { get; set; } = string.Empty;

    public string LeadersStandardChannelId { get; set; } = string.Empty;

    public string LeadersFracturedChannelId { get; set; } = string.Empty;

    public string AchievementsChannelId { get; set; } = string.Empty;

    public string TiglAdminChannelId { get; set; } = string.Empty;
}
