using System.Data;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public sealed class AsyncStatisticsSnapshotRepository(
    IDbContextFactory<TwilightImperiumDbContext> context)
    : IAsyncStatisticsSnapshotRepository
{
    private const int RetainedSnapshotCount = 2;

    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;

    public async Task<AsyncStatisticsSnapshot?> GetPublishedAsync(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        return await dbContext.AsyncStatisticsSnapshots
            .AsNoTracking()
            .Where(snapshot => snapshot.IsPublished)
            .OrderByDescending(snapshot => snapshot.SnapshotVersion)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AsyncStatisticsSnapshot> PublishAsync(
        DateTime generatedAtUtc,
        string? sourceDataVersion,
        string payload,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(payload);

        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        await using var transaction = await dbContext.Database.BeginTransactionAsync(
            IsolationLevel.Serializable,
            cancellationToken);

        var latestVersion = await dbContext.AsyncStatisticsSnapshots
            .Select(snapshot => (long?)snapshot.SnapshotVersion)
            .MaxAsync(cancellationToken) ?? 0;

        await dbContext.AsyncStatisticsSnapshots
            .Where(snapshot => snapshot.IsPublished)
            .ExecuteUpdateAsync(update => update.SetProperty(snapshot => snapshot.IsPublished, false), cancellationToken);

        var snapshot = new AsyncStatisticsSnapshot
        {
            GeneratedAtUtc = generatedAtUtc,
            SnapshotVersion = latestVersion + 1,
            SourceDataVersion = sourceDataVersion,
            IsPublished = true,
            Payload = payload,
        };

        dbContext.AsyncStatisticsSnapshots.Add(snapshot);
        await dbContext.SaveChangesAsync(cancellationToken);

        var snapshotCandidates = await dbContext.AsyncStatisticsSnapshots
            .Select(candidate => new AsyncStatisticsSnapshot
            {
                Id = candidate.Id,
                SnapshotVersion = candidate.SnapshotVersion,
            })
            .ToListAsync(cancellationToken);
        var snapshotIdsToDelete = AsyncStatisticsSnapshotRetention.SelectIdsToDelete(snapshotCandidates, RetainedSnapshotCount);

        if (snapshotIdsToDelete.Count > 0)
        {
            await dbContext.AsyncStatisticsSnapshots
                .Where(candidate => snapshotIdsToDelete.Contains(candidate.Id))
                .ExecuteDeleteAsync(cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);

        return snapshot;
    }
}
