namespace TwilightImperiumUltimate.Web.Services.Path;

public interface IPathProvider
{
    string GetCultureIconPath(string fileName);

    string GetFactionIconPath(string fileName);

    string GetCardTypeIconPath(string fileName);
}
