namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Tracks active SignalR connections for draft-room participants.
/// </summary>
public interface IPresenceTracker
{
    /// <summary>
    /// Occurs when a participant establishes their first active room connection.
    /// </summary>
    event Action<DraftRoomPresenceChange>? ParticipantOnline;

    /// <summary>
    /// Occurs after a participant has had no active room connection for the grace period.
    /// </summary>
    event Action<DraftRoomPresenceChange>? ParticipantOffline;

    /// <summary>
    /// Registers a SignalR connection for a room participant.
    /// </summary>
    /// <param name="roomId">The room the connection belongs to.</param>
    /// <param name="participantId">The participant using the connection.</param>
    /// <param name="connectionId">The SignalR connection identifier.</param>
    void AddConnection(Guid roomId, Guid participantId, string connectionId);

    /// <summary>
    /// Removes a SignalR connection and schedules the offline transition when it was the last one.
    /// </summary>
    /// <param name="connectionId">The SignalR connection identifier.</param>
    void RemoveConnection(string connectionId);

    /// <summary>
    /// Determines whether the participant is online or still within the disconnect grace period.
    /// </summary>
    /// <param name="roomId">The room to check.</param>
    /// <param name="participantId">The participant to check.</param>
    /// <returns><see langword="true"/> while the participant is connected or within the grace period.</returns>
    bool IsOnline(Guid roomId, Guid participantId);
}
