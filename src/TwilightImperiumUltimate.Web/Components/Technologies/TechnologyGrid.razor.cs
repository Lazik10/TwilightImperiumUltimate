using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Services.Cache;

namespace TwilightImperiumUltimate.Web.Components.Technologies;

public partial class TechnologyGrid : TwilightImperiumBaseComponent
{
    private static readonly SingleLoadCache<TechnologyModel> Cache = new();

    private IReadOnlyCollection<TechnologyModel> _technologies = new List<TechnologyModel>();

    private GameVersion _selectedFactionTechGameVersion = GameVersion.BaseGame;

    private FactionName _selectedFactionTechFaction = FactionName.TheArborec;

    [Parameter]
    public TechnologyType SelectedTechnologyType { get; set; } = TechnologyType.Biotic;

    protected override async Task OnInitializedAsync()
    {
        await InitializeTechnologies();
        EnsureValidFactionTechSelection();
    }

    private List<TechnologyModel> TechnologiesToShow()
    {
        if (SelectedTechnologyType == TechnologyType.Faction)
        {
            return _technologies
                .Where(x => x.IsFactionTechnology && x.GameVersion == _selectedFactionTechGameVersion && x.FactionName == _selectedFactionTechFaction)
                .OrderBy(x => x.TechnologyName)
                .ToList();
        }
        else if (SelectedTechnologyType == TechnologyType.UnitUpgrade)
        {
            return _technologies
                .Where(x => x.Type == TechnologyType.UnitUpgrade)
                .Where(x => !x.IsFactionTechnology)
                .OrderBy(x => x.Level)
                .ThenBy(x => x.TechnologyName)
                .ToList();
        }
        else
        {
            return _technologies
                .Where(x => x.Type == SelectedTechnologyType)
                .Where(x => !x.IsFactionTechnology)
                .OrderBy(x => x.Level)
                .ToList();
        }
    }

    private IReadOnlyCollection<KeyValuePair<GameVersion, string>> GetFactionTechGameVersions()
    {
        return _technologies
            .Where(x => x.IsFactionTechnology)
            .Select(x => x.GameVersion)
            .Distinct()
            .OrderBy(x => x)
            .Select(x => new KeyValuePair<GameVersion, string>(x, x.GetDisplayName()))
            .ToList();
    }

    private IReadOnlyCollection<KeyValuePair<FactionName, string>> GetFactionTechFactions()
    {
        return _technologies
            .Where(x => x.IsFactionTechnology && x.GameVersion == _selectedFactionTechGameVersion)
            .Select(x => x.FactionName)
            .Distinct()
            .OrderBy(x => x.GetFactionUIText(FactionResourceType.Title))
            .Select(x => new KeyValuePair<FactionName, string>(x, x.GetFactionUIText(FactionResourceType.Title)))
            .ToList();
    }

    private void OnFactionTechGameVersionChanged(GameVersion gameVersion)
    {
        _selectedFactionTechGameVersion = gameVersion;
        EnsureValidFactionTechSelection();
    }

    private void OnFactionTechFactionChanged(FactionName factionName)
    {
        _selectedFactionTechFaction = factionName;
    }

    private void EnsureValidFactionTechSelection()
    {
        var availableFactions = GetFactionTechFactions().Select(x => x.Key).ToList();
        if (!availableFactions.Contains(_selectedFactionTechFaction))
            _selectedFactionTechFaction = availableFactions.FirstOrDefault();
    }

    private async Task InitializeTechnologies()
    {
        _technologies = await Cache.GetOrLoadAsync(async () =>
        {
            var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<TechnologyDto>>>(Paths.ApiPath_Technologies);
            if (statusCode != HttpStatusCode.OK)
                return [];

            var technologies = Mapper.Map<List<TechnologyModel>>(response!.Data!.Items);
            return technologies.Where(x => x.GameVersion != GameVersion.Deprecated).ToList();
        });
    }
}
