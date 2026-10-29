using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.DraftRooms;

/// <summary>Stores an immutable public snapshot independently of the source room.</summary>
public sealed class DraftResult
{
    public Guid Id { get; set; }

    public string ShareToken { get; set; } = string.Empty;

    public DraftTypeId DraftType { get; set; }

    public string RoomName { get; set; } = string.Empty;

    public Guid? SourceRoomId { get; set; }

    public string? OwnerUserId { get; set; }

    public DateTime FinishedAtUtc { get; set; }

    public int PayloadVersion { get; set; } = 1;

    public string PayloadJson { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public ICollection<DraftResultPlayer> Players { get; set; } = new List<DraftResultPlayer>();
}
