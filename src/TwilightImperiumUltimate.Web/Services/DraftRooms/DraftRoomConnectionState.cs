namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <summary>
/// Describes the live connection state for a draft room.
/// </summary>
public enum DraftRoomConnectionState
{
    /// <summary>
    /// The connection is not available.
    /// </summary>
    Offline,

    /// <summary>
    /// The initial connection or a reconnect attempt is in progress.
    /// </summary>
    Reconnecting,

    /// <summary>
    /// The hub is connected and subscribed to the room.
    /// </summary>
    Connected,
}
