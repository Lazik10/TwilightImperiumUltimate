using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>Rechecks activity under each room's operation gate before deleting it and notifying connected clients.</summary>
public sealed class DraftRoomCleanupService(
    IDraftRoomRepository repository,
    IDraftRoomUnitOfWork unitOfWork,
    IOptions<DraftRoomRetentionOptions> options,
    TimeProvider timeProvider,
    ILogger<DraftRoomCleanupService> logger)
{
    /// <summary>Deletes inactive rooms in bounded batches without loading or deleting result snapshots.</summary>
    public async Task<int> CleanupAsync(CancellationToken cancellationToken)
    {
        var days = options.Value.InactiveRoomRetentionDays;
        ArgumentOutOfRangeException.ThrowIfNegative(days);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(days, 36500);
        var cutoff = timeProvider.GetUtcNow().UtcDateTime.AddDays(-days);
        var deleted = 0;
        while (true)
        {
            var codes = await repository.GetInactiveRoomCodesAsync(cutoff, 100, cancellationToken);
            if (codes.Count == 0)
            {
                break;
            }

            foreach (var code in codes)
            {
                try
                {
                    var removed = await unitOfWork.ExecuteAsync(
                        code,
                        scope =>
                        {
                            if (scope.Room.LastActivityUtc >= cutoff)
                            {
                                return Task.FromResult(false);
                            }

                            scope.Context.DraftRooms.Remove(scope.Room);
                            return Task.FromResult(true);
                        },
                        cancellationToken);
                    if (removed)
                    {
                        deleted++;
                    }
                }
                catch (DraftRoomNotFoundException exception)
                {
                    logger.LogDebug(exception, "Inactive room was already removed before cleanup acquired its operation gate");
                }
            }
        }

        logger.LogInformation("Draft room cleanup completed. DeletedRoomCount={DeletedRoomCount}, CutoffUtc={CutoffUtc}", deleted, cutoff);
        return deleted;
    }
}
