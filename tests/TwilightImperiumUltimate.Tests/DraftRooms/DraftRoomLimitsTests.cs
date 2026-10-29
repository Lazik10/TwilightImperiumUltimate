using FluentAssertions;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

public sealed class DraftRoomLimitsTests
{
    [Fact]
    public void CodeAlphabet_WhenRead_ShouldContainDistinctCharactersWithoutAmbiguousCharacters()
    {
        var alphabet = DraftRoomLimits.CodeAlphabet;

        alphabet.Should().HaveLength(31);
        alphabet.Distinct().Should().HaveCount(31);
        alphabet.Should().NotContain("0");
        alphabet.Should().NotContain("O");
        alphabet.Should().NotContain("1");
        alphabet.Should().NotContain("I");
        alphabet.Should().NotContain("L");
    }
}
