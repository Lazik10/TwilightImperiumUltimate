namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Tracks failed draft-room join attempts by client IP address and room code.
/// </summary>
public interface IJoinAttemptThrottle
{
    /// <summary>
    /// Determines whether the specified client and room code have reached the failure limit.
    /// </summary>
    /// <param name="ipAddress">The client IP address.</param>
    /// <param name="roomCode">The room code supplied by the client.</param>
    /// <returns><see langword="true"/> if further attempts are blocked; otherwise, <see langword="false"/>.</returns>
    bool IsBlocked(string ipAddress, string roomCode);

    /// <summary>
    /// Records a failed join attempt for the specified client and room code.
    /// </summary>
    /// <param name="ipAddress">The client IP address.</param>
    /// <param name="roomCode">The room code supplied by the client.</param>
    void RegisterFailure(string ipAddress, string roomCode);

    /// <summary>
    /// Clears failed join attempts for the specified client and room code.
    /// </summary>
    /// <param name="ipAddress">The client IP address.</param>
    /// <param name="roomCode">The room code supplied by the client.</param>
    void RegisterSuccess(string ipAddress, string roomCode);
}
