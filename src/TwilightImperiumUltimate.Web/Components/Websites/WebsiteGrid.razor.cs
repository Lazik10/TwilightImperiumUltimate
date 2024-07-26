namespace TwilightImperiumUltimate.Web.Components.Websites;

public partial class WebsiteGrid : TwilightImperiumBaseComponenet
{
    private List<WebsiteModel> _websites = new List<WebsiteModel>();

    protected override async Task OnInitializedAsync()
    {
        await GetWebsiteInfos();
    }

    private async Task GetWebsiteInfos()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<WebsiteDto>>>("api/websites");
        if (statusCode == HttpStatusCode.OK)
        {
            _websites = Mapper.Map<List<WebsiteModel>>(response!.Data!.Items);
        }
    }
}