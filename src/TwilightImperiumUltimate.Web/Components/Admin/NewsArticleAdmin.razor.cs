using TwilightImperiumUltimate.Contracts.ApiContracts.NewsArticle;
using TwilightImperiumUltimate.Contracts.DTOs.User;
using TwilightImperiumUltimate.Web.Components.Shared.Controls;
using TwilightImperiumUltimate.Web.Models.News;
using TwilightImperiumUltimate.Web.Services.Authentication;

namespace TwilightImperiumUltimate.Web.Components.Admin;

public partial class NewsArticleAdmin
{
    private const int PageSize = 5;

    private readonly Dictionary<int, ResponsiveDangerButton> _deleteButtonRefs = [];

    private ElementReference _articlesRegionRef;

    private NewsArticleFormModel _formModel = new();
    private IReadOnlyCollection<NewsArticleDto> _articles = [];
    private NewsArticleDto? _pendingDeleteArticle;
    private int _totalCount;
    private int _currentPage = 1;
    private bool _isLoading = true;
    private bool _hasError;
    private bool _hasSubmitError;
    private bool _isSubmitting;
    private bool _isPreviewMode;
    private bool _isDeleteDialogOpen;
    private bool _showCreateSuccess;
    private bool _showUpdateSuccess;
    private bool _showDeleteSuccess;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private ICurrentUserState CurrentUserState { get; set; } = default!;

    private bool IsEditMode => _formModel.Id is not null;

    private NewsArticleDto PreviewArticle => new(
        _formModel.Id ?? 0,
        _formModel.Title,
        _formModel.Content,
        DateOnly.FromDateTime(DateTime.Now),
        DateOnly.FromDateTime(DateTime.Now),
        new TwilightImperiumUserDto(
            string.Empty,
            CurrentUserState.User?.UserName,
            string.Empty,
            string.Empty,
            null,
            null,
            null,
            default,
            string.Empty,
            string.Empty,
            string.Empty));

    protected override async Task OnInitializedAsync()
    {
        await LoadArticlesAsync();
    }

    private async Task LoadArticlesAsync()
    {
        _isLoading = true;
        _hasError = false;
        StateHasChanged();

        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<PagedItemListDto<NewsArticleDto>>>(
            Paths.ApiPath_News,
            $"?pageNumber={_currentPage}&pageSize={PageSize}");

        if (statusCode == HttpStatusCode.OK)
        {
            _articles = response?.Data?.Items ?? [];
            _totalCount = response?.Data?.TotalCount ?? 0;
        }
        else
        {
            _hasError = true;
        }

        _isLoading = false;
    }

    private async Task OnPageChanged(int newPage)
    {
        _currentPage = newPage;
        await LoadArticlesAsync();
    }

    private void OnPreviewToggled(bool isPreviewMode) => _isPreviewMode = isPreviewMode;

    private void StartEdit(NewsArticleDto article)
    {
        _formModel = new NewsArticleFormModel
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
        };
        _isPreviewMode = false;
        _showCreateSuccess = false;
        _showUpdateSuccess = false;
        _showDeleteSuccess = false;
    }

    private void CancelEdit()
    {
        _formModel = new NewsArticleFormModel();
        _isPreviewMode = false;
    }

    private async Task HandleValidSubmitAsync()
    {
        if (_isSubmitting)
            return;

        _isSubmitting = true;
        _showCreateSuccess = false;
        _showUpdateSuccess = false;
        _hasSubmitError = false;

        if (IsEditMode)
            await UpdateArticleAsync();
        else
            await CreateArticleAsync();

        _isSubmitting = false;
    }

    private async Task CreateArticleAsync()
    {
        var request = new CreateNewsArticleRequest { Title = _formModel.Title, Content = _formModel.Content };
        var (response, statusCode) = await HttpClient.PostAsync<CreateNewsArticleRequest, ApiResponse<NewsArticleDto>>(Paths.ApiPath_News, request);

        if (statusCode == HttpStatusCode.OK && response.Success)
        {
            _showCreateSuccess = true;
            _formModel = new NewsArticleFormModel();
            _currentPage = 1;
            await LoadArticlesAsync();
        }
        else
        {
            _hasSubmitError = true;
        }
    }

    private async Task UpdateArticleAsync()
    {
        var request = new UpdateNewsArticleRequest { Id = _formModel.Id!.Value, Title = _formModel.Title, Content = _formModel.Content };
        var (response, statusCode) = await HttpClient.PutAsync<UpdateNewsArticleRequest, NewsArticleDto>(Paths.ApiPath_News, request, CancellationToken.None);

        if (statusCode == HttpStatusCode.OK && response.Success)
        {
            _showUpdateSuccess = true;
            _formModel = new NewsArticleFormModel();
            await LoadArticlesAsync();
        }
        else
        {
            _hasSubmitError = true;
        }
    }

    private void RequestDelete(NewsArticleDto article)
    {
        _pendingDeleteArticle = article;
        _isDeleteDialogOpen = true;
    }

    private async Task ConfirmDeleteAsync()
    {
        if (_pendingDeleteArticle is null)
            return;

        var articleId = _pendingDeleteArticle.Id;
        var request = new DeleteNewsArticleRequest { Id = articleId };
        var (response, statusCode) = await HttpClient.DeleteAsync<DeleteNewsArticleRequest, ApiResponse<DeleteNewsArticleResponse>>(Paths.ApiPath_News, request);

        _isDeleteDialogOpen = false;
        _pendingDeleteArticle = null;
        _hasSubmitError = false;

        if (statusCode == HttpStatusCode.OK && response.Success)
        {
            _showDeleteSuccess = true;
            _deleteButtonRefs.Remove(articleId);
            await LoadArticlesAsync();

            // The deleted article's row (and its delete button) no longer exists in the DOM,
            // so focus the stable articles region instead of the removed button.
            await _articlesRegionRef.FocusAsync();
        }
        else
        {
            _hasSubmitError = true;
            await FocusDeleteButtonAsync(articleId);
        }
    }

    private async Task CancelDeleteAsync()
    {
        var articleId = _pendingDeleteArticle?.Id;
        _isDeleteDialogOpen = false;
        _pendingDeleteArticle = null;

        if (articleId is not null)
            await FocusDeleteButtonAsync(articleId.Value);
    }

    private async Task FocusDeleteButtonAsync(int articleId)
    {
        if (_deleteButtonRefs.TryGetValue(articleId, out var buttonRef))
            await buttonRef.FocusAsync();
    }
}
