using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Routing;
using TwilightImperiumUltimate.Contracts.Seo;
using TwilightImperiumUltimate.Web.Helpers.Factions;

namespace TwilightImperiumUltimate.Web.Components.Shared.Seo;

public partial class SeoMetadata : IDisposable
{
    private static readonly Uri SiteUri = new("https://ti4ultimate.com/");

    private static readonly HashSet<string> NoIndexPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "/community/add-map-to-archive",
        "/community/add-slice-draft-to-archive",
        "/community/tigl/register",
        "/community/tigl/report-game",
    };

    private static readonly HashSet<string> PublicPaths = new(SitemapPaths.PublicPagePaths, StringComparer.OrdinalIgnoreCase);

    private static readonly IReadOnlyDictionary<string, (string Title, string Description)> PageDefinitions =
        new Dictionary<string, (string Title, string Description)>(StringComparer.OrdinalIgnoreCase)
        {
            ["/"] = ("Twilight Imperium 4 Tools, Rules and Community | TI4 Ultimate", "Build maps, explore factions and rules, use game tools, and discover the Twilight Imperium 4 community."),
            ["/about"] = ("About TI4 Ultimate | Twilight Imperium 4 Companion", "Learn about TI4 Ultimate, a community-built companion for Twilight Imperium 4 players, organizers, and creators."),
            ["/game/cards"] = ("Twilight Imperium 4 Cards Reference | TI4 Ultimate", "Browse Twilight Imperium 4 action cards, agendas, objectives, relics, and other game card references."),
            ["/game/factions"] = ("Twilight Imperium 4 Factions | TI4 Ultimate", "Compare Twilight Imperium 4 factions, including abilities, units, technologies, leaders, and faction-specific rules."),
            ["/game/planets"] = ("Twilight Imperium 4 Planets Reference | TI4 Ultimate", "Search Twilight Imperium 4 planets by trait, resources, influence, technology specialty, and legendary status."),
            ["/game/statistics"] = ("Twilight Imperium 4 Statistics | TI4 Ultimate", "Explore Twilight Imperium 4 faction and game statistics to inform your next table strategy."),
            ["/game/systemtiles"] = ("Twilight Imperium 4 System Tiles | TI4 Ultimate", "Browse Twilight Imperium 4 system tiles, including planets, anomalies, wormholes, and map-building information."),
            ["/game/technologies"] = ("Twilight Imperium 4 Technologies | TI4 Ultimate", "Browse Twilight Imperium 4 technologies, prerequisites, upgrades, and faction-specific technology options."),
            ["/tools/battle-calculator"] = ("Twilight Imperium 4 Battle Calculator | TI4 Ultimate", "Calculate Twilight Imperium 4 space and ground combat odds before committing your fleets and forces."),
            ["/tools/card-generator"] = ("Twilight Imperium 4 Card Generator | TI4 Ultimate", "Create custom Twilight Imperium 4 cards for homebrew games, variants, and community content."),
            ["/tools/color-picker"] = ("Twilight Imperium 4 Color Picker | TI4 Ultimate", "Choose distinct player colors for your Twilight Imperium 4 game and avoid table conflicts."),
            ["/tools/draft-rooms"] = ("Twilight Imperium 4 Draft Rooms | TI4 Ultimate", "Create and join Twilight Imperium 4 draft rooms for organized setup and player selection."),
            ["/tools/faction-draft"] = ("Twilight Imperium 4 Faction Draft | TI4 Ultimate", "Run a Twilight Imperium 4 faction draft to assign balanced starting factions for your game."),
            ["/tools/game-tracker"] = ("Twilight Imperium 4 Game Tracker | TI4 Ultimate", "Track objectives, scores, turns, and game progress during a Twilight Imperium 4 session."),
            ["/tools/map-generator"] = ("Twilight Imperium 4 Map Generator | TI4 Ultimate", "Generate and evaluate Twilight Imperium 4 maps for your player count, game mode, and preferred layout."),
            ["/tools/milty-draft"] = ("Twilight Imperium 4 Milty Draft | TI4 Ultimate", "Create a Twilight Imperium 4 Milty Draft with factions, slices, speaker order, and player assignments."),
            ["/tools/slice-generator"] = ("Twilight Imperium 4 Slice Generator | TI4 Ultimate", "Generate balanced Twilight Imperium 4 map slices and compare their resources, influence, and specialties."),
            ["/community/async"] = ("Twilight Imperium Async Statistics | TI4 Ultimate", "Explore statistics and rankings from the Twilight Imperium Async community."),
            ["/community/discord"] = ("Twilight Imperium 4 Discord Servers | TI4 Ultimate", "Find Discord communities for Twilight Imperium 4 games, discussion, strategy, and organized play."),
            ["/community/diy"] = ("Twilight Imperium 4 DIY Resources | TI4 Ultimate", "Find community resources for creating Twilight Imperium 4 accessories, organizers, and play aids."),
            ["/community/events"] = ("Twilight Imperium 4 Community Events | TI4 Ultimate", "Discover Twilight Imperium 4 community events, organized play, tournaments, and game-day resources."),
            ["/community/faction-guides"] = ("Twilight Imperium 4 Faction Guides | TI4 Ultimate", "Read community strategy guides for Twilight Imperium 4 factions, openings, technology paths, and table play."),
            ["/community/galaxy-map"] = ("Twilight Imperium 4 Galaxy Map | TI4 Ultimate", "Explore a visual reference map of the Twilight Imperium galaxy and its major locations."),
            ["/community/maps-archive"] = ("Twilight Imperium 4 Map Archive | TI4 Ultimate", "Browse community-created Twilight Imperium 4 maps, ratings, layouts, and downloadable game setups."),
            ["/community/other-websites"] = ("Twilight Imperium 4 Websites | TI4 Ultimate", "Find useful Twilight Imperium 4 websites, tools, communities, and reference resources."),
            ["/community/play-of-the-month"] = ("Twilight Imperium 4 Play of the Month | TI4 Ultimate", "Discover featured Twilight Imperium 4 community games and memorable table experiences."),
            ["/community/slices-archive"] = ("Twilight Imperium 4 Slice Archive | TI4 Ultimate", "Browse community-created Twilight Imperium 4 map slices, ratings, and draft-ready setups."),
            ["/community/tigl"] = ("Twilight Imperium Global League | TI4 Ultimate", "Follow the Twilight Imperium Global League, its competitive games, players, rankings, and achievements."),
            ["/community/tigl/achievements"] = ("TIGL Achievements | TI4 Ultimate", "Browse achievements earned in the Twilight Imperium Global League."),
            ["/community/tigl/games"] = ("TIGL Games | TI4 Ultimate", "Browse reported games and results from the Twilight Imperium Global League."),
            ["/community/tigl/leaderboard"] = ("TIGL Leaderboard | TI4 Ultimate", "See the current Twilight Imperium Global League leaderboard and player ratings."),
            ["/community/tigl/leaders"] = ("TIGL Leaders | TI4 Ultimate", "Explore Twilight Imperium Global League leaders and their competitive records."),
            ["/community/tigl/players"] = ("TIGL Players | TI4 Ultimate", "Browse Twilight Imperium Global League players and their public profiles."),
            ["/community/tigl/rankings"] = ("TIGL Rankings | TI4 Ultimate", "Compare Twilight Imperium Global League player rankings across supported rating systems."),
            ["/community/tigl/statistics"] = ("TIGL Statistics | TI4 Ultimate", "Explore Twilight Imperium Global League statistics, performance trends, and competitive results."),
            ["/rules"] = ("Twilight Imperium 4 Rules Reference | TI4 Ultimate", "Search and browse a practical Twilight Imperium 4 rules reference for play at the table."),
            ["/rules/chatgpt"] = ("Twilight Imperium 4 Rules Assistant | TI4 Ultimate", "Ask focused Twilight Imperium 4 rules questions with the TI4 Ultimate rules assistant."),
            ["/rules/faq"] = ("Twilight Imperium 4 FAQ | TI4 Ultimate", "Find answers to frequently asked Twilight Imperium 4 rules questions and edge cases."),
            ["/rules/resources"] = ("Twilight Imperium 4 Rules Resources | TI4 Ultimate", "Find official and community Twilight Imperium 4 rules documents, references, and learning resources."),
        };

    private static readonly SeoImage WidePreviewImage = new("resources/images/shared/Twilight_imperium_ultimate_background.webp", "Twilight Imperium 4 game board artwork", "image/webp", 3440, 1440, "summary_large_image");

    private static readonly SeoImage FactionsPreviewImage = new("resources/images/shared/social-factions.png", "Twilight Imperium 4 faction reference", "image/png", 1200, 630, "summary_large_image");

    private static readonly SeoImage ToolsPreviewImage = new("resources/images/shared/social-tools.png", "Twilight Imperium 4 map generator", "image/png", 1200, 630, "summary_large_image");

    private static readonly SeoImage TiglPreviewImage = new("resources/images/shared/social-tigl.png", "Twilight Imperium Global League leaderboard", "image/png", 1200, 630, "summary_large_image");

    private static readonly SeoImage RulesPreviewImage = new("resources/images/shared/social-rules.png", "Twilight Imperium 4 rules reference", "image/png", 1200, 630, "summary_large_image");

    private static readonly SeoImage MapsPreviewImage = new("resources/images/shared/social-maps.png", "Twilight Imperium 4 community map archive", "image/png", 1200, 630, "summary_large_image");

    private static readonly SeoImage SlicesPreviewImage = new("resources/images/shared/social-slices.png", "Twilight Imperium 4 community slice archive", "image/png", 1200, 630, "summary_large_image");

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private string CurrentPath { get; set; } = "/";

    private bool HasQueryString { get; set; }

    private string CanonicalPath => string.Equals(CurrentPath, "/news", StringComparison.OrdinalIgnoreCase) ? "/" : CurrentPath;

    private string CanonicalUrl => new Uri(SiteUri, CanonicalPath.TrimStart('/')).AbsoluteUri;

    private SeoImage OpenGraphImage => string.Equals(CurrentPath, "/game/factions", StringComparison.OrdinalIgnoreCase)
        || IsFactionPage(out _)
        ? FactionsPreviewImage
        : CurrentPath.StartsWith("/tools/", StringComparison.OrdinalIgnoreCase)
            ? ToolsPreviewImage
            : CurrentPath.StartsWith("/community/tigl", StringComparison.OrdinalIgnoreCase)
                ? TiglPreviewImage
                : string.Equals(CurrentPath, "/rules", StringComparison.OrdinalIgnoreCase)
                    || CurrentPath.StartsWith("/rules/", StringComparison.OrdinalIgnoreCase)
                    ? RulesPreviewImage
                    : CurrentPath.StartsWith("/community/maps-archive", StringComparison.OrdinalIgnoreCase)
                        ? MapsPreviewImage
                        : CurrentPath.StartsWith("/community/slices-archive", StringComparison.OrdinalIgnoreCase)
                            ? SlicesPreviewImage
                            : WidePreviewImage;

    private string OpenGraphImageUrl => new Uri(SiteUri, OpenGraphImage.Path).AbsoluteUri;

    private string OpenGraphImageAlt => OpenGraphImage.Alt;

    private bool IsNoIndex => HasQueryString
        || string.Equals(CurrentPath, "/news", StringComparison.OrdinalIgnoreCase)
        || CurrentPath.StartsWith("/account/", StringComparison.OrdinalIgnoreCase)
        || NoIndexPaths.Contains(CurrentPath)
        || !IsPublicRoute();

    private string Title => IsFactionPage(out var factionName)
        ? $"{factionName} | TI4 Faction | TI4 Ultimate"
        : IsNumericDetailRoute("/community/maps-archive/map/")
            ? "Twilight Imperium 4 Community Map | TI4 Ultimate"
            : IsNumericDetailRoute("/community/slices-archive/slice-draft/")
                ? "Twilight Imperium 4 Community Slice Draft | TI4 Ultimate"
        : GetPageDefinition().Title;

    private string Description => IsFactionPage(out var factionName)
        ? $"Explore {factionName} abilities, technologies, units, leaders, and rules for Twilight Imperium 4."
        : IsNumericDetailRoute("/community/maps-archive/map/")
            ? "Explore a community-created Twilight Imperium 4 map, including its layout, author, event, rating, and setup details."
            : IsNumericDetailRoute("/community/slices-archive/slice-draft/")
                ? "Explore a community-created Twilight Imperium 4 slice draft, including its slices, author, event, rating, and setup details."
        : GetPageDefinition().Description;

    private string StructuredData => JsonSerializer.Serialize(new object[]
    {
        new
        {
            @context = "https://schema.org",
            @type = IsCollectionPage() ? "CollectionPage" : "WebPage",
            name = Title,
            description = Description,
            url = CanonicalUrl,
            isPartOf = new
            {
                @type = "WebSite",
                name = "TI4 Ultimate",
                url = SiteUri.AbsoluteUri,
            },
        },
        new
        {
            @context = "https://schema.org",
            @type = "BreadcrumbList",
            itemListElement = GetBreadcrumbItems(),
        },
    });

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }

    protected override void OnInitialized()
    {
        UpdateLocation(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs args)
    {
        UpdateLocation(args.Location);
        _ = InvokeAsync(StateHasChanged);
    }

    private void UpdateLocation(string location)
    {
        var uri = new Uri(location);
        CurrentPath = uri.AbsolutePath.TrimEnd('/');
        if (string.IsNullOrEmpty(CurrentPath))
            CurrentPath = "/";

        HasQueryString = !string.IsNullOrEmpty(uri.Query);
    }

    private bool IsPublicRoute()
    {
        if (PublicPaths.Contains(CurrentPath))
            return true;

        if (IsFactionPage(out var factionName))
            return FactionNameAliasResolver.TryResolve(factionName.Replace(" ", string.Empty, StringComparison.Ordinal), out _);

        return IsNumericDetailRoute("/community/maps-archive/map/")
            || IsNumericDetailRoute("/community/slices-archive/slice-draft/");
    }

    private bool IsNumericDetailRoute(string prefix) => CurrentPath.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
        && int.TryParse(CurrentPath[prefix.Length..], out _);

    private bool IsFactionPage(out string factionName)
    {
        const string factionPrefix = "/game/factions/";

        factionName = string.Empty;
        if (!CurrentPath.StartsWith(factionPrefix, StringComparison.OrdinalIgnoreCase))
            return false;

        factionName = Humanize(CurrentPath[factionPrefix.Length..]);
        return !string.IsNullOrWhiteSpace(factionName);
    }

    private bool IsCollectionPage() => CurrentPath is "/"
        or "/game/cards"
        or "/game/factions"
        or "/game/planets"
        or "/game/systemtiles"
        or "/game/technologies"
        or "/community/faction-guides"
        or "/community/maps-archive"
        or "/community/other-websites"
        or "/community/slices-archive"
        or "/community/tigl/achievements"
        or "/community/tigl/games"
        or "/community/tigl/leaders"
        or "/community/tigl/players"
        or "/rules/faq"
        or "/rules/resources";

    private object[] GetBreadcrumbItems()
    {
        var segments = CanonicalPath.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        var items = new List<object>
        {
            new { @type = "ListItem", position = 1, name = "Home", item = SiteUri.AbsoluteUri },
        };
        var pathBuilder = new StringBuilder();

        for (var index = 0; index < segments.Length; index++)
        {
            pathBuilder.Append('/').Append(segments[index]);
            var currentPath = pathBuilder.ToString();
            items.Add(new
            {
                @type = "ListItem",
                position = index + 2,
                name = index == segments.Length - 1 ? Title : GetBreadcrumbLabel(currentPath),
                item = new Uri(SiteUri, currentPath.TrimStart('/')).AbsoluteUri,
            });
        }

        return items.ToArray();
    }

    private static string GetBreadcrumbLabel(string path) => path switch
    {
        "/game" => "Game Reference",
        "/tools" => "Tools",
        "/community" => "Community",
        "/rules" => "Rules",
        "/community/tigl" => "TIGL",
        "/community/maps-archive" => "Map Archive",
        "/community/slices-archive" => "Slice Archive",
        _ => Humanize(path[(path.LastIndexOf('/') + 1)..]),
    };

    private (string Title, string Description) GetPageDefinition() =>
        PageDefinitions.TryGetValue(CanonicalPath, out var definition)
            ? definition
            : ("TI4 Ultimate", "Twilight Imperium 4 tools, rules, game references, and community resources.");

    private static string Humanize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "TI4 Ultimate";

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

    private sealed record SeoImage(string Path, string Alt, string ContentType, int Width, int Height, string TwitterCard);
}