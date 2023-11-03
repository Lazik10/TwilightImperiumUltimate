using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.Custom;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.EightPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.FourPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.SixPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.ThreePlayers;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Models.Galaxy;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapGeneratorMainGrid
{
    private bool _showSettings;
    private bool _showTilesMenu;
    private bool _lastShowTilesMenuState;

    [Parameter]
    public IReadOnlyDictionary<int, SystemTile> GeneratedPositionsWithSystemTiles { get; set; } = default!;

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
            MapTemplate.SmallMapThreePlayers => typeof(SmallMapThreePlayers),
            MapTemplate.SmallMapAlternateThreePlayers => typeof(SmallMapAlternateThreePlayers),
            MapTemplate.TriangleMapThreePlayers => typeof(TriangleMapThreePlayers),
            MapTemplate.TriangleNarrowMapThreePlayers => typeof(TriangleNarrowMapThreePlayers),
            MapTemplate.SnowflakeMapThreePlayers => typeof(SnowflakeMapThreePlayers),
            MapTemplate.TridentMapThreePlayers => typeof(TridentMapThreePlayers),
            MapTemplate.MantaRayMapThreePlayers => typeof(MantaRayMapThreePlayers),
            MapTemplate.MediumMapFourPlayers => typeof(MediumMapFourPlayers),
            MapTemplate.MediumMapSixPlayers => typeof(MediumMapSixPlayers),
            MapTemplate.LargeMapSixPlayers => typeof(LargeMapSixPlayers),
            MapTemplate.LargeMapEightPlayers => typeof(LargeMapEightPlayers),
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

    private async Task UpdateMapTemplate(MapTemplate mapTemplate)
    {
        await MapGeneratorService.GenerateMapAsync(true);
        GeneratedPositionsWithSystemTiles = MapGeneratorService.GeneratedPositionsWithSystemTiles;
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
        GeneratedPositionsWithSystemTiles = await MapGeneratorService.GenerateMapAsync(false);
        StateHasChanged();
    }
}
