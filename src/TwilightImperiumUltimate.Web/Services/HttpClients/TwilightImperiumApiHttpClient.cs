using Blazored.LocalStorage;
using Serilog;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TwilightImperiumUltimate.Web.Models.Account;
using TwilightImperiumUltimate.Web.Options.Api;

namespace TwilightImperiumUltimate.Web.Services.HttpClients;

public class TwilightImperiumApiHttpClient : ITwilightImperiumApiHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;
    private readonly ILocalStorageService _localStorageService;

    public TwilightImperiumApiHttpClient(HttpClient httpClient, IConfiguration configuration, ILocalStorageService localStorageService)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var baseUrl = configuration.GetSection(nameof(TwilightImperiumApiOptions))[nameof(TwilightImperiumApiOptions.BaseUrl)];
        _localStorageService = localStorageService;

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(baseUrl!);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
            },
        };
    }

    public async Task<(TResponse? Response, HttpStatusCode StatusCode)> GetAsync<TResponse>(string endpointPath, CancellationToken cancellationToken = default)
        where TResponse : class
    {
        try
        {
            Uri uri = new(string.Concat(_httpClient.BaseAddress, endpointPath));
            await SetAuthorizationHeaderAsync(cancellationToken);

            HttpResponseMessage response = await _httpClient.GetAsync(uri, cancellationToken);

            // Check the status code
            if (response.IsSuccessStatusCode)
            {
                TResponse? result = await response.Content.ReadFromJsonAsync<TResponse>(_options, cancellationToken);
                return (result, response.StatusCode);
            }
            else
            {
                // Handle different status codes as needed
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        Log.Warning("Unauthorized access to endpoint: {EndpointPath}", endpointPath);
                        break;
                    case HttpStatusCode.NotFound:
                        Log.Warning("Endpoint not found: {EndpointPath}", endpointPath);
                        break;
                    case HttpStatusCode.InternalServerError:
                        Log.Warning("Server error while accessing endpoint: {EndpointPath}", endpointPath);
                        break;
                    default:
                        Log.Warning("Error while getting data from endpoint: {EndpointPath}. Status Code: {StatusCode}", endpointPath, response.StatusCode);
                        break;
                }

                return (null, response.StatusCode);
            }
        }
        catch (HttpRequestException ex)
        {
            Log.Warning(ex, "Error while getting data from endpoint: {EndpointPath}", endpointPath);
            return (null, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<bool> GetAsync(string query, string endpointPath, CancellationToken cancellationToken)
    {
        Uri uri = new(string.Concat(_httpClient.BaseAddress, endpointPath, query));
        HttpResponseMessage response = await _httpClient.GetAsync(uri, cancellationToken);

        return response.IsSuccessStatusCode;
    }

    public async Task<(TResponse Response, HttpStatusCode StatusCode)> PostAsync<TRequest, TResponse>(string endpointPath, TRequest request, CancellationToken cancellationToken = default)
    where TRequest : class
    where TResponse : class, new()
    {
        try
        {
            Uri uri = new(string.Concat(_httpClient.BaseAddress, endpointPath));
            await SetAuthorizationHeaderAsync(cancellationToken);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                if (response.Content.Headers.ContentLength == 0)
                {
                    return (new TResponse(), response.StatusCode);
                }

                TResponse? result = await response.Content.ReadFromJsonAsync<TResponse>(_options, cancellationToken);
                return (result ?? new TResponse(), response.StatusCode);
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync(cancellationToken);
                Log.Error("Error while posting data to endpoint: {EndpointPath}. Status Code: {StatusCode}. Error: {Error}", endpointPath, response.StatusCode, errorResponse);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        Log.Warning("Unauthorized access to endpoint: {EndpointPath}", endpointPath);
                        break;
                    case HttpStatusCode.NotFound:
                        Log.Warning("Endpoint not found: {EndpointPath}", endpointPath);
                        break;
                    case HttpStatusCode.InternalServerError:
                        Log.Warning("Server error while accessing endpoint: {EndpointPath}", endpointPath);
                        break;
                    default:
                        Log.Warning("Error while posting data to endpoint: {EndpointPath}. Status Code: {StatusCode}", endpointPath, response.StatusCode);
                        break;
                }

                return (new TResponse(), response.StatusCode);
            }
        }
        catch (HttpRequestException ex)
        {
            Log.Warning(ex, "Error while posting data to endpoint: {EndpointPath}", endpointPath);
            return (new TResponse(), HttpStatusCode.InternalServerError);
        }
    }

    public async Task<(TResponse Response, HttpStatusCode StatusCode)> PutAsync<TRequest, TResponse>(string endpointPath, TRequest request, CancellationToken cancellationToken)
        where TRequest : class
        where TResponse : class, new()
    {
        try
        {
            Uri uri = new(string.Concat(_httpClient.BaseAddress, endpointPath));
            await SetAuthorizationHeaderAsync(cancellationToken);

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                TResponse? result = await response.Content.ReadFromJsonAsync<TResponse>(_options, cancellationToken);
                return (result ?? new TResponse(), response.StatusCode);
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        Log.Warning("Unauthorized access to endpoint: {EndpointPath}", endpointPath);
                        break;
                    case HttpStatusCode.NotFound:
                        Log.Warning("Endpoint not found: {EndpointPath}", endpointPath);
                        break;
                    case HttpStatusCode.InternalServerError:
                        Log.Warning("Server error while accessing endpoint: {EndpointPath}", endpointPath);
                        break;
                    default:
                        Log.Warning("Error while putting data to endpoint: {EndpointPath}. Status Code: {StatusCode}", endpointPath, response.StatusCode);
                        break;
                }

                return (new TResponse(), response.StatusCode);
            }
        }
        catch (HttpRequestException ex)
        {
            Log.Warning(ex, "Error while putting data to endpoint: {EndpointPath}", endpointPath);
            return (new TResponse(), HttpStatusCode.InternalServerError);
        }
        catch (OperationCanceledException ex)
        {
            Log.Warning(ex, "Operation canceled for endpoint: {EndpointPath}", endpointPath);
            return (new TResponse(), HttpStatusCode.RequestTimeout);
        }
    }

    public async Task SetAuthorizationHeaderAsync(CancellationToken cancellationToken, string? token = null)
    {
        if (token is null)
        {
            var accessToken = await GetAccessTokenAsync(cancellationToken);
            if (!string.IsNullOrEmpty(accessToken))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            else
                _httpClient.DefaultRequestHeaders.Authorization = null;
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        var loginResponse = await _localStorageService.GetItemAsync<LoginResponse>("authentication", cancellationToken);

        if (loginResponse is not null && loginResponse.AccessToken is not null)
        {
            return loginResponse.AccessToken;
        }
        else
        {
            return string.Empty;
        }
    }
}
