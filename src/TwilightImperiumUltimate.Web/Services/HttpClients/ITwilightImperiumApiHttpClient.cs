namespace TwilightImperiumUltimate.Web.Services.HttpClients;

public interface ITwilightImperiumApiHttpClient
{
    Task<TResponse> GetAsync<TResponse>(string endpointPath)
        where TResponse : class, new();

    Task<TResponse> PostAsync<TRequest, TResponse>(string endpointPath, TRequest request)
        where TRequest : class, new()
        where TResponse : class, new();
}
