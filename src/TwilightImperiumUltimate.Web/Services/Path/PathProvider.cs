using System.Globalization;

namespace TwilightImperiumUltimate.Web.Services.Path;

public class PathProvider : IPathProvider
{
    private const string _basePath = "resources\\images";
    private readonly string _language = CultureInfo.CurrentCulture.Name;
    private static Random _random = new Random();

    public string GetCultureIconPath(string fileName)
    {
        return $"{Paths.ResourcePath_CulturePath}{Strings.BackSlash}{fileName}{Strings.FileExtensionWebp}";
    }

    public string GetFactionIconPath(FactionName factionName)
    {
        return $"{Paths.ResourcePath_FactionIconPath}{Strings.BackSlash}{factionName}{Strings.FileExtensionWebp}";
    }

    public string GetAsyncFactionIconPath(AsyncFactionName factionName)
    {
        return $"{Paths.ResourcePath_FactionIconPath}{Strings.BackSlash}{factionName}{Strings.FileExtensionWebp}";
    }

    public string GetCardTypeIconPath(string fileName)
    {
        return $"{Paths.ResourcePath_CardTypeIconPath}{Strings.BackSlash}{fileName}{Strings.FileExtensionWebp}";
    }

    public string GetComplexityIconPath(ComplexityRating complexity)
    {
        var complexityName = complexity switch
        {
            ComplexityRating.Low => Strings.Low,
            ComplexityRating.Medium => Strings.Medium,
            ComplexityRating.High => Strings.High,
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

    public string GetLargeTileImagePath(SystemTileName systemTileName)
    {
        return $"{Paths.ResourcePath_LargeTilePath}{Strings.BackSlash}{systemTileName}{Strings.FileExtensionWebp}";
    }

    public string GetTechnologyImagePath(TechnologyName technologyName)
    {
        return $"{GetCorrectLanguagePath(Paths.ResourcePath_TechnologyImagePath)}{Strings.BackSlash}{technologyName}{Strings.FileExtensionWebp}";
    }

    public string GetTechnologyIconPath(TechnologyType technologyType)
    {
        return $"{Paths.ResourcePath_TechnologyIconPath}{Strings.BackSlash}{technologyType}{Strings.FileExtensionWebp}";
    }

    public string GetPlanetTraitPath(PlanetTrait planetTrait)
    {
        return $"{Paths.ResourcePath_PlanetTraitIconPath}{Strings.BackSlash}{planetTrait}{Strings.FileExtensionWebp}";
    }

    public string GetUnitImagePath(UnitName unitName)
    {
        return $"{Paths.ResourcePath_UnitImagePath}{Strings.BackSlash}{unitName}{Strings.FileExtensionWebp}";
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

    public string GetCardImagePath(string fileName, string cardType)
    {
        ArgumentNullException.ThrowIfNull(cardType);

        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentException("Enum name is a required parameter.");

        return $"{_basePath}\\{_language}\\cards\\{cardType.ToLowerInvariant()}\\{fileName}.webp";
    }

    public string GetObjectiveCardBackPath(ObjectiveCardType objectiveCardType)
    {
        return $"{_basePath}\\shared\\cardbacks\\{objectiveCardType}{Strings.FileExtensionWebp}";
    }

    public string GetGameVersionIconPath(GameVersion gameVersion)
    {
        return $"{Paths.ResourcePath_GameVersionIconPath}{Strings.BackSlash}{gameVersion}{Strings.FileExtensionWebp}";
    }

    public string GetTiglFactionIconPath(TiglFactionName faction)
    {
        var factionString = faction.ToString();
        if (faction == TiglFactionName.TheFirmamentTheObsidian)
        {
            var variants = new List<string> { "TheFirmament", "TheObsidian" };
            factionString = variants[_random.Next(variants.Count)].ToString();
        }

        return $"{Paths.ResourcePath_FactionIconPath}{Strings.BackSlash}{factionString}{Strings.FileExtensionWebp}";
    }

    public string GetLeaderIconPath(TiglFactionName faction, LeaderType type)
    {
        var factionString = faction.ToString();

        if (faction == TiglFactionName.TheGheminaRaiders)
        {
            var variants = new List<string> { "Lady", "Lord" };
            factionString += variants[_random.Next(variants.Count)].ToString();
        }

        if (faction == TiglFactionName.TheFirmamentTheObsidian)
        {
            var variants = new List<string> { "TheFirmament", "TheObsidian" };
            factionString = variants[_random.Next(variants.Count)].ToString();
        }

        return $"{Paths.ResourcePath_LeaderIconPath}{Strings.BackSlash}{factionString}{type}{Strings.FileExtensionWebp}";
    }

    public string GetWebsitePreviewImagePath(string fileName)
    {
        // TODO: This is bad, should be handled differently, but I don't have the time to refactor this now.
        var websitePath = fileName switch
        {
            "Fantasy Flight Games" => "FFG",
            "Twilight Imperium Wiki" => "Wiki",
            "Twilight Imperium 4th Edition Rules Reference" => "Rules",
            "Board Game Geek" => "BGG",
            "Reddit" => "Reddit",
            "Facebook CZSK" => "FacebookCZSK",
            "Facebook TI" => "Facebook",
            "Facebook Global" => "FacebookGlobal",
            "SCPT Podcasts" => "SCPTPodcasts",
            "Twilight Wars" => "TwilightWars",
            "Twilight Imperium Assistant" => "TIAssistant",
            "Online Match Stats" => "OnlineMatchStats",
            "Keegan Map Generator" => "KeeganMapGenerator",
            "TI4 Lab" => "Lab",
            "Twilight Imperium 4 Balanced Map Generator" => "BalancedMapGenerator",
            "TI4 Map Lab" => "MapLab",
            "Milty Draft Generator" => "MiltyDraft",
            "Milty Draft Map Generator" => "MiltyDraftMapGenerator",
            "TI4 Cartographer" => "Cartographer",
            "Color Picker" => "ColorPicker",
            "Twilight Imperium 4th Color Assigner" => "ColorAssigner",
            "Card Generator" => "CardGenerator",
            "Discord" => "Discord",
            _ => string.Empty,
        };

        return $"{Paths.ResourcePath_WebsitePreviewPath}{Strings.BackSlash}{websitePath}{Strings.FileExtensionWebp}";
    }

    public string GetBreakthroughImagePath(BreakthroughName breakthroughName)
    {
        return $"{GetCorrectLanguagePath(Paths.ResourcePath_BreakthroughPath)}{Strings.BackSlash}{breakthroughName}{Strings.FileExtensionWebp}";
    }

    public string GetBreakthroughImagePath(string breakthroughName)
    {
        return $"{GetCorrectLanguagePath(Paths.ResourcePath_BreakthroughPath)}{Strings.BackSlash}{breakthroughName}{Strings.FileExtensionWebp}";
    }

    public string GetFlagshipImagePath(FlagshipName flagshipName)
    {
        return $"{GetCorrectLanguagePath(Paths.ResourcePath_FlagshipPath)}{Strings.BackSlash}{flagshipName}{Strings.FileExtensionWebp}";
    }

    public string GetSpecialComponentImagePath(SpecialComponentName specialComponentName)
    {
        return $"{GetCorrectLanguagePath(Paths.ResourcePath_SpecialComponentPath)}{Strings.BackSlash}{specialComponentName}{Strings.FileExtensionWebp}";
    }

    private string GetCorrectLanguagePath(string path)
    {
        return path.Replace(Strings.LanguagePlaceholder, _language);
    }
}
