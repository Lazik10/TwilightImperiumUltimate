using System.Globalization;
using System.Text.Json;

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

    private string ArticleStructuredData => _newsArticles is null
        ? string.Empty
        : JsonSerializer.Serialize(new
        {
            @context = "https://schema.org",
            @type = "ItemList",
            name = "TI4 Ultimate news",
            itemListElement = _newsArticles.Select((article, index) => new
            {
                @type = "ListItem",
                position = index + 1,
                item = new
                {
                    @type = "Article",
                    headline = article.Title,
                    datePublished = article.CreatedAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    dateModified = article.UpdatedAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    author = new
                    {
                        @type = "Person",
                        name = article.User?.UserName ?? "TI4 Ultimate",
                    },
                },
            }),
        });

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
