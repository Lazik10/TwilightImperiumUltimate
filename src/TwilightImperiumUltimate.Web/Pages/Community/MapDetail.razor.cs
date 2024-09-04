using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.ApiContracts.Maps;
using TwilightImperiumUltimate.Web.Components.Shared.Bars;
using TwilightImperiumUltimate.Web.Helpers.Maps;
using TwilightImperiumUltimate.Web.Models.Users;
using TwilightImperiumUltimate.Web.Services.MapGenerators;
using TwilightImperiumUltimate.Web.Services.User;

namespace TwilightImperiumUltimate.Web.Pages.Community;

public partial class MapDetail
{
    private Dictionary<int, SystemTileModel> _map = new Dictionary<int, SystemTileModel>();

    private TwilightImperiumUser? _user;

    [Parameter]
    public float UserRating { get; set; }

    [Parameter]
    public int MapId { get; set; }

    public MapModel Map { get; set; } = new MapModel();

    [Inject]
    private IMapToStringConverter MapToStringConverter { get; set; } = default!;

    [Inject]
    private IMapGeneratorSettingsService MapGeneratorSettingsService { get; set; } = default!;

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

    private MarkupString MarkupStringDescription => (MarkupString)Map.Description;

    private RenderFragment DynamicComponent => builder =>
    {
        builder.OpenComponent(0, Map.MapTemplate.GetPreviewMapTypeFromMapTemplate());
        builder.AddAttribute(1, "GeneratedPositionsWithSystemTiles", _map);
        builder.CloseComponent();
    };

    protected override async Task OnInitializedAsync()
    {
        _user = await UserService.GetCurrentUserAsync();
        await InitializeMap();
        await InitializeUserRating();
    }

    protected override async Task OnParametersSetAsync()
    {
        MapGeneratorSettingsService.MapTemplate = Map.MapTemplate;
        await MapToStringConverter.ConvertTtsStringToMap(Map.MapTemplate, Map.TtsString);
        _map = MapToStringConverter.Map.ToDictionary();
        await InitializeUserRating();
    }

    private async Task InitializeUserRating()
    {
        if (_user is not null)
        {
            var request = new UserMapRatingRequest() { UserId = _user.Id, MapId = MapId };
            var (response, statusCode) = await HttpClient.PostAsync<UserMapRatingRequest, ApiResponse<MapRatingDto>>(Paths.ApiPath_MapRatingsUser, request);

            if (statusCode == HttpStatusCode.OK)
            {
                UserRating = response!.Data!.Rating;
            }
        }
    }

    private async Task InitializeMap()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<MapDto>>($"{Paths.ApiPath_MapArchiveMaps}/{MapId}");
        if (statusCode == HttpStatusCode.OK)
        {
            var map = Mapper.Map<MapModel>(response!.Data);
            Map = map;
        }
    }

    private async Task UpdateUserMapRating(float newRating)
    {
        if (_user is not null)
        {
            var request = new UserMapRatingRequest() { UserId = _user.Id, MapId = MapId, Rating = newRating };
            var (response, statusCode) = await HttpClient.PutAsync<UserMapRatingRequest, ApiResponse<MapRatingDto>>(Paths.ApiPath_MapRatingsUser, request, default);

            if (statusCode == HttpStatusCode.OK)
            {
                UserRating = response!.Data!.Rating;
                StateHasChanged();
            }

            await InitializeMap();
            StateHasChanged();

            NavigationManager.NavigateTo(NavigationManager.Uri, false);
        }
    }

    private void RedirectBack() => NavigationManager.NavigateTo(Pages.MapsArchive);

    private async Task HandleDownloadIconClick()
    {
        var mapTemplate = Map.MapTemplate.ToString();
        var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Community/MapDetail.razor.js");
        await module.InvokeVoidAsync("saveMapAsImage", "mapArea", $"TI4_UltimateMap_{mapTemplate}_{Map.Name}_{Map.EventName}_{Map.UserName}.png");
    }
}