namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionComponents : FactionInfoComponentBase
{
    private List<SystemTileModel> _systemTiles = new();

    private SystemTileModel _systemTile = new();

    private List<PlanetModel> _planets = new();

    private List<TechnologyModel> _technologies = new();

    private List<PromissoryNoteCardModel> _promissoryNotes = new List<PromissoryNoteCardModel>();

    private List<BreakthroughCardModel> _breakthroughCards = new();

    private List<FlagshipCardModel> _flagshipCards = new();

    private List<SpecialComponentCardModel> _specialComponentCards = new();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await FactionComponentsCache.EnsureLoadedAsync(HttpClient, Mapper);
        UpdateFactionComponents();
    }

    protected override void OnParametersSet()
    {
        UpdateFactionComponents();
    }

    private bool IsFirmamentObsidian() => FactionName == FactionName.TheFirmamentTheObsidian;

    private SystemTileModel GetObsidianSystemTile() => _systemTiles.Single(x => x.SystemTileName == SystemTileName.TileTE96B);

    private IReadOnlyList<PlanetModel> GetFirmamentPlanets() => _planets
        .Where(x => x.PlanetName is PlanetName.Cronos or PlanetName.Tallin)
        .ToList();

    private IReadOnlyList<PlanetModel> GetObsidianPlanets() => _planets
        .Where(x => x.PlanetName is PlanetName.CronosHollow or PlanetName.TallinHollow)
        .ToList();

    private void UpdateFactionComponents()
    {
        if (FactionComponentsCache.SystemTiles.Count == 0)
            return;

        _systemTiles = FactionComponentsCache.SystemTiles.ToList();
        _systemTile = FactionName switch
        {
            FactionName.TheGhostsOfCreuss => _systemTiles.First(x => x.SystemTileName == SystemTileName.Tile51),
            FactionName.TheCrimsonRebellion => _systemTiles.First(x => x.SystemTileName == SystemTileName.TileTE118),
            _ => _systemTiles.First(x => x.FactionName == FactionName),
        };
        _planets = _systemTile.Planets.ToList();

        if (FactionName == FactionName.TheFirmamentTheObsidian)
            _planets.AddRange(_systemTiles.Single(x => x.SystemTileName == SystemTileName.TileTE96B).Planets);

        _technologies = FactionComponentsCache.Technologies.Where(x => x.FactionName == FactionName).ToList();
        _promissoryNotes = FactionComponentsCache.PromissoryNotes.Where(x => x.FactionName == FactionName).ToList();
        _breakthroughCards = FactionComponentsCache.BreakthroughCards.Where(x => x.FactionName == FactionName).ToList();
        _flagshipCards = FactionComponentsCache.FlagshipCards.Where(x => x.FactionName == FactionName).ToList();
        _specialComponentCards = FactionComponentsCache.SpecialComponentCards.Where(x => x.FactionName == FactionName).ToList();
    }
}
