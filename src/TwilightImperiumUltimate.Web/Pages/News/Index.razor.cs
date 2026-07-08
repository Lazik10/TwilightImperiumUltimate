namespace TwilightImperiumUltimate.Web.Pages.News;

public partial class Index
{
    private IReadOnlyCollection<NewsArticleDto>? _newsArticles;
    private bool _isLoading = true;
    private bool _hasError;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await InitializeNewsAsync();
    }

    private async Task InitializeNewsAsync()
    {
        _isLoading = true;
        _hasError = false;

        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<NewsArticleDto>>>(Paths.ApiPath_News);

        if (statusCode == HttpStatusCode.OK)
            _newsArticles = response?.Data?.Items;
        else
            _hasError = true;

        _isLoading = false;
    }
}
