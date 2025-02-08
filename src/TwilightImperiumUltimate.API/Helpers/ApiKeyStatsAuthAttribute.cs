using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using TwilightImperiumUltimate.API.Options;

namespace TwilightImperiumUltimate.API.Helpers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ApiKeyStatsAuthAttribute : Attribute, IAuthorizationFilter
{
    private const string _invalidApiResponse = "Invalid or missing API Key";
    private const string _apiKeyHeader = "X-API-KEY";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var request = context.HttpContext.Request;
        var serviceProvider = context.HttpContext.RequestServices;

        // Resolve services dynamically
        var logger = serviceProvider.GetRequiredService<ILogger<ApiKeyStatsAuthAttribute>>();
        var statsApiOptions = serviceProvider.GetRequiredService<IOptions<StatsApiOptions>>();

        string apiKey = statsApiOptions.Value.ApiKey ?? string.Empty;
        if (string.IsNullOrEmpty(apiKey) || apiKey.Length <= 0)
        {
            logger.LogWarning(_invalidApiResponse);
        }

        if (!request.Headers.TryGetValue(_apiKeyHeader, out var extractedApiKey)
            || extractedApiKey != apiKey
            || apiKey.Length <= 0)
        {
            context.Result = new UnauthorizedObjectResult(new { message = _invalidApiResponse });
        }
    }
}

