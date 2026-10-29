namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Normalizes and validates guest display names for draft rooms.
/// </summary>
public interface IDraftRoomDisplayNameValidator
{
    /// <summary>
    /// Trims leading and trailing spaces and collapses consecutive spaces.
    /// </summary>
    /// <param name="displayName">The display name to normalize.</param>
    /// <returns>The normalized display name.</returns>
    string Normalize(string displayName);

    /// <summary>
    /// Creates the case-insensitive key used to identify a display name in a room.
    /// </summary>
    /// <param name="displayName">The display name to normalize and key.</param>
    /// <returns>The normalized display name in invariant uppercase.</returns>
    string MakeKey(string displayName);

    /// <summary>
    /// Determines whether a guest display name satisfies the room's naming rules
    /// and does not match a registered username.
    /// </summary>
    /// <param name="displayName">The guest display name to validate.</param>
    /// <returns>
    /// A task whose result is <see langword="true"/> when the name is valid;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    Task<bool> ValidateGuestName(string displayName);
}
