using System.Collections;
using System.Reflection;
using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public static class AsyncStatisticsSnapshotValidator
{
    private const int CanonicalLimit = 200;
    private static readonly string[] RequiredCategories =
    {
        nameof(AsyncStatisticsSnapshotDto.General),
        nameof(AsyncStatisticsSnapshotDto.Games),
        nameof(AsyncStatisticsSnapshotDto.Wins),
        nameof(AsyncStatisticsSnapshotDto.VictoryPoints),
        nameof(AsyncStatisticsSnapshotDto.Eliminations),
        nameof(AsyncStatisticsSnapshotDto.Turns),
        nameof(AsyncStatisticsSnapshotDto.Combat),
        nameof(AsyncStatisticsSnapshotDto.Durations),
        nameof(AsyncStatisticsSnapshotDto.Factions),
        nameof(AsyncStatisticsSnapshotDto.Opponents),
        nameof(AsyncStatisticsSnapshotDto.History),
    };

    public static void Validate(AsyncStatisticsSnapshotDto snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        foreach (var categoryName in RequiredCategories)
        {
            var category = typeof(AsyncStatisticsSnapshotDto).GetProperty(categoryName)?.GetValue(snapshot)
                ?? throw new InvalidOperationException($"Async statistics snapshot category '{categoryName}' is missing.");

            foreach (var filterName in new[] { "All", "Tigl", "Custom" })
            {
                var filter = category.GetType().GetProperty(filterName)?.GetValue(category)
                    ?? throw new InvalidOperationException($"Async statistics snapshot category '{categoryName}' is missing filter '{filterName}'.");

                ValidateCollections(filter, $"{categoryName}.{filterName}", new HashSet<object>(ReferenceEqualityComparer.Instance));
            }
        }

        ValidateHistorySnapshot(snapshot.History.All, $"{nameof(AsyncStatisticsSnapshotDto.History)}.All");
        ValidateHistorySnapshot(snapshot.History.Tigl, $"{nameof(AsyncStatisticsSnapshotDto.History)}.Tigl");
        ValidateHistorySnapshot(snapshot.History.Custom, $"{nameof(AsyncStatisticsSnapshotDto.History)}.Custom");
    }

    private static void ValidateHistorySnapshot(AsyncHistoryStatsDto historyStats, string path)
    {
        if (historyStats.GamesHistory.Count > 0 && historyStats.GamesEndedHistory.Count == 0)
        {
            throw new InvalidOperationException($"Async statistics snapshot history '{path}' has games history but no ended-games history.");
        }

        var invalidEndedPoint = historyStats.GamesEndedHistory.FirstOrDefault(point => point.Count <= 0
            || point.Ended <= 0
            || point.Ended != point.Count
            || point.New != 0);
        if (invalidEndedPoint is not null)
        {
            throw new InvalidOperationException(
                $"Async statistics snapshot history '{path}' has invalid ended-games point for {invalidEndedPoint.Year:D4}-{invalidEndedPoint.Month:D2}: "
                + $"Count={invalidEndedPoint.Count}, Ended={invalidEndedPoint.Ended}, New={invalidEndedPoint.New}.");
        }
    }

    private static void ValidateCollections(object value, string path, HashSet<object> visited)
    {
        if (!visited.Add(value))
        {
            return;
        }

        if (value is IEnumerable collection and not string)
        {
            var count = collection.Cast<object?>().Count();
            if (count > CanonicalLimit)
            {
                throw new InvalidOperationException($"Async statistics snapshot ranking '{path}' contains {count} entries; the canonical limit is {CanonicalLimit}.");
            }

            return;
        }

        foreach (var property in value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (property.GetIndexParameters().Length != 0)
            {
                continue;
            }

            var propertyValue = property.GetValue(value);
            if (propertyValue is not null)
            {
                ValidateCollections(propertyValue, $"{path}.{property.Name}", visited);
            }
        }
    }
}
