using FluentAssertions;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Tigl.Helpers;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Tigl;

public sealed class TiglFactionParserTests
{
    public static IEnumerable<object[]> NewFactions =>
    [
        [TiglFactionName.AtokeraLegacy, "Atokera Legacy"],
        [TiglFactionName.BelkoseaAlliedStates, "Belkosea Allied States"],
        [TiglFactionName.PharadnOrder, "Pharadn Order"],
        [TiglFactionName.PrincipalityOfKaltrim, "Principality of Kaltrim"],
        [TiglFactionName.QhetRepublic, "Qhet Republic"],
        [TiglFactionName.SarcosaDrift, "Sarcosa Drift"],
        [TiglFactionName.ToldarConcordat, "Toldar Concordat"],
        [TiglFactionName.UydaiConclave, "Uydai Conclave"],
        [TiglFactionName.XinCourt, "Xin Court"],
        [TiglFactionName.TheArvaxiCovenant, "The Arvaxi Covenant"],
        [TiglFactionName.TheConstituentsOfXan, "The Constituents of Xan"],
        [TiglFactionName.TheKaloraSwarm, "The Kalora Swarm"],
        [TiglFactionName.TheLunariumConspiracies, "The Lunarium Conspiracies"],
        [TiglFactionName.TheOnyxxaComplex, "The Onyxxa Complex"],
        [TiglFactionName.TheTyrisContinuum, "The Tyris Continuum"],
        [TiglFactionName.TheVyserixAscendancy, "The Vyserix Ascendancy"],
        [TiglFactionName.TheZephyrionPursuit, "The Zephyrion Pursuit"],
    ];

    [Theory]
    [MemberData(nameof(NewFactions))]
    public void ParseFaction_WhenNewFactionNameIsProvided_ShouldReturnMatchingTiglFaction(
        TiglFactionName expectedFaction,
        string factionName)
    {
        var result = TwilightImperiumUltimate.Tigl.Helpers.TiglFactionParser.ParseFaction(factionName);

        result.Should().Be(expectedFaction);
    }

    [Theory]
    [MemberData(nameof(NewFactions))]
    public void ToFactionString_WhenNewTiglFactionIsProvided_ShouldReturnParseableFactionName(
        TiglFactionName faction,
        string expectedFactionName)
    {
        var result = TwilightImperiumUltimate.Business.Helpers.TiglFactionParser.ToFactionString(faction);

        result.Should().Be(expectedFactionName);
    }

    [Theory]
    [MemberData(nameof(NewFactions))]
    public void ParseFaction_WhenNewFactionNameIsProvided_ShouldReturnMatchingAsyncFaction(
        TiglFactionName expectedFaction,
        string factionName)
    {
        var result = TwilightImperiumUltimate.Business.Helpers.AsyncFactionParser.ParseFaction(factionName);

        result.ToString().Should().Be(expectedFaction.ToString());
    }

    [Fact]
    public void ShatteredLeagueFactions_WhenNewFactionsAreAdded_ShouldIncludeEveryNewFaction()
    {
        var expectedFactions = NewFactions.Select(faction => (TiglFactionName)faction[0]);

        TiglFactionsProvider.FracturedLeagueFactions.Should().Contain(expectedFactions);
    }
}
