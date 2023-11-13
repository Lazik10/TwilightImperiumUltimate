using Serilog;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using TwilightImperiumUltimate.Web.Models.Drafts;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Settings.AppSettings;

namespace TwilightImperiumUltimate.Web.Services.HttpClients;

public class TwilightImperiumApiHttpClient : ITwilightImperiumApiHttpClient
{
    private readonly HttpClient _httpClient;

    public TwilightImperiumApiHttpClient(HttpClient httpClient, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _httpClient = httpClient;
        var baseUrl = configuration.GetSection(nameof(TwilightImperiumApiSettings))[nameof(TwilightImperiumApiSettings.BaseUrl)];
        _httpClient.BaseAddress = new Uri(baseUrl!);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<TResponse> GetAsync<TResponse>(string endpointPath)
        where TResponse : class, new()
    {
        try
        {
            Uri uri = new(string.Concat(_httpClient.BaseAddress, endpointPath));

            return await _httpClient.GetFromJsonAsync<TResponse>(uri) ?? new TResponse();
        }
        catch (HttpRequestException ex)
        {
            Log.Error(ex, "Error while getting data from endpoint: {endpointPath}", endpointPath);
            return new TResponse();
        }
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpointPath, TRequest request)
        where TRequest : class, new()
        where TResponse : class, new()
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(endpointPath, request);

            return await response.Content.ReadFromJsonAsync<TResponse>() ?? new TResponse();
        }
        catch (HttpRequestException ex)
        {
            Log.Error(ex, "Error while getting data from endpoint: {endpointPath}", endpointPath);
            return new TResponse();
        }
    }
}
