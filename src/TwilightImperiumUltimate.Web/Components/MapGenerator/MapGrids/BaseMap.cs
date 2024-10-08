using TwilightImperiumUltimate.Web.Options.MapGenerators;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids;

public abstract class BaseMap : ComponentBase
{
    [Parameter]
    public IReadOnlyDictionary<int, SystemTileModel> GeneratedPositionsWithSystemTiles { get; set; } = default!;

    protected abstract IEnumerable<int> MapPositions { get; set; }

    protected SystemTileName SystemTileName { get; set; } = MapGeneratorOptions.DefaultTileName;

    protected SystemTileModel CurrentSystemTile { get; set; } = default!;

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
        await InitializeBaseMap();
    }

    private async Task InitializeBaseMap(CancellationToken ct = default)
    {
        await MapGeneratorService.InitializeSystemTilesAsync(ct);
        GeneratedPositionsWithSystemTiles = MapGeneratorService.GeneratedPositionsWithSystemTiles;
        StateHasChanged();
    }
}
