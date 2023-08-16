using TwilightImperiumUltimate.Business.Services;

namespace TwilightImperiumUltimate.API.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services.RegisterBusinessLayer(configuration);

        return services;
    }
}
