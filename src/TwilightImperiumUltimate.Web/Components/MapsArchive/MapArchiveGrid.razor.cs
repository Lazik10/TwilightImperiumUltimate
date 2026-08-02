using TwilightImperiumUltimate.Web.Models.MapArchive;

namespace TwilightImperiumUltimate.Web.Components.MapsArchive;

public partial class MapArchiveGrid
{
    [Parameter]
    public IReadOnlyCollection<MapModel> AllMaps { get; set; } = new List<MapModel>();

    private IReadOnlyCollection<MapModel> FilteredMaps { get; set; } = new List<MapModel>();

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnParametersSet()
    {
        FilteredMaps = AllMaps.OrderByDescending(map => map.Rating).ToList();
    }

    private void RedirectToMapDetails(int mapId)
    {
        NavigationManager.NavigateTo($"{Pages.Pages.MapPreview}{mapId}");
    }

    private void RedirectToSelectedMap(MapModel? map)
    {
        if (map is not null)
        {
            RedirectToMapDetails(map.Id);
        }
    }
}
