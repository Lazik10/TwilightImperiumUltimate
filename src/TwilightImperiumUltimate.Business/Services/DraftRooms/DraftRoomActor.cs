using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Represents an active, authenticated participant acting in a draft room.
/// </summary>
public sealed class DraftRoomActor
{
    internal DraftRoomActor(DraftRoom room, DraftRoomParticipant participant, DraftRoomSeat? seat)
    {
        ArgumentNullException.ThrowIfNull(room);
        ArgumentNullException.ThrowIfNull(participant);

        this.Room = room;
        this.Participant = participant;
        this.Seat = seat;
    }

    /// <summary>
    /// Gets the draft room containing the participant.
    /// </summary>
    public DraftRoom Room { get; }

    /// <summary>
    /// Gets the authenticated room participant.
    /// </summary>
    public DraftRoomParticipant Participant { get; }

    /// <summary>
    /// Gets the participant's seat, or <see langword="null"/> when they are a spectator.
    /// </summary>
    public DraftRoomSeat? Seat { get; }

    /// <summary>
    /// Gets the participant's current room role.
    /// </summary>
    public DraftRoomRole Role => this.Participant.Role;
}
