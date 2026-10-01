using Microsoft.AspNetCore.Components.Web;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

/// <summary>
/// Single tab button within a <see cref="ResponsiveTabMenu"/>, following the WAI-ARIA tabs
/// pattern (role="tab", aria-selected, roving tabindex) with Left/Right/Home/End keyboard
/// navigation between sibling tabs.
/// </summary>
public partial class ResponsiveTab : IDisposable
{
    private ElementReference _buttonRef;
    private bool _disposed;

    /// <summary>
    /// Gets or sets the tab's own DOM id, referenced by the associated tab panel's
    /// aria-labelledby attribute.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the id of the tab panel this tab controls (aria-controls).
    /// </summary>
    [Parameter]
    public string? ControlsId { get; set; }

    [Parameter]
    public bool IsActive { get; set; }

    [Parameter]
    public EventCallback OnClick { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [CascadingParameter]
    private ResponsiveTabMenu? TabMenu { get; set; }

    private string ComputedCssClass => IsActive ? "responsive-tab responsive-tab-active" : "responsive-tab";

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    internal ValueTask FocusAsync() => _buttonRef.FocusAsync();

    protected override void OnInitialized() => TabMenu?.Register(this);

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            TabMenu?.Unregister(this);

        _disposed = true;
    }

    private async Task HandleClickAsync()
    {
        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync();
    }

    private async Task HandleKeyDownAsync(KeyboardEventArgs e)
    {
        if (TabMenu is null)
            return;

        switch (e.Key)
        {
            case "ArrowRight":
            case "ArrowDown":
                await TabMenu.FocusNextAsync(this);
                break;
            case "ArrowLeft":
            case "ArrowUp":
                await TabMenu.FocusPreviousAsync(this);
                break;
            case "Home":
                await TabMenu.FocusFirstAsync();
                break;
            case "End":
                await TabMenu.FocusLastAsync();
                break;
        }
    }
}
