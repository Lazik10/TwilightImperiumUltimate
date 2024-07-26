using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapBuilders;

internal abstract class MapBuilderBase : IMapBuilder
{
    protected static Random Random { get; set; } = new Random();

    public abstract Task<IReadOnlyDictionary<int, SystemTile>> BuildMapLayout(
        IMapSettings mapSettings,
        GenerateMapRequest request,
        SystemTilesForMapSetup systemTilesForMapSetup);

    protected static Dictionary<int, SystemTile> CreateRandomMapLayoutForSpecificMap(
        IMapSettings mapSettings,
        GenerateMapRequest request,
        SystemTilesForMapSetup systemTilesForMapSetup)
    {
        var mapLayout = CreateInitialMapLayout(mapSettings, systemTilesForMapSetup.EmptyHomeSystemPlaceholder);

        SetHomeSystemPositions(mapLayout, mapSettings, systemTilesForMapSetup, request.HomeSystemDraftType, request.Factions);
        SetMecatolPosition(mapLayout, mapSettings, systemTilesForMapSetup.MecatolRex);
        SetRemainingMapPositionsRandomly(mapLayout, mapSettings, systemTilesForMapSetup.BuildTiles);

        return mapLayout;
    }

    protected static Dictionary<int, SystemTile> CreateInitialMapLayout(IMapSettings mapSettings, SystemTile emptySystemTilePlaceholder)
    {
        return Enumerable.Range(0, mapSettings.MaxMapPositions)
            .ToDictionary(x => x, x => emptySystemTilePlaceholder);
    }

    protected static void SetHomeSystemPositions(
        Dictionary<int, SystemTile> mapLayout,
        IMapSettings mapSettings,
        SystemTilesForMapSetup systemTilesForMapSetup,
        HomeSystemDraftType homeSystemDraftType,
        IReadOnlyCollection<FactionName> factions)
    {
        if (homeSystemDraftType == HomeSystemDraftType.Placeholders)
        {
            mapSettings.HomePositions.ToList().ForEach(x => mapLayout[x] = systemTilesForMapSetup.EmptyHomeSystemPlaceholder);
            return;
        }

        if (homeSystemDraftType == HomeSystemDraftType.RandomFactions)
        {
            var homeTiles = systemTilesForMapSetup.HomeTiles.ToList();
            mapSettings.HomePositions
                .Select((position, index) => new { position, homeTile = homeTiles[index] })
                .ToList()
                .ForEach(x =>
                {
                    mapLayout[x.position] = x.homeTile;
                    homeTiles.Remove(x.homeTile);
                });
            return;
        }

        if (homeSystemDraftType == HomeSystemDraftType.SpecificFactions)
        {
            var specificFactionHomeTiles = systemTilesForMapSetup.HomeTiles
                .Where(x => factions.Contains(x.FactionName))
                .Take(mapSettings.NumberOfPlayers)
                .ToList();

            mapSettings.HomePositions
                .Select((position, index) => new { position, homeTile = specificFactionHomeTiles[index] })
                .ToList()
                .ForEach(x =>
                {
                    mapLayout[x.position] = x.homeTile;
                    specificFactionHomeTiles.Remove(x.homeTile);
                });
        }
    }

    protected static void SetMecatolPosition(Dictionary<int, SystemTile> mapLayout, IMapSettings mapSettings, SystemTile mecatolRexSystemTile)
    {
        mapLayout[mapSettings.MecatolRexPosition] = mecatolRexSystemTile;
    }

    protected static void SetRemainingMapPositionsRandomly(Dictionary<int, SystemTile> mapLayout, IMapSettings mapSettings, IReadOnlyCollection<SystemTile> buildTiles)
    {
        var tiles = buildTiles.ToList();

        foreach (var position in mapLayout.Keys.Where(x => IsValidRemainingPositionToFill(x, mapSettings)))
        {
            var tile = tiles[0];
            mapLayout[position] = tile;
            tiles.Remove(tile);
        }
    }

    private static bool IsValidRemainingPositionToFill(int position, IMapSettings mapSettings)
    {
        return !mapSettings.HomePositions.Contains(position) && position != mapSettings.MecatolRexPosition;
    }
}
