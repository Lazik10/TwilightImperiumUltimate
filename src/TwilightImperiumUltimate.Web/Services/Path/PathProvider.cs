using System.Globalization;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.Language;

namespace TwilightImperiumUltimate.Web.Services.Path;

public class PathProvider : IPathProvider
{
    public string GetCultureIconPath(string fileName)
    {
        return $"{Paths.ResourcePath_CulturePath}{Strings.BackSlash}{fileName}{Strings.FileExtensionWebp}";
    }

    public string GetFactionIconPath(string fileName)
    {
        return $"{Paths.ResourcePath_FactionIconPath}{Strings.BackSlash}{fileName}{Strings.FileExtensionWebp}";
    }

    public string GetCardTypeIconPath(string fileName)
    {
        return $"{Paths.ResourcePath_CardTypeIconPath}{Strings.BackSlash}{fileName}{Strings.FileExtensionWebp}";
    }

    public string GetComplexityIconPath(int complexity)
    {
        var complexityName = complexity switch
        {
            1 => Strings.Low,
            2 => Strings.Medium,
            3 => Strings.High,
            _ => string.Empty,
        };

        return $"{Paths.ResourcePath_ComplexityPath}{Strings.BackSlash}{complexityName}{Strings.FileExtensionWebp}";
    }

    public string GetFactionSheetPath(string fileName, bool front = true)
    {
        var factionSheet = front ? Strings.FactionSheet : Strings.FactionSheetBack;
        return $"{Paths.ResourcePath_FactionSheetPath}{Strings.BackSlash}{fileName}_{factionSheet}{Strings.FileExtensionWebp}";
    }

    public string GetFactionImagePath(string fileName)
    {
        return $"{Paths.ResourcePath_FactionImagePath}{Strings.BackSlash}{fileName}{Strings.FileExtensionWebp}";
    }

    public string GetCardGeneratorImageBackground(string fileName)
    {
        var culture = CultureInfo.CurrentCulture.Name;
        return $"{Paths.ResourcePath_CardGeneratorPath.Replace(Strings.LanguagePlaceholder, culture)}{Strings.ForwardSlash}{fileName}{Strings.FileExtensionWebp}";
    }

    public string GetLargeTileImagePath(string fileName)
    {
        return $"{Paths.ResourcePath_LargeTilePath}{Strings.BackSlash}{fileName}{Strings.FileExtensionWebp}";
    }
}
