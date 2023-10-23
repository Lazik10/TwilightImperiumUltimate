using Microsoft.AspNetCore.Components;

namespace TwilightImperiumUltimate.Web.Components.CardGenerator.CardTypeGrids;

public partial class ActionCardGeneratorGrid : BaseCardPreview
{
    [Parameter]
    public string FlavoredText { get; set; } = string.Empty;

    [Parameter]
    public string KeywordsText { get; set; } = string.Empty;

    [Parameter]
    public int TitleFontSize { get; set; } = 28;

    [Parameter]
    public int TextFontSize { get; set; } = 28;

    [Parameter]
    public int FlavoredFontSize { get; set; } = 28;
}
