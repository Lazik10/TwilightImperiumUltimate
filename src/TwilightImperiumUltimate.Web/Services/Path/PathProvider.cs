﻿using System.Globalization;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Services.Path;

public class PathProvider : IPathProvider
{
    private readonly string _language = CultureInfo.CurrentCulture.Name;

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
        return $"{GetCorrectLanguagePath(Paths.ResourcePath_CardGeneratorPath)}{Strings.ForwardSlash}{fileName}{Strings.FileExtensionWebp}";
    }

    public string GetLargeTileImagePath(string fileName)
    {
        return $"{Paths.ResourcePath_LargeTilePath}{Strings.BackSlash}{fileName}{Strings.FileExtensionWebp}";
    }

    public string GetTechnologyImagePath(string fileName)
    {
        return $"{GetCorrectLanguagePath(Paths.ResourcePath_TechnologyImagePath)}{Strings.BackSlash}{fileName}{Strings.FileExtensionWebp}";
    }

    public string GetUnitImagePath(string fileName)
    {
        return $"{Paths.ResourcePath_UnitImagePath}{Strings.BackSlash}{fileName}{Strings.FileExtensionWebp}";
    }

    public string GetPlanetImagePath(string fileName)
    {
        return $"{GetCorrectLanguagePath(Paths.ResourcePath_PlanetImagePath)}{Strings.BackSlash}{fileName}{Strings.FileExtensionWebp}";
    }

    public string GetFactionComponenetTypeImagePath(string fileName, ComponentType componenetType)
    {
        return $"{GetCorrectLanguagePath(Paths.ResourcePath_FactionComponentsPath)}{Strings.BackSlash}{fileName}{componenetType}{Strings.FileExtensionWebp}";
    }

    public string GetIconPath(IconType iconType)
    {
        return $"{Paths.ResourcePath_IconPath}{Strings.BackSlash}{iconType}{Strings.FileExtensionWebp}";
    }

    public string GetTexturePath(Texture texture)
    {
        return $"{Paths.ResourcePath_TexturePath}{Strings.BackSlash}{texture}{Strings.FileExtensionWebp}";
    }

    public string GetPromissoryNotePath(string fileName)
    {
        return $"{GetCorrectLanguagePath(Paths.ResourcePath_PromissoryNotePath)}{Strings.BackSlash}{fileName}{Strings.FileExtensionWebp}";
    }

    private string GetCorrectLanguagePath(string path)
    {
        return path.Replace(Strings.LanguagePlaceholder, _language);
    }
}
