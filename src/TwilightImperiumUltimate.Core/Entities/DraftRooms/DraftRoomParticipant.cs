using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.DraftRooms;

public class DraftRoomParticipant
{
    public Guid Id { get; set; }

    public Guid RoomId { get; set; }

    public string? UserId { get; set; }

    public byte[]? SecretHash { get; set; }

    public string DisplayName { get; set; } = string.Empty;

    public string DisplayNameKey { get; set; } = string.Empty;

    public DraftRoomRole Role { get; set; }

    public DraftParticipantStatus Status { get; set; }

    public DateTime JoinedAtUtc { get; set; }

    public DateTime LastSeenUtc { get; set; }

    public Guid? DraftSeatId { get; set; }

    public bool CanReclaimDraftSeat { get; set; }

    public DraftRoom Room { get; set; } = default!;
}
