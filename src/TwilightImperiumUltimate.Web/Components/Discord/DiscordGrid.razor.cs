namespace TwilightImperiumUltimate.Web.Components.Discord;

public partial class DiscordGrid
{
    private readonly List<DiscordModel> _discordServers = new List<DiscordModel>()
    {
        new DiscordModel()
        {
            Title = Strings.Discord_Online,
            DiscordInviteLink = Strings.Discord_OnlineInvite,
        },
        new DiscordModel()
        {
            Title = Strings.Discord_SpaceCatsPeaceTurtles,
            DiscordInviteLink = Strings.Discord_SpaceCatsPeaceTurtlesInvite,
        },
        new DiscordModel()
        {
            Title = Strings.Discord_Homebrew,
            DiscordInviteLink = Strings.Discord_HomebrewInvite,
        },
        new DiscordModel()
        {
            Title = Strings.Discord_Asynchronous,
            DiscordInviteLink = Strings.Discord_AsynchronousInvite,
        },
    };
}