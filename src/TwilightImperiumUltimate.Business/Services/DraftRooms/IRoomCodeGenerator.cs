namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Generates human-readable codes for draft rooms.
/// </summary>
public interface IRoomCodeGenerator
{
    /// <summary>
    /// Generates a cryptographically random room code.
    /// </summary>
    /// <returns>A room code with the configured length and alphabet.</returns>
    string Generate();
}
