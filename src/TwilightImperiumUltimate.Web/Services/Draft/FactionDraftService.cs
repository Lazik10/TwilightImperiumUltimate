using TwilightImperiumUltimate.Contracts.DTOs.Draft;
using TwilightImperiumUltimate.Web.Options.Drafts;

namespace TwilightImperiumUltimate.Web.Services.Draft;

public class FactionDraftService(ITwilightImperiumApiHttpClient httpClient)
    : IFactionDraftService
{
    private static readonly Random Random = new();
    private readonly ITwilightImperiumApiHttpClient _httpClient = httpClient;
    private readonly List<FactionDraftPlayerModel> _players = [];
    private IReadOnlyCollection<FactionModel> _factionsWithBanStatus = [];

    public event EventHandler? OnFactionUpdate;

    public IReadOnlyCollection<FactionDraftPlayerModel> Players => _players;

    public IReadOnlyCollection<FactionModel> FactionsWithUpdatedBanStatus => _factionsWithBanStatus;

    public int NumberOfPlayers { get; set; } = FactionDraftOptions.InitialNumberOfPlayers;

    public int NumberOfDraftFactions { get; set; } = FactionDraftOptions.MinNumberOfDraftFactions;

    public bool ToManyBans => FactionsWithUpdatedBanStatus
        .Count(x => x.Banned) > FactionDraftOptions.MumberOfFactions - (NumberOfPlayers * NumberOfDraftFactions);

    public void InitializePlayers()
    {
        if (_players.Count == 0)
        {
            _players.AddRange(Enumerable
                .Range(1, FactionDraftOptions.InitialNumberOfPlayers)
                .Select(x => new FactionDraftPlayerModel()
                {
                    Name = Strings.PlayerNamePlaceholder.FormatWith(x),
                    DraftedFactions = [FactionName.None],
                }));
        }
    }

    public void IncreasePlayerCount()
    {
        if (NumberOfPlayers < FactionDraftOptions.MaxNumberOfPlayers)
        {
            NumberOfPlayers++;

            if (!PossibleDraftCombination())
                NumberOfDraftFactions = FactionDraftOptions.MumberOfFactions / NumberOfPlayers;

            _players.Add(new FactionDraftPlayerModel()
            {
                Name = Strings.PlayerNamePlaceholder.FormatWith(Players.Count + 1),
            });

            ResetPlayerFactions();
        }
    }

    public void DecreasePlayerCount()
    {
        if (NumberOfPlayers > FactionDraftOptions.MinNumberOfPlayers)
        {
            NumberOfPlayers--;
            _players.RemoveAt(Players.Count - 1);
        }
    }

    public void IncreaseFactionCount()
    {
        if (NumberOfDraftFactions < FactionDraftOptions.MumberOfFactions)
        {
            NumberOfDraftFactions++;
            ResetPlayerFactions();

            if (!PossibleDraftCombination())
            {
                var playerDifference = NumberOfPlayers - (FactionDraftOptions.MumberOfFactions / NumberOfDraftFactions);

                for (int i = 0; i < playerDifference; i++)
                {
                    DecreasePlayerCount();
                }
            }
        }
    }

    public void DecreaseFactionCount()
    {
        if (NumberOfDraftFactions > FactionDraftOptions.MinNumberOfDraftFactions)
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

    public void GameVersionGlobalEnableDisable(GameVersion version)
    {
        if (_factionsWithBanStatus.Any(x => x.GameVersion == version && !x.Banned))
        {
            var updatedList = _factionsWithBanStatus.ToList();
            updatedList.ForEach(x =>
            {
                if (x.GameVersion == version)
                    x.Banned = true;
            });
            _factionsWithBanStatus = updatedList;
        }
        else if (_factionsWithBanStatus.Where(x => x.GameVersion == version).All(x => x.Banned))
        {
            var updatedList = _factionsWithBanStatus.ToList();
            updatedList.ForEach(x =>
            {
                if (x.GameVersion == version)
                    x.Banned = false;
            });
            _factionsWithBanStatus = updatedList;
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

    private FactionName GetRandomFaction()
    {
        var pickedRandomFactions = _factionsWithBanStatus.Where(x => !x.Banned).Select(x => x.FactionName).ToList();

        pickedRandomFactions.RemoveAt(0);

        var randomFaction = pickedRandomFactions.OrderBy(x => Random.Next()).Take(1).Single();

        return randomFaction;
    }

    private async Task StartRandomFactionAssignmentAsync()
    {
        for (int i = 0; i < FactionDraftOptions.DefaultNumberOfAssignments; i++)
        {
            RandomFactionAssignment();
            OnFactionUpdate?.Invoke(this, EventArgs.Empty);
            await Task.Delay(FactionDraftOptions.DefaultDelayInMilliseconds);
        }
    }

    private void AssignDraftResultsToPlayers(IReadOnlyCollection<DraftResult>? draftResult)
    {
        if (draftResult is not null && draftResult.Count != 0)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                Players.ToArray()[i].DraftedFactions = draftResult.ToArray()[i].Factions.ToList();
            }
        }
        else
        {
            for (int i = 0; i < Players.Count; i++)
            {
                Players.ToArray()[i].DraftedFactions = new List<FactionName>() { FactionName.None };
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

        var (response, statusCode) = await _httpClient.PostAsync<FactionDraftRequest, ApiResponse<FactionDraftResultDto>>(Paths.ApiPath_FactionDraft, request);

        if (statusCode == HttpStatusCode.OK)
        {
            var draftResults = new List<DraftResult>();
            foreach (var draftResult in response!.Data!.FactionDraftResults)
            {
                draftResults.Add(new DraftResult()
                {
                    PlayerId = draftResult.Key,
                    Factions = draftResult.Value,
                });
            }

            return draftResults;
        }

        return new List<DraftResult>();
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
        return NumberOfPlayers * NumberOfDraftFactions <= FactionDraftOptions.MumberOfFactions;
    }
}
