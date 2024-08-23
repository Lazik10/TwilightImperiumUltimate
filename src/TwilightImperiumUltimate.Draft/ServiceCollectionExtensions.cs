using Microsoft.Extensions.DependencyInjection;
using TwilightImperiumUltimate.Draft.Drafts.ColorDraft;
using TwilightImperiumUltimate.Draft.Drafts.FactionDraft;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.SliceDraft;

namespace TwilightImperiumUltimate.Draft;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDraftServices(
        this IServiceCollection services)
    {
        services.AddScoped<IFactionDraftService, FactionDraftService>();
        services.AddScoped<IColorDraftService, ColorDraftService>();

        services.AddScoped<IGenerateMapService, GenerateMapService>();

        services.AddTransient<ISystemTilesForMapSetupProvider, SystemTilesForMapSetupProvider>();
        services.AddScoped<ISpecificMapSettingProvider, SpecificMapSettingsProvider>();

        services.AddScoped<IPreviewMapBuilder, PreviewMapBuilder>();
        services.AddScoped<IMapBuilder, MapBuilder>();

        services.AddScoped<IGalaxyBuilder, GalaxyBuilder>();
        services.AddScoped<IGalaxyRedPositionSolver, GalaxyRedPositionSolver>();
        services.AddScoped<ISystemTileSetter, SystemTileSetter>();
        services.AddScoped<ISystemTilesForGalaxyDistributionProvider, SystemTilesForGalaxyDistributionProvider>();

        services.AddScoped<IPlacementStyleHandler, PlacementStyleHandler>();
        services.AddScoped<ISliceBalancer, SliceBalancer>();

        services.AddScoped<IDraftSlicesService, DraftSlicesService>();
        services.AddScoped<ISliceSystemTilePreparer, SliceSystemTilePreparer>();
        services.AddScoped<IMiltyDraftSliceBalancer, MiltyDraftSliceBalancer>();
        services.AddScoped<IMiltyDraftSystemTileSetter, MiltyDraftSystemTileSetter>();

        return services;
    }
}

