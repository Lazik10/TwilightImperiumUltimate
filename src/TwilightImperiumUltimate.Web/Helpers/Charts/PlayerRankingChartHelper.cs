using TwilightImperiumUltimate.Web.Components.Charts;
using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Helpers.Charts;

/// <summary>Builds <see cref="RankingBarPoint"/> rows shared by the Async statistics player-ranking charts.</summary>
public static class PlayerRankingChartHelper
{
    /// <summary>
    /// Converts ranked player rows into chart points. A player <paramref name="idSelector"/> of
    /// <c>0</c> represents a deleted/unknown account, rendered in red with no click-through.
    /// </summary>
    public static IReadOnlyCollection<RankingBarPoint> BuildPoints<TItem>(
        IEnumerable<TItem> items,
        Func<TItem, int> idSelector,
        Func<TItem, string> nameSelector,
        Func<TItem, double> valueSelector,
        TextColor fillColor,
        Func<TItem, string>? categorySuffixSelector = null)
    {
        return items.Select((item, index) =>
        {
            var id = idSelector(item);
            var fill = id == 0 ? TextColor.Red.GetChartFillColor() : fillColor.GetChartFillColor();
            var label = nameSelector(item);
            var suffix = categorySuffixSelector?.Invoke(item);
            if (!string.IsNullOrWhiteSpace(suffix))
                label = $"{label} ({suffix})";

            return new RankingBarPoint(index.ToString(), label, valueSelector(item), fill, id == 0 ? null : id, id == 0);
        }).ToList();
    }
}
