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
    public EventCallback OnClick { get; set; }

    [Parameter]
    public string Text { get; set; } = string.Empty;

    [Parameter]
    public string TextAlign { get; set; } = "center";

    private MarkupString MarkupString => (MarkupString)Text;
}