using FluentAssertions;
using TwilightImperiumUltimate.Contracts.Enums;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Tigl;

public sealed class TiglFactionNameExtensionsTests
{
    [Fact]
    public void GetPrestigeCountIdentity_WhenKeleresVariantsAreAwarded_ShouldCountThemAsOneFaction()
    {
        // Arrange
        TiglFactionName[] awardedFactions =
        [
            TiglFactionName.TheCouncilKeleresXxcha,
            TiglFactionName.TheBaronyOfLetnev,
            TiglFactionName.TheNekroVirus,
            TiglFactionName.TheCouncilKeleresMentak,
        ];

        // Act
        var count = awardedFactions
            .Select(faction => faction.GetPrestigeCountIdentity())
            .Distinct()
            .Count();

        // Assert
        count.Should().Be(3);
    }

    [Theory]
    [InlineData(TiglFactionName.TheCouncilKeleresArgent)]
    [InlineData(TiglFactionName.TheCouncilKeleresMentak)]
    [InlineData(TiglFactionName.TheCouncilKeleresXxcha)]
    public void GetPrestigeCountIdentity_WhenFactionIsKeleresVariant_ShouldReturnSharedIdentity(TiglFactionName faction)
    {
        // Act
        var identity = faction.GetPrestigeCountIdentity();

        // Assert
        identity.Should().Be(TiglFactionName.TheCouncilKeleresArgent);
    }
}
