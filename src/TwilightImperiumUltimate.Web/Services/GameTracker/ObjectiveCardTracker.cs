using TwilightImperiumUltimate.Contracts.DTOs.Card;
using TwilightImperiumUltimate.Web.Models.GameTracker;

namespace TwilightImperiumUltimate.Web.Services.GameTracker;

public class ObjectiveCardTracker(
    ITwilightImperiumApiHttpClient httpClient,
    IGameTrackerSettingsService gameTrackerSettingsService,
    IMapper mapper)
    : IObjectiveCardTracker
{
    private const int NumberOfObjectiveCards = 5;

    private readonly ITwilightImperiumApiHttpClient _httpClient = httpClient;
    private readonly IGameTrackerSettingsService _gameTrackerSettingsService = gameTrackerSettingsService;
    private readonly IMapper _mapper = mapper;

    private List<GameTrackerObjectiveCardModel> _allObjectiveCards = new();
    private List<GameTrackerObjectiveCardModel> _draftedStageOneObjectiveCards = new();
    private List<GameTrackerObjectiveCardModel> _draftedStageTwoObjectiveCards = new();

    public ObjectiveCardType? IncentiveProgram { get; set; }

    public IReadOnlyCollection<GameTrackerObjectiveCardModel> ObjectiveCards => _allObjectiveCards;

    public IReadOnlyCollection<GameTrackerObjectiveCardModel> Secrets => GetSecrets();

    public IReadOnlyCollection<GameTrackerObjectiveCardModel> DraftedStageOneObjectives => _draftedStageOneObjectiveCards;

    public IReadOnlyCollection<GameTrackerObjectiveCardModel> DraftedStageTwoObjectives => _draftedStageTwoObjectiveCards;

    public bool ManualPickEnabled { get; set; }

    public async Task InitializeRequiredCards()
    {
        await InitializeObjectiveCards();
    }

    public Task DraftObjectiveCards()
    {
        if (_draftedStageOneObjectiveCards is null || _draftedStageOneObjectiveCards.Count == 0)
        {
            _draftedStageOneObjectiveCards = _allObjectiveCards
                .Where(x => x.ObjectiveCard.ObjectiveCardType == ObjectiveCardType.StageOne)
                .OrderBy(x => Guid.NewGuid())
                .Take(NumberOfObjectiveCards)
                .ToList();
        }

        if (_draftedStageTwoObjectiveCards is null || _draftedStageTwoObjectiveCards.Count == 0)
        {
            _draftedStageTwoObjectiveCards = _allObjectiveCards
                .Where(x => x.ObjectiveCard.ObjectiveCardType == ObjectiveCardType.StageTwo)
                .OrderBy(x => Guid.NewGuid())
                .Take(NumberOfObjectiveCards)
                .ToList();
        }

        return Task.CompletedTask;
    }

    public Task RevealNextObjective(bool initialReveal = false)
    {
        if (initialReveal && _draftedStageOneObjectiveCards.Count(x => x.Revealed) >= 2)
            return Task.CompletedTask;

        bool revealed = false;

        foreach (var objective in _draftedStageOneObjectiveCards)
        {
            if (!objective.Revealed)
            {
                objective.Revealed = true;
                revealed = true;

                if (initialReveal && _draftedStageOneObjectiveCards.Count(x => x.Revealed) < 2)
                    continue;

                break;
            }
        }

        if (_draftedStageOneObjectiveCards.TrueForAll(x => x.Revealed) && !revealed)
        {
            foreach (var objective in _draftedStageTwoObjectiveCards)
            {
                if (!objective.Revealed)
                {
                    objective.Revealed = true;
                    break;
                }
            }
        }

        return Task.CompletedTask;
    }

    public Task UpdateObjectiveCards(ObjectiveCardName selectedObjectiveCard, ObjectiveCardName previousObjectiveCard)
    {
        if (_draftedStageOneObjectiveCards.Exists(x => x.ObjectiveCard.ObjectiveCardName == previousObjectiveCard))
        {
            var objective = _draftedStageOneObjectiveCards.Find(x => x.ObjectiveCard.ObjectiveCardName == previousObjectiveCard);
            var newObjective = _allObjectiveCards.Find(x => x.ObjectiveCard.ObjectiveCardName == selectedObjectiveCard);

            if (objective is not null && newObjective is not null)
            {
                var originalObjective = new GameTrackerObjectiveCardModel()
                {
                    ObjectiveCard = objective.ObjectiveCard,
                    Revealed = objective.Revealed,
                    ScoredFactions = objective.ScoredFactions,
                };

                objective.ObjectiveCard = newObjective.ObjectiveCard;
                objective.Revealed = true;
                objective.ScoredFactions = new List<FactionName>();

                newObjective.ObjectiveCard = originalObjective.ObjectiveCard;
                newObjective.Revealed = false;
                newObjective.ScoredFactions = new List<FactionName>();
            }
        }
        else if (_draftedStageTwoObjectiveCards.Exists(x => x.ObjectiveCard.ObjectiveCardName == previousObjectiveCard))
        {
            var objective = _draftedStageTwoObjectiveCards.Find(x => x.ObjectiveCard.ObjectiveCardName == previousObjectiveCard);
            var newObjective = _allObjectiveCards.Find(x => x.ObjectiveCard.ObjectiveCardName == selectedObjectiveCard);
            var previousObjective = _allObjectiveCards.First(x => x.ObjectiveCard.ObjectiveCardName == previousObjectiveCard);
            previousObjective.Revealed = false;

            if (objective is not null && newObjective is not null)
            {
                var originalObjective = new GameTrackerObjectiveCardModel()
                {
                    ObjectiveCard = objective.ObjectiveCard,
                    Revealed = objective.Revealed,
                    ScoredFactions = objective.ScoredFactions,
                };

                objective.ObjectiveCard = newObjective.ObjectiveCard;
                objective.Revealed = true;
                objective.ScoredFactions = new List<FactionName>();

                newObjective.ObjectiveCard = originalObjective.ObjectiveCard;
                newObjective.Revealed = false;
                newObjective.ScoredFactions = new List<FactionName>();
            }
        }

        var stageOne = _draftedStageOneObjectiveCards.Where(x => x.Revealed).Select(x => x.ObjectiveCard.ObjectiveCardName).ToList();
        var stageTwo = _draftedStageTwoObjectiveCards.Where(x => x.Revealed).Select(x => x.ObjectiveCard.ObjectiveCardName).ToList();

        foreach (var objective in _allObjectiveCards)
        {
            if (!stageOne.Contains(objective.ObjectiveCard.ObjectiveCardName)
                && !stageTwo.Contains(objective.ObjectiveCard.ObjectiveCardName))
            {
                objective.Revealed = false;
            }
        }

        return Task.CompletedTask;
    }

    public Task UpdateClassifiedDocumentLeaks(ObjectiveCardName? selectedSecretCard)
    {
        if (selectedSecretCard is null)
            return Task.CompletedTask;

        var secretObjectiveCard = _allObjectiveCards.Find(x => x.ObjectiveCard.ObjectiveCardName == selectedSecretCard);
        if (secretObjectiveCard is not null)
        {
            if (_draftedStageOneObjectiveCards.Count == 5)
            {
                secretObjectiveCard.Revealed = true;
                secretObjectiveCard.IsLeakedSecret = true;
                _draftedStageOneObjectiveCards.Add(secretObjectiveCard);
            }
            else if (_draftedStageTwoObjectiveCards.Count == 5)
            {
                secretObjectiveCard.Revealed = true;
                secretObjectiveCard.IsLeakedSecret = true;
                _draftedStageTwoObjectiveCards.Add(secretObjectiveCard);
            }
        }

        return Task.CompletedTask;
    }

    public Task ClearClassifiedDocumentLeaks()
    {
        _draftedStageOneObjectiveCards.RemoveAll(x => x.ObjectiveCard.ObjectiveCardType == ObjectiveCardType.Secret);
        _draftedStageTwoObjectiveCards.RemoveAll(x => x.ObjectiveCard.ObjectiveCardType == ObjectiveCardType.Secret);
        return Task.CompletedTask;
    }

    public Task ResetIncentiveProgram(ObjectiveCardType objectiveCardType)
    {
        if (objectiveCardType == ObjectiveCardType.StageOne)
        {
            if (_draftedStageOneObjectiveCards.Count(x => x.ObjectiveCard.ObjectiveCardType == ObjectiveCardType.StageOne) == 6)
            {
                var incentiveObjective = _draftedStageOneObjectiveCards[_draftedStageOneObjectiveCards.Count - 1];
                incentiveObjective.Revealed = false;
                incentiveObjective.ScoredFactions = new List<FactionName>();

                _draftedStageOneObjectiveCards.RemoveAt(_draftedStageOneObjectiveCards.Count - 1);
            }
        }
        else if (objectiveCardType == ObjectiveCardType.StageTwo)
        {
            if (_draftedStageTwoObjectiveCards.Count(x => x.ObjectiveCard.ObjectiveCardType == ObjectiveCardType.StageTwo) == 6)
            {
                var incentiveObjective = _draftedStageTwoObjectiveCards[_draftedStageTwoObjectiveCards.Count - 1];
                incentiveObjective.Revealed = false;
                incentiveObjective.ScoredFactions = new List<FactionName>();

                _draftedStageTwoObjectiveCards.RemoveAt(_draftedStageTwoObjectiveCards.Count - 1);
            }
        }

        return Task.CompletedTask;
    }

    public Task RevealIncentiveProgramCard(ObjectiveCardType objectiveCardType)
    {
        if (objectiveCardType == ObjectiveCardType.StageOne)
        {
            var lastStageOneObjective = _draftedStageOneObjectiveCards[_draftedStageOneObjectiveCards.Count - 1];
            if (_draftedStageOneObjectiveCards.Count == 6
                && _draftedStageTwoObjectiveCards.Count == 5
                && lastStageOneObjective.ObjectiveCard.ObjectiveCardType == ObjectiveCardType.Secret)
            {
                _draftedStageTwoObjectiveCards.Add(lastStageOneObjective);
                _draftedStageOneObjectiveCards.Remove(lastStageOneObjective);
            }

            var draftedStageOne = _draftedStageOneObjectiveCards
                .Select(x => x.ObjectiveCard.ObjectiveCardName)
                .ToList();

            var drafted = _allObjectiveCards
                .Where(x =>
                    !draftedStageOne.Contains(x.ObjectiveCard.ObjectiveCardName)
                    && x.ObjectiveCard.ObjectiveCardType == ObjectiveCardType.StageOne)
                .OrderBy(x => Guid.NewGuid())
                .First();

            drafted.Revealed = true;
            _draftedStageOneObjectiveCards.Add(drafted);
        }
        else if (objectiveCardType == ObjectiveCardType.StageTwo)
        {
            var lastStageTwoObjective = _draftedStageTwoObjectiveCards[_draftedStageTwoObjectiveCards.Count - 1];
            if (_draftedStageOneObjectiveCards.Count == 5
                && _draftedStageTwoObjectiveCards.Count == 6
                && lastStageTwoObjective.ObjectiveCard.ObjectiveCardType == ObjectiveCardType.Secret)
            {
                _draftedStageOneObjectiveCards.Add(lastStageTwoObjective);
                _draftedStageTwoObjectiveCards.Remove(lastStageTwoObjective);
            }

            var draftedStageTwo = _draftedStageTwoObjectiveCards
                .Select(x => x.ObjectiveCard.ObjectiveCardName)
                .ToList();

            var drafted = _allObjectiveCards
                .Where(x =>
                    !draftedStageTwo.Contains(x.ObjectiveCard.ObjectiveCardName)
                    && x.ObjectiveCard.ObjectiveCardType == ObjectiveCardType.StageTwo)
                .OrderBy(x => Guid.NewGuid())
                .First();

            drafted.Revealed = true;
            _draftedStageTwoObjectiveCards.Add(drafted);
        }

        return Task.CompletedTask;
    }

    private async Task InitializeObjectiveCards()
    {
        if (_allObjectiveCards is not null && _allObjectiveCards.Count > 0)
            return;

        var (response, statusCode) = await _httpClient.GetAsync<ApiResponse<ItemListDto<ObjectiveCardDto>>>(Paths.ApiPath_ObjectiveCards);

        if (statusCode == HttpStatusCode.OK)
        {
            var objectiveCards = _mapper.Map<List<ObjectiveCardModel>>(response!.Data!.Items);
            var allObjectiveCards = objectiveCards.Select(x => new GameTrackerObjectiveCardModel() { ObjectiveCard = x }).ToList();

            if (_gameTrackerSettingsService.GameVersions.Contains(GameVersion.ProphecyOfKings))
                _allObjectiveCards = allObjectiveCards;
            else
                _allObjectiveCards = allObjectiveCards.Where(x => x.ObjectiveCard.GameVersion != GameVersion.ProphecyOfKings).ToList();
        }
    }

    private List<GameTrackerObjectiveCardModel> GetSecrets()
    {
        return _allObjectiveCards
                .Where(x => x.ObjectiveCard.ObjectiveCardType == ObjectiveCardType.Secret)
                .ToList();
    }
}
