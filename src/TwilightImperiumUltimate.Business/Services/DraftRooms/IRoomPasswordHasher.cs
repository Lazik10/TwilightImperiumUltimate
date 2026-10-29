using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Hashes and verifies draft room passwords.
/// </summary>
public interface IRoomPasswordHasher
{
    /// <summary>
    /// Creates a password hash for a draft room.
    /// </summary>
    /// <param name="room">The room associated with the password.</param>
    /// <param name="password">The password to hash.</param>
    /// <returns>A salted password hash.</returns>
    string Hash(DraftRoom room, string password);

    /// <summary>
    /// Verifies a supplied password against a draft room's stored password hash.
    /// </summary>
    /// <param name="room">The room associated with the password.</param>
    /// <param name="passwordHash">The stored password hash.</param>
    /// <param name="password">The supplied password.</param>
    /// <returns><see langword="true"/> if the password is valid; otherwise, <see langword="false"/>.</returns>
    bool Verify(DraftRoom room, string passwordHash, string password);

    /// <summary>
    /// Performs password-hash verification when the requested room does not exist.
    /// </summary>
    /// <param name="password">The candidate password.</param>
    /// <returns><see langword="true"/> if the candidate matches the dummy hash; otherwise, <see langword="false"/>.</returns>
    bool VerifyDummyPassword(string password);
}
