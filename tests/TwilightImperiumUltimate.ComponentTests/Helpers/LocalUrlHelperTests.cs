using FluentAssertions;
using TwilightImperiumUltimate.Web.Helpers;

namespace TwilightImperiumUltimate.ComponentTests.Helpers;

public sealed class LocalUrlHelperTests
{
    [Theory]
    [InlineData("/tools/draft-rooms/ABC234", true)]
    [InlineData("/tools/draft-rooms/ABC234?tab=seats", true)]
    [InlineData("/account/login#form", true)]
    [InlineData("https://evil.example", false)]
    [InlineData("//evil.example", false)]
    [InlineData("/\\evil.example", false)]
    [InlineData("javascript:alert(1)", false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData(null, false)]
    [InlineData("tools/draft-rooms/ABC234", false)]
    [InlineData("/%2f%2fevil.example", false)]
    [InlineData("/%5cevil.example", false)]
    [InlineData("/javascript%3aalert(1)", false)]
    [InlineData("/%252f%252fevil.example", false)]
    [InlineData("/path\\to\\evil.example", false)]
    [InlineData("/https://evil.example", false)]
    [InlineData("/path\r\nLocation: https://evil.example", false)]
    public void IsLocalPath_WhenGivenPath_ShouldMatchExpectedResult(string? path, bool expected)
    {
        var isLocalPath = LocalUrlHelper.IsLocalPath(path);

        isLocalPath.Should().Be(expected);
    }
}
