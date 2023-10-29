using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids;

public abstract class BaseMap : ComponentBase
{
    protected virtual IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, Settings.MapGenerators.MapGeneratorSettings.MaxTilePositions);

    protected virtual IReadOnlyDictionary<int, int>? MapTilePositions { get; set; } = default!;

    protected int MapTileId { get; set; } = Settings.MapGenerators.MapGeneratorSettings.DefaultTileId;

    [Inject]
    protected virtual IMapGeneratorService MapGeneratorService { get; set; } = default!;

    [Inject]
    protected virtual IMapGeneratorSettingsService MapGeneratorSettingsService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        MapTilePositions = await MapGeneratorService.GenerateMapAsync();
    }
}
