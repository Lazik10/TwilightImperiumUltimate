using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

/// <inheritdoc />
public sealed class FactionPickRepository : IFactionPickRepository
{
    /// <inheritdoc />
    public Task<FactionPickSettings?> LoadSettingsAsync(DraftRoomScope scope, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        return scope.Context.FactionPickSettings.AsTracking()
            .SingleOrDefaultAsync(settings => settings.RoomId == scope.Room.Id, cancellationToken);
    }

    /// <inheritdoc />
    public Task<FactionPickState?> LoadStateAsync(DraftRoomScope scope, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        return scope.Context.FactionPickStates.AsTracking()
            .SingleOrDefaultAsync(state => state.RoomId == scope.Room.Id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<FactionPickTurnEntry>> LoadTurnOrderAsync(
        DraftRoomScope scope,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        return await scope.Context.FactionPickTurnOrder.AsTracking()
            .Where(entry => entry.RoomId == scope.Room.Id)
            .OrderBy(entry => entry.Position)
            .ToArrayAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<FactionPickSelection>> LoadSelectionsAsync(DraftRoomScope scope, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        return await scope.Context.FactionPickSelections.AsTracking()
            .Where(selection => selection.RoomId == scope.Room.Id).ToArrayAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task SaveSelectionsAsync(
        DraftRoomScope scope,
        IReadOnlyList<FactionPickSelection> selections,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        ArgumentNullException.ThrowIfNull(selections);
        if (selections.Any(selection => selection.RoomId != scope.Room.Id
            || (selection.SeatId is Guid id && !scope.Room.Seats.Any(seat => seat.Id == id))))
        {
            throw new ArgumentException("Selections must belong to the current room and its seats.", nameof(selections));
        }

        var existing = await this.LoadSelectionsAsync(scope, cancellationToken);
        var wanted = selections.ToDictionary(selection => (selection.SeatId, selection.Kind, selection.Faction));
        foreach (var stored in existing)
        {
            if (!wanted.Remove((stored.SeatId, stored.Kind, stored.Faction), out var incoming))
            {
                scope.Context.FactionPickSelections.Remove(stored);
                continue;
            }

            stored.State = incoming.State;
            stored.Round = incoming.Round;
            stored.DecidedByParticipantId = incoming.DecidedByParticipantId;
            stored.LockedAtUtc = incoming.State == Contracts.Enums.FactionPickSelectionState.Locked
                ? stored.LockedAtUtc ?? scope.Now
                : null;
        }

        foreach (var incoming in wanted.Values)
        {
            incoming.Id = Guid.NewGuid();
            incoming.CreatedAtUtc = scope.Now;
            incoming.LockedAtUtc = incoming.State == Contracts.Enums.FactionPickSelectionState.Locked ? scope.Now : null;
            scope.Context.FactionPickSelections.Add(incoming);
        }
    }

    /// <inheritdoc />
    public async Task SaveSettingsAsync(
        DraftRoomScope scope,
        FactionPickSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        ArgumentNullException.ThrowIfNull(settings);
        if (settings.RoomId != scope.Room.Id)
        {
            throw new ArgumentException("Settings must belong to the current room.", nameof(settings));
        }

        var existing = await this.LoadSettingsAsync(scope, cancellationToken);
        if (existing is null)
        {
            scope.Context.FactionPickSettings.Add(settings);
        }
        else
        {
            scope.Context.Entry(existing).CurrentValues.SetValues(settings);
        }
    }

    /// <inheritdoc />
    public async Task SaveStateAsync(
        DraftRoomScope scope,
        FactionPickState state,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        ArgumentNullException.ThrowIfNull(state);
        if (state.RoomId != scope.Room.Id)
        {
            throw new ArgumentException("State must belong to the current room.", nameof(state));
        }

        var existing = await this.LoadStateAsync(scope, cancellationToken);
        if (existing is null)
        {
            scope.Context.FactionPickStates.Add(state);
        }
        else
        {
            scope.Context.Entry(existing).CurrentValues.SetValues(state);
        }
    }

    /// <inheritdoc />
    public void AddTurnOrder(DraftRoomScope scope, IReadOnlyList<Guid> seatIds)
    {
        ArgumentNullException.ThrowIfNull(scope);
        ArgumentNullException.ThrowIfNull(seatIds);
        if (seatIds.Count != seatIds.Distinct().Count()
            || seatIds.Any(id => !scope.Room.Seats.Any(seat => seat.Id == id)))
        {
            throw new ArgumentException("Turn order must contain distinct seats belonging to the current room.", nameof(seatIds));
        }

        scope.Context.FactionPickTurnOrder.AddRange(seatIds.Select((id, position) => new FactionPickTurnEntry
        {
            RoomId = scope.Room.Id,
            Position = position,
            SeatId = id,
        }));
    }

    /// <inheritdoc />
    public async Task DeleteStateAsync(DraftRoomScope scope, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        var state = await this.LoadStateAsync(scope, cancellationToken);
        if (state is not null)
        {
            scope.Context.FactionPickStates.Remove(state);
        }

        scope.Context.FactionPickTurnOrder.RemoveRange(await this.LoadTurnOrderAsync(scope, cancellationToken));
        scope.Context.FactionPickSelections.RemoveRange(await this.LoadSelectionsAsync(scope, cancellationToken));
    }

    /// <inheritdoc />
    public async Task DeleteAllAsync(DraftRoomScope scope, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        await this.DeleteStateAsync(scope, cancellationToken);
        var settings = await this.LoadSettingsAsync(scope, cancellationToken);
        if (settings is not null)
        {
            scope.Context.FactionPickSettings.Remove(settings);
        }
    }
}
