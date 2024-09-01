using TwilightImperiumUltimate.Web.Pages.Game;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapToStringConverter
{
    IReadOnlyDictionary<int, SystemTileModel> Map { get; }

    Task<string> ConvertMapToTtsString();

    Task<string> ConvertMapToTtsString(MapTemplate mapTemplate, IReadOnlyDictionary<int, SystemTileModel> map);

    Task<string> ConvertMapToBase64String();

    Task<string> ConvertMapToBase64String(MapTemplate mapTemplate, IReadOnlyDictionary<int, SystemTileModel> map);

    Task ConvertBase64StringToMap(MapTemplate mapTemplate, string base64String);

    Task ConvertTtsStringToMap(MapTemplate mapTemplate, string ttsString);

    Task<string> GenerateMapGeneratorLink(MapTemplate mapTemplate, string mapCode);

    Task<string> GenerateMapArchiveLink(string mapName, string mapEvent);
}
