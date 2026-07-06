using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class TextButton
{
    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public int FontSize { get; set; } = 18;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Href { get; set; } = string.Empty;

    [Parameter]
    public string Target { get; set; } = string.Empty;

    [Parameter]
    public string Rel { get; set; } = string.Empty;

    [Parameter]
    public EventCallback OnClick { get; set; }

    [Parameter]
    public string Text { get; set; } = string.Empty;

    [Parameter]
    public string TextAlign { get; set; } = "center";

    [Parameter]
    public JustifyContent JustifyContent { get; set; } = JustifyContent.SpaceEvenly;

    [Parameter]
    public AlignItems AlignItems { get; set; } = AlignItems.Center;

    [Parameter]
    public TextColor TextColor { get; set; } = TextColor.White;

    private MarkupString MarkupString => (MarkupString)Text;

    private string Justify => JustifyContent.GetJustifyString();

    private string Align => AlignItems.GetAlignString();

    private bool HasHref => !string.IsNullOrWhiteSpace(Href);

    private string SetColor() => TextColor.ConvertToString();

    private string ComponentStyle => $"--text-button-font-size:{FontSize}px; color:{SetColor()}; text-align:{TextAlign}; align-items:{Align}; justify-content:{Justify}; cursor:pointer; width:{Width}%; box-sizing:border-box; padding:2px; height:100%; {Style}";

    private async Task HandleClickAsync()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }
}
