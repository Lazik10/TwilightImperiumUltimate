namespace TwilightImperiumUltimate.Web.Components.Charts;

/// <summary>
/// A single ranked row rendered by <see cref="RankingBarChart"/>. <paramref name="CategoryKey"/> must be
/// unique per point (e.g. suffixed with a row index) so Radzen's category scale never collapses rows
/// that share the same display label; <paramref name="Label"/> is what actually gets shown on the axis.
/// </summary>
public sealed record RankingBarPoint(string CategoryKey, string Label, double Value, string Fill, object? Tag, bool IsPrivateProfile = false);
