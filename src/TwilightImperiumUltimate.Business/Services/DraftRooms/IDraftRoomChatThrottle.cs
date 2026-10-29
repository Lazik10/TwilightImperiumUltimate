namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Limits the number of chat messages a participant can send in a rolling window.
/// </summary>
public interface IDraftRoomChatThrottle
{
    /// <summary>
    /// Reserves a message slot for the participant when they are below the limit.
    /// </summary>
    /// <param name="participantId">The participant identifier.</param>
    /// <returns><see langword="true"/> when the message is allowed; otherwise, <see langword="false"/>.</returns>
    bool TryAcquire(Guid participantId);
}
