namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Generates and verifies secrets used to authenticate guest participants.
/// </summary>
public interface IGuestSecretService
{
    /// <summary>
    /// Generates a cryptographically random guest secret.
    /// </summary>
    /// <returns>A base64url-encoded 256-bit secret.</returns>
    string Generate();

    /// <summary>
    /// Computes the SHA-256 hash of a guest secret.
    /// </summary>
    /// <param name="secret">The secret to hash.</param>
    /// <returns>The 32-byte SHA-256 hash.</returns>
    byte[] Hash(string secret);

    /// <summary>
    /// Verifies a guest secret against its stored hash using a fixed-time comparison.
    /// </summary>
    /// <param name="secret">The candidate secret.</param>
    /// <param name="storedHash">The stored SHA-256 hash.</param>
    /// <returns><see langword="true"/> if the secret matches; otherwise, <see langword="false"/>.</returns>
    bool Verify(string secret, byte[] storedHash);
}
