using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwilightImperiumUltimate.DataAccess.Services;

namespace TwilightImperiumUltimate.Business.Services;

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

        return services;
    }
}
