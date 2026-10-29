using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Core.Interfaces.DraftRooms;

/// <summary>
/// Publishes committed draft-room changes to connected clients.
/// </summary>
public interface IRoomChangeNotifier
{
    /// <summary>
    /// Notifies room subscribers that the room version changed.
    /// </summary>
    /// <param name="roomId">The changed room identifier.</param>
    /// <param name="version">The committed room version.</param>
    /// <returns>A task representing notification dispatch.</returns>
    Task NotifyRoomChangedAsync(Guid roomId, int version);

    /// <summary>
    /// Notifies room subscribers that the room was closed.
    /// </summary>
    /// <param name="roomId">The closed room identifier.</param>
    /// <returns>A task representing notification dispatch.</returns>
    Task NotifyRoomClosedAsync(Guid roomId);

    /// <summary>
    /// Notifies a participant that they were removed from a room.
    /// </summary>
    /// <param name="roomId">The room identifier.</param>
    /// <param name="participantId">The removed participant identifier.</param>
    /// <returns>A task representing notification dispatch.</returns>
    Task NotifyParticipantRemovedAsync(Guid roomId, Guid participantId);

    /// <summary>
    /// Notifies room subscribers that a chat message was committed.
    /// </summary>
    /// <param name="roomId">The room that received the message.</param>
    /// <param name="message">The public chat message.</param>
    /// <returns>A task representing notification dispatch.</returns>
    Task NotifyChatMessageReceivedAsync(Guid roomId, ChatMessageDto message);

    /// <summary>
    /// Notifies room subscribers that an activity entry was committed.
    /// </summary>
    /// <param name="roomId">The room that received the activity.</param>
    /// <param name="entry">The public activity entry.</param>
    /// <returns>A task representing notification dispatch.</returns>
    Task NotifyLogEntryAddedAsync(Guid roomId, DraftLogEntryDto entry);
}
