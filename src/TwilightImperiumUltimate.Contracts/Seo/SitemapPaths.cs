namespace TwilightImperiumUltimate.Contracts.Seo;

/// <summary>
/// Defines the stable, public Web routes that are eligible for sitemap inclusion.
/// </summary>
public static class SitemapPaths
{
    /// <summary>
    /// Gets the stable, non-parameterized paths that are publicly indexable.
    /// </summary>
    public static IReadOnlyCollection<string> PublicPagePaths { get; } =
    [
        "/",
        "/about",
        "/game/cards",
        "/game/factions",
        "/game/planets",
        "/game/statistics",
        "/game/systemtiles",
        "/game/technologies",
        "/tools/battle-calculator",
        "/tools/card-generator",
        "/tools/color-picker",
        "/tools/draft-rooms",
        "/tools/faction-draft",
        "/tools/game-tracker",
        "/tools/map-generator",
        "/tools/milty-draft",
        "/tools/slice-generator",
        "/community/async",
        "/community/discord",
        "/community/diy",
        "/community/events",
        "/community/faction-guides",
        "/community/galaxy-map",
        "/community/maps-archive",
        "/community/other-websites",
        "/community/play-of-the-month",
        "/community/slices-archive",
        "/community/tigl",
        "/community/tigl/achievements",
        "/community/tigl/games",
        "/community/tigl/leaderboard",
        "/community/tigl/leaders",
        "/community/tigl/players",
        "/community/tigl/rankings",
        "/community/tigl/statistics",
        "/rules",
        "/rules/chatgpt",
        "/rules/resources",
    ];
}
