using TwilightImperiumUltimate.Business.Services;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace TwilightImperiumUltimate.API.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options => options.AddEnumsWithValuesFixFilters());

        services.AddCors();

        services.RegisterBusinessLayer(configuration);

        return services;
    }
}
