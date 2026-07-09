namespace TwilightImperiumUltimate.Web.Components.Shared;

public partial class Page
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the desktop horizontal padding as a percentage of the page width.
    /// Tablet (&lt;=1024px) and mobile (&lt;=768px) breakpoints always use fixed 5% and 1%
    /// respectively, regardless of this value -- see page-horizontal-padding in Page.razor.css.
    /// </summary>
    [Parameter]
    public int HorizontalPadding { get; set; } = 20;

    private string GetHorizontalPaddingStyle() => $"padding-left: {HorizontalPadding}%; padding-right: {HorizontalPadding}%;";
}
