using Microsoft.AspNetCore.Components.Forms;
using TwilightImperiumUltimate.Contracts.ApiContracts.Website;
using TwilightImperiumUltimate.Web.Models.Website;

namespace TwilightImperiumUltimate.Web.Components.Admin;

/// <summary>
/// Admin add/remove editor for the "Other Websites" community list. Unlike <see cref="NewsArticleAdmin"/>
/// there is no edit mode -- a website is either added or removed. A preview image can optionally be
/// uploaded and is stored directly in the database (see <see cref="WebsiteFormModel.ImageData"/>),
/// so a newly added website's image is available immediately at runtime without requiring a new
/// deployment, unlike the legacy built-in images looked up by title
/// (<see cref="Services.Path.IPathProvider.GetWebsitePreviewImagePath"/>).
/// </summary>
public partial class WebsiteAdmin : TwilightImperiumBaseComponent
{
    /// <summary>The largest preview image upload accepted, in bytes.</summary>
    private const long MaxImageSizeBytes = 2 * 1024 * 1024;

    private WebsiteFormModel _formModel = new();
    private List<WebsiteModel> _websites = [];
    private WebsiteModel? _pendingDeleteWebsite;
    private bool _isLoading = true;
    private bool _hasError;
    private bool _hasSubmitError;
    private bool _isSubmitting;
    private bool _isDeleteDialogOpen;
    private bool _showCreateSuccess;
    private bool _showDeleteSuccess;
    private string? _imageError;

    protected override async Task OnInitializedAsync()
    {
        await LoadWebsitesAsync();
    }

    private async Task LoadWebsitesAsync()
    {
        _isLoading = true;
        _hasError = false;
        StateHasChanged();

        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<WebsiteDto>>>(Paths.ApiPath_Websites);

        if (statusCode == HttpStatusCode.OK && response?.Data?.Items is not null)
        {
            _websites = Mapper.Map<List<WebsiteModel>>(response.Data.Items);
        }
        else
        {
            _hasError = true;
        }

        _isLoading = false;
        StateHasChanged();
    }

    private async Task OnImageSelectedAsync(InputFileChangeEventArgs e)
    {
        _imageError = null;
        var file = e.File;

        if (file is null)
            return;

        if (file.Size > MaxImageSizeBytes)
        {
            _imageError = ValidationMessages.WebsiteAdmin_ImageTooLarge;
            _formModel.ImageData = null;
            _formModel.ImageContentType = null;
            _formModel.ImageFileName = null;
            return;
        }

        await using var stream = file.OpenReadStream(MaxImageSizeBytes);
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);

        _formModel.ImageData = memoryStream.ToArray();
        _formModel.ImageContentType = file.ContentType;
        _formModel.ImageFileName = file.Name;
    }

    private async Task HandleValidSubmitAsync()
    {
        if (_isSubmitting)
            return;

        _isSubmitting = true;
        _showCreateSuccess = false;
        _hasSubmitError = false;
        StateHasChanged();

        var request = new CreateWebsiteRequest
        {
            Title = _formModel.Title,
            Description = _formModel.Description,
            WebsitePath = _formModel.WebsitePath,
            ImageData = _formModel.ImageData,
            ImageContentType = _formModel.ImageContentType,
        };

        var (response, statusCode) = await HttpClient.PostAsync<CreateWebsiteRequest, ApiResponse<WebsiteDto>>(Paths.ApiPath_Websites, request);

        if (statusCode == HttpStatusCode.OK && response.Success)
        {
            _showCreateSuccess = true;
            _formModel = new WebsiteFormModel();
            await LoadWebsitesAsync();
        }
        else
        {
            _hasSubmitError = true;
        }

        _isSubmitting = false;
        StateHasChanged();
    }

    private void RequestDelete(WebsiteModel website)
    {
        _pendingDeleteWebsite = website;
        _isDeleteDialogOpen = true;
    }

    private async Task ConfirmDeleteAsync()
    {
        if (_pendingDeleteWebsite is null)
            return;

        var request = new DeleteWebsiteRequest { Id = _pendingDeleteWebsite.Id };
        var (response, statusCode) = await HttpClient.DeleteAsync<DeleteWebsiteRequest, ApiResponse<DeleteWebsiteResponse>>(Paths.ApiPath_Websites, request);

        _isDeleteDialogOpen = false;
        _pendingDeleteWebsite = null;
        _hasSubmitError = false;

        if (statusCode == HttpStatusCode.OK && response.Success)
        {
            _showDeleteSuccess = true;
            await LoadWebsitesAsync();
        }
        else
        {
            _hasSubmitError = true;
        }
    }

    private void CancelDelete()
    {
        _isDeleteDialogOpen = false;
        _pendingDeleteWebsite = null;
    }
}
