namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class Page
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the horizontal padding as a percentage of the page width, applied only at
    /// viewport widths of 1570px and wider. A value of zero disables horizontal padding at every
    /// breakpoint. Otherwise, below 1570px this value is ignored and a fixed 3% is used instead,
    /// dropping to 2% at &lt;=1024px and 1% at &lt;=768px -- see page-horizontal-padding in
    /// Page.razor.css.
    /// </summary>
    [Parameter]
    public int HorizontalPadding { get; set; } = 10;

    /// <summary>
    /// Gets or sets the left padding as a percentage of the page width, applied only at viewport
    /// widths of 1570px and wider. When null, falls back to <see cref="HorizontalPadding"/>.
    /// Narrower breakpoints always use the fixed values described on
    /// <see cref="HorizontalPadding"/>, regardless of this parameter -- see
    /// page-horizontal-padding in Page.razor.css.
    /// </summary>
    [Parameter]
    public int? LeftPadding { get; set; }

    /// <summary>
    /// Gets or sets the right padding as a percentage of the page width, applied only at viewport
    /// widths of 1570px and wider. When null, falls back to <see cref="HorizontalPadding"/>.
    /// Narrower breakpoints always use the fixed values described on
    /// <see cref="HorizontalPadding"/>, regardless of this parameter -- see
    /// page-horizontal-padding in Page.razor.css.
    /// </summary>
    [Parameter]
    public int? RightPadding { get; set; }

    private string PageCssClass => HorizontalPadding is 0
        ? "page-template page-horizontal-padding page-no-horizontal-padding"
        : "page-template page-horizontal-padding";

    private string GetHorizontalPaddingStyle() =>
        $"padding-left: {LeftPadding ?? HorizontalPadding}%; padding-right: {RightPadding ?? HorizontalPadding}%;";
}
