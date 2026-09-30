using Microsoft.AspNetCore.Components.Routing;
using System.Text;

namespace TwilightImperiumUltimate.Web.Components.Shared.Seo;

public partial class Breadcrumbs : IDisposable
{
    private static readonly IReadOnlyDictionary<string, string> Labels = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["/game"] = "Game Reference",
        ["/tools"] = "Tools",
        ["/community"] = "Community",
        ["/rules"] = "Rules",
        ["/community/tigl"] = "TIGL",
        ["/community/maps-archive"] = "Map Archive",
        ["/community/slices-archive"] = "Slice Archive",
        ["/game/systemtiles"] = "System Tiles",
        ["/community/async"] = "Async Statistics",
        ["/community/other-websites"] = "Websites",
        ["/community/play-of-the-month"] = "Play of the Month",
    };

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private List<BreadcrumbItem> Items { get; set; } = new();

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }

    protected override void OnInitialized()
    {
        UpdateItems(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private static string GetLabel(string path) => Labels.TryGetValue(path, out var label)
        ? label
        : Humanize(path[(path.LastIndexOf('/') + 1)..]);

    private static string Humanize(string value)
    {
        var words = new List<char>(value.Length + 8);
        for (var index = 0; index < value.Length; index++)
        {
            var character = value[index];
            if (character is '-' or '_')
            {
                words.Add(' ');
                continue;
            }

            if (index > 0 && char.IsUpper(character) && char.IsLower(value[index - 1]))
                words.Add(' ');

            words.Add(character);
        }

        return string.Join(' ', new string(words.ToArray()).Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs args)
    {
        UpdateItems(args.Location);
        _ = InvokeAsync(StateHasChanged);
    }

    private void UpdateItems(string location)
    {
        var path = new Uri(location).AbsolutePath.TrimEnd('/');
        if (string.IsNullOrEmpty(path) || path.StartsWith("/account", StringComparison.OrdinalIgnoreCase))
        {
            Items = new List<BreadcrumbItem>();
            return;
        }

        var items = new List<BreadcrumbItem> { new("Home", "/", false) };
        var segments = path.TrimStart('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        var pathBuilder = new StringBuilder();

        foreach (var segment in segments)
        {
            pathBuilder.Append('/').Append(segment);
            var currentPath = pathBuilder.ToString();
            items.Add(new BreadcrumbItem(GetLabel(currentPath), currentPath, false));
        }

        Items = items.Select((item, index) => item with { IsCurrent = index == items.Count - 1 }).ToList();
    }

    private sealed record BreadcrumbItem(string Label, string Url, bool IsCurrent);
}