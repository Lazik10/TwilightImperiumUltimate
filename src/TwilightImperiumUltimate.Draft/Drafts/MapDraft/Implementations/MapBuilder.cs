using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Extensions;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

internal class MapBuilder(
    IGalaxyBuilder galaxyBuilder,
    IGalaxyRedPositionSolver galaxyRedPositionSolver,
    ISystemTileSetter systemTileSetter,
    IPlacementStyleHandler placementStyleHandler,
    ILogger<MapBuilder> logger)
    : IMapBuilder
{
    private readonly IGalaxyBuilder _galaxyBuilder = galaxyBuilder;
    private readonly IGalaxyRedPositionSolver _galaxyRedPositionSolver = galaxyRedPositionSolver;
    private readonly ISystemTileSetter _systemTileSetter = systemTileSetter;
    private readonly IPlacementStyleHandler _placementStyleHandler = placementStyleHandler;
    private readonly ILogger<MapBuilder> _logger = logger;

    public async Task<Dictionary<(int X, int Y), Hex>> CreateMapLayout(
        IMapSettings mapSettings,
        GenerateMapRequest request,
        SystemTilesForMapSetup systemTilesForMapSetup)
    {
        var initialGalaxy = await _galaxyBuilder.GenerateGalaxy(mapSettings);
        var galaxy = await _galaxyRedPositionSolver.SolveRedPositions(mapSettings, initialGalaxy, request);

        if (mapSettings.MapTemplate == MapTemplate.CustomMap)
        {
            _systemTileSetter.SetFrameTiles(galaxy, systemTilesForMapSetup.FrameSystemPlaceholder);
            return galaxy;
        }

        _systemTileSetter.SetMecatolSystemTile(galaxy, mapSettings, systemTilesForMapSetup.MecatolRex);
        _systemTileSetter.SetHomeSystemTiles(galaxy, systemTilesForMapSetup, mapSettings, request.HomeSystemDraftType, request.Factions, request.PlayerNames);
        _systemTileSetter.SetRedSystemTiles(galaxy, systemTilesForMapSetup, mapSettings, request);

        if (mapSettings is IHyperlineSettings hyperlineSettings)
        {
            _systemTileSetter.SetHyperlines(galaxy, hyperlineSettings, systemTilesForMapSetup);
        }

        _logger.LogInformation("{GeneratedMapLayout}", galaxy.GetMapLayoutLog(mapSettings.DimensionX, mapSettings.DimensionY));

        await _placementStyleHandler.HandleRemainingPositions(galaxy, mapSettings, systemTilesForMapSetup, request);

        await _placementStyleHandler.HandleRemainingNonSlicePositions(galaxy, mapSettings, systemTilesForMapSetup);

        return galaxy;
    }
}
