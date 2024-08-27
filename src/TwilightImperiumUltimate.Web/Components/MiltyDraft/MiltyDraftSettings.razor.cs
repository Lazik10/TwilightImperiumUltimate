using TwilightImperiumUltimate.Web.Services.MiltyDraft;

namespace TwilightImperiumUltimate.Web.Components.MiltyDraft;

public partial class MiltyDraftSettings
{
    [Inject]
    private IMiltyDraftSettingsService MiltyDraftSettingsService { get; set; } = default!;

    [Inject]
    private IMiltyDraftService MiltyDraftService { get; set; } = default!;

    private async Task UpdateGameVersion(GameVersion gameVersion) => await MiltyDraftSettingsService.UpdateGameVersion(gameVersion);

    private async Task DecreaseNumberOfLegendaryPlanets() => await MiltyDraftSettingsService.DecreaseNumberOfLegendaryPlanets();

    private async Task IncreaseNumberOfLegendaryPlanets() => await MiltyDraftSettingsService.IncreaseNumberOfLegendaryPlanets();

    private async Task DecreaseNumberOfFactions() => await MiltyDraftSettingsService.DecreaseNumberOfFactions();

    private async Task IncreaseNumberOfFactions() => await MiltyDraftSettingsService.IncreaseNumberOfFactions();

    private async Task DecreaseNumberOfSlices() => await MiltyDraftSettingsService.DecreaseNumberOfSlices();

    private async Task IncreaseNumberOfSlices() => await MiltyDraftSettingsService.IncreaseNumberOfSlices();

    private async Task DecreaseNumberOfPlayers() => await MiltyDraftSettingsService.DecreaseNumberOfPlayers();

    private async Task IncreaseNumberOfPlayers() => await MiltyDraftSettingsService.IncreaseNumberOfPlayers();

    private async Task EnablePlayerNames(bool option)
    {
        await MiltyDraftSettingsService.SetPlayerNamesOption(option);
        StateHasChanged();
    }

    private Task EnableImportSlices(bool option)
    {
        MiltyDraftSettingsService.ImportSlices = option;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private Task HandleFactionClick(FactionModel faction)
    {
        MiltyDraftService.SetFactionBanStatus(faction);
        return Task.CompletedTask;
    }

    private Task HandleGameVersionClick(GameVersion gameVersion)
    {
        MiltyDraftService.UpdateGameVersionFactions(gameVersion);
        return Task.CompletedTask;
    }
}