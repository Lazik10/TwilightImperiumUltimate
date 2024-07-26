using TwilightImperiumUltimate.Contracts.DTOs.Card;

namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionComponents : FactionInfoComponentBase
{
    private List<SystemTileModel> _systemTiles = new();

    private SystemTileModel _systemTile = new();

    private List<PlanetModel> _planets = new();

    private List<TechnologyModel> _technologies = new();

    private List<PromissoryNoteCardModel> _promissoryNotes = new List<PromissoryNoteCardModel>();

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
    }

    private async Task InitializeSystemTilesAsync()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<SystemTileDto>>>(Paths.ApiPath_SystemTiles);
        if (statusCode == HttpStatusCode.OK)
        {
            _systemTiles = Mapper.Map<List<SystemTileModel>>(response!.Data!.Items);
            _systemTile = _systemTiles.First(x => x.FactionName == Faction.FactionName);
            _planets = _systemTile.Planets.ToList();

            // Add the planets from the Creuss wormhole system tile
            if (Faction.FactionName == FactionName.TheGhostsOfCreuss)
                _planets.AddRange(_systemTiles.Single(x => x.SystemTileName == SystemTileName.Tile51).Planets);
        }
    }

    private async Task InitializeTechnologiesAsync()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<TechnologyDto>>>(Paths.ApiPath_Technologies);
        if (statusCode == HttpStatusCode.OK)
        {
            _technologies = Mapper.Map<List<TechnologyModel>>(response!.Data!.Items!.Where(x => x.FactionName == Faction.FactionName).ToList());
        }
    }

    private async Task InitializePromissoryNoteCardsAsync()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<PromissoryNoteCardDto>>>(Paths.ApiPath_PromissoryNotes);
        if (statusCode == HttpStatusCode.OK)
        {
            _promissoryNotes = Mapper.Map<List<PromissoryNoteCardModel>>(response!.Data!.Items!.Where(x => x.FactionName == Faction.FactionName).ToList());
        }
    }
}
