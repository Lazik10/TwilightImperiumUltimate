namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class ResponsiveGridContainer
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public int Columns { get; set; } = 1;

    [Parameter]
    public int TabletColumns { get; set; }

    [Parameter]
    public int MobileColumns { get; set; }

    [Parameter]
    public string Gap { get; set; } = "var(--space-sm)";

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public string AlignItems { get; set; } = "center";

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    private string GetContainerStyle()
    {
        var tabletColumns = TabletColumns > 0 ? TabletColumns : Columns;
        var mobileColumns = MobileColumns > 0 ? MobileColumns : tabletColumns;

        return $"--grid-columns: {Columns}; --grid-columns-tablet: {tabletColumns}; --grid-columns-mobile: {mobileColumns}; --grid-gap: {Gap}; width: {Width}%; align-items: {AlignItems}; {Style}";
    }
}
