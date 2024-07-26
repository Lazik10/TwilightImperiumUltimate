namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class VerticalSpace
{
    [Parameter]
    public int Height { get; set; } = 100;

    private string GetHeightString() => $"{Height}px";
}
