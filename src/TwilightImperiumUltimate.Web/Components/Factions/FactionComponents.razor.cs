using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.Galaxy;
using TwilightImperiumUltimate.Web.Models.Technologies;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionComponents : FactionInfoComponentBase
{
    private List<SystemTile> _systemTiles = new();

    private SystemTile SystemTile { get; set; } = new();

    private List<Planet> Planets { get; set; } = new();

    private List<Technology> Technologies { get; set; } = new();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        _systemTiles = await HttpClient.GetAsync<List<SystemTile>>(Paths.ApiPath_SystemTiles);
        SystemTile = _systemTiles.Find(x => x.FactionName == Faction.FactionName) ?? new SystemTile();
        Planets = SystemTile.Planets.ToList();

        var technologies = await HttpClient.GetAsync<List<Technology>>(Paths.ApiPath_Technologies);
        Technologies = technologies.Where(x => x.FactionName == Faction.FactionName).ToList();
    }
}
