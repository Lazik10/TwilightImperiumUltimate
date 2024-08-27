using TwilightImperiumUltimate.Web.Services.MiltyDraft.SpecificMapPositions;

namespace TwilightImperiumUltimate.Web.Services.MiltyDraft;

public interface IMiltyDraftSpecificMapPositionProvider
{
    Task<IMapPositions> GetSpecificMapPositions(MapTemplate mapTemplate);
}
