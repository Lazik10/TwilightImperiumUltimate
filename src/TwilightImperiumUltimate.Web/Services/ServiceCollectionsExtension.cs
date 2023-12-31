﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using TwilightImperiumUltimate.Web.Services.Draft;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Language;
using TwilightImperiumUltimate.Web.Services.MapGenerators;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.Web.Services;

public static class ServiceCollectionsExtension
{
    public static IServiceCollection RegisterServices(
        this IServiceCollection services,
        WebAssemblyHostBuilder builder)
    {
        services.AddBlazoredLocalStorage();
        services.AddScoped<ContextMenuService>();
        services.AddRadzenComponents();

        services.AddScoped(serviceProvider => new HttpClient
        {
            BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
        });

        services.AddScoped<ICultureProvider, CultureProvider>();
        services.AddScoped<IPathProvider, PathProvider>();
        services.AddScoped<IFactionDraftService, FactionDraftService>();
        services.AddScoped<IColorPickerService, ColorPickerService>();
        services.AddScoped<IMapGeneratorService, MapGeneratorService>();
        services.AddSingleton<IMapGeneratorSettingsService, MapGeneratorSettingsService>();

        services.AddScoped<ITwilightImperiumApiHttpClient, TwilightImperiumApiHttpClient>();

        return services;
    }
}
