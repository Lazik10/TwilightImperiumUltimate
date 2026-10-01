using System.Text.Json;
using System.Xml;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Contracts.Seo;

namespace TwilightImperiumUltimate.SitemapGenerator;

internal static class Program
{
    private const string SiteHost = "ti4ultimate.com";

    public static async Task Main(string[] args)
    {
        var options = SitemapGeneratorOptions.Parse(args);
        var paths = new SortedSet<string>(SitemapPaths.PublicPagePaths, StringComparer.Ordinal);

        foreach (var faction in Enum.GetValues<FactionName>().Where(faction => faction != FactionName.None))
        {
            paths.Add($"/game/factions/{faction}");
        }

        if (options.ApiBaseUrl is not null)
        {
            using var httpClient = new HttpClient { BaseAddress = options.ApiBaseUrl };
            await AddArchiveDetailPathsAsync(httpClient, "api/map-archive/maps", "/community/maps-archive/map/", paths);
            await AddArchiveDetailPathsAsync(httpClient, "api/slices-archive/drafts", "/community/slices-archive/slice-draft/", paths);
        }

        var sitemapPath = Path.Combine(options.RepositoryRoot, "src", "TwilightImperiumUltimate.Web", "wwwroot", "sitemap.xml");
        var settings = new XmlWriterSettings { Async = true, Encoding = new System.Text.UTF8Encoding(false), Indent = true };
        var siteUri = new UriBuilder(Uri.UriSchemeHttps, SiteHost).Uri;

        await using var stream = File.Create(sitemapPath);
        await using var writer = XmlWriter.Create(stream, settings);
        await writer.WriteStartDocumentAsync();
        await writer.WriteStartElementAsync(null, "urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

        foreach (var path in paths)
        {
            await writer.WriteStartElementAsync(null, "url", null);
            await writer.WriteElementStringAsync(null, "loc", null, new Uri(siteUri, path).AbsoluteUri);
            await writer.WriteEndElementAsync();
        }

        await writer.WriteEndElementAsync();
        await writer.WriteEndDocumentAsync();
    }

    private static async Task AddArchiveDetailPathsAsync(HttpClient httpClient, string endpoint, string routePrefix, ISet<string> paths)
    {
        using var response = await httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);
        if (!document.RootElement.TryGetProperty("data", out var data)
            || !data.TryGetProperty("items", out var items)
            || items.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException($"The response from '{endpoint}' did not contain data.items.");
        }

        foreach (var item in items.EnumerateArray())
        {
            if (!item.TryGetProperty("id", out var id) || !id.TryGetInt32(out var value) || value <= 0)
                continue;

            paths.Add($"{routePrefix}{value}");
        }
    }
}
