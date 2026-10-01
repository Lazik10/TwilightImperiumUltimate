using Microsoft.AspNetCore.Components.Web;

namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

/// <summary>
/// Code-behind for <see cref="ResponsiveCard"/>.
/// </summary>
public partial class ResponsiveCard
{
    /// <summary>
    /// Gets or sets the child content to render inside the card.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets additional CSS classes.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether left and right card padding should be removed.
    /// </summary>
    [Parameter]
    public bool RemoveHorizontalPadding { get; set; }

    /// <summary>
    /// Gets or sets the optional action raised when an interactive card is activated.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets whether an interactive card is unavailable for activation.
    /// </summary>
    [Parameter]
    public bool IsDisabled { get; set; }

    private async Task HandleClickAsync()
    {
        if (OnClick.HasDelegate && !IsDisabled)
            await OnClick.InvokeAsync();
    }

    private async Task HandleKeyDownAsync(KeyboardEventArgs keyboardEventArgs)
    {
        if (keyboardEventArgs.Key is "Enter" or " ")
            await HandleClickAsync();
    }
}
