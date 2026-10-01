using Microsoft.AspNetCore.Components.Web;
using ButtonType = Radzen.ButtonType;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

/// <summary>
/// Destructive-action variant of <see cref="ResponsiveButton"/> (e.g. "Delete"). Reuses
/// ResponsiveButton's markup/behavior entirely and only overrides its background palette (via
/// the --responsive-button-bg/-hover-bg/-hover-color custom properties it exposes) with a red
/// "darker to lighter on hover" treatment instead of the default nav-blue one.
/// </summary>
public partial class ResponsiveDangerButton
{
    private ResponsiveButton? _buttonRef;

    [Parameter]
    public string Text { get; set; } = string.Empty;

    [Parameter]
    public string ButtonText { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Parameter]
    public bool IsDisabled { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public ButtonType ButtonType { get; set; } = ButtonType.Button;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback OnClick { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClickMouse { get; set; }

    [Parameter]
    public int Width { get; set; } = 100;

    private string ComputedStyle =>
        "--responsive-button-bg: var(--color-danger-bg-solid); " +
        "--responsive-button-hover-bg: var(--color-danger-hover-bg-solid); " +
        "--responsive-button-hover-color: var(--color-danger-hover-text); " + Style;

    public ValueTask FocusAsync() => _buttonRef?.FocusAsync() ?? ValueTask.CompletedTask;
}
