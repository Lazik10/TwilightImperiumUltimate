namespace TwilightImperiumUltimate.Web.Services.HttpClients;

public interface ITwilightImperiumApiHttpClient
{
    Task<(TResponse? Response, HttpStatusCode StatusCode)> GetAsync<TResponse>(string endpointPath, string query = "", CancellationToken cancellationToken = default)
        where TResponse : class;

    Task<bool> GetAsync(string query, string endpointPath, CancellationToken cancellationToken = default);

    Task<(TResponse Response, HttpStatusCode StatusCode)> PostAsync<TRequest, TResponse>(string endpointPath, TRequest request, CancellationToken cancellationToken = default)
        where TRequest : class
        where TResponse : class, new();

    Task<(ApiResponse<TDto> Response, HttpStatusCode StatusCode)> PostApiAsync<TRequest, TDto>(string endpointPath, TRequest request, CancellationToken cancellationToken = default)
        where TRequest : class
        where TDto : class;

    Task<(ApiResponse<TResponse> Response, HttpStatusCode StatusCode)> PutAsync<TRequest, TResponse>(string endpointPath, TRequest request, CancellationToken cancellationToken)
        where TRequest : class
        where TResponse : class;

    Task<(TResponse Response, HttpStatusCode StatusCode)> DeleteAsync<TRequest, TResponse>(string endpointPath, TRequest request, CancellationToken cancellationToken = default)
        where TRequest : class
        where TResponse : class, new();

    Task SetAuthorizationHeaderAsync(CancellationToken cancellationToken, string? token = null);
}
