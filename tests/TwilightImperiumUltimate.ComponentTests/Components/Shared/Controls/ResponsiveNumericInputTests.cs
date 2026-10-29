using TwilightImperiumUltimate.Web.Components.Shared.Controls;

namespace TwilightImperiumUltimate.ComponentTests.Components.Shared.Controls;

public sealed class ResponsiveNumericInputTests : BunitContext
{
    [Fact]
    public void NumericInput_WhenNamed_ShouldKeepExistingContainerAndInputNamesAndReferenceAccessibleText()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var changed = 0;
        var input = Render<ResponsiveNumericInput>(parameters => parameters
            .Add(component => component.Id, "numeric-field")
            .Add(component => component.Name, "numeric-name")
            .Add(component => component.AriaLabel, "Number of players")
            .Add(component => component.AriaDescribedBy, "numeric-help")
            .Add(component => component.Min, 2)
            .Add(component => component.Max, 8)
            .Add(component => component.Value, 4)
            .Add(component => component.ValueChanged, value => changed = value));

        input.Find("#numeric-field").TagName.Should().Be("SPAN");
        var control = input.Find("input#numeric-name");
        control.GetAttribute("name").Should().Be("numeric-name");
        control.GetAttribute("aria-describedby").Should().Be("numeric-help");
        control.GetAttribute("aria-valuemin").Should().Be("2");
        control.GetAttribute("aria-valuemax").Should().Be("8");
        var label = input.Find($"#{control.GetAttribute("aria-labelledby")}");
        label.TextContent.Should().Be("Number of players");

        control.Change("6");

        changed.Should().Be(6);
    }
}
