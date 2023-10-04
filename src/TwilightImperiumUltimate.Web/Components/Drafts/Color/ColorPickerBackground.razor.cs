using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Components.Drafts.Color;

public partial class ColorPickerBackground
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public PlayerColor BackgroundColor { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    private static string ConvertColorNameToRGB(PlayerColor colorName)
    {
        var colorMap = new Dictionary<PlayerColor, string>
        {
            { PlayerColor.Red, "255, 0, 0" },
            { PlayerColor.Green, "0, 128, 0" },
            { PlayerColor.Blue, "0, 0, 255" },
            { PlayerColor.Orange, "255, 140, 0" },
            { PlayerColor.Yellow, "255, 255, 0" },
            { PlayerColor.Purple, "102, 51, 153" },
            { PlayerColor.Pink, "255, 105, 180" },
            { PlayerColor.Black, "0, 0, 0" },
        };

        return colorMap.TryGetValue(colorName, out var rgb) ? rgb : "0,0,0";
    }
}
