using TwilightImperiumUltimate.Web.Models.MapArchive;

namespace TwilightImperiumUltimate.Web.Components.MapsArchive;

public partial class MapArchiveGrid
{
    [Parameter]
    public IReadOnlyCollection<MapModel> AllMaps { get; set; } = new List<MapModel>();

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private IReadOnlyCollection<MapModel> FilteredMaps { get; set; } = new List<MapModel>();

    protected override void OnParametersSet()
    {
        FilteredMaps = AllMaps.OrderByDescending(map => map.Rating).ToList();
    }

    private static string GetMapDetailsUrl(int mapId) => $"{Pages.Pages.MapPreview}{mapId}";
}
