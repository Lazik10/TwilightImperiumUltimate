using System.Globalization;
using System.Resources;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Helpers.Enums;

public static class EnumExtensions
{
    private static readonly ResourceManager StringsResourceManager = new(Paths.ResourceNamespace_Strings, typeof(Program).Assembly);

    public static string GetDisplayName<TEnum>(this TEnum enumValue)
    {
        ArgumentNullException.ThrowIfNull(enumValue);

        string key = $"{enumValue.GetType().Name}_{enumValue}";
        string? displayName = StringsResourceManager.GetString(key, CultureInfo.InvariantCulture);
        return displayName ?? enumValue.GetType().ToString();
    }

    public static IReadOnlyCollection<KeyValuePair<TEnum, string>> GetEnumValuesWithDisplayNames<TEnum>()
        where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum))
                   .Cast<TEnum>()
                   .Select(x => new KeyValuePair<TEnum, string>(x, x.GetDisplayName()))
                   .ToList();
    }
}
