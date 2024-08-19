namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapGeneratorMenu
{
    [Parameter]
    public EventCallback<MapGeneratorMenuItem> OnMenuItemClick { get; set; }
}