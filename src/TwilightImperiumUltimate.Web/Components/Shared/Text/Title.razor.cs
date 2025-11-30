using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Shared.Text;

public partial class Title
{
    [Parameter]
    public string Text { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public int FontSize { get; set; } = 42;

    [Parameter]
    public int MarginTop { get; set; } = 50;

    [Parameter]
    public int MarginBottom { get; set; } = 0;

    [Parameter]
    public TextColor TextColor { get; set; }

    private string GetTextColor() => TextColor.ConvertToString();
}