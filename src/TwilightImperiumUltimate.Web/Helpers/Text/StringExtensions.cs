using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace TwilightImperiumUltimate.Web.Helpers.Text;

public static class StringExtensions
{
    public static string FormatWith(this string format, params object[] args)
    {
        return string.Format(CultureInfo.InvariantCulture, format, args);
    }

    public static string FormatText(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        var sentences = input.Split('.')
                              .Select(s => s.Trim())
                              .Where(s => !string.IsNullOrEmpty(s));

        var capitalizedSentences = sentences
            .Select(s => char.ToUpper(s[0], CultureInfo.InvariantCulture) + s.Substring(1));

        var result = string.Join(". ", capitalizedSentences);
        result += ".";

        return Regex.Replace(result, "action", "ACTION", RegexOptions.IgnoreCase);
    }

    public static string KeywordsToUpper(this string input, string[]? keywords)
    {
        if (string.IsNullOrEmpty(input) || keywords is null || keywords.Length == 0)
            return input;

        foreach (var keyword in keywords)
        {
            StringBuilder result = new StringBuilder();
            int lastIndex = 0;
            int index = 0;
            int keywordLength = keyword.Length;

            while ((index = CultureInfo.CurrentCulture.CompareInfo.IndexOf(input, keyword, index, CompareOptions.IgnoreCase)) != -1)
            {
                result.Append(input, lastIndex, index - lastIndex);
                result.Append(keyword.ToUpper(CultureInfo.CurrentCulture));
                lastIndex = index + keywordLength;
                index += keywordLength;
            }

            result.Append(input.AsSpan(lastIndex));
            input = result.ToString();
        }

        return input;
    }

    public static IReadOnlyCollection<string> TextSplittedByKeywords(this string input, string[]? keywords)
    {
        if (string.IsNullOrEmpty(input) || keywords is null || keywords.Length == 0)
            return new List<string> { input };

        List<string> result = new();
        int startIndex = 0;

        while (startIndex < input.Length)
        {
            int closestIndex = int.MaxValue;
            string? closestKeyword = null;

            foreach (string keyword in keywords)
            {
                int index = input.IndexOf(keyword, startIndex, StringComparison.InvariantCultureIgnoreCase);
                if (index != -1 && index < closestIndex)
                {
                    closestIndex = index;
                    closestKeyword = keyword;
                }
            }

            if (closestIndex != int.MaxValue && closestKeyword != null)
            {
                string beforeKeyword = input.Substring(startIndex, closestIndex - startIndex);
                if (!string.IsNullOrEmpty(beforeKeyword.Trim()))
                {
                    result.Add(beforeKeyword);
                }

                result.Add(closestKeyword);
                startIndex = closestIndex + closestKeyword.Length;
            }
            else
            {
                string remaining = input.Substring(startIndex);
                if (!string.IsNullOrEmpty(remaining.Trim()))
                {
                    result.Add(remaining);
                }

                break;
            }
        }

        return result;
    }

    public static string HighlightSearchWord(this string text, string search, string highlightColor, bool toUpper)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(search))
            return text;

        if (toUpper)
            search = search.ToUpper(CultureInfo.CurrentCulture);
        else
            search = search.CapitalizeFirstLetter(CultureInfo.CurrentCulture);

        return text.Replace(search, $"<span style=\"color:{highlightColor};\">{search}</span>", StringComparison.CurrentCultureIgnoreCase);
    }

    public static string CapitalizeFirstLetter(this string text, CultureInfo cultureInfo)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        return char.ToUpper(text[0], cultureInfo) + text.Substring(1).ToLower(CultureInfo.CurrentCulture);
    }
}