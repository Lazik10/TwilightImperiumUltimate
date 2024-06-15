using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Models.Galaxy;
using TwilightImperiumUltimate.Web.Options.MapGenerators;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids;

public abstract class BaseMap : ComponentBase
{
    [Parameter]
    public IReadOnlyDictionary<int, SystemTile> GeneratedPositionsWithSystemTiles { get; set; } = default!;

    protected virtual IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, MapGeneratorOptions.MaxTilePositions);

    protected SystemTileName SystemTileName { get; set; } = MapGeneratorOptions.DefaultTileName;

    protected SystemTile CurrentSystemTile { get; set; } = default!;

    [Inject]
    protected virtual IMapGeneratorService MapGeneratorService { get; set; } = default!;

    [Inject]
    protected virtual IMapGeneratorSettingsService MapGeneratorSettingsService { get; set; } = default!;

    protected virtual void RefreshAfterSwap()
    {
        GeneratedPositionsWithSystemTiles = MapGeneratorService.GeneratedPositionsWithSystemTiles;
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        await MapGeneratorService.InitializeSystemTilesAsync();
        await MapGeneratorService.GenerateMapAsync(true);
        GeneratedPositionsWithSystemTiles = MapGeneratorService.GeneratedPositionsWithSystemTiles;
    }
}
