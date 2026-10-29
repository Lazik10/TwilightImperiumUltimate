using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.DataAccess.UnitOfWork;

/// <summary>Handles occupant changes inside the room transaction before any notification is sent.</summary>
public interface IDraftSeatOccupantChangeHandler
{
    /// <summary>Discards seat-private data when a running seat changes occupant, without revealing its contents.</summary>
    Task HandleAsync(
        DraftRoomScope scope,
        DraftRoomSeat seat,
        Guid? previousParticipantId,
        CancellationToken cancellationToken);
}
