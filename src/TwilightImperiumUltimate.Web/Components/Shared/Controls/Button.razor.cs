using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class Button
{
    [Parameter]
    public string ButtonText { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter]
    public bool IsDisabled { get; set; } = false;

    [Parameter]
    public ButtonType ButtonType { get; set; } = ButtonType.Button;

    [Parameter]
    public int Width { get; set; } = 30;

    /// <summary>
    /// Gets or sets additional attributes (aria-*, data-*, id, etc.) splatted directly onto the
    /// rendered &lt;button&gt; element, so this component can fully substitute a plain HTML button.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string GetCssClass() =>
        $"div-like-button clickable white handel shadow {(IsDisabled ? "button-disabled" : string.Empty)} {CssClass}".Trim();

    private string GetContainerStyle() => $"width: {Width}%; {Style}";

    private async Task OnClickHandler(MouseEventArgs e)
    {
        if (IsDisabled)
            return;

        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(e);
        }
    }

    private string GetButtonType()
    {
        return ButtonType switch
        {
            ButtonType.Button => "button",
            ButtonType.Submit => "submit",
            ButtonType.Reset => "reset",
            _ => "button",
        };
    }
}
