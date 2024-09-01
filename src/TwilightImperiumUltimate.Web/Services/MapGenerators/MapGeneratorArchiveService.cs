namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapGeneratorArchiveService(
    NavigationManager navigationManager)
    : IMapGeneratorArchiveService
{
    private readonly NavigationManager _navigationManager = navigationManager;

    public Task RedirectToAddToArchivePage(MapTemplate mapTemplate, string mapCode)
    {
        _navigationManager.NavigateTo($"{@Pages.Pages.AddMap}?template={mapTemplate}&tiles={mapCode}");
        return Task.CompletedTask;
    }
}
