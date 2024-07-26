using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TwilightImperiumUltimate.DataAccess;
using TwilightImperiumUltimate.Draft;

namespace TwilightImperiumUltimate.Business;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterBusinessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.RegisterDataAccessLayer(configuration);
        services.RegisterDraftServices();

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
