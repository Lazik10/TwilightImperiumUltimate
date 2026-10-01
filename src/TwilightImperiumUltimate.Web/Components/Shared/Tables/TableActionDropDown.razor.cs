using System.Collections;

namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// Compact ResponsiveDropDown for use inside a ResponsiveTable cell. Same contract
/// as <see cref="Controls.ResponsiveDropDown{TValue}"/>, just defaulting to the smallest FontSize
/// token and a shorter input height so it fits a narrow column.
/// </summary>
/// <typeparam name="TValue">The bound value type.</typeparam>
public partial class TableActionDropDown<TValue>
{
    [Parameter]
    [EditorRequired]
    public string Id { get; set; } = string.Empty;

    [Parameter]
    public TValue Value { get; set; } = default!;

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public IEnumerable? Data { get; set; }

    [Parameter]
    public string? TextProperty { get; set; }

    [Parameter]
    public string? ValueProperty { get; set; }

    [Parameter]
    public string? Placeholder { get; set; }

    [Parameter]
    public string? AriaDescribedBy { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public bool AllowClear { get; set; }

    [Parameter]
    public bool AllowFiltering { get; set; }

    [Parameter]
    public bool AllowVirtualization { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the font size token (xs/sm/base/md/lg/xl/2xl/3xl). Defaults to "xs" -- smaller
    /// than ResponsiveDropDown's own "md" default -- since this variant only ever sits inside a
    /// narrow table/grid cell.
    /// </summary>
    [Parameter]
    public string FontSize { get; set; } = "xs";

    [Parameter]
    public EventCallback<TValue> ValueChanged { get; set; }

    private async Task OnValueChanged(TValue value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(value);
    }
}
