using TwilightImperiumUltimate.Web.Services.Draft;
using TwilightImperiumUltimate.Web.Components.Factions;
using TwilightImperiumUltimate.Web.Options.Drafts;

namespace TwilightImperiumUltimate.Web.Components.Drafts.Color;

public partial class ColorPickerGrid
{
    private FactionMenuPicker _factionMenuPicker = null!;

    private DraftStage _draftStage = DraftStage.Draft;

    private IReadOnlyCollection<FactionColorDraftResult> _previewDraftResults = [];

    private IReadOnlyCollection<FactionModel> _previewFactions = [];

    private bool _showSettings;

    [Inject]
    private IColorPickerService ColorPickerService { get; set; } = null!;

    private bool HasDraftResults => ColorPickerService.FactionColorDraftResults is not null && ColorPickerService.FactionColorDraftResults.Any();

    private bool HasPreviewDraftResults => _previewDraftResults.Count > 0;

    private bool HasAvailableColors => ColorPickerService.Colors.Any(color => !color.Value);

    private bool HasEnoughSelectedFactions => ColorPickerService.SelectedFactions.Count >= ColorDraftOptions.MinNumberOfFactions;

    private bool HasTooFewColors => ColorPickerService.Colors.Count(color => !color.Value) < ColorPickerService.SelectedFactions.Count;

    protected override void OnInitialized()
    {
        ColorPickerService.OnFactionUpdate += HandleOnDataUpdated;
    }

    private async Task DraftColors()
    {
        if (HasEnoughSelectedFactions && !ColorPickerService.IsDraftPossible())
            return;

        _draftStage = DraftStage.DraftInProgress;
        if (HasEnoughSelectedFactions)
            await ColorPickerService.PerformDraft();
        else
            await RunPreviewDraftAsync();

        _draftStage = DraftStage.Draft;
        StateHasChanged();
    }

    private async Task HandlePrimaryButtonClick()
    {
        if (HasEnoughSelectedFactions && HasDraftResults)
        {
            ResetDraft();
            return;
        }

        if (!HasEnoughSelectedFactions && HasPreviewDraftResults)
        {
            _previewDraftResults = [];
            return;
        }

        await DraftColors();
    }

    private void ResetDraft()
    {
        ColorPickerService.ResetDraft();
        _factionMenuPicker.SetAllFactionsBanStatus(true);
        StateHasChanged();
    }

    private void InitializePreviewFactions(IReadOnlyCollection<FactionModel> factions)
    {
        _previewFactions = factions
            .OrderBy(_ => Random.Shared.Next())
            .Take(6)
            .ToArray();
    }

    private async Task RunPreviewDraftAsync()
    {
        for (var index = 0; index < FactionDraftOptions.DefaultNumberOfAssignments; index++)
        {
            _previewDraftResults = GeneratePreviewDraftResults();
            await InvokeAsync(StateHasChanged);
            await Task.Delay(FactionDraftOptions.DefaultDelayInMilliseconds);
        }

        var draftResults = await ColorPickerService.GetDraftResultsAsync(_previewFactions);
        if (draftResults.Count > 0)
            _previewDraftResults = draftResults;
    }

    private IReadOnlyCollection<FactionColorDraftResult> GeneratePreviewDraftResults()
    {
        var availableColors = ColorPickerService.Colors
            .Where(color => !color.Value)
            .Select(color => color.Key)
            .OrderBy(_ => Random.Shared.Next())
            .ToArray();

        if (availableColors.Length == 0)
            return [];

        return _previewFactions
            .Select((faction, index) => new FactionColorDraftResult
            {
                FactionName = faction.FactionName,
                Color = availableColors[index % availableColors.Length],
            })
            .ToArray();
    }

    private void ShowDraft() => _showSettings = false;

    private void ShowSettings() => _showSettings = true;

    private void HandleColorClick(PlayerColor color)
    {
        ColorPickerService.UpdateColorBanStatus(color);
    }

    private void UpdateSelectedFactions(FactionModel faction)
    {
        ColorPickerService.UpdateSelectedFactions(_factionMenuPicker.Factions, faction);
        _previewDraftResults = [];
    }

    private string GetPrimaryButtonText()
    {
        return (HasEnoughSelectedFactions && HasDraftResults) || (!HasEnoughSelectedFactions && HasPreviewDraftResults)
            ? Strings.ColorPickerButton_ResetDraft
            : GetButtonStateText();
    }

    private string GetButtonStateText()
    {
        return _draftStage switch
        {
            DraftStage.Draft when HasTooFewColors => Strings.ColorPickerButton_NotEnoughColors,
            DraftStage.DraftInProgress => Strings.ColorPcikerButton_Drafting,
            _ => Strings.ColorPickerButton_DraftReady,
        };
    }

    private bool GetButtonState()
    {
        if (_draftStage != DraftStage.Draft)
            return true;

        return HasEnoughSelectedFactions
            ? !HasDraftResults && !ColorPickerService.IsDraftPossible()
            : !HasPreviewDraftResults && (!HasAvailableColors || _previewFactions.Count == 0);
    }

    private void HandleOnDataUpdated(object? sender, EventArgs e)
    {
        StateHasChanged();
    }
}
