using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Components.Shared.Text;

public partial class Label
{
    [Parameter]
    public string Text { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public int FontSize { get; set; } = 16;

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public int MaxWidth { get; set; } = 100;

    [Parameter]
    public int MinWidth { get; set; } = 0;

    [Parameter]
    public bool CenterText { get; set; } = false;

    [Parameter]
    public TextColor TextColor { get; set; }

    public string SetColor()
    {
        return TextColor switch
        {
            TextColor.White => "white",
            TextColor.Red => "red",
            TextColor.Green => "lawngreen",
            TextColor.Blue => "blue",
            TextColor.Yellow => "yellow",
            _ => "white",
        };
    }
}