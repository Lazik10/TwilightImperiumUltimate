using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Services.Path;

public interface IPathProvider
{
    string GetCultureIconPath(string fileName);

    string GetFactionIconPath(string fileName);

    string GetCardTypeIconPath(string fileName);

    string GetComplexityIconPath(int complexity);

    string GetFactionSheetPath(string fileName, bool front = true);

    string GetFactionImagePath(string fileName);

    string GetCardGeneratorImageBackground(string fileName);

    string GetLargeTileImagePath(string fileName);

    string GetTechnologyImagePath(string fileName);

    string GetUnitImagePath(string fileName);

    string GetPlanetImagePath(string fileName);

    string GetFactionComponenetTypeImagePath(string fileName, ComponentType componenetType);

    string GetIconPath(IconType iconType);

    string GetTexturePath(Texture texture);

    string GetPromissoryNotePath(string fileName);
}
