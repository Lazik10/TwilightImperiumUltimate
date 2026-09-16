using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public static class AsyncStatisticsSnapshotRetention
{
    public static IReadOnlyList<int> SelectIdsToDelete(
        IEnumerable<AsyncStatisticsSnapshot> snapshots,
        int retainedSnapshotCount)
    {
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(retainedSnapshotCount);

        return snapshots
            .OrderByDescending(snapshot => snapshot.SnapshotVersion)
            .Skip(retainedSnapshotCount)
            .Select(snapshot => snapshot.Id)
            .ToList();
    }
}
