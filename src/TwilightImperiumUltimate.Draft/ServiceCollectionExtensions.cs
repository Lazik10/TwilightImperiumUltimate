using Microsoft.Extensions.DependencyInjection;
using TwilightImperiumUltimate.Draft.Drafts.ColorDraft;
using TwilightImperiumUltimate.Draft.Drafts.FactionDraft;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapBuilders;

namespace TwilightImperiumUltimate.Draft;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDraftServices(
        this IServiceCollection services)
    {
        services.AddScoped<IFactionDraftService, FactionDraftService>();
        services.AddScoped<IColorDraftService, ColorDraftService>();
        services.AddScoped<IGenerateMapService, GenerateMapService>();
        services.AddScoped<ISpecificMapBuilderProvider, SpecificMapBuilderProvider>();
        services.AddTransient<ISystemTilesForMapSetupProvider, SystemTilesForMapSetupProvider>();
        services.AddScoped<ISpecificMapSettingProvider, SpecificMapSettingsProvider>();
        services.AddScoped<IGenerateMapService, GenerateMapService>();
        services.AddScoped<IPreviewMapBuilder, PreviewMapBuilder>();

        return services;
    }
}

