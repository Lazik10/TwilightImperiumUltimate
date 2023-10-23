using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Helpers.Text;

namespace TwilightImperiumUltimate.Web.Components.Shared.Text.CardGenerator.ActionCard;

public partial class ActionCardText
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

    private bool HasActionWindow => Text is not null
        && Text.Contains(':')
        && !Text.StartsWith("Action:", StringComparison.InvariantCultureIgnoreCase);

    private string WindowText => HasActionWindow ? FormattedText.Split(':')[0] : string.Empty;

    private string RemainingText => HasActionWindow ? FormattedText.Split(':')[1] : FormattedText;

    private string FormattedText => Text.FormatText().KeywordsToUpper(Keywords);

    private IReadOnlyCollection<string> ActionWindowStrings => WindowText.TextSplittedByKeywords(Keywords);

    private IReadOnlyCollection<string> RemainingTextStrings => RemainingText.TextSplittedByKeywords(Keywords);

    private string[]? GetKeywords() => KeywordText?.Split(',').ToArray();
}
