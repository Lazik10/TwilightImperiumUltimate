using TwilightImperiumUltimate.Web.Models.SlicesArchive;

namespace TwilightImperiumUltimate.Web.Components.SlicesArchive;

public partial class SlicesArchiveGrid
{
    [Parameter]
    public IReadOnlyCollection<SliceDraftModel> AllSliceDrafts { get; set; } = new List<SliceDraftModel>();

    private IReadOnlyCollection<SliceDraftModel> FilteredSliceDrafts { get; set; } = new List<SliceDraftModel>();

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnParametersSet()
    {
        FilteredSliceDrafts = AllSliceDrafts.OrderByDescending(sliceDraft => sliceDraft.Rating).ToList();
    }

    private void RedirectToSliceDraftDetails(int sliceDraftId)
    {
        NavigationManager.NavigateTo($"{Pages.Pages.SliceDraftPreview}{sliceDraftId}");
    }

    private void RedirectToSelectedSliceDraft(SliceDraftModel? sliceDraft)
    {
        if (sliceDraft is not null)
        {
            RedirectToSliceDraftDetails(sliceDraft.Id);
        }
    }
}
