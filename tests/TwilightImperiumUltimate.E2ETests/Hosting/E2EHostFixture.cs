using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using TwilightImperiumUltimate.IntegrationTests.Hosting;
using TwilightImperiumUltimate.IntegrationTests.Infrastructure;
using Xunit;

namespace TwilightImperiumUltimate.E2ETests.Hosting;

/// <summary>Owns a disposable LocalDB database, real API/static Web hosts and an isolated browser process.</summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1001:Types that own disposable fields should be disposable",
    Justification = "xUnit owns this fixture and calls IAsyncLifetime.DisposeAsync, including on initialization failure.")]
public sealed class E2EHostFixture : IAsyncLifetime
{
    private TestDatabase? _database;
    private TestApiFactory? _api;
    private WebApplication? _web;
    private IPlaywright? _playwright;
    private IBrowser? _browser;

    public string WebUrl { get; private set; } = string.Empty;

    public string ApiUrl { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        var initialized = false;
        try
        {
            var root = FindRepositoryRoot();
            var assets = Path.Combine(root, "src", "TwilightImperiumUltimate.Web", "bin", "Release", "net10.0", "publish", "wwwroot");
            if (!File.Exists(Path.Combine(assets, "index.html")))
            {
                throw new InvalidOperationException("Publish the Release Web project before running E2E tests: dotnet publish src\\TwilightImperiumUltimate.Web -c Release.");
            }

            _database = await TestDatabase.CreateAsync();
            var configuration = JsonNode.Parse(await File.ReadAllTextAsync(Path.Combine(assets, "appsettings.json")))
                ?? throw new InvalidOperationException("The published Web configuration is missing.");
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                EnvironmentName = "Testing",
                ContentRootPath = root,
                WebRootPath = assets,
            });
            builder.WebHost.UseUrls("http://127.0.0.1:0");
            _web = builder.Build();
            _web.MapGet("/appsettings.json", () => Results.Json(configuration));
            var contentTypes = new FileExtensionContentTypeProvider();
            contentTypes.Mappings[".dat"] = "application/octet-stream";
            _web.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = contentTypes });
            _web.MapFallbackToFile("index.html");
            await _web.StartAsync();
            this.WebUrl = ServerUrl(_web.Services);

            _api = new TestApiFactory(_database, TimeProvider.System, this.WebUrl.TrimEnd('/'));
            _api.UseKestrel(0);
            _api.StartServer();
            this.ApiUrl = ServerUrl(_api.Services);
            var apiOptions = configuration["TwilightImperiumApiOptions"]?.AsObject()
                ?? throw new InvalidOperationException("The published Web API options are missing.");
            apiOptions["BaseUrl"] = this.ApiUrl;

            using var client = new HttpClient();
            using var response = await client.GetAsync(new Uri($"{this.WebUrl}tools/draft-rooms"));
            response.EnsureSuccessStatusCode();

            _playwright = await Playwright.CreateAsync();
            Assertions.SetDefaultExpectTimeout(30_000);
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true,
                Channel = Environment.GetEnvironmentVariable("DRAFT_ROOMS_E2E_BROWSER_CHANNEL"),
            });
            initialized = true;
        }
        finally
        {
            if (!initialized)
            {
                await this.DisposeAsync();
            }
        }
    }

    public async Task<IBrowserContext> CreateContextAsync()
    {
        var browser = _browser ?? throw new InvalidOperationException("The E2E host has not started.");
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            BaseURL = this.WebUrl,
            Permissions = ["clipboard-read", "clipboard-write"],
        });
        await context.RouteAsync(
            url => !url.StartsWith(this.WebUrl, StringComparison.Ordinal)
                && !url.StartsWith(this.ApiUrl, StringComparison.Ordinal),
            route => route.AbortAsync());
        return context;
    }

    public async Task DisposeAsync()
    {
        try
        {
            if (_browser is not null)
            {
                await _browser.CloseAsync();
                _browser = null;
            }
        }
        finally
        {
            _playwright?.Dispose();
            try
            {
                if (_api is not null)
                {
                    await _api.DisposeAsync();
                    _api = null;
                }
            }
            finally
            {
                try
                {
                    if (_web is not null)
                    {
                        await _web.StopAsync();
                        await _web.DisposeAsync();
                        _web = null;
                    }
                }
                finally
                {
                    if (_database is not null)
                    {
                        await _database.DisposeAsync();
                        _database = null;
                    }
                }
            }
        }
    }

    private static string ServerUrl(IServiceProvider services) =>
        services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>()!.Addresses.Single().TrimEnd('/') + "/";

    private static string FindRepositoryRoot()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
        {
            if (File.Exists(Path.Combine(directory.FullName, "TwilightImperiumUltimate.slnx")))
            {
                return directory.FullName;
            }
        }

        throw new DirectoryNotFoundException("The E2E suite must run from a repository build output.");
    }
}
