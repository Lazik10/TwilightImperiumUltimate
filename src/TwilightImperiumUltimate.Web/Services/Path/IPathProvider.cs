namespace TwilightImperiumUltimate.Web.Services.Path;

public interface IPathProvider
{
    string GetCultureIconPath(string fileName);

    string GetFactionIconPath(FactionName factionName);

    string GetAsyncFactionIconPath(AsyncFactionName factionName);

    string GetCardTypeIconPath(string fileName);

    string GetComplexityIconPath(ComplexityRating complexity);

    string GetFactionSheetPath(string fileName, bool front = true);

    string GetFactionImagePath(string fileName);

    string GetCardGeneratorImageBackground(string fileName);

    string GetLargeTileImagePath(SystemTileName systemTileName);

    string GetTechnologyImagePath(TechnologyName technologyName);

    string GetTechnologyIconPath(TechnologyType technologyType);

    string GetPlanetTraitPath(PlanetTrait planetTrait);

    string GetUnitImagePath(UnitName unitName);

    string GetPlanetImagePath(string fileName);

    string GetFactionComponenetTypeImagePath(string fileName, ComponentType componenetType);

    string GetIconPath(IconType iconType);

    string GetTexturePath(Texture texture);

    string GetPromissoryNotePath(string fileName);

    string GetCardImagePath(string fileName, string cardType);

    string GetObjectiveCardBackPath(ObjectiveCardType objectiveCardType);

    string GetWebsitePreviewImagePath(string fileName);

    string GetGameVersionIconPath(GameVersion gameVersion);

    string GetTiglFactionIconPath(TiglFactionName faction);

    string GetLeaderIconPath(TiglFactionName faction, LeaderType type);

    string GetBreakthroughImagePath(BreakthroughName breakthroughName);

    string GetBreakthroughImagePath(string breakthroughName);

    string GetFlagshipImagePath(FlagshipName flagshipName);

    string GetSpecialComponentImagePath(SpecialComponentName specialComponentName);
}
