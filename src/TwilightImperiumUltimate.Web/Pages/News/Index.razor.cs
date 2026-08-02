namespace TwilightImperiumUltimate.Web.Pages.News;

public partial class Index
{
    private const int PageSize = 5;

    private IReadOnlyCollection<NewsArticleDto>? _newsArticles;
    private int _totalCount;
    private int _currentPage = 1;
    private bool _isLoading = true;
    private bool _hasError;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private bool IsHomeRoute =>
        !NavigationManager.ToBaseRelativePath(NavigationManager.Uri).StartsWith("news", StringComparison.OrdinalIgnoreCase);

    private string PageTitleText => IsHomeRoute ? Strings.Page_Home_PageTitle : Strings.Page_News_PageTitle;

    protected override async Task OnInitializedAsync()
    {
        await InitializeNewsAsync();
    }

    private async Task OnPageChanged(int newPage)
    {
        _currentPage = newPage;
        await InitializeNewsAsync();
    }

    private async Task InitializeNewsAsync()
    {
        _isLoading = true;
        _hasError = false;
        StateHasChanged();

        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<PagedItemListDto<NewsArticleDto>>>(
            Paths.ApiPath_News,
            $"?pageNumber={_currentPage}&pageSize={PageSize}");

        if (statusCode == HttpStatusCode.OK)
        {
            _newsArticles = response?.Data?.Items;
            _totalCount = response?.Data?.TotalCount ?? 0;
        }
        else
        {
            _hasError = true;
        }

        _isLoading = false;
    }
}
