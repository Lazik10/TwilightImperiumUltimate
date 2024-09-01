namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapGeneratorArchiveService
{
    Task RedirectToAddToArchivePage(MapTemplate mapTemplate, string mapCode);
}
