namespace TwilightImperiumUltimate.Web.Components.Ranking;

public partial class RankingsMenu
{
    [Parameter]
    public EventCallback<RankingsMenuItem> OnMenuItemClick { get; set; }
}