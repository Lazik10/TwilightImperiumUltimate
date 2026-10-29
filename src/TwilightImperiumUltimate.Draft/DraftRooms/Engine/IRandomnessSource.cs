namespace TwilightImperiumUltimate.Draft.DraftRooms.Engine;

/// <summary>
/// Supplies unbiased randomness for draft decisions; callers persist the resulting decisions in the log.
/// </summary>
public interface IRandomnessSource
{
    /// <summary>
    /// Returns a value from zero inclusive to the positive upper bound exclusive.
    /// </summary>
    int NextInt(int exclusiveUpperBound);

    /// <summary>
    /// Shuffles the supplied mutable list in place without adding or removing items.
    /// </summary>
    void Shuffle<T>(IList<T> items);
}
