using TwilightImperiumUltimate.Web.Services.Draft;

namespace TwilightImperiumUltimate.Web.Components.Drafts.Color;

public partial class ColorPickerGrid
{
    private ColorPickerMenu _colorPickerMenu = null!;

    private DraftStage _draftStage = DraftStage.Draft;

    [Inject]
    private IColorPickerService ColorPickerService { get; set; } = null!;

    private bool HasDraftResults => ColorPickerService.FactionColorDraftResults is not null && ColorPickerService.FactionColorDraftResults.Any();

    private bool HasTooFewColors => ColorPickerService.Colors.Count(color => !color.Value) < ColorPickerService.SelectedFactions.Count;

    protected override void OnInitialized()
    {
        ColorPickerService.OnFactionUpdate += HandleOnDataUpdated;
    }

    private async Task DraftColors()
    {
        if (!ColorPickerService.IsDraftPossible())
            return;

        _draftStage = DraftStage.DraftInProgress;
        await ColorPickerService.PerformDraft();
        StateHasChanged();
        _draftStage = DraftStage.Draft;
    }

    private async Task HandlePrimaryButtonClick()
    {
        if (HasDraftResults)
        {
            ResetDraft();
            return;
        }

        await DraftColors();
    }

    private void ResetDraft()
    {
        ColorPickerService.ResetDraft();
        _colorPickerMenu.SetAllFactionsBanStatus(true);
        StateHasChanged();
    }

    private void HandleColorClick(PlayerColor color)
    {
        ColorPickerService.UpdateColorBanStatus(color);
    }

    private void UpdateSelectedFactions(FactionModel faction)
    {
        ColorPickerService.UpdateSelectedFactions(_colorPickerMenu.Factions, faction);
    }

    private string GetPrimaryButtonText()
    {
        return HasDraftResults ? Strings.ColorPickerButton_ResetDraft : GetButtonStateText();
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

    private bool GetButtonState() => !HasDraftResults && _draftStage == DraftStage.Draft && !ColorPickerService.IsDraftPossible();

    private void HandleOnDataUpdated(object? sender, EventArgs e)
    {
        StateHasChanged();
    }
}
