using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Helpers.Text;
using TwilightImperiumUltimate.Web.Resources;

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
        && Text.Contains(Strings.Keyword_KeywordSplit)
        && !Text.StartsWith(Strings.Keyword_EnglishAction, StringComparison.InvariantCultureIgnoreCase)
        && !Text.StartsWith(Strings.Keyword_CzechAction, StringComparison.InvariantCultureIgnoreCase);

    private string WindowText => HasActionWindow ? FormattedText.Split(Strings.Keyword_KeywordSplit)[0] : string.Empty;

    private string RemainingText => HasActionWindow ? FormattedText.Split(Strings.Keyword_KeywordSplit)[1] : FormattedText;

    private string FormattedText => Text.FormatText().KeywordsToUpper(Keywords);

    private IReadOnlyCollection<string> ActionWindowStrings => WindowText.TextSplittedByKeywords(Keywords);

    private IReadOnlyCollection<string> RemainingTextStrings => RemainingText.TextSplittedByKeywords(Keywords);

    private string[]? GetKeywords() => KeywordText?
        .Split(Strings.Keyword_Split)
        .Append(Strings.Keyword_CzechAction)
        .Append(Strings.Keyword_EnglishAction)
        .ToArray();

    private bool IsActionText(string text)
    {
        return text.StartsWith(Strings.Keyword_CzechAction, StringComparison.InvariantCultureIgnoreCase)
            || text.StartsWith(Strings.Keyword_EnglishAction, StringComparison.InvariantCultureIgnoreCase);
    }

    private bool IsValidKeyword(string text)
    {
        return Keywords is not null && Keywords.Contains(text) && !IsActionText(text);
    }
}
