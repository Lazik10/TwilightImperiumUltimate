using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Core.Interfaces.DraftRooms;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.DataAccess.UnitOfWork;

/// <summary>
/// Serializes draft room operations and persists each operation transactionally.
/// </summary>
public sealed class DraftRoomUnitOfWork(
    IDbContextFactory<TwilightImperiumDbContext> contextFactory,
    IRoomChangeNotifier roomChangeNotifier,
    IEnumerable<IDraftSeatOccupantChangeHandler> occupantChangeHandlers)
    : IDraftRoomUnitOfWork
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _contextFactory = contextFactory
        ?? throw new ArgumentNullException(nameof(contextFactory));

    private readonly IRoomChangeNotifier _roomChangeNotifier = roomChangeNotifier
        ?? throw new ArgumentNullException(nameof(roomChangeNotifier));

    private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _roomGates = new();

    private readonly IDraftSeatOccupantChangeHandler[] _occupantChangeHandlers = occupantChangeHandlers.ToArray();

    /// <summary>Initializes a new instance of the <see cref="DraftRoomUnitOfWork"/> class.Initializes a unit of work without type-specific occupant hooks.</summary>
    public DraftRoomUnitOfWork(
        IDbContextFactory<TwilightImperiumDbContext> contextFactory,
        IRoomChangeNotifier roomChangeNotifier)
        : this(contextFactory, roomChangeNotifier, [])
    {
    }

    /// <inheritdoc />
    public async Task<TResult> ExecuteAsync<TResult>(
        string code,
        Func<DraftRoomScope, Task<TResult>> work,
        CancellationToken cancellationToken,
        bool changesState = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentNullException.ThrowIfNull(work);

        Guid roomId;
        await using (var lookupContext = await this._contextFactory.CreateDbContextAsync(cancellationToken))
        {
            roomId = await lookupContext.DraftRooms
                .Where(room => room.Code == code)
                .Select(room => (Guid?)room.Id)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new DraftRoomNotFoundException();
        }

        var roomGate = this._roomGates.GetOrAdd(roomId, static _ => new SemaphoreSlim(1, 1));
        await roomGate.WaitAsync(cancellationToken);

        TResult result;
        int committedVersion;
        bool hasChanges;
        bool isClosed;
        Guid[] removedParticipantIds;
        DraftRoomLogEntry[] addedLogEntries;

        try
        {
            await using var context = await this._contextFactory.CreateDbContextAsync(cancellationToken);
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            var room = await context.DraftRooms
                .Include(room => room.Participants)
                .Include(room => room.Seats)
                .SingleOrDefaultAsync(item => item.Id == roomId, cancellationToken)
                ?? throw new DraftRoomNotFoundException();

            var scope = new DraftRoomScope(room, context, DateTime.UtcNow);
            var previousStatus = room.Status;
            var previousOccupants = room.Seats.ToDictionary(seat => seat.Id, seat => seat.ParticipantId);
            var trackedLogEntriesBeforeWork = context.ChangeTracker
                .Entries<DraftRoomLogEntry>()
                .Select(entry => entry.Entity)
                .ToArray();
            result = await work(scope).ConfigureAwait(false);
            if (previousStatus == DraftRoomStatus.InProgress && room.Status == DraftRoomStatus.InProgress)
            {
                foreach (var seat in room.Seats)
                {
                    if (previousOccupants.TryGetValue(seat.Id, out var previousId) && previousId != seat.ParticipantId)
                    {
                        seat.HasBeenReleased = true;
                        if (previousId is Guid releasedId)
                        {
                            var released = room.Participants.Single(participant => participant.Id == releasedId);
                            released.DraftSeatId = seat.Id;
                            released.CanReclaimDraftSeat = released.Status == DraftParticipantStatus.Active;
                        }

                        if (seat.ParticipantId is Guid replacementId)
                        {
                            foreach (var former in room.Participants.Where(participant => participant.DraftSeatId == seat.Id))
                            {
                                former.CanReclaimDraftSeat = false;
                            }

                            var replacement = room.Participants.Single(participant => participant.Id == replacementId);
                            replacement.DraftSeatId = seat.Id;
                            replacement.CanReclaimDraftSeat = false;
                        }

                        foreach (var handler in this._occupantChangeHandlers)
                        {
                            await handler.HandleAsync(scope, seat, previousId, cancellationToken).ConfigureAwait(false);
                        }
                    }
                }
            }

            addedLogEntries = context.ChangeTracker
                .Entries<DraftRoomLogEntry>()
                .Select(entry => entry.Entity)
                .Where(entry => !trackedLogEntriesBeforeWork.Contains(entry))
                .ToArray();
            hasChanges = context.ChangeTracker.HasChanges() || addedLogEntries.Length > 0;
            isClosed = context.Entry(room).State == EntityState.Deleted;
            removedParticipantIds = room.Participants
                .Where(participant =>
                    participant.Status == DraftParticipantStatus.Kicked
                    && context.Entry(participant)
                        .Property(item => item.Status)
                        .OriginalValue != DraftParticipantStatus.Kicked)
                .Select(participant => participant.Id)
                .ToArray();

            if (hasChanges)
            {
                room.LastActivityUtc = scope.Now;
                if (changesState)
                {
                    room.Version = checked(room.Version + 1);
                }

                await context.SaveChangesAsync(cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
            committedVersion = room.Version;
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new DraftRoomConflictException(exception);
        }
        finally
        {
            roomGate.Release();
        }

        if (hasChanges)
        {
            if (isClosed)
            {
                await this._roomChangeNotifier.NotifyRoomClosedAsync(roomId).ConfigureAwait(false);
            }
            else
            {
                if (changesState)
                {
                    await this._roomChangeNotifier
                        .NotifyRoomChangedAsync(roomId, committedVersion)
                        .ConfigureAwait(false);
                }

                foreach (var participantId in removedParticipantIds)
                {
                    await this._roomChangeNotifier
                        .NotifyParticipantRemovedAsync(roomId, participantId)
                        .ConfigureAwait(false);
                }

                foreach (var logEntry in addedLogEntries)
                {
                    await this._roomChangeNotifier
                        .NotifyLogEntryAddedAsync(roomId, new DraftLogEntryDto
                        {
                            Id = logEntry.Id,
                            ActorParticipantId = logEntry.ActorParticipantId,
                            Type = logEntry.Type,
                            DataJson = logEntry.DataJson,
                            CreatedAtUtc = logEntry.CreatedAtUtc,
                        })
                        .ConfigureAwait(false);
                }
            }
        }

        return result;
    }
}
