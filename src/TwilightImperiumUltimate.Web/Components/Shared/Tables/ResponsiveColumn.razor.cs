using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// Code-behind for <see cref="ResponsiveColumn"/>.
/// </summary>
public partial class ResponsiveColumn : IAsyncDisposable
{
    private const int ResizeKeyboardStepPx = 16;

    private ElementReference _headerElement;
    private ElementReference _resizeHandleElement;
    private IJSObjectReference? _jsModule;

    /// <summary>
    /// Gets or sets the plain text content. Ignored when <see cref="ChildContent"/> is set.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets custom child content. Takes precedence over <see cref="Text"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this cell renders as a header cell (&lt;th&gt;)
    /// instead of a data cell (&lt;td&gt;).
    /// </summary>
    [Parameter]
    public bool IsHeader { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this header column can be resized by dragging its
    /// right-edge handle (or via ArrowLeft/ArrowRight while the handle has focus). Only applies
    /// when <see cref="IsHeader"/> is true.
    /// </summary>
    [Parameter]
    public bool Resizable { get; set; }

    /// <summary>
    /// Gets or sets the "scope" attribute used when <see cref="IsHeader"/> is true: "col" for a
    /// column header, "row" for a row header.
    /// </summary>
    [Parameter]
    public string Scope { get; set; } = "col";

    /// <summary>
    /// Gets or sets the "aria-sort" attribute ("ascending"/"descending"/"none") for a sortable
    /// header cell. Left null for non-sortable columns so no attribute is rendered.
    /// </summary>
    [Parameter]
    public string? AriaSort { get; set; }

    /// <summary>
    /// Gets or sets the text color. Defaults to white.
    /// </summary>
    [Parameter]
    public TextColor TextColor { get; set; } = TextColor.White;

    /// <summary>
    /// Gets or sets a value indicating whether the text is rendered vertically (rotated), useful
    /// for narrow column headers with long titles.
    /// </summary>
    [Parameter]
    public bool Vertical { get; set; }

    /// <summary>
    /// Gets or sets the text alignment: "left", "center", or "right".
    /// </summary>
    [Parameter]
    public string Align { get; set; } = "left";

    /// <summary>
    /// Gets or sets additional CSS classes.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional inline styles.
    /// </summary>
    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private string ComputedCssClass =>
        $"responsive-column handel shadow {(Vertical ? "responsive-column-vertical" : string.Empty)} {CssClass}".Trim();

    private string ComputedStyle =>
        $"text-align: {Align}; color: {GetColorValue()}; {(IsHeader ? "vertical-align: top; " : string.Empty)}{Style}";

    public async ValueTask DisposeAsync()
    {
        if (_jsModule is null)
            return;

        try
        {
            await _jsModule.InvokeVoidAsync("dispose", _resizeHandleElement);
            await _jsModule.DisposeAsync();
        }
        catch (JSDisconnectedException)
        {
            // Circuit already gone; nothing left to clean up.
        }

        GC.SuppressFinalize(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender || !IsHeader || !Resizable)
            return;

        _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>(
            "import",
            "./Components/Shared/Tables/ResponsiveColumn.razor.js");

        await _jsModule.InvokeVoidAsync("initialize", _resizeHandleElement, _headerElement);
    }

    private string GetColorValue() => TextColor switch
    {
        TextColor.White => "white",
        TextColor.Red => "red",
        TextColor.Green => "lawngreen",
        TextColor.Blue => "blue",
        TextColor.Yellow => "yellow",
        TextColor.Deepskyblue => "deepskyblue",
        TextColor.Purple => "purple",
        TextColor.Pink => "magenta",
        TextColor.Orange => "orange",
        TextColor.Grey => "grey",
        TextColor.DarkGreen => "darkgreen",
        TextColor.Black => "black",
        TextColor.LightBlue => "lightblue",
        TextColor.Transparent => "transparent",
        _ => "white",
    };

    /// <summary>
    /// Handles ArrowLeft/ArrowRight on the resize handle as a keyboard-accessible alternative to
    /// pointer dragging.
    /// </summary>
    private async Task OnResizeKeyDownAsync(KeyboardEventArgs args)
    {
        if (_jsModule is null)
            return;

        var delta = args.Key switch
        {
            "ArrowLeft" => -ResizeKeyboardStepPx,
            "ArrowRight" => ResizeKeyboardStepPx,
            _ => 0,
        };

        if (delta == 0)
            return;

        await _jsModule.InvokeVoidAsync("resizeStep", _headerElement, delta);
    }
}
