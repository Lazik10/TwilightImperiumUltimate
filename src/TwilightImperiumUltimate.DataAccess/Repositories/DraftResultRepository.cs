using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

/// <inheritdoc />
public sealed class DraftResultRepository(IDbContextFactory<TwilightImperiumDbContext> contextFactory) : IDraftResultRepository
{
    /// <inheritdoc />
    public void Add(DraftRoomScope scope, DraftResult result)
    {
        ArgumentNullException.ThrowIfNull(scope);
        ArgumentNullException.ThrowIfNull(result);
        if (result.SourceRoomId != scope.Room.Id)
        {
            throw new ArgumentException("The snapshot must belong to the current room transaction.", nameof(result));
        }

        scope.Context.DraftResults.Add(result);
    }

    /// <inheritdoc />
    public async Task<DraftResult?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.DraftResults.AsNoTracking().SingleOrDefaultAsync(result => result.ShareToken == token, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<DraftResult?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.DraftResults.AsNoTracking().Include(result => result.Players)
            .SingleOrDefaultAsync(result => result.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<DraftResultHistoryItemDto>> GetHistoryAsync(string userId, int skip, int take, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentOutOfRangeException.ThrowIfNegative(skip);
        ArgumentOutOfRangeException.ThrowIfLessThan(take, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(take, 21);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.DraftResults.AsNoTracking()
            .Where(result => result.OwnerUserId == userId || result.Players.Any(player => player.UserId == userId))
            .OrderByDescending(result => result.FinishedAtUtc).ThenByDescending(result => result.Id)
            .Skip(skip).Take(take)
            .Select(result => new
            {
                result.Id,
                result.ShareToken,
                result.DraftType,
                result.RoomName,
                result.FinishedAtUtc,
                CanDelete = context.DraftRooms.Any(room => room.Id == result.SourceRoomId
                    && room.Participants.Any(participant => participant.UserId == userId
                        && participant.Status == DraftParticipantStatus.Active
                        && (participant.Role == DraftRoomRole.Host || participant.Role == DraftRoomRole.CoHost)))
                    || (result.OwnerUserId == userId && !context.DraftRooms.Any(room => room.Id == result.SourceRoomId)),
            })
            .Select(item => new DraftResultHistoryItemDto(item.Id, item.ShareToken, item.DraftType, item.RoomName, item.FinishedAtUtc, item.CanDelete))
            .ToArrayAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string?> GetSourceRoomCodeAsync(Guid? roomId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.DraftRooms.AsNoTracking().Where(room => room.Id == roomId)
            .Select(room => room.Code).SingleOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task<DraftResult?> LoadForRoomAsync(DraftRoomScope scope, Guid id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        return scope.Context.DraftResults.AsTracking()
            .SingleOrDefaultAsync(result => result.Id == id && result.SourceRoomId == scope.Room.Id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteOwnedOrphanAsync(Guid id, string userId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var deleted = await context.DraftResults
            .Where(result => result.Id == id && result.OwnerUserId == userId
                && !context.DraftRooms.Any(room => room.Id == result.SourceRoomId))
            .ExecuteDeleteAsync(cancellationToken);
        return deleted == 1;
    }
}
