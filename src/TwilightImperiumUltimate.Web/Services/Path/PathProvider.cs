using TwilightImperiumUltimate.Web.Resources;

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
}
