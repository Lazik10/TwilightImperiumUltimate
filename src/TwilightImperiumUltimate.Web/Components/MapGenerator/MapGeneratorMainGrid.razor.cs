using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.Custom;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.EightPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.FourPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.SixPlayers;
using TwilightImperiumUltimate.Web.Components.MapGenerator.MapGrids.ThreePlayers;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapGeneratorMainGrid
{
    private MapGeneratorMenuItem _selectedSegment;

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

    protected override async Task OnInitializedAsync()
    {
        GeneratedPositionsWithSystemTiles = await MapGeneratorService.GenerateMapAsync(true, default);
    }

    private Type AssignMapType()
    {
        var mapTemplateType = MapGeneratorSettingsService.MapTemplate switch
        {
            MapTemplate.CustomMap => typeof(CustomMap),
            MapTemplate.ThreePlayersSmallMap => typeof(ThreePlayersSmallMap),
            MapTemplate.ThreePlayersSmallAlternateMap => typeof(ThreePlayersSmallAlternateMap),
            MapTemplate.ThreePlayersTriangleMap => typeof(ThreePlayersTriangleMap),
            MapTemplate.ThreePlayersTriangleNarrowMap => typeof(ThreePlayersTriangleNarrowMap),
            MapTemplate.ThreePlayersSnowflakeMap => typeof(ThreePlayersSnowflakeMap),
            MapTemplate.ThreePlayersMantaRayMap => typeof(ThreePlayersMantaRayMap),
            MapTemplate.FourPlayersMediumMap => typeof(FourPlayersMediumMap),
            MapTemplate.SixPlayersMediumMap => typeof(SixPlayersMediumMap),
            MapTemplate.SixPlayersLargeMap => typeof(SixPlayersLargeMap),
            MapTemplate.EightPlayersLargeMap => typeof(EightPlayersLargeMap),
            _ => throw new NotImplementedException(),
        };

        return mapTemplateType;
    }

    private async Task UpdateMapTemplate(MapTemplate mapTemplate)
    {
        await MapGeneratorService.GenerateMapAsync(true, CancellationToken.None);
        GeneratedPositionsWithSystemTiles = MapGeneratorService.GeneratedPositionsWithSystemTiles;
        StateHasChanged();
    }

    private async Task UpdateMapOverlay()
    {
        StateHasChanged();
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

    private void ChangeSegment(MapGeneratorMenuItem segment)
    {
        _selectedSegment = segment;
        StateHasChanged();
    }
}
