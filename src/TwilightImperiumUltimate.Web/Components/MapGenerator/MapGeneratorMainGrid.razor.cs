using Microsoft.JSInterop;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.Custom;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.EightPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.FivePlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.FourPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.SevenPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.SixPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.ThreePlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.TwoPlayers;
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
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private MapEvaluations MapEvaluations { get; set; } = new MapEvaluations();

    private RenderFragment DynamicComponent => builder =>
    {
        builder.OpenComponent(0, AssignMapType());
        builder.AddAttribute(1, "GeneratedPositionsWithSystemTiles", GeneratedPositionsWithSystemTiles);
        builder.CloseComponent();
    };

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

    private Type AssignMapType()
    {
        var mapTemplateType = MapGeneratorSettingsService.MapTemplate switch
        {
            MapTemplate.CustomMap => typeof(CustomMap),
            MapTemplate.TwoPlayersMediumMap => typeof(TwoPlayersMediumMap),
            MapTemplate.ThreePlayersSmallMap => typeof(ThreePlayersSmallMap),
            MapTemplate.ThreePlayersSmallAlternateMap => typeof(ThreePlayersSmallAlternateMap),
            MapTemplate.ThreePlayersMediumTriangleMap => typeof(ThreePlayersMediumTriangleMap),
            MapTemplate.ThreePlayersMediumTriangleNarrowMap => typeof(ThreePlayersMediumTriangleNarrowMap),
            MapTemplate.ThreePlayersMediumSnowflakeMap => typeof(ThreePlayersMediumSnowflakeMap),
            MapTemplate.ThreePlayersMediumMantaRayMap => typeof(ThreePlayersMediumMantaRayMap),
            MapTemplate.ThreePlayersMediumTridentMap => typeof(ThreePlayersMediumTridentMap),
            MapTemplate.ThreePlayersMediumRexMap => typeof(ThreePlayersMediumRexMap),
            MapTemplate.FourPlayersMediumMap => typeof(FourPlayersMediumMap),
            MapTemplate.FourPlayersMediumHorizontalMap => typeof(FourPlayersMediumHorizontalMap),
            MapTemplate.FourPlayersMediumVerticalMap => typeof(FourPlayersMediumVerticalMap),
            MapTemplate.FourPlayersMediumGapsMap => typeof(FourPlayersMediumGapsMap),
            MapTemplate.FourPlayersMediumWarpMap => typeof(FourPlayersMediumWarpMap),
            MapTemplate.FourPlayersMediumMiniWarpMap => typeof(FourPlayersMediumMiniWarpMap),
            MapTemplate.FourPlayersMediumDoubleWarpMap => typeof(FourPlayersMediumDoubleWarpMap),
            MapTemplate.FivePlayersMediumMap => typeof(FivePlayersMediumMap),
            MapTemplate.FivePlayersMediumHyperlineMap => typeof(FivePlayersMediumHyperlineMap),
            MapTemplate.FivePlayersMediumDiamondMap => typeof(FivePlayersMediumDiamondMap),
            MapTemplate.FivePlayersLargeFlatMap => typeof(FivePlayersLargeFlatMap),
            MapTemplate.SixPlayersMediumMap => typeof(SixPlayersMediumMap),
            MapTemplate.SixPlayersMediumSpiralMap => typeof(SixPlayersMediumSpiralMap),
            MapTemplate.SixPlayersLargeMap => typeof(SixPlayersLargeMap),
            MapTemplate.SevenPlayersLargeHyperlineMap => typeof(SevenPlayersLargeHyperlineMap),
            MapTemplate.SevenPlayersLargeWarpMap => typeof(SevenPlayersLargeWarpMap),
            MapTemplate.EightPlayersLargeMap => typeof(EightPlayersLargeMap),
            MapTemplate.EightPlayersLargeWarpMap => typeof(EightPlayersLargeWarpMap),
            _ => throw new NotImplementedException(),
        };

        return mapTemplateType;
    }

    private async Task UpdateMapTemplate(MapTemplate mapTemplate)
    {
        await MapGeneratorService.GenerateMapAsync(true, CancellationToken.None);
        GeneratedPositionsWithSystemTiles = MapGeneratorService.GeneratedPositionsWithSystemTiles;
        await MapGeneratorSettingsService.InitializePlayersForMapGenerator();
        StateHasChanged();
    }

    private Task UpdateMapOverlay()
    {
        StateHasChanged();
        return Task.CompletedTask;
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
            var mapTemplate = MapGeneratorSettingsService.MapTemplate.ToString();
            var baseAddress = NavigationManager.BaseUri;
            var mapUrl = $"{baseAddress}tools/map-generator?template={mapTemplate}&tiles={mapCode}";
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
    }
}
