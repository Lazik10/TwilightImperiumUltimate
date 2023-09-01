using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Services.Path;

public class PathProvider : IPathProvider
{
    public string GetCultureIconPath(string fileName)
    {
        return $"{Paths.ResourcePath_CulturePath}{Strings.BackSlash}{fileName}{Strings.FileExtensionPng}";
    }
}
