using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Models.Galaxy;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids;

public abstract class BaseMap : ComponentBase
{
    [Parameter]
    public IReadOnlyDictionary<int, SystemTile> GeneratedPositionsWithSystemTiles { get; set; } = default!;

    protected virtual IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, Settings.MapGenerators.MapGeneratorSettings.MaxTilePositions);

    protected SystemTileName SystemTileName { get; set; } = Settings.MapGenerators.MapGeneratorSettings.DefaultTileName;

    protected SystemTile CurrentSystemTile { get; set; } = default!;

    [Inject]
    protected virtual IMapGeneratorService MapGeneratorService { get; set; } = default!;

    [Inject]
    protected virtual IMapGeneratorSettingsService MapGeneratorSettingsService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await MapGeneratorService.InitializeSystemTilesAsync();
        await MapGeneratorService.GenerateMapAsync(true);
        GeneratedPositionsWithSystemTiles = MapGeneratorService.GeneratedPositionsWithSystemTiles;
    }
}
