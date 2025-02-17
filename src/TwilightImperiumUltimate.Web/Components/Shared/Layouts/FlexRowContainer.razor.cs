namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class FlexRowContainer
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public JustifyContent JustifyContent { get; set; } = JustifyContent.SpaceEvenly;

    [Parameter]
    public AlignItems AlignItems { get; set; } = AlignItems.Center;

    private string Justify => GetJustifyString(JustifyContent);

    private string Align => GetAlignString(AlignItems);

    private static string GetAlignString(AlignItems alignItems)
    {
        return alignItems switch
        {
            AlignItems.Baseline => "baseline;",
            AlignItems.Stretch => "stretch;",
            AlignItems.Center => "center;",
            AlignItems.FlexEnd => "flex-end;",
            AlignItems.FlexStart => "flex-start;",
            _ => "center;",
        };
    }

    private static string GetJustifyString(JustifyContent justifyContent)
    {
        return justifyContent switch
        {
            JustifyContent.Center => "center;",
            JustifyContent.FlexEnd => "flex-end;",
            JustifyContent.SpaceAround => "space-around;",
            JustifyContent.SpaceBetween => "space-between;",
            JustifyContent.SpaceEvenly => "space-evenly;",
            JustifyContent.FlexStart => "flex-start;",
            _ => "center;",
        };
    }
}
