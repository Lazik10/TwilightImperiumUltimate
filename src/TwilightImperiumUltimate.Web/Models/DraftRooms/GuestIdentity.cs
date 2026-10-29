namespace TwilightImperiumUltimate.Web.Models.DraftRooms;

/// <summary>
/// Stores the browser-held identity needed to re-enter a draft room as a guest.
/// </summary>
public sealed record GuestIdentity(
    Guid RoomId,
    string Code,
    Guid ParticipantId,
    string Secret,
    string DisplayName,
    DateTimeOffset SavedAtUtc)
{
    /// <summary>
    /// Gets the last known room name, when available.
    /// </summary>
    public string? RoomName { get; init; }
}
