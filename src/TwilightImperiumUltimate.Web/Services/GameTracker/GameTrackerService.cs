using TwilightImperiumUltimate.Contracts.DTOs.Draft;

namespace TwilightImperiumUltimate.Web.Services.GameTracker;

public class GameTrackerService(
    ITwilightImperiumApiHttpClient httpClient) : IGameTrackerService
{
    private readonly ITwilightImperiumApiHttpClient _httpClient = httpClient;

    public GameTrackerPhase CurrentPhase { get; private set; } = GameTrackerPhase.Setup;

    public Task EndGame()
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyDictionary<FactionName, PlayerColor>> GetCorrectColors(IReadOnlyCollection<FactionName> factions)
    {
        var colors = Enum.GetValues<PlayerColor>().ToList();

        ColorDraftRequest request = new() { Factions = factions, Colors = colors, };

        var (response, statusCode) = await _httpClient.PostAsync<ColorDraftRequest, ApiResponse<FactionColorDraftResultDto>>(Paths.ApiPath_ColorDraft, request);

        if (statusCode == HttpStatusCode.OK)
        {
            return response!.Data!.FactionColorDraftResults;
        }

        return new Dictionary<FactionName, PlayerColor>();
    }

    public Task PreviousPhase()
    {
        throw new NotImplementedException();
    }

    public Task SetGamePhase(GameTrackerPhase phase)
    {
        CurrentPhase = phase;
        return Task.CompletedTask;
    }

    public Task StartGame()
    {
        throw new NotImplementedException();
    }

    public Task UpdatePlayerFaction(int playerIndex, FactionName factionName)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePlayerInitiative(int playerIndex, InitiativeOrder initiative)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePlayerName(int playerIndex, string name)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePlayerScore(int playerIndex, int score)
    {
        throw new NotImplementedException();
    }
}
