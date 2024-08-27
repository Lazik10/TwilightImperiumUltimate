using TwilightImperiumUltimate.Web.Services.MiltyDraft.SpecificMapPositions;

namespace TwilightImperiumUltimate.Web.Services.MiltyDraft;

public class MiltyDraftSpecificMapPositionProvider : IMiltyDraftSpecificMapPositionProvider
{
    public Task<IMapPositions> GetSpecificMapPositions(MapTemplate mapTemplate)
    {
        return mapTemplate switch
        {
            MapTemplate.FivePlayersMediumHyperlineMap => Task.FromResult(new FivePlayersMapPositions() as IMapPositions),
            MapTemplate.SixPlayersMediumMap => Task.FromResult(new SixPlayersMapPositions() as IMapPositions),
            MapTemplate.SevenPlayersLargeWarpMap => Task.FromResult(new SevenPlayersMapPositions() as IMapPositions),
            MapTemplate.EightPlayersLargeWarpMap => Task.FromResult(new EightPlayersMapPositions() as IMapPositions),
            _ => throw new NotImplementedException(),
        };
    }
}
