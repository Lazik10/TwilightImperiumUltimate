using Microsoft.Extensions.DependencyInjection;
using TwilightImperiumUltimate.Tigl.AsyncRating;
using TwilightImperiumUltimate.Tigl.Glicko2Rating;
using TwilightImperiumUltimate.Tigl.Services;
using TwilightImperiumUltimate.Tigl.TrueSkillRating;

namespace TwilightImperiumUltimate.Tigl;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterTiglServices(this IServiceCollection services)
    {
        services.AddScoped<IEndOfSeasonProcessor, EndOfSeasonProcessor>();
        services.AddScoped<ITiglFactionValidator, TiglFactionValidator>();
        services.AddScoped<ITiglLeagueResolver, TiglLeagueResolver>();
        services.AddScoped<ITiglMatchInserter, TiglMatchInserter>();
        services.AddScoped<ITiglMatchUsersValidator, TiglMatchUserValidator>();
        services.AddScoped<ITiglRankUpResolver, TiglRankUpResolver>();
        services.AddScoped<ITiglResultValidator, TiglResultValidator>();

        services.AddScoped<IAsyncRatingProcessor, AsyncRatingProcessor>();
        services.AddScoped<IAsyncRatingCalculatorService, AsyncRatingCalculatorService>();
        services.AddScoped<IAsyncPlayerMatchStatsService, AsyncPlayerMatchStatsService>();

        services.AddScoped<IGlickoRatingProcessor, GlickoRatingProcessor>();
        services.AddScoped<IGlickoRatingCalculatorService, GlickoRatingCalculatorService>();
        services.AddScoped<IGlickoPlayerMatchStatsService, GlickoPlayerMatchStatsService>();

        services.AddScoped<ITrueSkillRatingProcessor, TrueSkillRatingProcessor>();
        services.AddScoped<ITrueSkillRatingCalculatorService, TrueSkillRatingCalculatorService>();
        services.AddScoped<ITrueSkillPlayerMatchStatsService, TrueSkillPlayerMatchStatsService>();

        return services;
    }
}
