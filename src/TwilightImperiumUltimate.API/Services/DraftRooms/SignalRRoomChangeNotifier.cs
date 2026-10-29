using Microsoft.AspNetCore.SignalR;
using TwilightImperiumUltimate.API.Hubs;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Core.Interfaces.DraftRooms;

namespace TwilightImperiumUltimate.API.Services.DraftRooms;

/// <summary>
/// Publishes draft-room changes and presence transitions through SignalR.
/// </summary>
public sealed class SignalRRoomChangeNotifier : IRoomChangeNotifier
{
    private readonly IHubContext<DraftRoomHub> _hubContext;
    private readonly ILogger<SignalRRoomChangeNotifier> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignalRRoomChangeNotifier"/> class.
    /// </summary>
    /// <param name="hubContext">The draft-room hub context.</param>
    /// <param name="presenceTracker">The room presence tracker.</param>
    /// <param name="logger">The logger used for failed notification dispatch.</param>
    public SignalRRoomChangeNotifier(
        IHubContext<DraftRoomHub> hubContext,
        IPresenceTracker presenceTracker,
        ILogger<SignalRRoomChangeNotifier> logger)
    {
        this._hubContext = hubContext
            ?? throw new ArgumentNullException(nameof(hubContext));
        ArgumentNullException.ThrowIfNull(presenceTracker);
        this._logger = logger
            ?? throw new ArgumentNullException(nameof(logger));

        presenceTracker.ParticipantOnline += this.HandleParticipantOnline;
        presenceTracker.ParticipantOffline += this.HandleParticipantOffline;
    }

    /// <inheritdoc />
    public Task NotifyRoomChangedAsync(Guid roomId, int version)
    {
        return this.SendToRoomAsync(roomId, "RoomChanged", version);
    }

    /// <inheritdoc />
    public Task NotifyRoomClosedAsync(Guid roomId)
    {
        return this.SendToRoomAsync(roomId, "RoomClosed");
    }

    /// <inheritdoc />
    public Task NotifyParticipantRemovedAsync(Guid roomId, Guid participantId)
    {
        return this.SendToParticipantAsync(roomId, participantId, "RemovedFromRoom");
    }

    /// <inheritdoc />
    public Task NotifyChatMessageReceivedAsync(Guid roomId, ChatMessageDto message)
    {
        ArgumentNullException.ThrowIfNull(message);

        return this.SendToRoomAsync(roomId, "ChatMessageReceived", message);
    }

    /// <inheritdoc />
    public Task NotifyLogEntryAddedAsync(Guid roomId, DraftLogEntryDto entry)
    {
        ArgumentNullException.ThrowIfNull(entry);

        return this.SendToRoomAsync(roomId, "LogEntryAdded", entry);
    }

    private void HandleParticipantOnline(DraftRoomPresenceChange change)
    {
        _ = this.SendPresenceChangedAsync(change, isOnline: true);
    }

    private void HandleParticipantOffline(DraftRoomPresenceChange change)
    {
        _ = this.SendPresenceChangedAsync(change, isOnline: false);
    }

    private async Task SendPresenceChangedAsync(DraftRoomPresenceChange change, bool isOnline)
    {
        try
        {
            await this._hubContext.Clients
                .Group(DraftRoomHub.GetRoomGroupName(change.RoomId))
                .SendAsync(
                    "PresenceChanged",
                    new DraftRoomPresenceUpdateDto
                    {
                        ParticipantId = change.ParticipantId,
                        IsOnline = isOnline,
                    })
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            this._logger.LogError(
                exception,
                "Failed to publish a presence change for draft room {RoomId}.",
                change.RoomId);
        }
    }

    private Task SendToRoomAsync(Guid roomId, string methodName, params object?[] args)
    {
        return this.SendAsync(
            this._hubContext.Clients.Group(DraftRoomHub.GetRoomGroupName(roomId)),
            methodName,
            roomId,
            args);
    }

    private Task SendToParticipantAsync(Guid roomId, Guid participantId, string methodName)
    {
        return this.SendAsync(
            this._hubContext.Clients.Group(DraftRoomHub.GetParticipantGroupName(roomId, participantId)),
            methodName,
            roomId,
            []);
    }

    private async Task SendAsync(
        IClientProxy clientProxy,
        string methodName,
        Guid roomId,
        object?[] args)
    {
        try
        {
            await clientProxy.SendCoreAsync(methodName, args).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            this._logger.LogError(
                exception,
                "Failed to publish {NotificationName} for draft room {RoomId}.",
                methodName,
                roomId);
        }
    }
}
