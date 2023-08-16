using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TwilightImperiumUltimate.Business.Services.Database;
using TwilightImperiumUltimate.DataAccess.Services;

namespace TwilightImperiumUltimate.Business.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterBusinessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.RegisterDataAccessLayer(configuration);

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddSingleton<DatabaseService>();

        return services;
    }
}
