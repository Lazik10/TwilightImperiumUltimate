using TwilightImperiumUltimate.Web.Services.Draft;

namespace TwilightImperiumUltimate.Web.Components.Drafts.Color;

public partial class ColorPickerGrid
{
    private FactionIconRow _factionIconRow = null!;

    private DraftStage _draftStage = DraftStage.Draft;

    [Inject]
    private IColorPickerService ColorPickerService { get; set; } = null!;

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

    private void ResetSelectedFactions()
    {
        _factionIconRow.SetAllFactionsBanStatus(true);
        ColorPickerService.ResetSelectedFactions();
        StateHasChanged();
    }

    private void ResetBannedColors()
    {
        ColorPickerService.ResetBannedColors();
    }

    private void HandleColorClick(PlayerColor color)
    {
        ColorPickerService.UpdateColorBanStatus(color);
    }

    private void UpdateSelectedFactions(FactionModel faction)
    {
        ColorPickerService.UpdateSelectedFactions(_factionIconRow.Factions, faction);
    }

    private string GetButtonStateText()
    {
        return _draftStage switch
        {
            DraftStage.Draft when !ColorPickerService.IsDraftPossible() => Strings.ColorPickerButton_NotEnoughColors,
            DraftStage.DraftInProgress => Strings.ColorPcikerButton_Drafting,
            _ => Strings.ColorPickerButton_DraftReady,
        };
    }

    private bool GetButtonState() => _draftStage == DraftStage.Draft && !ColorPickerService.IsDraftPossible();

    private void HandleOnDataUpdated(object? sender, EventArgs e)
    {
        StateHasChanged();
    }
}
