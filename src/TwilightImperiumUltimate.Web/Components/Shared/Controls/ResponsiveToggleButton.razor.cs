namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

/// <summary>
/// Thin wrapper around RadzenSwitch with an off/on text label flanking either side (e.g.
/// "Edit ⟷ Preview"), the currently active side highlighted to match ResponsiveTab's colors.
/// </summary>
public partial class ResponsiveToggleButton
{
    [Parameter]
    public bool IsToggled { get; set; }

    [Parameter]
    [EditorRequired]
    public string OnText { get; set; } = string.Empty;

    [Parameter]
    [EditorRequired]
    public string OffText { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<bool> IsToggledChanged { get; set; }

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public int Width { get; set; } = 100;

    private string ComputedStyle => Width is > 0 and < 100 ? $"width: {Width}%;" : string.Empty;

    private async Task HandleValueChangedAsync(bool value)
    {
        IsToggled = value;
        await IsToggledChanged.InvokeAsync(value);
    }
}
