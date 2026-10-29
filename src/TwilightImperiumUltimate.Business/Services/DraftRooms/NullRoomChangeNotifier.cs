using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Core.Interfaces.DraftRooms;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Provides a no-op room notifier for environments without a real-time transport.
/// </summary>
public sealed class NullRoomChangeNotifier : IRoomChangeNotifier
{
    /// <inheritdoc />
    public Task NotifyRoomChangedAsync(Guid roomId, int version) => Task.CompletedTask;

    /// <inheritdoc />
    public Task NotifyRoomClosedAsync(Guid roomId) => Task.CompletedTask;

    /// <inheritdoc />
    public Task NotifyParticipantRemovedAsync(Guid roomId, Guid participantId) => Task.CompletedTask;

    /// <inheritdoc />
    public Task NotifyChatMessageReceivedAsync(Guid roomId, ChatMessageDto message) => Task.CompletedTask;

    /// <inheritdoc />
    public Task NotifyLogEntryAddedAsync(Guid roomId, DraftLogEntryDto entry) => Task.CompletedTask;
}
