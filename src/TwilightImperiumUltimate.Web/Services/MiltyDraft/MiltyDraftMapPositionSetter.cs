namespace TwilightImperiumUltimate.Web.Services.MiltyDraft;

public class MiltyDraftMapPositionSetter(
    IMiltyDraftSpecificMapPositionProvider miltyDraftSpecificMapPositionProvider)
    : IMiltyDraftMapPositionSetter
{
    private readonly IMiltyDraftSpecificMapPositionProvider _miltyDraftSpecificMapPositionProvider = miltyDraftSpecificMapPositionProvider;

    public async Task SetSliceToMapPositions(
        SliceModel slice,
        MapTemplate mapTemplate,
        MiltyDraftInitiative miltyDraftInitiative,
        Dictionary<int, SystemTileModel> generatedMapPositionsWithSystemTiles)
    {
        var specificMapPositions = await _miltyDraftSpecificMapPositionProvider.GetSpecificMapPositions(mapTemplate);
        var mapPositions = specificMapPositions.SlicePositions[miltyDraftInitiative];

        for (int i = 0; i < mapPositions.Count; i++)
        {
            generatedMapPositionsWithSystemTiles[mapPositions[i]] = slice.SystemTiles[i + 1];
        }
    }

    public async Task SetHomePositionWithSpecificFactionSystemTile(
        SystemTileModel systemTileModel,
        MapTemplate mapTemplate,
        MiltyDraftInitiative miltyDraftInitiative,
        Dictionary<int, SystemTileModel> generatedMapPositionsWithSystemTiles)
    {
        var specificMapHomePositions = await _miltyDraftSpecificMapPositionProvider.GetSpecificMapPositions(mapTemplate);
        var homePosition = specificMapHomePositions.HomePositions[miltyDraftInitiative];

        generatedMapPositionsWithSystemTiles[homePosition] = systemTileModel;
    }
}
