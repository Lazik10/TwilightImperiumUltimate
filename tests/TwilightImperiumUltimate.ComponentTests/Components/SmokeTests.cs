using TwilightImperiumUltimate.Web.Components.Shared.Text;

namespace TwilightImperiumUltimate.ComponentTests.Components;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class SmokeTests : BunitContext
{
    [Fact]
    public void ResponsiveText_WhenRendered_ShouldShowText()
    {
        var component = Render<ResponsiveText>(parameters => parameters
            .Add(text => text.Text, "Hello"));

        component.Markup.Should().Contain("Hello");
    }
}
