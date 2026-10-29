using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>Discards private choices inside the occupant-change transaction without logging their contents.</summary>
public sealed class FactionPickOccupantChangeHandler(IDraftRoomLog log) : IDraftSeatOccupantChangeHandler
{
    /// <inheritdoc />
    public async Task HandleAsync(
        DraftRoomScope scope,
        DraftRoomSeat seat,
        Guid? previousParticipantId,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(scope);
        ArgumentNullException.ThrowIfNull(seat);
        if (scope.Room.DraftType != nameof(DraftTypeId.FactionPick))
        {
            return;
        }

        var state = await scope.Context.FactionPickStates.AsTracking()
            .SingleAsync(item => item.RoomId == scope.Room.Id, cancellationToken);
        var discarded = await scope.Context.FactionPickSelections.AsTracking()
            .Where(choice => choice.RoomId == scope.Room.Id && choice.SeatId == seat.Id
                && ((choice.Kind == FactionPickSelectionKind.Ban && !state.BansRevealed)
                    || (choice.Kind == FactionPickSelectionKind.Pick && choice.State == FactionPickSelectionState.Pending)))
            .ToArrayAsync(cancellationToken);
        scope.Context.FactionPickSelections.RemoveRange(discarded);
        if (!state.BansRevealed)
        {
            state.LockedBanSeatIds.Remove(seat.Id);
        }

        log.Append(scope, DraftLogEntryType.SeatChoicesDiscarded, null, new { SeatId = seat.Id, seat.Index });
    }
}
