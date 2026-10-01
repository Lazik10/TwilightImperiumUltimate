namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class TabNavItem
{
    /// <summary>
    /// Gets or sets the minimum width of the tab (e.g. "33%", "20%"). When null, the tab sizes to its content.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public EventCallback OnClick { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private string GetWidthStyle() => Width is null ? string.Empty : $"min-width: {Width};";

    private async Task HandleClickAsync()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }
}
