namespace TwilightImperiumUltimate.Web.Pages.Game;

/// <summary>
/// Legacy route kept for backward compatibility with existing bookmarks, links, and search engine
/// results. Redirects to the generic Discordant Stars faction route.
/// </summary>
public partial class FactionsDS
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        NavigationManager.NavigateTo("/game/factions/discordantstars", replace: true);
    }
}
