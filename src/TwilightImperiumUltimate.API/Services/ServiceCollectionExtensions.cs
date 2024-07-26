using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using TwilightImperiumUltimate.API.Email;
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
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

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
            });
        });

        services.AddCors();

        return services;
    }
}
