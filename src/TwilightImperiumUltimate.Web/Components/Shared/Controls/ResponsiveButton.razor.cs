using Microsoft.AspNetCore.Components.Web;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using AlignItemsEnum = TwilightImperiumUltimate.Web.Enums.AlignItems;
using ButtonType = Radzen.ButtonType;
using JustifyContentEnum = TwilightImperiumUltimate.Web.Enums.JustifyContent;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class ResponsiveButton : TwilightImperiumBaseComponent
{
    [Parameter]
    public string Text { get; set; } = string.Empty;

    [Parameter]
    public string ButtonText { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Parameter]
    public int FontSize { get; set; } = 16;

    [Parameter]
    public string TextAlign { get; set; } = "center";

    [Parameter]
    public JustifyContentEnum JustifyContent { get; set; } = JustifyContentEnum.Center;

    [Parameter]
    public AlignItemsEnum AlignItems { get; set; } = AlignItemsEnum.Center;

    [Parameter]
    public TextColor TextColor { get; set; } = TextColor.White;

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

    private string DisplayText => string.IsNullOrWhiteSpace(Text) ? ButtonText : Text;

    private bool IsActuallyDisabled => IsDisabled || Disabled;

    private string ComputedCssClass => $"responsive-button clickable handel shadow {CssClass}";

    private string ComputedStyle
    {
        get
        {
            var widthStyle = Width is > 0 and < 100 ? $"width: {Width}%;" : string.Empty;
            return $"{widthStyle} color: {TextColor.ConvertToString()}; font-size: {FontSize}px; text-align: {TextAlign}; justify-content: {JustifyContent.GetJustifyString()}; align-items: {AlignItems.GetAlignString()}; {Style}";
        }
    }

    private async Task OnClickHandler(MouseEventArgs e)
    {
        if (IsActuallyDisabled)
            return;

        if (OnClickMouse.HasDelegate)
            await OnClickMouse.InvokeAsync(e);

        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync();
    }

    private string GetButtonType() => ButtonType switch
    {
        ButtonType.Button => "button",
        ButtonType.Submit => "submit",
        ButtonType.Reset => "reset",
        _ => "button",
    };
}
