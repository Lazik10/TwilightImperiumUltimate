using TwilightImperiumUltimate.Web.Resources;
using SystemPath = System.IO;

namespace TwilightImperiumUltimate.Web.Services.Path;

public class PathProvider : IPathProvider
{
    public string GetLaguageIconPath(string fileName)
    {
        return $"{Paths.ResourcePath_Languages}{Strings.BackSlash}{fileName}{Strings.FileExtensionPng}";
    }
}
