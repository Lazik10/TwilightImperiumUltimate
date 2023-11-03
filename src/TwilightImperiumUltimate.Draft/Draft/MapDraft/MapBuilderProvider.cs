using TwilightImperiumUltimate.Core.Enums.Galaxy;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Draft.Draft.MapDraft.MapBuilders;
using TwilightImperiumUltimate.Draft.Draft.MapDraft.MapBuilders.Custom;
using TwilightImperiumUltimate.Draft.Draft.MapDraft.MapBuilders.EightPlayers;
using TwilightImperiumUltimate.Draft.Draft.MapDraft.MapBuilders.FourPlayers;
using TwilightImperiumUltimate.Draft.Draft.MapDraft.MapBuilders.SixPlayers;
using TwilightImperiumUltimate.Draft.Draft.MapDraft.MapBuilders.ThreePlayers;

namespace TwilightImperiumUltimate.Draft.Draft.MapDraft;

internal class MapBuilderProvider : IMapBuilderProvider
{
    private readonly ISystemTileRepository _systemTileRepository;

    public MapBuilderProvider(ISystemTileRepository systemTileRepository)
    {
        _systemTileRepository = systemTileRepository;
    }

    public Task<IMapBuilder> GetMapBuilder(MapTemplate mapTemplate)
    {
        return mapTemplate switch
        {
            MapTemplate.CustomMap => Task.FromResult<IMapBuilder>(new CustomMapBuilder(_systemTileRepository)),
            MapTemplate.SmallMapThreePlayers => Task.FromResult<IMapBuilder>(new SmallMapThreePlayersMapBuilder(_systemTileRepository)),
            MapTemplate.SmallMapAlternateThreePlayers => Task.FromResult<IMapBuilder>(new SmallMapAlternateThreePlayersMapBuilder(_systemTileRepository)),
            MapTemplate.TriangleMapThreePlayers => Task.FromResult<IMapBuilder>(new TriangleMapThreePlayersMapBuilder(_systemTileRepository)),
            MapTemplate.TriangleNarrowMapThreePlayers => Task.FromResult<IMapBuilder>(new TriangleNarrowMapThreePlayersMapBuilder(_systemTileRepository)),
            MapTemplate.SnowflakeMapThreePlayers => Task.FromResult<IMapBuilder>(new SnowflakeMapThreePlayersMapBuilder(_systemTileRepository)),
            MapTemplate.TridentMapThreePlayers => Task.FromResult<IMapBuilder>(new TridentMapThreePlayersMapBuilder(_systemTileRepository)),
            MapTemplate.MantaRayMapThreePlayers => Task.FromResult<IMapBuilder>(new MantaRayMapThreePlayersMapBuilder(_systemTileRepository)),
            MapTemplate.MediumMapFourPlayers => Task.FromResult<IMapBuilder>(new MediumMapFourPlayersMapBuilder(_systemTileRepository)),
            MapTemplate.MediumMapSixPlayers => Task.FromResult<IMapBuilder>(new MediumSixPlayersMapBuilder(_systemTileRepository)),
            MapTemplate.LargeMapSixPlayers => Task.FromResult<IMapBuilder>(new LargeMapSixPlayersMapBuilder(_systemTileRepository)),
            MapTemplate.LargeMapEightPlayers => Task.FromResult<IMapBuilder>(new LargeMapEightPlayersMapBuilder(_systemTileRepository)),
            _ => throw new ArgumentOutOfRangeException(nameof(mapTemplate), mapTemplate, null),
        };
    }
}
