namespace TwilightImperiumUltimate.Web.Components.Shared.Text.CardGenerator.AgendaCard;

public partial class AgendaCardText
{
    [Parameter]
    public string Text { get; set; } = string.Empty;

    [Parameter]
    public string KeywordText { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public int FontSize { get; set; } = 28;

    [Parameter]
    public int MarginTop { get; set; } = 0;

    private string[]? Keywords => GetKeywords();

    private string FormattedText => Text.FormatText().KeywordsToUpper(Keywords);

    private IReadOnlyCollection<string> TextStrings => FormattedText.TextSplittedByKeywords(Keywords);

    private string[]? GetKeywords() => KeywordText?.Split(',').ToArray();
}
