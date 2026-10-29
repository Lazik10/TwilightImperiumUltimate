using FluentAssertions;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms.Engine;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class RandomnessSourceTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NextInt_WhenUpperBoundIsNotPositive_ShouldThrow(int upperBound)
    {
        var source = new CryptoRandomnessSource();

        var action = () => source.NextInt(upperBound);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void NextInt_WhenUpperBoundIsPositive_ShouldStayInsideExclusiveRange()
    {
        var source = new CryptoRandomnessSource();

        var samples = Enumerable.Range(0, 1000).Select(_ => source.NextInt(3)).ToArray();

        samples.Should().OnlyContain(value => value >= 0 && value < 3);
        source.NextInt(1).Should().Be(0);
    }

    [Fact]
    public void Shuffle_WhenThreeItemsAreShuffledRepeatedly_ShouldProduceEveryPermutationWithoutBias()
    {
        var source = new CryptoRandomnessSource();
        var counts = new Dictionary<string, int>(StringComparer.Ordinal);

        for (var iteration = 0; iteration < 6000; iteration++)
        {
            var items = new[] { 1, 2, 3 };
            source.Shuffle(items);
            var key = string.Concat(items);
            counts[key] = counts.GetValueOrDefault(key) + 1;
        }

        counts.Keys.Should().BeEquivalentTo("123", "132", "213", "231", "312", "321");
        counts.Values.Should().OnlyContain(count => count >= 750 && count <= 1250);
    }

    [Fact]
    public void Shuffle_WhenListIsEmptyOrSingleton_ShouldNotChangeItems()
    {
        var source = new CryptoRandomnessSource();
        var empty = Array.Empty<int>();
        var singleton = new[] { 42 };

        source.Shuffle(empty);
        source.Shuffle(singleton);

        empty.Should().BeEmpty();
        singleton.Should().Equal(42);
    }

    [Fact]
    public void Shuffle_WhenUsingDeterministicSource_ShouldProduceThePrescribedOrder()
    {
        var source = new DeterministicRandomnessSource(0, 0);
        var items = new[] { 1, 2, 3 };

        source.Shuffle(items);

        items.Should().Equal(2, 3, 1);
    }
}
