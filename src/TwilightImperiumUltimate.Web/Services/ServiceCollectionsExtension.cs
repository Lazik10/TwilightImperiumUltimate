using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using System.Reflection;
using TwilightImperiumUltimate.Web.Services.Authentication;
using TwilightImperiumUltimate.Web.Services.Draft;
using TwilightImperiumUltimate.Web.Services.Language;
using TwilightImperiumUltimate.Web.Services.MapGenerators;
using TwilightImperiumUltimate.Web.Services.SliceGenerators;
using TwilightImperiumUltimate.Web.Services.User;

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
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        services.AddScoped(serviceProvider => new HttpClient
        {
            BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
        });

        services.AddScoped<ICultureProvider, CultureProvider>();
        services.AddScoped<IPathProvider, PathProvider>();
        services.AddScoped<IFactionDraftService, FactionDraftService>();
        services.AddScoped<IColorPickerService, ColorPickerService>();
        services.AddScoped<IMapGeneratorService, MapGeneratorService>();
        services.AddScoped<IMapGeneratorSettingsService, MapGeneratorSettingsService>();
        services.AddScoped<IMapDataProvider, MapDataProvider>();
        services.AddScoped<IMapEvaluationService, MapEvaluationService>();
        services.AddScoped<IMapToStringConverter, MapToStringConverter>();
        services.AddScoped<ISliceGeneratorService, SliceGeneratorService>();
        services.AddScoped<ISliceGeneratorSettingsService, SliceGeneratorSettingsService>();

        services.AddScoped<ITwilightImperiumApiHttpClient, TwilightImperiumApiHttpClient>();

        services.AddScoped<TwilightImperiumAuthenticationStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(serviceProvider =>
            serviceProvider.GetRequiredService<TwilightImperiumAuthenticationStateProvider>());
        services.AddAuthorizationCore();
        services.AddCascadingAuthenticationState();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILoginService, LoginService>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
