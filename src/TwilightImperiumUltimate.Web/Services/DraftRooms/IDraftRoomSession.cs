using System.Diagnostics.CodeAnalysis;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <summary>
/// Holds the current view and request state for one open draft-room page.
/// </summary>
public interface IDraftRoomSession : IAsyncDisposable
{
    /// <summary>
    /// Occurs after the session state changes following a room refresh.
    /// </summary>
    event Action? Changed;

    /// <summary>
    /// Occurs when the live connection state changes.
    /// </summary>
    event Action<DraftRoomConnectionState>? ConnectionStateChanged;

    /// <summary>
    /// Occurs when a participant's online status changes.
    /// </summary>
    event Action<DraftRoomPresenceUpdateDto>? PresenceChanged;

    /// <summary>
    /// Occurs when a chat message is received in the room.
    /// </summary>
    event Func<ChatMessageDto, Task>? ChatMessageReceived;

    /// <summary>
    /// Occurs when an activity entry is received in the room.
    /// </summary>
    event Func<DraftLogEntryDto, Task>? LogEntryAdded;

    /// <summary>
    /// Gets the latest personalized room view.
    /// </summary>
    DraftRoomViewDto? View { get; }

    /// <summary>
    /// Gets whether a room refresh is in progress.
    /// </summary>
    bool IsLoading { get; }

    /// <summary>
    /// Gets the latest API error kind, if the refresh failed.
    /// </summary>
    [SuppressMessage(
        "Naming",
        "CA1716:Rename virtual/interface member",
        Justification = "Retained as part of the existing public room-session contract.")]
    DraftRoomApiResultKind? Error { get; }

    /// <summary>
    /// Gets the current live connection state.
    /// </summary>
    DraftRoomConnectionState ConnectionState { get; }

    /// <summary>
    /// Initializes this session for a room code.
    /// </summary>
    void Initialize(string code);

    /// <summary>
    /// Connects to the room, subscribes, and loads the latest room view.
    /// </summary>
    Task ConnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes the personalized room view.
    /// </summary>
    Task RefreshAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a bounded page of chat messages for this room.
    /// </summary>
    Task<DraftRoomApiResult<IReadOnlyList<ChatMessageDto>>> GetChatHistoryAsync(
        long? afterId,
        int limit,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a bounded page of activity entries for this room.
    /// </summary>
    Task<DraftRoomApiResult<IReadOnlyList<DraftLogEntryDto>>> GetLogHistoryAsync(
        long? afterId,
        int limit,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a chat message to this room.
    /// </summary>
    Task<DraftRoomApiResult<ChatMessageDto>> SendChatAsync(
        string text,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a room command and refreshes the personalized view afterwards.
    /// </summary>
    Task<DraftRoomApiCommandResult> ExecuteAsync(
        Func<Task<DraftRoomApiCommandResult>> command,
        CancellationToken cancellationToken = default);
}
