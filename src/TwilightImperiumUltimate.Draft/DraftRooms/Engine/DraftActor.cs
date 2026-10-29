using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Draft.DraftRooms.Engine;

/// <summary>
/// Carries identity resolved by the server, never by action payloads.
/// </summary>
/// <param name="ParticipantId">The active participant performing the action.</param>
/// <param name="SeatId">The participant's current seat, when occupied.</param>
/// <param name="Role">The participant's verified room role.</param>
public sealed record DraftActor(Guid ParticipantId, Guid? SeatId, DraftRoomRole Role)
{
    /// <summary>
    /// Gets a value indicating whether the actor can manage a draft.
    /// </summary>
    public bool IsManager => Role is DraftRoomRole.Host or DraftRoomRole.CoHost;
}
