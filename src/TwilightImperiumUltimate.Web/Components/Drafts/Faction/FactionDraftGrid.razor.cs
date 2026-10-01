using TwilightImperiumUltimate.Web.Services.Draft;
using TwilightImperiumUltimate.Web.Options.Drafts;

namespace TwilightImperiumUltimate.Web.Components.Drafts.Faction;

public partial class FactionDraftGrid
{
    private readonly IReadOnlyCollection<KeyValuePair<bool, string>> _booleanOptions =
    [
        new(false, Strings.String_Disabled),
        new(true, Strings.String_Enabled),
    ];

    private DraftStage _draftStage = DraftStage.Draft;

    private FactionMenuPicker? _factionMenuPicker;

    private bool _showSettings;

    [Inject]
    private ILogger<FactionDraftGrid> Logger { get; set; } = null!;

    [Inject]
    private IFactionDraftService FactionDraftService { get; set; } = null!;

    private IReadOnlyCollection<KeyValuePair<bool, string>> BooleanOptions => _booleanOptions;

    protected override void OnInitialized()
    {
        FactionDraftService.InitializePlayers();
        FactionDraftService.ResetBans();
        FactionDraftService.OnFactionUpdate += HandleOnDataUpdated;
    }

    private void UpdateBanFactions(FactionModel faction)
    {
        FactionDraftService.UpdateBanFactions(_factionMenuPicker?.Factions);
    }

    private void InitializeBanFactions(IReadOnlyCollection<FactionModel> factionsWithBanStatus)
    {
        FactionDraftService.UpdateBanFactions(factionsWithBanStatus);
    }

    private void SetPlayerCount(int numberOfPlayers)
    {
        while (FactionDraftService.NumberOfPlayers > numberOfPlayers)
            FactionDraftService.DecreasePlayerCount();

        while (FactionDraftService.NumberOfPlayers < numberOfPlayers)
            FactionDraftService.IncreasePlayerCount();

        StateHasChanged();
    }

    private void SetFactionCount(int numberOfFactions)
    {
        while (FactionDraftService.NumberOfDraftFactions > numberOfFactions)
            FactionDraftService.DecreaseFactionCount();

        while (FactionDraftService.NumberOfDraftFactions < numberOfFactions)
            FactionDraftService.IncreaseFactionCount();

        StateHasChanged();
    }

    private void SetPlayerNamesEnabled(bool enablePlayerNames)
    {
        FactionDraftService.EnablePlayerNames = enablePlayerNames;
        StateHasChanged();
    }

    private static IReadOnlyCollection<int> GetPlayerCountOptions() =>
        Enumerable.Range(FactionDraftOptions.MinNumberOfPlayers, (FactionDraftOptions.MaxNumberOfPlayers - FactionDraftOptions.MinNumberOfPlayers) + 1).ToArray();

    private static IReadOnlyCollection<int> GetFactionCountOptions() =>
        Enumerable.Range(FactionDraftOptions.MinNumberOfDraftFactions, (FactionDraftOptions.MumberOfFactions - FactionDraftOptions.MinNumberOfDraftFactions) + 1).ToArray();

    private string GetButtonStateText()
    {
        return _draftStage switch
        {
            DraftStage.Draft when FactionDraftService.ToManyBans => Strings.DraftButton_BanToMany,
            DraftStage.DraftEnded => Strings.DraftButton_Reset,
            DraftStage.DraftInProgress => Strings.DraftButton_DraftInProgress,
            _ => Strings.DraftButton_DraftReady,
        };
    }

    private async Task DraftFactions()
    {
        if (!IsDraftReady())
            return;

        if (_draftStage == DraftStage.DraftEnded)
        {
            ResetDraft();
            return;
        }

        await ExecuteDraft();
    }

    private async Task ExecuteDraft()
    {
        try
        {
            SetDraftInProgress();
            await FactionDraftService.PerformDraft();
            SetDraftEnded();
        }
        catch (HttpRequestException ex)
        {
            Logger.LogError(ex, "API call failed during draft process");
        }
    }

    private void ResetDraft()
    {
        _draftStage = DraftStage.Draft;
        FactionDraftService.ResetPlayerFactions();
    }

    private void ShowDraft() => _showSettings = false;

    private void ShowSettings() => _showSettings = true;

    private bool IsDraftReady()
    {
        return _draftStage != DraftStage.DraftInProgress && !(FactionDraftService.ToManyBans && _draftStage == DraftStage.Draft);
    }

    private bool IsDraftButtonDisabled()
    {
        return _draftStage == DraftStage.DraftInProgress || (FactionDraftService.ToManyBans && _draftStage == DraftStage.Draft);
    }

    private void SetDraftInProgress() => _draftStage = DraftStage.DraftInProgress;

    private void SetDraftEnded() => _draftStage = DraftStage.DraftEnded;

    private void HandleOnDataUpdated(object? sender, EventArgs e)
    {
        StateHasChanged();
    }

}
