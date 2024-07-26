using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapBuilders;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

public class GenerateMapService(
    ISpecificMapBuilderProvider mapBuilderProvider,
    IPreviewMapBuilder previewMapBuilder,
    ISpecificMapSettingProvider mapSettingProvider,
    ISystemTilesForMapSetupProvider systemTilesForMapSetupProvider)
    : IGenerateMapService
{
    private readonly ISpecificMapBuilderProvider _mapBuilderProvider = mapBuilderProvider;
    private readonly IPreviewMapBuilder _previewMapBuilder = previewMapBuilder;
    private readonly ISpecificMapSettingProvider _mapSettingProvider = mapSettingProvider;
    private readonly ISystemTilesForMapSetupProvider _systemTilesForMapSetupProvider = systemTilesForMapSetupProvider;

    public async Task<GeneratedMapLayout> GenerateMapLayout(GenerateMapRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var mapBuilder = await _mapBuilderProvider.GetMapBuilderForSpecificTemplate(request.MapTemplate);
        var mapSettings = await _mapSettingProvider.GetMapSettingsForSpecificTemplate(request.MapTemplate);
        var systemTilesForMapSetup = await _systemTilesForMapSetupProvider.GetSystemTilesForMapSetup(cancellationToken);

        if (request.PreviewMap)
            return new GeneratedMapLayout(await _previewMapBuilder.BuildPreviewMapLayout(mapSettings, systemTilesForMapSetup));

        return new GeneratedMapLayout(await mapBuilder.BuildMapLayout(mapSettings, request, systemTilesForMapSetup));
    }
}
