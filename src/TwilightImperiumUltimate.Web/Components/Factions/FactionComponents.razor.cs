using TwilightImperiumUltimate.Contracts.DTOs.Card;

namespace TwilightImperiumUltimate.Web.Components.Factions;

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
    private IPathProvider PathProvider { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        await InitializeSystemTilesAsync();
        await InitializeTechnologiesAsync();
        await InitializePromissoryNoteCardsAsync();
        await InitializeBreakthroughCardsAsync();
        await InitializeFlagshipCardsAsync();
        await InitializeSpecialComponentCardsAsync();
    }

    private async Task InitializeSystemTilesAsync()
    {
        var result = await HttpClient.GetAsync<ApiResponse<ItemListDto<SystemTileDto>>>(Paths.ApiPath_SystemTiles);
        var response = result.Response;
        var statusCode = result.StatusCode;
        if (statusCode == HttpStatusCode.OK)
        {
            _systemTiles = Mapper.Map<List<SystemTileModel>>(response!.Data!.Items);
            _systemTile = _systemTiles.First(x => x.FactionName == Faction.FactionName);
            _planets = _systemTile.Planets.ToList();

            // Add the planets from the Creuss wormhole system tile
            if (Faction.FactionName == FactionName.TheGhostsOfCreuss)
                _planets.AddRange(_systemTiles.Single(x => x.SystemTileName == SystemTileName.Tile51).Planets);

            if (Faction.FactionName == FactionName.TheCrimsonRebellion)
                _planets.AddRange(_systemTiles.Single(x => x.SystemTileName == SystemTileName.TileTE118).Planets);

            if (Faction.FactionName == FactionName.TheFirmamentTheObsidian)
                _planets.AddRange(_systemTiles.Single(x => x.SystemTileName == SystemTileName.TileTE96B).Planets);
        }
    }

    private async Task InitializeTechnologiesAsync()
    {
        var result = await HttpClient.GetAsync<ApiResponse<ItemListDto<TechnologyDto>>>(Paths.ApiPath_Technologies);
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _technologies = Mapper.Map<List<TechnologyModel>>(result.Response!.Data!.Items!.Where(x => x.FactionName == Faction.FactionName).ToList());
        }
    }

    private async Task InitializePromissoryNoteCardsAsync()
    {
        var result = await HttpClient.GetAsync<ApiResponse<ItemListDto<PromissoryNoteCardDto>>>(Paths.ApiPath_PromissoryNotes);
        var response = result.Response;
        var statusCode = result.StatusCode;
        if (statusCode == HttpStatusCode.OK)
        {
            _promissoryNotes = Mapper.Map<List<PromissoryNoteCardModel>>(response!.Data!.Items!.Where(x => x.FactionName == Faction.FactionName).ToList());
        }
    }

    private async Task InitializeBreakthroughCardsAsync()
    {
        var result = await HttpClient.GetAsync<ApiResponse<ItemListDto<BreakthroughCardDto>>>(Paths.ApiPath_BreakthroughCards);
        var response = result.Response;
        var statusCode = result.StatusCode;
        if (statusCode == HttpStatusCode.OK)
        {
            _breakthroughCards = Mapper.Map<List<BreakthroughCardModel>>(response!.Data!.Items!.Where(x => x.FactionName == Faction.FactionName).ToList());
        }
    }

    private async Task InitializeFlagshipCardsAsync()
    {
        var result = await HttpClient.GetAsync<ApiResponse<ItemListDto<FlagshipCardDto>>>(Paths.ApiPath_FlagshipCards);
        var response = result.Response;
        var statusCode = result.StatusCode;
        if (statusCode == HttpStatusCode.OK)
        {
            _flagshipCards = Mapper.Map<List<FlagshipCardModel>>(response!.Data!.Items!.Where(x => x.FactionName == Faction.FactionName).ToList());
        }
    }

    private async Task InitializeSpecialComponentCardsAsync()
    {
        var result = await HttpClient.GetAsync<ApiResponse<ItemListDto<SpecialComponentCardDto>>>(Paths.ApiPath_SpecialComponentCards);
        var response = result.Response;
        var statusCode = result.StatusCode;
        if (statusCode == HttpStatusCode.OK)
        {
            _specialComponentCards = Mapper.Map<List<SpecialComponentCardModel>>(response!.Data!.Items!.Where(x => x.FactionName == Faction.FactionName).ToList());
        }
    }

    private string GetBreakthroughPath()
    {
        var name = _breakthroughCards.First().BreakthroughName;
        return PathProvider.GetBreakthroughImagePath(name);
    }

    private string GetSpecificBreakthroughPath(string name)
    {
        return PathProvider.GetBreakthroughImagePath(name);
    }
}
