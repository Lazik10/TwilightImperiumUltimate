using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Simpl;
using System.Text.Json.Serialization;
using TwilightImperiumUltimate.API.Discord;
using TwilightImperiumUltimate.API.Discord.Services;
using TwilightImperiumUltimate.API.Email;
using TwilightImperiumUltimate.API.Helpers;
using TwilightImperiumUltimate.API.Jobs;
using TwilightImperiumUltimate.Business;
using TwilightImperiumUltimate.Core.Entities.Users;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.API.Services;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.RegisterBusinessLayer(configuration);

        services.AddAuthentication()
            .AddBearerToken();
        services.AddScoped<ApiKeyStatsAuthAttribute>();

        services.AddAuthorization();
        services.AddEndpointsApiExplorer();
        services.AddIdentityApiEndpoints<TwilightImperiumUser>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
        })
            .AddEntityFrameworkStores<TwilightImperiumDbContext>();
        services.AddScoped<IRoleStore<IdentityRole>, RoleStore<IdentityRole, TwilightImperiumDbContext>>();
        services.AddScoped<IUserStore<TwilightImperiumUser>, UserStore<TwilightImperiumUser, IdentityRole, TwilightImperiumDbContext>>();
        services.AddScoped<IUserClaimStore<TwilightImperiumUser>, UserStore<TwilightImperiumUser, IdentityRole, TwilightImperiumDbContext>>();
        services.AddScoped<IUserRoleStore<TwilightImperiumUser>, UserStore<TwilightImperiumUser, IdentityRole, TwilightImperiumDbContext>>();
        services.AddSingleton<IEmailSender<TwilightImperiumUser>, IdentityEmailSender>();
        services.AddSingleton<SmtpMailer>();

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddCors();

        services.AddMemoryCache();
        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        // IHttpClientFactory for jobs/services
        services.AddHttpClient();

        services.RegisterSwagger();
        services.RegisterDiscordServices();
        services.RegisterQuartzJobs(configuration);

        return services;
    }

    private static IServiceCollection RegisterDiscordServices(this IServiceCollection services)
    {
        services.AddSingleton<DiscordBotClient>();
        services.AddSingleton<IDiscordClient>(sp => sp.GetRequiredService<DiscordBotClient>());
        services.AddSingleton<IHostedService>(sp => sp.GetRequiredService<DiscordBotClient>());

        services.AddTransient<IRankUpPublisher, RankUpPublisher>();
        services.AddTransient<IPrestigePublisher, PrestigePublisher>();
        services.AddTransient<ILeaderPublisher, LeaderPublisher>();
        services.AddTransient<IAchievementPublisher, AchievementPublisher>();
        services.AddTransient<IGameLogPublishWorkflow, GameLogPublishWorkflow>();

        services.AddSingleton<IDiscordRoleChangePublisher, DiscordRoleChangePublisher>();

        return services;
    }

    private static IServiceCollection RegisterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Twilight Imperium Ultimate API",
                Version = "v1",
            });
            options.UseInlineDefinitionsForEnums();
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
            });
            options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "Enter API Key in the format: `X-API-KEY: {your_api_key}`",
                Name = "X-API-KEY",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "ApiKeyScheme",
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                    },
                    Array.Empty<string>()
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey",
                        },
                    },
                    Array.Empty<string>()
                },
            });
        });

        return services;
    }

    private static IServiceCollection RegisterQuartzJobs(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddQuartz(q =>
        {
            q.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();

            var jobKey = new JobKey(nameof(AsyncGameDataJob));
            var defaultCroneExpression = "0 0 * * * ?";
            var croneExpression = configuration.GetValue<string>("AsyncStats:CroneExpression");
            q.AddJob<AsyncGameDataJob>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity($"{nameof(AsyncGameDataJob)}-trigger")
                .WithCronSchedule(croneExpression ?? defaultCroneExpression));

            // Season leaderboard refresh: run immediately at startup, then every 5 minutes
            var seasonLeaderboardJobKey = new JobKey(nameof(SeasonLeaderboardRefreshJob));
            q.AddJob<SeasonLeaderboardRefreshJob>(opts => opts.WithIdentity(seasonLeaderboardJobKey));
            q.AddTrigger(opts => opts
                .ForJob(seasonLeaderboardJobKey)
                .WithIdentity($"{nameof(SeasonLeaderboardRefreshJob)}-trigger")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(60).RepeatForever()));

            // Unified game logs publisher
            var gameLogsJobKey = new JobKey(nameof(GameLogsPublishJob));
            q.AddJob<GameLogsPublishJob>(opts => opts.WithIdentity(gameLogsJobKey));
            q.AddTrigger(opts => opts
                .ForJob(gameLogsJobKey)
                .WithIdentity($"{nameof(GameLogsPublishJob)}-trigger")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever()));
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        services.AddTransient<AsyncGameDataJob>();
        services.AddTransient<SeasonLeaderboardRefreshJob>();
        services.AddTransient<GameLogsPublishJob>();

        return services;
    }
}
