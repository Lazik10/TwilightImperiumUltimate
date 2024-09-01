namespace TwilightImperiumUltimate.Web.Pages.Community;

public partial class MapsArchive
{
    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private List<MapModel> Maps { get; set; } = new List<MapModel>();

    protected override async Task OnInitializedAsync()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<MapDto>>>(Paths.ApiPath_MapArchiveMaps);

        if (statusCode == HttpStatusCode.OK)
        {
            Maps = Mapper.Map<List<MapModel>>(response!.Data!.Items);
        }
    }

    private void Redirect() => NavigationManager.NavigateTo(Pages.MapGenerator);
}