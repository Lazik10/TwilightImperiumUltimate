using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Extensions;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

internal class PreviewMapBuilder(
    IGalaxyBuilder galaxyBuilder,
    ISystemTileSetter systemTileSetter,
    ILogger<PreviewMapBuilder> logger)
    : IPreviewMapBuilder
{
    private readonly IGalaxyBuilder _galaxyBuilder = galaxyBuilder;
    private readonly ISystemTileSetter _systemTileSetter = systemTileSetter;
    private readonly ILogger<PreviewMapBuilder> _logger = logger;

    public async Task<Dictionary<(int X, int Y), Hex>> CreatePreviewMapLayout(IMapSettings mapSettings, SystemTilesForMapSetup systemTilesForMapSetup)
    {
        var galaxy = await _galaxyBuilder.GenerateGalaxy(mapSettings);

        if (mapSettings.MapTemplate == MapTemplate.CustomMap)
        {
            _systemTileSetter.SetTransparentTiles(galaxy, systemTilesForMapSetup.TransparentSystemPlaceholder);
            return galaxy;
        }

        _systemTileSetter.SetHomeSystemTiles(galaxy, systemTilesForMapSetup, mapSettings, HomeSystemDraftType.Placeholders, new List<FactionName>(), new List<string>());
        _systemTileSetter.SetMecatolSystemTile(galaxy, mapSettings, systemTilesForMapSetup.MecatolRex);

        if (mapSettings is IHyperlineSettings hyperlineSettings)
            _systemTileSetter.SetHyperlines(galaxy, hyperlineSettings, systemTilesForMapSetup);

        _logger.LogInformation("{GeneratedMapLayout}", galaxy.GetMapLayoutLog(mapSettings.DimensionX, mapSettings.DimensionY));

        return galaxy;
    }
}
