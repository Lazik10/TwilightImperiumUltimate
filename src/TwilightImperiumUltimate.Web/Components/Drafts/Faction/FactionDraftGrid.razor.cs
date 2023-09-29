using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Components.Factions;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Models.Factions;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.Draft;

namespace TwilightImperiumUltimate.Web.Components.Drafts.Faction;

public partial class FactionDraftGrid
{
    private DraftStage _draftStage = DraftStage.Draft;

    private FactionIconRow? _childComponentRef;

    [Inject]
    private ILogger<FactionDraftGrid> Logger { get; set; } = null!;

    [Inject]
    private IFactionDraftService FactionDraftService { get; set; } = null!;

    protected override void OnInitialized()
    {
        FactionDraftService.InitializePlayers();
        FactionDraftService.ResetBans();
        FactionDraftService.OnFactionUpdate += HandleOnDataUpdated;
    }

    private void UpdateBanFactions(List<FactionModel> factionsWithBanStatus)
    {
        FactionDraftService.UpdateBanFactions(factionsWithBanStatus);
    }

    private void IncreasePlayerCount()
    {
        FactionDraftService.IncreasePlayerCount();
    }

    private void DecreasePlayerCount()
    {
        FactionDraftService.DecreasePlayerCount();
    }

    private void IncreaseFactionCount()
    {
        FactionDraftService.IncreaseFactionCount();
    }

    private void DecreaseFactionCount()
    {
        FactionDraftService.DecreaseFactionCount();
    }

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

    private void ResetBans()
    {
        _childComponentRef?.ResetBans();
        FactionDraftService.ResetBans();
    }

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
