namespace TwilightImperiumUltimate.Core.Entities.DraftRooms;

public class DraftRoomSeat
{
    public Guid Id { get; set; }

    public Guid RoomId { get; set; }

    public int Index { get; set; }

    public Guid? ParticipantId { get; set; }

    public DateTime? ClaimedAtUtc { get; set; }

    public bool HasBeenReleased { get; set; }

    public DraftRoom Room { get; set; } = default!;

    public DraftRoomParticipant? Participant { get; set; }
}
