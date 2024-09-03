using TwilightImperiumUltimate.Web.Models.SlicesArchive;

namespace TwilightImperiumUltimate.Web.Pages.Community;

public partial class SlicesArchive
{
    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private List<SliceDraftModel> SliceDrafts { get; set; } = new List<SliceDraftModel>();

    protected override async Task OnInitializedAsync()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<SliceDraftDto>>>(Paths.ApiPath_SlicesArchiveSliceDrafts);

        if (statusCode == HttpStatusCode.OK)
        {
            SliceDrafts = Mapper.Map<List<SliceDraftModel>>(response!.Data!.Items);
        }
    }

    private void Redirect() => NavigationManager.NavigateTo(Pages.SliceGenerator);
}