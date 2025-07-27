using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Simpl;
using System.Reflection;
using System.Text.Json.Serialization;
using TwilightImperiumUltimate.API.Email;
using TwilightImperiumUltimate.API.Helpers;
using TwilightImperiumUltimate.API.Jobs;
using TwilightImperiumUltimate.Business;
using TwilightImperiumUltimate.Core.Entities.Users;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.API.Services;

public static class ServiceCollectionExtensions
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
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        services.AddSingleton<AsyncGameDataJob>();

        services.AddHttpClient();

        services.AddCors();

        return services;
    }
}
