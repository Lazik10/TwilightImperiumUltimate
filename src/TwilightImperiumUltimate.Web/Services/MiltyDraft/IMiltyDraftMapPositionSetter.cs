namespace TwilightImperiumUltimate.Web.Services.MiltyDraft;

public interface IMiltyDraftMapPositionSetter
{
    Task SetHomePositionWithSpecificFactionSystemTile(
        SystemTileModel systemTileModel,
        MapTemplate mapTemplate,
        MiltyDraftInitiative miltyDraftInitiative,
        Dictionary<int, SystemTileModel> generatedMapPositionsWithSystemTiles);

    Task SetSliceToMapPositions(
        SliceModel slice,
        MapTemplate mapTemplate,
        MiltyDraftInitiative miltyDraftInitiative,
        Dictionary<int, SystemTileModel> generatedMapPositionsWithSystemTiles);
}
