namespace TwilightImperiumUltimate.Web.Components.Rules;

internal static class RulesAlphabet
{
    public static IReadOnlyCollection<RulesIndexItem> Build(IReadOnlySet<string> availableLetters) =>
        [
            .. Enumerable.Range('A', 26)
                .Select(value => ((char)value).ToString())
                .Select(letter => new RulesIndexItem(
                    letter,
                    letter,
                    IsEnabled: availableLetters.Contains(letter))),
        ];

    public static string Normalize(string? letter, IReadOnlySet<string> availableLetters) =>
        !string.IsNullOrEmpty(letter) && availableLetters.Contains(letter)
            ? letter
            : string.Empty;
}
