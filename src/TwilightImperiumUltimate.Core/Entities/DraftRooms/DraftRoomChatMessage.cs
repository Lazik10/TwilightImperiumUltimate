using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.DraftRooms;

public class DraftRoomChatMessage
{
    public long Id { get; set; }

    public Guid RoomId { get; set; }

    public Guid? ParticipantId { get; set; }

    public ChatMessageKind Kind { get; set; }

    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public DraftRoom Room { get; set; } = default!;
}
