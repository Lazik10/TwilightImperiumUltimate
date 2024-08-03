using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

public class GenerateMapService(
    IMapBuilder mapBuilder,
    IPreviewMapBuilder previewMapBuilder,
    ISpecificMapSettingProvider mapSettingProvider,
    ISystemTilesForMapSetupProvider systemTilesForMapSetupProvider)
    : IGenerateMapService
{
    private readonly IMapBuilder _mapBuilder = mapBuilder;
    private readonly IPreviewMapBuilder _previewMapBuilder = previewMapBuilder;
    private readonly ISpecificMapSettingProvider _mapSettingProvider = mapSettingProvider;
    private readonly ISystemTilesForMapSetupProvider _systemTilesForMapSetupProvider = systemTilesForMapSetupProvider;

    public async Task<GeneratedMapLayout> GenerateMapLayout(GenerateMapRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var mapSettings = await _mapSettingProvider.GetMapSettingsForSpecificTemplate(request.MapTemplate);
        var systemTilesForMapSetup = await _systemTilesForMapSetupProvider.GetSystemTilesForMapSetup(mapSettings, cancellationToken);

        var generatedMapLayout = request.PreviewMap ?
            await _previewMapBuilder.CreatePreviewMapLayout(mapSettings, systemTilesForMapSetup) :
            await _mapBuilder.CreateMapLayout(mapSettings, request, systemTilesForMapSetup);

        return new GeneratedMapLayout(generatedMapLayout);
    }
}
