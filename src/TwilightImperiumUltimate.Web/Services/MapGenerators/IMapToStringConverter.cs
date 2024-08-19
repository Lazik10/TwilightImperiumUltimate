namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapToStringConverter
{
    Task<string> ConvertMapToTtsString();

    Task<string> ConvertMapToBase64String();

    Task ConvertBase64StringToMap(MapTemplate mapTemplate, string base64String);

    Task ConvertTtsStringToMap(MapTemplate mapTemplate, string ttsString);
}
