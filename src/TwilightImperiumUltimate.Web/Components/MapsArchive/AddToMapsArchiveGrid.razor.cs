using TwilightImperiumUltimate.Web.Helpers.Maps;
using TwilightImperiumUltimate.Web.Models.Users;
using TwilightImperiumUltimate.Web.Services.MapGenerators;
using TwilightImperiumUltimate.Web.Services.User;

namespace TwilightImperiumUltimate.Web.Components.MapsArchive;

public partial class AddToMapsArchiveGrid
{
    private Dictionary<int, SystemTileModel> _map = new Dictionary<int, SystemTileModel>();

    private MapTemplate _mapTemplate;

    private MapModel _mapModel = new MapModel();

    private TwilightImperiumUser? _user;

    private bool _enableEditModeForDescription = true;

    private bool _showMapDuplicateError;

    [CascadingParameter(Name = "MapTemplateValue")]
    public string MapTemplateImported { get; set; } = string.Empty;

    [CascadingParameter(Name = "TilesValue")]
    public string TilesImported { get; set; } = string.Empty;

    private MarkupString MarkupStringDescription => (MarkupString)_mapModel.Description;

    [Inject]
    private IMapToStringConverter MapToStringConverter { get; set; } = default!;

    [Inject]
    private IUserService UserService { get; set; } = default!;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private RenderFragment DynamicComponent => builder =>
    {
        builder.OpenComponent(0, _mapTemplate.GetPreviewMapTypeFromMapTemplate());
        builder.AddAttribute(1, "GeneratedPositionsWithSystemTiles", _map);
        builder.CloseComponent();
    };

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(MapTemplateImported)
            && !string.IsNullOrEmpty(TilesImported)
            && Enum.TryParse(MapTemplateImported, out MapTemplate mapTemplate))
        {
            _mapTemplate = mapTemplate;
            _mapModel.MapTemplate = mapTemplate;
            await MapToStringConverter.ConvertBase64StringToMap(mapTemplate, TilesImported);
            _map = MapToStringConverter.Map.ToDictionary();
            _user = await UserService.GetCurrentUserAsync();

            await AssignMapModelStringsAndUserInfo();
        }
    }

    private bool IsNameOrEventNotFilled()
    {
        return string.IsNullOrEmpty(_mapModel.EventName) || string.IsNullOrWhiteSpace(_mapModel.EventName)
            || string.IsNullOrEmpty(_mapModel.Name) || string.IsNullOrWhiteSpace(_mapModel.Name);
    }

    private async Task SaveMap()
    {
        _showMapDuplicateError = false;

        if (IsNameOrEventNotFilled())
            return;

        var mapDto = Mapper.Map<MapDto>(_mapModel);
        var (response, statusCode) = await HttpClient.PostAsync<MapDto, ApiResponse<MapDto>>(Paths.ApiPath_AddNewMapToArchive, mapDto);

        if (statusCode == HttpStatusCode.OK && response.Success)
        {
            NavigationManager.NavigateTo(Pages.Pages.MapsArchive);
        }
        else if (statusCode == HttpStatusCode.OK && !response.Success)
        {
            _showMapDuplicateError = true;
        }
    }

    private async Task AssignMapModelStringsAndUserInfo()
    {
        _mapModel.TtsString = await MapToStringConverter.ConvertMapToTtsString(_mapTemplate, _map);
        var mapCode = await MapToStringConverter.ConvertMapToBase64String(_mapTemplate, _map);
        _mapModel.MapGeneratorLink = await MapToStringConverter.GenerateMapGeneratorLink(_mapTemplate, mapCode);
        _mapModel.MapArchiveLink = await MapToStringConverter.GenerateMapArchiveLink(_mapModel.Name, _mapModel.EventName);
        _mapModel.UserName = _user?.UserName ?? string.Empty;
        _mapModel.UserId = _user?.Id ?? string.Empty;
    }

    private void ToggleEditDescription(bool editMode)
    {
        _enableEditModeForDescription = editMode;
        StateHasChanged();
    }

    private async Task HandleNameChange(ChangeEventArgs e)
    {
        _mapModel.Name = e?.Value?.ToString() ?? string.Empty;
        _mapModel.MapArchiveLink = await MapToStringConverter.GenerateMapArchiveLink(_mapModel.Name, _mapModel.EventName);
        StateHasChanged();
    }

    private async Task HandleEventChange(ChangeEventArgs e)
    {
        _mapModel.EventName = e?.Value?.ToString() ?? string.Empty;
        _mapModel.MapArchiveLink = await MapToStringConverter.GenerateMapArchiveLink(_mapModel.Name, _mapModel.EventName);
        StateHasChanged();
    }
}