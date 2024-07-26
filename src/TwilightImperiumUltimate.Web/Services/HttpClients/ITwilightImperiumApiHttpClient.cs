namespace TwilightImperiumUltimate.Web.Services.HttpClients;

public interface ITwilightImperiumApiHttpClient
{
    Task<(TResponse? Response, HttpStatusCode StatusCode)> GetAsync<TResponse>(string endpointPath, CancellationToken cancellationToken = default)
        where TResponse : class;

    Task<bool> GetAsync(string query, string endpointPath, CancellationToken ct);

    Task<(TResponse Response, HttpStatusCode StatusCode)> PostAsync<TRequest, TResponse>(string endpointPath, TRequest request, CancellationToken ct)
        where TRequest : class, new()
        where TResponse : class, new();

    Task<(TResponse Response, HttpStatusCode StatusCode)> PutAsync<TRequest, TResponse>(string endpointPath, TRequest request, CancellationToken ct)
        where TRequest : class, new()
        where TResponse : class, new();

    Task SetAuthorizationHeaderAsync(CancellationToken ct, string? token = null);
}
