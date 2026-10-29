using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.DraftRooms;

public class DraftRoomLogEntry
{
    public long Id { get; set; }

    public Guid RoomId { get; set; }

    public Guid? ActorParticipantId { get; set; }

    public DraftLogEntryType Type { get; set; }

    public string DataJson { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public DraftRoom Room { get; set; } = default!;
}
