using System.Net.Http.Json;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Helpers.Text;
using TwilightImperiumUltimate.Web.Models.Drafts;
using TwilightImperiumUltimate.Web.Models.Factions;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Settings.Drafts;

namespace TwilightImperiumUltimate.Web.Services.Draft;

public class FactionDraftService : IFactionDraftService
{
    private static readonly Random Random = new();
    private readonly ITwilightImperiumApiHttpClient _httpClient;
    private readonly List<FactionDraftPlayerModel> _players = new();
    private IReadOnlyCollection<FactionModel> _factionsWithBanStatus = new List<FactionModel>();

    public FactionDraftService(ITwilightImperiumApiHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public event EventHandler? OnFactionUpdate;

    public IReadOnlyCollection<FactionDraftPlayerModel> Players => _players;

    public IReadOnlyCollection<FactionModel> FactionsWithUpdatedBanStatus => _factionsWithBanStatus;

    public int NumberOfPlayers { get; set; } = FactionDraftSettings.InitialNumberOfPlayers;

    public int NumberOfDraftFactions { get; set; } = FactionDraftSettings.MinNumberOfDraftFactions;

    public bool ToManyBans => FactionsWithUpdatedBanStatus
        .Count(x => x.Banned) > FactionDraftSettings.MumberOfFactions - (NumberOfPlayers * NumberOfDraftFactions);

    public void InitializePlayers()
    {
        if (_players.Count == 0)
        {
            _players.AddRange(Enumerable
                .Range(1, FactionDraftSettings.InitialNumberOfPlayers)
                .Select(x => new FactionDraftPlayerModel()
                {
                    Name = Strings.PlayerNamePlaceholder.FormatWith(x),
                    DraftedFactions = new List<FactionName>() { FactionName.None },
                }));
        }
    }

    public void IncreasePlayerCount()
    {
        if (NumberOfPlayers < FactionDraftSettings.MaxNumberOfPlayers)
        {
            NumberOfPlayers++;

            if (!PossibleDraftCombination())
                NumberOfDraftFactions = FactionDraftSettings.MumberOfFactions / NumberOfPlayers;

            _players.Add(new FactionDraftPlayerModel()
            {
                Name = Strings.PlayerNamePlaceholder.FormatWith(Players.Count + 1),
            });

            ResetPlayerFactions();
        }
    }

    public void DecreasePlayerCount()
    {
        if (NumberOfPlayers > FactionDraftSettings.MinNumberOfPlayers)
        {
            NumberOfPlayers--;
            _players.RemoveAt(Players.Count - 1);
        }
    }

    public void IncreaseFactionCount()
    {
        if (NumberOfDraftFactions < FactionDraftSettings.MumberOfFactions)
        {
            NumberOfDraftFactions++;
            ResetPlayerFactions();

            if (!PossibleDraftCombination())
            {
                var playerDifference = NumberOfPlayers - (FactionDraftSettings.MumberOfFactions / NumberOfDraftFactions);

                for (int i = 0; i < playerDifference; i++)
                {
                    DecreasePlayerCount();
                }
            }
        }
    }

    public void DecreaseFactionCount()
    {
        if (NumberOfDraftFactions > FactionDraftSettings.MinNumberOfDraftFactions)
            NumberOfDraftFactions--;

        ResetPlayerFactions();
    }

    public void ResetBans()
    {
        foreach (var faction in _factionsWithBanStatus)
        {
            faction.Banned = false;
        }
    }

    public void ResetPlayerFactions()
    {
        foreach (var player in Players)
        {
            player.DraftedFactions = Enumerable.Range(1, NumberOfDraftFactions)
                .Select(x => FactionName.None)
                .ToList();
        }
    }

    public async Task PerformDraft()
    {
        await StartRandomFactionAssignmentAsync();
        var draftResult = await GetDraftResults();
        AssignDraftResultsToPlayers(draftResult);
    }

    public void UpdateBanFactions(IReadOnlyCollection<FactionModel>? factionsWithBanStatus)
    {
        _factionsWithBanStatus = factionsWithBanStatus ?? new List<FactionModel>();
    }

    private async Task StartRandomFactionAssignmentAsync()
    {
        for (int i = 0; i < FactionDraftSettings.DefaultNumberOfAssignments; i++)
        {
            RandomFactionAssignment();
            OnFactionUpdate?.Invoke(this, EventArgs.Empty);
            await Task.Delay(FactionDraftSettings.DefaultDelayInMilliseconds);
        }
    }

    private void AssignDraftResultsToPlayers(IReadOnlyCollection<DraftResult>? draftResult)
    {
        if (draftResult is not null)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                Players.ToArray()[i].DraftedFactions = draftResult.ToArray()[i].Factions.ToList();
            }
        }
    }

    private async Task<IReadOnlyCollection<DraftResult>?> GetDraftResults()
    {
        FactionDraftRequest request = new()
        {
            NumberOfPlayers = NumberOfPlayers,
            NumberOfFactionsPerPlayer = NumberOfDraftFactions,
            Factions = _factionsWithBanStatus,
        };

        var result = await _httpClient.PostAsync<FactionDraftRequest, List<DraftResult>>(Paths.ApiPath_FactionDraft, request);

        return result;
    }

    private FactionName GetRandomFaction()
    {
        var pickedRandomFactions = Enum.GetValues(typeof(FactionName))
           .Cast<FactionName>()
           .ToList();

        pickedRandomFactions.RemoveAt(0);

        var randomFaction = pickedRandomFactions.OrderBy(x => Random.Next()).Take(1).Single();

        return randomFaction;
    }

    private void RandomFactionAssignment()
    {
        foreach (var player in Players)
        {
            player.DraftedFactions = player.DraftedFactions.Select(x => GetRandomFaction()).ToList();
        }
    }

    private bool PossibleDraftCombination()
    {
        return NumberOfPlayers * NumberOfDraftFactions <= FactionDraftSettings.MumberOfFactions;
    }
}
