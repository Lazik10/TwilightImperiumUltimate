namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class FaqRow
{
    [Parameter]
    public FaqModel Faq { get; set; } = new FaqModel();

    [Parameter]
    public string Culture { get; set; } = string.Empty;

    [Parameter]
    public int Width { get; set; } = 100;
}