namespace TwilightImperiumUltimate.Core.Entities.DraftRooms;

/// <summary>Associates snapshot seats with account history without changing public snapshot names.</summary>
public sealed class DraftResultPlayer
{
    public Guid ResultId { get; set; }

    public int SeatIndex { get; set; }

    public string DisplayName { get; set; } = string.Empty;

    public string? UserId { get; set; }
}
