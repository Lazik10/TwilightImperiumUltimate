using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Resolves draft-room identities and evaluates room permission policies.
/// </summary>
public interface IDraftRoomAuthorizer
{
    /// <summary>
    /// Resolves a caller to an active participant in the room.
    /// </summary>
    /// <param name="room">The room whose participants are checked.</param>
    /// <param name="caller">The account or guest identity to resolve.</param>
    /// <returns>The active actor, or <see langword="null"/> when the caller is forbidden.</returns>
    DraftRoomActor? Resolve(DraftRoom room, DraftRoomCaller caller);

    /// <summary>
    /// Determines whether the actor can add or remove seats in the lobby.
    /// </summary>
    /// <param name="actor">The room actor requesting the operation.</param>
    /// <returns><see langword="true"/> when the actor is the host or a co-host in the lobby.</returns>
    bool CanManageSeats(DraftRoomActor actor);

    /// <summary>
    /// Determines whether the actor can change room settings in the lobby.
    /// </summary>
    /// <param name="actor">The room actor requesting the operation.</param>
    /// <returns><see langword="true"/> when the actor is the host or a co-host in the lobby.</returns>
    bool CanChangeSettings(DraftRoomActor actor);

    /// <summary>Determines whether the active actor can start or reset a draft, subject to lifecycle rules.</summary>
    bool CanManageDraft(DraftRoomActor actor);

    /// <summary>
    /// Determines whether the actor can kick the target participant.
    /// </summary>
    /// <param name="actor">The room actor requesting the operation.</param>
    /// <param name="target">The participant to kick.</param>
    /// <returns><see langword="true"/> when the actor may kick the target.</returns>
    bool CanKick(DraftRoomActor actor, DraftRoomParticipant target);

    /// <summary>
    /// Determines whether the actor can change the target participant's role.
    /// </summary>
    /// <param name="actor">The room actor requesting the operation.</param>
    /// <param name="target">The participant whose role would change.</param>
    /// <returns><see langword="true"/> when the actor is the host and the target is eligible.</returns>
    bool CanChangeRoles(DraftRoomActor actor, DraftRoomParticipant target);

    /// <summary>
    /// Determines whether the actor can close the room.
    /// </summary>
    /// <param name="actor">The room actor requesting the operation.</param>
    /// <returns><see langword="true"/> when the actor is the host or a co-host.</returns>
    bool CanClose(DraftRoomActor actor);

    /// <summary>
    /// Determines whether the actor can claim or move their own seat.
    /// </summary>
    /// <param name="actor">The room actor requesting the operation.</param>
    /// <returns><see langword="true"/> when the room is in the lobby.</returns>
    bool CanClaimSeat(DraftRoomActor actor);

    /// <summary>Determines whether the actor may claim this specific vacancy, including running-draft reclaim restrictions.</summary>
    bool CanClaimSeat(DraftRoomActor actor, DraftRoomSeat target);

    /// <summary>
    /// Determines whether the actor can unclaim the target seat.
    /// </summary>
    /// <param name="actor">The room actor requesting the operation.</param>
    /// <param name="target">The seat to unclaim.</param>
    /// <returns><see langword="true"/> when the actor may unclaim the occupied seat.</returns>
    bool CanUnclaimSeat(DraftRoomActor actor, DraftRoomSeat target);
}
