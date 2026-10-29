using System.Security.Cryptography;
using System.Text;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Creates and verifies guest participant secrets.
/// </summary>
public sealed class GuestSecretService : IGuestSecretService
{
    private const int SecretByteLength = 32;

    /// <inheritdoc />
    public string Generate()
    {
        var secretBytes = RandomNumberGenerator.GetBytes(SecretByteLength);
        var base64 = Convert.ToBase64String(secretBytes);

        return base64.TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    /// <inheritdoc />
    public byte[] Hash(string secret)
    {
        ArgumentNullException.ThrowIfNull(secret);

        return SHA256.HashData(Encoding.UTF8.GetBytes(secret));
    }

    /// <inheritdoc />
    public bool Verify(string secret, byte[] storedHash)
    {
        ArgumentNullException.ThrowIfNull(storedHash);

        var calculatedHash = this.Hash(secret);

        return CryptographicOperations.FixedTimeEquals(calculatedHash, storedHash);
    }
}
