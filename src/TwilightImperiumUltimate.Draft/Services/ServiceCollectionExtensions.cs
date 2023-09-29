﻿using Microsoft.Extensions.DependencyInjection;
using TwilightImperiumUltimate.Draft.Draft.FactionDraft;

namespace TwilightImperiumUltimate.Business.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDraftServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IFactionDraftService, FactionDraftService>();

        return services;
    }
}

