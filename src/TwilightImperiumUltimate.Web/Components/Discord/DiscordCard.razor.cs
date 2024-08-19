namespace TwilightImperiumUltimate.Web.Components.Discord;

public partial class DiscordCard
{
    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public string DiscordInviteLink { get; set; } = string.Empty;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string GetDiscordIconPath()
    {
        return PathProvider.GetWebsitePreviewImagePath(Strings.Discord_PreviewImage);
    }
}