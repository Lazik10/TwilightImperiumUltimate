using TwilightImperiumUltimate.Web.Models.SlicesArchive;

namespace TwilightImperiumUltimate.Web.Components.SlicesArchive;

public partial class SlicesArchiveGrid
{
    [Parameter]
    public IReadOnlyCollection<SliceDraftModel> AllSliceDrafts { get; set; } = new List<SliceDraftModel>();

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private IReadOnlyCollection<SliceDraftModel> FilteredSliceDrafts { get; set; } = new List<SliceDraftModel>();

    protected override void OnParametersSet()
    {
        FilteredSliceDrafts = AllSliceDrafts.OrderByDescending(sliceDraft => sliceDraft.Rating).ToList();
    }

    private static string GetSliceDraftDetailsUrl(int sliceDraftId) => $"{Pages.Pages.SliceDraftPreview}{sliceDraftId}";
}
