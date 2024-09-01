using Microsoft.JSInterop;
using TwilightImperiumUltimate.Web.Helpers.Maps;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapGeneratorMainGrid
{
    private FactionIconRow? factionIconRowRef;

    private MapGeneratorMenuItem _selectedSegment;

    public IReadOnlyDictionary<int, SystemTileModel> GeneratedPositionsWithSystemTiles { get; set; } = default!;

    [CascadingParameter(Name = "MapTemplateValue")]
    public string MapTemplateImported { get; set; } = string.Empty;

    [CascadingParameter(Name = "TilesValue")]
    public string TilesImported { get; set; } = string.Empty;

    [Inject]
    private IMapGeneratorService MapGeneratorService { get; set; } = default!;

    [Inject]
    private IMapGeneratorSettingsService MapGeneratorSettingsService { get; set; } = default!;

    [Inject]
    private IMapToStringConverter MapToStringConverter { get; set; } = default!;

    [Inject]
    private IMapEvaluationService MapEvaluationService { get; set; } = default!;

    [Inject]
    private IMapGeneratorArchiveService MapGeneratorArchiveService { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private MapEvaluations MapEvaluations { get; set; } = new MapEvaluations();

    private RenderFragment DynamicComponent => builder =>
    {
        builder.OpenComponent(0, MapGeneratorSettingsService.MapTemplate.GetMapTypeFromMapTemplate());
        builder.AddAttribute(1, "GeneratedPositionsWithSystemTiles", GeneratedPositionsWithSystemTiles);
        builder.CloseComponent();
    };

    public Task UpdateMapOverlay()
    {
        StateHasChanged();
        return Task.CompletedTask;
    }

    protected override async Task OnInitializedAsync()
    {
        GeneratedPositionsWithSystemTiles = await MapGeneratorService.GenerateMapAsync(true, default);

        if (MapGeneratorSettingsService.FactionsForMapGenerator.Count == 0)
            await InitializeFactionsForFactionRow();

        await MapGeneratorSettingsService.InitializePlayersForMapGenerator();

        await MapGeneratorService.InitializeSystemTilesAsync(default);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(MapTemplateImported)
            && !string.IsNullOrEmpty(TilesImported)
            && Enum.TryParse(MapTemplateImported, out MapTemplate mapTemplate))
        {
            await MapToStringConverter.ConvertBase64StringToMap(mapTemplate, TilesImported);
        }

        GeneratedPositionsWithSystemTiles = MapGeneratorService.GeneratedPositionsWithSystemTiles;
        StateHasChanged();
    }

    private async Task UpdateMapTemplate(MapTemplate mapTemplate)
    {
        await MapGeneratorService.GenerateMapAsync(true, CancellationToken.None);
        GeneratedPositionsWithSystemTiles = MapGeneratorService.GeneratedPositionsWithSystemTiles;
        await MapGeneratorSettingsService.InitializePlayersForMapGenerator();
        StateHasChanged();
    }

    private async Task GenerateMap()
    {
        GeneratedPositionsWithSystemTiles = await MapGeneratorService.GenerateMapAsync(false, default);
        await MapEvaluationService.GetMapEvaluation();

        StateHasChanged();
    }

    private void Refresh()
    {
        StateHasChanged();
    }

    private void ChangeSegment(MapGeneratorMenuItem segment)
    {
        _selectedSegment = segment;
        StateHasChanged();
    }

    private void ToggleFactionPick()
    {
        MapGeneratorSettingsService.EnableFactionPick = !MapGeneratorSettingsService.EnableFactionPick;
        StateHasChanged();
    }

    private void HandleGameVersionClick(GameVersion gameVersion)
    {
        MapGeneratorSettingsService.GameVersionGlobalEnableDisable(gameVersion);
        StateHasChanged();
    }

    private void HandleFactionClick(FactionModel faction)
    {
        MapGeneratorSettingsService.UpdateFactionBanStatus(faction);
        factionIconRowRef?.RefreshFactions();
        StateHasChanged();
    }

    private async Task InitializeFactionsForFactionRow()
    {
        await MapGeneratorSettingsService.InitializeFactionsForMapGenerator();
    }

    private void IncreaseMapScale()
    {
        MapGeneratorSettingsService.IncreaseMapScale();
        StateHasChanged();
    }

    private void DecreaseMapScale()
    {
        MapGeneratorSettingsService.DecreaseMapScale();
        StateHasChanged();
    }

    private async Task DownloadMapImage()
    {
        var mapTemplate = MapGeneratorSettingsService.MapTemplate.ToString();
        var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/MapGenerator/MapGeneratorMainGrid.razor.js");
        await module.InvokeVoidAsync("saveMapAsImage", "mapArea", $"TI4_Ultimate_{mapTemplate}_{DateTime.Now.ToLocalTime().ToLongTimeString()}.png");
    }

    private async Task HandleMenuIconClick(IconType iconType)
    {
        if (iconType == IconType.DownloadMapImage)
            await DownloadMapImage();

        if (iconType == IconType.Hashtag)
        {
            if (MapGeneratorSettingsService.SystemTileOverlay != SystemTileOverlay.Id)
                MapGeneratorSettingsService.SystemTileOverlay = SystemTileOverlay.Id;
            else
                MapGeneratorSettingsService.SystemTileOverlay = SystemTileOverlay.None;

            await UpdateMapOverlay();
        }

        if (iconType == IconType.ShareMap)
        {
            var mapCode = await MapToStringConverter.ConvertMapToBase64String();
            var mapTemplate = MapGeneratorSettingsService.MapTemplate;
            var mapUrl = await MapToStringConverter.GenerateMapGeneratorLink(mapTemplate, mapCode);
            var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/MapGenerator/MapGeneratorMainGrid.razor.js");
            await module.InvokeVoidAsync("copyToClipboard", mapUrl);
        }

        if (iconType == IconType.CopyMapString)
        {
            var mapCode = await MapToStringConverter.ConvertMapToTtsString();
            var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/MapGenerator/MapGeneratorMainGrid.razor.js");
            await module.InvokeVoidAsync("copyToClipboard", mapCode);
        }

        if (iconType == IconType.ImportMapString)
        {
            GeneratedPositionsWithSystemTiles = MapGeneratorService.GeneratedPositionsWithSystemTiles;
            await UpdateMapOverlay();
            StateHasChanged();
        }

        if (iconType == IconType.Archive)
        {
            var mapTemplate = MapGeneratorSettingsService.MapTemplate;
            var map64BaseString = await MapToStringConverter.ConvertMapToBase64String();
            await MapGeneratorArchiveService.RedirectToAddToArchivePage(mapTemplate, map64BaseString);
        }
    }
}
