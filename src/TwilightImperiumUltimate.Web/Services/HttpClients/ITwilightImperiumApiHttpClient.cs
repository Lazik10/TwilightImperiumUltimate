using System.Diagnostics.CodeAnalysis;

namespace TwilightImperiumUltimate.Web.Services.HttpClients;

public interface ITwilightImperiumApiHttpClient
{
    Task<(TResponse? Response, HttpStatusCode StatusCode, string? ETag, DateTimeOffset? SnapshotGeneratedAtUtc)> GetWithValidationAsync<TResponse>(string endpointPath, string query = "", string? etag = null, CancellationToken cancellationToken = default)
        where TResponse : class;

    [SuppressMessage(
        "Usage",
        "CA1068",
        Justification = "The established parameter order is retained for source compatibility.")]
    Task<(TResponse? Response, HttpStatusCode StatusCode)> GetAsync<TResponse>(
        string endpointPath,
        string query = "",
        CancellationToken cancellationToken = default,
        IReadOnlyDictionary<string, string>? headers = null)
        where TResponse : class;

    Task<bool> GetAsync(string query, string endpointPath, CancellationToken cancellationToken = default);

    Task<(TResponse Response, HttpStatusCode StatusCode)> PostAsync<TRequest, TResponse>(string endpointPath, TRequest request, CancellationToken cancellationToken = default)
        where TRequest : class
        where TResponse : class, new();

    [SuppressMessage(
        "Usage",
        "CA1068",
        Justification = "The established parameter order is retained for source compatibility.")]
    Task<(ApiResponse<TDto> Response, HttpStatusCode StatusCode)> PostApiAsync<TRequest, TDto>(
        string endpointPath,
        TRequest request,
        CancellationToken cancellationToken = default,
        IReadOnlyDictionary<string, string>? headers = null)
        where TRequest : class
        where TDto : class;

    Task<(ApiResponse<TResponse> Response, HttpStatusCode StatusCode)> PutAsync<TRequest, TResponse>(string endpointPath, TRequest request, CancellationToken cancellationToken)
        where TRequest : class
        where TResponse : class;

    Task<(ApiResponse<TResponse> Response, HttpStatusCode StatusCode)> PutAsync<TRequest, TResponse>(
        string endpointPath,
        TRequest request,
        IReadOnlyDictionary<string, string>? headers,
        CancellationToken cancellationToken)
        where TRequest : class
        where TResponse : class;

    Task<(TResponse Response, HttpStatusCode StatusCode)> DeleteAsync<TRequest, TResponse>(string endpointPath, TRequest request, CancellationToken cancellationToken = default)
        where TRequest : class
        where TResponse : class, new();

    Task<HttpStatusCode> SendAsync(
        HttpMethod method,
        string endpointPath,
        IReadOnlyDictionary<string, string>? headers,
        CancellationToken cancellationToken);

    Task SetAuthorizationHeaderAsync(CancellationToken cancellationToken, string? token = null);
}
