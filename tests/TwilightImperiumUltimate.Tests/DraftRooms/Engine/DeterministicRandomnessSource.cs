using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Tests.DraftRooms.Engine;

internal sealed class DeterministicRandomnessSource(params int[] values) : IRandomnessSource
{
    private int _position;

    public int NextInt(int exclusiveUpperBound)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(exclusiveUpperBound);
        if (_position >= values.Length)
        {
            throw new InvalidOperationException("The deterministic random sequence is exhausted.");
        }

        var value = values[_position++];
        if (value < 0 || value >= exclusiveUpperBound)
        {
            throw new InvalidOperationException("The deterministic value is outside the requested random range.");
        }

        return value;
    }

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
