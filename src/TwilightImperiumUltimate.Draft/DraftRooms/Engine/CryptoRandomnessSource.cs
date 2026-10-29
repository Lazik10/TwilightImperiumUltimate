using System.Security.Cryptography;

namespace TwilightImperiumUltimate.Draft.DraftRooms.Engine;

/// <summary>
/// Uses the platform cryptographic random generator and Fisher-Yates for unbiased seat order.
/// </summary>
public sealed class CryptoRandomnessSource : IRandomnessSource
{
    /// <inheritdoc />
    public int NextInt(int exclusiveUpperBound)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(exclusiveUpperBound);

        return RandomNumberGenerator.GetInt32(exclusiveUpperBound);
    }

    /// <inheritdoc />
    public void Shuffle<T>(IList<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        for (var index = items.Count - 1; index > 0; index--)
        {
            var otherIndex = this.NextInt(index + 1);
            (items[index], items[otherIndex]) = (items[otherIndex], items[index]);
        }
    }
}
