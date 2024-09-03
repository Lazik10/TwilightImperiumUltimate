using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.ApiContracts.SliceDrafts;
using TwilightImperiumUltimate.Web.Models.SlicesArchive;
using TwilightImperiumUltimate.Web.Models.Users;
using TwilightImperiumUltimate.Web.Services.MapGenerators;
using TwilightImperiumUltimate.Web.Services.User;

namespace TwilightImperiumUltimate.Web.Pages.Community;

public partial class SliceDraftDetail
{
    private List<SliceModel> _slices = new List<SliceModel>();

    private TwilightImperiumUser? _user;

    private float _userRating;

    [Parameter]
    public int SliceDraftId { get; set; }

    public SliceDraftModel SliceDraft { get; set; } = new SliceDraftModel();

    [Inject]
    private ISlicesDraftToStringConverter SlicesDraftToStringConverter { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    [Inject]
    private IUserService UserService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private MarkupString MarkupStringDescription => (MarkupString)SliceDraft.Description;

    private MarkupString SlicesString => CreatePreviewSlicesString();

    protected override async Task OnInitializedAsync()
    {
        _user = await UserService.GetCurrentUserAsync();
        await InitializeSliceDraft();
        await InitializeUserRating();
    }

    private MarkupString CreatePreviewSlicesString()
    {
        var slices = SliceDraft.SliceDraftString.Replace("\n", "<br/>");
        return (MarkupString)slices;
    }

    private async Task InitializeUserRating()
    {
        if (_user is not null)
        {
            var request = new UserSliceDraftRatingRequest() { UserId = _user.Id, SliceDraftId = SliceDraftId };
            var (response, statusCode) = await HttpClient.PostAsync<UserSliceDraftRatingRequest, ApiResponse<SliceDraftRatingDto>>(Paths.ApiPath_SliceDraftRatingsUser, request);

            if (statusCode == HttpStatusCode.OK)
            {
                _userRating = response!.Data!.Rating;
            }
        }
    }

    private async Task InitializeSliceDraft()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<SliceDraftDto>>($"{Paths.ApiPath_SlicesArchiveSliceDraft}/{SliceDraftId}");
        if (statusCode == HttpStatusCode.OK)
        {
            var sliceDraft = Mapper.Map<SliceDraftModel>(response!.Data);
            SliceDraft = sliceDraft;
            await SlicesDraftToStringConverter.InitializeSystemTiles();
            _slices = await SlicesDraftToStringConverter.ConvertStringToSlicesDraft(SliceDraft.SliceDraftString);
            await AssignSliceNames();
        }
    }

    private Task AssignSliceNames()
    {
        var sliceNames = SliceDraft.SliceNames.Split(',')
            .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
            .ToList();

        for (var i = 0; i < _slices.Count; i++)
        {
            _slices[i].Name = sliceNames[i];
        }

        return Task.CompletedTask;
    }

    private async Task UpdateUserMapRating(float newRating)
    {
        if (_user is not null)
        {
            var request = new UserSliceDraftRatingRequest() { UserId = _user.Id, SliceDraftId = SliceDraft.Id, Rating = newRating };
            var (response, statusCode) = await HttpClient.PutAsync<UserSliceDraftRatingRequest, ApiResponse<SliceDraftRatingDto>>(Paths.ApiPath_SliceDraftRatingsUser, request, default);

            if (statusCode == HttpStatusCode.OK)
            {
                _userRating = response!.Data!.Rating;
            }

            await InitializeSliceDraft();
            StateHasChanged();
        }
    }

    private void RedirectBack() => NavigationManager.NavigateTo(Pages.SlicesArchive);

    private async Task HandleDownloadIconClick()
    {
        var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Community/SliceDraftDetail.razor.js");
        await module.InvokeVoidAsync("saveMapAsImage", "sliceDraftArea", $"TI4_UltimateSliceDraft_{SliceDraft.SliceCount}_{SliceDraft.Name}_{SliceDraft.EventName}_{SliceDraft.UserName}.png");
    }
}