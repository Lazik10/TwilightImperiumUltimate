namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class RankingsMenu
{
    [Parameter]
    public EventCallback<RankingsMenuItem> OnMenuItemClick { get; set; }
}