using TwilightImperiumUltimate.Contracts.DTOs.Card;
using TwilightImperiumUltimate.Web.Models.GameTracker;
using TwilightImperiumUltimate.Web.Services.GameTracker;

namespace TwilightImperiumUltimate.Web.Components.GameTracker;

public partial class StrategyWindow
{
    private List<StrategyCardModel> _strategyCards = new List<StrategyCardModel>();

    private int PlayersCount => (GameTrackerService.Players.Count / 2) + (GameTrackerService.Players.Count % 2 == 0 ? 0 : 1);

    [Inject]
    private IGameTrackerService GameTrackerService { get; set; } = default!;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await InitializeStrategyCards();
    }

    private IReadOnlyCollection<GameTrackerPlayerModel> GetPlayers()
    {
        return GameTrackerService.Players;
    }

    private async Task InitializeStrategyCards()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<StrategyCardDto>>>(Paths.ApiPath_StrategyCards);

        if (statusCode == System.Net.HttpStatusCode.OK)
        {
            var cards = Mapper.Map<List<StrategyCardModel>>(response!.Data!.Items);
            _strategyCards = cards.Where(x => x.GameVersion != GameVersion.Deprecated).ToList();
        }
    }
}