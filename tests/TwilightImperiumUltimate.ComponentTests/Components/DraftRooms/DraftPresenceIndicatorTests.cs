using TwilightImperiumUltimate.Web.Components.DraftRooms;

namespace TwilightImperiumUltimate.ComponentTests.Components.DraftRooms;

public sealed class DraftPresenceIndicatorTests : BunitContext
{
    [Theory]
    [InlineData(true, "Online", "draft-presence-indicator__dot--online")]
    [InlineData(false, "Offline", "draft-presence-indicator__dot--offline")]
    public void DraftPresenceIndicator_WhenPresenceChanges_ShouldShowTextAndDotState(
        bool isOnline,
        string expectedText,
        string expectedDotClass)
    {
        var component = Render<DraftPresenceIndicator>(parameters =>
            parameters.Add(parameter => parameter.IsOnline, isOnline));

        component.Markup.Should().Contain(expectedText);
        component.Find(".draft-presence-indicator__dot")
            .GetAttribute("class")
            .Should().Contain(expectedDotClass);
    }
}
