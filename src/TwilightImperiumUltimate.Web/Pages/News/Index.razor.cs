namespace TwilightImperiumUltimate.Web.Pages.News;

public partial class Index
{
    private IReadOnlyCollection<NewsArticleDto>? _newsArticles = new List<NewsArticleDto>();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected async override Task OnInitializedAsync()
    {
        await InitializeNewsAsync();
    }

    private async Task InitializeNewsAsync()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<NewsArticleDto>>>(Paths.ApiPath_News);
        if (statusCode == HttpStatusCode.OK)
            _newsArticles = response?.Data?.Items;
    }
}