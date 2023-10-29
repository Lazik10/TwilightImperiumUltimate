using TwilightImperiumUltimate.Web.Settings.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids;

public partial class CustomMap : BaseMap
{
    protected override IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, MapTemplateSettings.MaxTilePositionsCustomMap);

    protected override IReadOnlyDictionary<int, int>? MapTilePositions { get; set; } = Enumerable.Range(0, MapTemplateSettings.MaxTilePositionsCustomMap).ToDictionary(x => x, x => x % 82);

    protected override async Task OnInitializedAsync()
    {
        _ = await Task.FromResult(Task.CompletedTask);
    }
}
