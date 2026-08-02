namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class ClickableTile
{
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets an optional destination URL. When set, the tile renders as a real
    /// <c>&lt;a&gt;</c> element (so the destination is a crawlable, shareable link and works
    /// with the browser back/forward/middle-click behavior) instead of a <c>&lt;button&gt;</c>,
    /// while still raising <see cref="OnClick"/> on activation.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private async Task HandleClickAsync()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }
}
