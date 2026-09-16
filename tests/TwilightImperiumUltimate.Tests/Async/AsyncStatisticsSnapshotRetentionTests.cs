using FluentAssertions;
using TwilightImperiumUltimate.Core.Entities.Async;
using TwilightImperiumUltimate.DataAccess.Repositories;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Async;

public sealed class AsyncStatisticsSnapshotRetentionTests
{
    [Fact]
    public void SelectIdsToDelete_WhenSnapshotsAreUnordered_ShouldRetainNewestVersions()
    {
        var snapshots = new[]
        {
            new AsyncStatisticsSnapshot { Id = 10, SnapshotVersion = 2 },
            new AsyncStatisticsSnapshot { Id = 11, SnapshotVersion = 4 },
            new AsyncStatisticsSnapshot { Id = 12, SnapshotVersion = 1 },
            new AsyncStatisticsSnapshot { Id = 13, SnapshotVersion = 3 },
        };

        var result = AsyncStatisticsSnapshotRetention.SelectIdsToDelete(snapshots, 2);

        result.Should().Equal(10, 12);
    }
}
