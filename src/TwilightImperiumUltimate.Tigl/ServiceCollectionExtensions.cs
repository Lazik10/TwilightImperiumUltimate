using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Tigl.AsyncRating;
using TwilightImperiumUltimate.Tigl.Glicko2Rating;
using TwilightImperiumUltimate.Tigl.Services;
using TwilightImperiumUltimate.Tigl.TrueSkillRating;
using System.Linq;
using TwilightImperiumUltimate.Tigl.Achievements;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;
using TwilightImperiumUltimate.Tigl.RankUp;

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
        services.AddScoped<IRankUpResolver, TiglRankUpResolver>();

        services.AddKeyedScoped<ITiglRankUpResolver, TiglProphecyOfKingsRankUpResolver>(TiglLeague.ProphecyOfKings);
        services.AddKeyedScoped<ITiglRankUpResolver, TiglThundersEdgeRankUpResolver>(TiglLeague.ThundersEdge);
        services.AddKeyedScoped<ITiglRankUpResolver, TiglFracturedRankUpResolver>(TiglLeague.Fractured);

        services.AddScoped<IPrestigeRankService, PrestigeRankService>();
        services.AddScoped<ILeaderUpdateService, LeaderUpdateService>();

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

        services.AddScoped<IDecayService, DecayService>();

        services.AddSingleton<IAchievementService, AchievementService>();

        services.AddScoped<ISeasonLeaderboardService, SeasonLeaderboardService>();

        RegisterAchievementEvaluators(services);
        RegisterEndOfSeasonAchievementEvaluators(services);

        return services;
    }

    private static void RegisterAchievementEvaluators(IServiceCollection services)
    {
        // Auto-register all evaluators with AchievementAttribute
        var asm = Assembly.GetExecutingAssembly();
        var evaluatorTypes = asm.GetTypes()
            .Where(t => !t.IsAbstract && typeof(IAchievementEvaluator).IsAssignableFrom(t))
            .ToList();

        foreach (var type in evaluatorTypes.Where(type =>
            type.GetCustomAttribute<AchievementEvaluatorAttribute>() is not null
            || type.GetCustomAttribute<AchievementSummaryEvaluatorAttribute>() is not null))
        {
            services.AddScoped(typeof(IAchievementEvaluator), type);
            services.AddScoped(type);
        }
    }

    private static void RegisterEndOfSeasonAchievementEvaluators(IServiceCollection services)
    {
        // Auto-register all evaluators with AchievementEndOfSeasonEvaluatorAttribute
        var asm = Assembly.GetExecutingAssembly();
        var evaluatorTypes = asm.GetTypes()
            .Where(t => !t.IsAbstract && typeof(IEndOfSeasonAchievementEvaluator).IsAssignableFrom(t))
            .ToList();

        foreach (var type in evaluatorTypes.Where(type =>
            type.GetCustomAttribute<AchievementEndOfSeasonEvaluatorAttribute>() is not null))
        {
            services.AddScoped(typeof(IEndOfSeasonAchievementEvaluator), type);
            services.AddScoped(type);
        }
    }
}
