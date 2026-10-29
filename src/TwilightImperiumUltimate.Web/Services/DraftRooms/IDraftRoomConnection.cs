using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <summary>
/// Manages one live SignalR connection to a draft room.
/// </summary>
public interface IDraftRoomConnection : IAsyncDisposable
{
    /// <summary>
    /// Occurs when a newer room version is available.
    /// </summary>
    event Func<int, Task>? RoomChanged;

    /// <summary>
    /// Occurs when a participant's online status changes.
    /// </summary>
    event Func<DraftRoomPresenceUpdateDto, Task>? PresenceChanged;

    /// <summary>
    /// Occurs when a chat message is received in the subscribed room.
    /// </summary>
    event Func<ChatMessageDto, Task>? ChatMessageReceived;

    /// <summary>
    /// Occurs when an activity entry is received in the subscribed room.
    /// </summary>
    event Func<DraftLogEntryDto, Task>? LogEntryAdded;

    /// <summary>
    /// Occurs after the transport reconnects and the room view is rechecked.
    /// </summary>
    event Func<Task>? Reconnected;

    /// <summary>
    /// Occurs when the room is closed.
    /// </summary>
    event Func<Task>? Closed;

    /// <summary>
    /// Occurs when the current participant is removed from the room.
    /// </summary>
    event Func<Task>? RemovedFromRoom;

    /// <summary>
    /// Occurs when the connection state changes.
    /// </summary>
    event Action<DraftRoomConnectionState>? StateChanged;

    /// <summary>
    /// Gets the current SignalR connection state.
    /// </summary>
    DraftRoomConnectionState State { get; }

    /// <summary>
    /// Connects to the hub and subscribes to the specified room.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="credentials">Guest credentials, or an empty contract for account callers.</param>
    /// <param name="cancellationToken">A token used to cancel connection startup.</param>
    /// <returns>The room version observed while subscribing.</returns>
    Task<int> ConnectAsync(
        string code,
        DraftRoomCredentials credentials,
        CancellationToken cancellationToken = default);
}
