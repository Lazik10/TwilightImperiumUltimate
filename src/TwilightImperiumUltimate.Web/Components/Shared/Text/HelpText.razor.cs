namespace TwilightImperiumUltimate.Web.Components.Shared.Text;

public partial class HelpText
{
    [Parameter]
    public string Text { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Style { get; set; } = string.Empty;

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
            TextColor.Deepskyblue => "deepskyblue",
            TextColor.Purple => "purple",
            TextColor.Pink => "magenta",
            TextColor.Orange => "orange",
            TextColor.Grey => "grey",
            TextColor.DarkGreen => "darkgreen",
            _ => "white",
        };
    }
}
