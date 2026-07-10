namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

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
    public int HorizontalPadding { get; set; } = 10;

    /// <summary>
    /// Gets or sets the desktop left padding as a percentage of the page width. When null,
    /// falls back to <see cref="HorizontalPadding"/>. Tablet/mobile breakpoints always use
    /// fixed values regardless of this parameter -- see page-horizontal-padding in Page.razor.css.
    /// </summary>
    [Parameter]
    public int? LeftPadding { get; set; }

    /// <summary>
    /// Gets or sets the desktop right padding as a percentage of the page width. When null,
    /// falls back to <see cref="HorizontalPadding"/>. Tablet/mobile breakpoints always use
    /// fixed values regardless of this parameter -- see page-horizontal-padding in Page.razor.css.
    /// </summary>
    [Parameter]
    public int? RightPadding { get; set; }

    private string GetHorizontalPaddingStyle() =>
        $"padding-left: {LeftPadding ?? HorizontalPadding}%; padding-right: {RightPadding ?? HorizontalPadding}%;";
}
