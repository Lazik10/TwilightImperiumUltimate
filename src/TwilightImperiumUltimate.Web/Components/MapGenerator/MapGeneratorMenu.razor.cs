namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapGeneratorMenu
{
    [Parameter]
    public MapGeneratorMenuItem SelectedMenuItem { get; set; }

    [Parameter]
    public EventCallback<MapGeneratorMenuItem> OnMenuItemClick { get; set; }
}
