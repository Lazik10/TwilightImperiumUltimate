using System.Net;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.Extensions.Configuration;
using Moq;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Web.Models.Account;
using TwilightImperiumUltimate.Web.Services.HttpClients;

namespace TwilightImperiumUltimate.ComponentTests.Services.HttpClients;

public sealed class TwilightImperiumApiHttpClientHeaderTests
{
    private static readonly JsonSerializerOptions CamelCaseJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    [Fact]
    public async Task GetAsync_WhenHeadersAreProvided_ShouldSendThemOnlyOnThatRequest()
    {
        using var handler = new CapturingHttpMessageHandler();
        using var httpClient = new HttpClient(handler, disposeHandler: false);
        var apiClient = CreateApiClient(httpClient);

        var result = await apiClient.GetAsync<TestResponse>(
            "api/test",
            headers: new Dictionary<string, string> { ["X-Draft-Guest-Secret"] = "guest-secret" });

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        handler.RequestHeaders.Should().ContainKey("X-Draft-Guest-Secret").WhoseValue.Should().Be("guest-secret");
        httpClient.DefaultRequestHeaders.Contains("X-Draft-Guest-Secret").Should().BeFalse();
    }

    [Fact]
    public async Task PostApiAsync_WhenHeadersAreProvided_ShouldSendThemOnlyOnThatRequest()
    {
        using var handler = new CapturingHttpMessageHandler();
        using var httpClient = new HttpClient(handler, disposeHandler: false);
        var apiClient = CreateApiClient(httpClient);

        var result = await apiClient.PostApiAsync<TestRequest, TestResponse>(
            "api/test",
            new TestRequest { Name = "Room" },
            headers: new Dictionary<string, string> { ["X-Draft-Guest-Secret"] = "guest-secret" });

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        handler.RequestHeaders.Should().ContainKey("X-Draft-Guest-Secret").WhoseValue.Should().Be("guest-secret");
        httpClient.DefaultRequestHeaders.Contains("X-Draft-Guest-Secret").Should().BeFalse();
    }

    [Fact]
    public async Task PutAsync_WhenHeadersAreProvided_ShouldSendThemOnlyOnThatRequest()
    {
        using var handler = new CapturingHttpMessageHandler();
        using var httpClient = new HttpClient(handler, disposeHandler: false);
        var apiClient = CreateApiClient(httpClient);

        var result = await apiClient.PutAsync<TestRequest, TestResponse>(
            "api/test",
            new TestRequest { Name = "Room" },
            new Dictionary<string, string> { ["X-Draft-Guest-Secret"] = "guest-secret" },
            CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        handler.RequestMethod.Should().Be(HttpMethod.Put);
        handler.RequestHeaders.Should().ContainKey("X-Draft-Guest-Secret").WhoseValue.Should().Be("guest-secret");
        httpClient.DefaultRequestHeaders.Contains("X-Draft-Guest-Secret").Should().BeFalse();
    }

    [Fact]
    public async Task SendAsync_WhenHeadersAreProvided_ShouldSendMethodAndHeadersOnThatRequest()
    {
        using var handler = new CapturingHttpMessageHandler();
        using var httpClient = new HttpClient(handler, disposeHandler: false);
        var apiClient = CreateApiClient(httpClient);

        var statusCode = await apiClient.SendAsync(
            HttpMethod.Delete,
            "api/test",
            new Dictionary<string, string> { ["X-Draft-Guest-Secret"] = "guest-secret" },
            CancellationToken.None);

        statusCode.Should().Be(HttpStatusCode.OK);
        handler.RequestMethod.Should().Be(HttpMethod.Delete);
        handler.HasRequestContent.Should().BeFalse();
        handler.RequestHeaders.Should().ContainKey("X-Draft-Guest-Secret").WhoseValue.Should().Be("guest-secret");
        httpClient.DefaultRequestHeaders.Contains("X-Draft-Guest-Secret").Should().BeFalse();
    }

    [Fact]
    public async Task GetAsync_WhenHeadersAreNotProvided_ShouldSendTheRequestWithoutCustomHeaders()
    {
        using var handler = new CapturingHttpMessageHandler();
        using var httpClient = new HttpClient(handler, disposeHandler: false);
        var apiClient = CreateApiClient(httpClient);

        var result = await apiClient.GetAsync<TestResponse>("api/test");

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        handler.RequestHeaders.Should().NotContainKey("X-Draft-Guest-Secret");
    }

    [Fact]
    public async Task PostApiAsync_WhenResponseIsBareDto_ShouldExposeItAsSuccessfulData()
    {
        var expectedEntry = new DraftRoomEntryDto
        {
            RoomId = Guid.NewGuid(),
            Code = "ABC234",
            RoomName = "Room One",
            ParticipantId = Guid.NewGuid(),
            DisplayName = "Guest One",
            IsGuest = true,
            GuestSecret = "guest-secret",
        };
        var responseBody = JsonSerializer.Serialize(expectedEntry, CamelCaseJsonSerializerOptions);
        using var handler = new CapturingHttpMessageHandler(responseBody);
        using var httpClient = new HttpClient(handler, disposeHandler: false);
        var apiClient = CreateApiClient(httpClient);

        var result = await apiClient.PostApiAsync<TestRequest, DraftRoomEntryDto>(
            "api/draft-rooms",
            new TestRequest { Name = "Room One" });

        result.Response.Success.Should().BeTrue();
        result.Response.Data.Should().BeEquivalentTo(expectedEntry);
    }

    [Fact]
    public async Task PostApiAsync_WhenResponseUsesEnvelope_ShouldPreserveEnvelopeData()
    {
        using var handler = new CapturingHttpMessageHandler("""{"success":true,"data":{"value":"response"}}""");
        using var httpClient = new HttpClient(handler, disposeHandler: false);
        var apiClient = CreateApiClient(httpClient);

        var result = await apiClient.PostApiAsync<TestRequest, TestResponse>(
            "api/test",
            new TestRequest { Name = "Room" });

        result.Response.Success.Should().BeTrue();
        result.Response.Data!.Value.Should().Be("response");
    }

    private static TwilightImperiumApiHttpClient CreateApiClient(HttpClient httpClient)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["TwilightImperiumApiOptions:BaseUrl"] = "https://api.example.test/",
            })
            .Build();
        var localStorage = new Mock<ILocalStorageService>();
        localStorage
            .Setup(storage => storage.GetItemAsync<LoginResponse>("authentication", It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginResponse?)null);

        return new TwilightImperiumApiHttpClient(httpClient, configuration, localStorage.Object);
    }

    private sealed class CapturingHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _responseContent;

        public CapturingHttpMessageHandler(string responseContent = """{"value":"response"}""")
        {
            _responseContent = responseContent;
        }

        public Dictionary<string, string> RequestHeaders { get; private set; } = new(StringComparer.OrdinalIgnoreCase);

        public HttpMethod? RequestMethod { get; private set; }

        public bool HasRequestContent { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            this.RequestMethod = request.Method;
            this.HasRequestContent = request.Content is not null;
            RequestHeaders = request.Headers.ToDictionary(
                header => header.Key,
                header => string.Join(",", header.Value),
                StringComparer.OrdinalIgnoreCase);

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    _responseContent,
                    Encoding.UTF8,
                    "application/json"),
            });
        }
    }

    private sealed class TestRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    private sealed class TestResponse
    {
        public string Value { get; set; } = string.Empty;
    }
}
