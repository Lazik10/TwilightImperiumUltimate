namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class GridLayout
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public int Columns { get; set; } = 1;

    [Parameter]
    public int Gap { get; set; } = 0;

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public string AlignItems { get; set; } = "center";

    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;
}