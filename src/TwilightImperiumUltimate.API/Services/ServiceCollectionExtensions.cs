using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using TwilightImperiumUltimate.Business.Services;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace TwilightImperiumUltimate.API.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Twilight Imperium Ultimate API",
                Version = "v1",
            });
            options.UseInlineDefinitionsForEnums();
        });

        services.AddCors();

        services.RegisterBusinessLayer(configuration);

        return services;
    }
}
