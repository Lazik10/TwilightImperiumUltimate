using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.Custom;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.EightPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.FourPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.SixPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.ThreePlayers;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapGeneratorMainGrid
{
    private bool _showSettings;
    private bool _showTilesMenu;
    private bool _lastShowTilesMenuState;

    [Parameter]
    public IReadOnlyDictionary<int, SystemTileModel> GeneratedPositionsWithSystemTiles { get; set; } = default!;

    [Inject]
    private IMapGeneratorService MapGeneratorService { get; set; } = default!;

    [Inject]
    private IMapGeneratorSettingsService MapGeneratorSettingsService { get; set; } = default!;

    private RenderFragment DynamicComponent => builder =>
    {
        builder.OpenComponent(0, AssignMapType());
        builder.AddAttribute(1, "GeneratedPositionsWithSystemTiles", GeneratedPositionsWithSystemTiles);
        builder.CloseComponent();
    };

    private Type AssignMapType()
    {
        var mapTemplateType = MapGeneratorSettingsService.MapTemplate switch
        {
            MapTemplate.CustomMap => typeof(CustomMap),
            MapTemplate.ThreePlayersSmallMap => typeof(SmallMapThreePlayers),
            MapTemplate.ThreePlayersSmallAlternateMap => typeof(SmallMapAlternateThreePlayers),
            MapTemplate.ThreePlayersTriangleMap => typeof(TriangleMapThreePlayers),
            MapTemplate.ThreePlayersTriangleNarrowMap => typeof(TriangleNarrowMapThreePlayers),
            MapTemplate.ThreePlayersSnowflakeMap => typeof(SnowflakeMapThreePlayers),
            MapTemplate.ThreePlayersTridentMap => typeof(TridentMapThreePlayers),
            MapTemplate.ThreePlayersMantaRayMap => typeof(MantaRayMapThreePlayers),
            MapTemplate.FourPlayersMediumMap => typeof(MediumMapFourPlayers),
            MapTemplate.SixPlayersMediumMap => typeof(MediumMapSixPlayers),
            MapTemplate.SixPlayersLargeMap => typeof(LargeMapSixPlayers),
            MapTemplate.EightPlayersLargeMap => typeof(LargeMapEightPlayers),
            _ => throw new NotImplementedException(),
        };

        return mapTemplateType;
    }

    private void ToggleSettings()
    {
        _showSettings = !_showSettings;
        _lastShowTilesMenuState = _showTilesMenu;
        _showTilesMenu = false;
    }

    private void ToggleTilesMenu()
    {
        _showTilesMenu = !_showTilesMenu;
    }

    private async Task UpdateMapTemplate()
    {
        await MapGeneratorService.GenerateMapAsync(true, CancellationToken.None);
        GeneratedPositionsWithSystemTiles = MapGeneratorService.GeneratedPositionsWithSystemTiles;
        StateHasChanged();
    }

    private void UpdateMapOverlay()
    {
        StateHasChanged();
    }

    private void HideSettings()
    {
        _showSettings = false;
        RestoreMapTilesState();
    }

    private void RestoreMapTilesState()
    {
        _showTilesMenu = _lastShowTilesMenuState;
    }

    private async Task GenerateMap()
    {
        GeneratedPositionsWithSystemTiles = await MapGeneratorService.GenerateMapAsync(false, default);
        StateHasChanged();
    }

    private void Refresh()
    {
        StateHasChanged();
    }
}
