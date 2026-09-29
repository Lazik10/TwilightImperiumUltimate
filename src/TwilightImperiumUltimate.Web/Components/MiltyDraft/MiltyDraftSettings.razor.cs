using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Options.MiltyDraft;
using TwilightImperiumUltimate.Web.Services.MiltyDraft;

namespace TwilightImperiumUltimate.Web.Components.MiltyDraft;

public partial class MiltyDraftSettings
{
    private readonly IReadOnlyCollection<KeyValuePair<bool, string>> _booleanOptions =
    [
        new(false, Strings.String_Disabled),
        new(true, Strings.String_Enabled),
    ];

    private IReadOnlyCollection<KeyValuePair<WormholeDensity, string>> _wormholeDensities = default!;

    [Inject]
    private IMiltyDraftSettingsService MiltyDraftSettingsService { get; set; } = default!;

    [Inject]
    private IMiltyDraftService MiltyDraftService { get; set; } = default!;

    private IReadOnlyCollection<KeyValuePair<bool, string>> BooleanOptions => _booleanOptions;

    protected override void OnInitialized() => _wormholeDensities = EnumExtensions.GetEnumValuesWithDisplayNames<WormholeDensity>();

    private async Task SetGameVersionAsync(GameVersion gameVersion, bool isEnabled)
    {
        if (MiltyDraftSettingsService.GameVersions.Contains(gameVersion) != isEnabled)
            await MiltyDraftSettingsService.UpdateGameVersion(gameVersion);

        StateHasChanged();
    }

    private async Task SetLegendaryPlanetCountAsync(int numberOfLegendaryPlanets)
    {
        while (MiltyDraftSettingsService.NumberOfLegendaryPlanets > numberOfLegendaryPlanets)
            await MiltyDraftSettingsService.DecreaseNumberOfLegendaryPlanets();

        while (MiltyDraftSettingsService.NumberOfLegendaryPlanets < numberOfLegendaryPlanets)
        {
            var previousCount = MiltyDraftSettingsService.NumberOfLegendaryPlanets;
            await MiltyDraftSettingsService.IncreaseNumberOfLegendaryPlanets();
            if (MiltyDraftSettingsService.NumberOfLegendaryPlanets == previousCount)
                break;
        }

        StateHasChanged();
    }

    private async Task SetFactionCountAsync(int numberOfFactions)
    {
        while (MiltyDraftSettingsService.NumberOfFactions > numberOfFactions)
            await MiltyDraftSettingsService.DecreaseNumberOfFactions();

        while (MiltyDraftSettingsService.NumberOfFactions < numberOfFactions)
        {
            var previousCount = MiltyDraftSettingsService.NumberOfFactions;
            await MiltyDraftSettingsService.IncreaseNumberOfFactions();
            if (MiltyDraftSettingsService.NumberOfFactions == previousCount)
                break;
        }

        StateHasChanged();
    }

    private async Task SetPlayerCountAsync(int numberOfPlayers)
    {
        while (MiltyDraftSettingsService.NumberOfPlayers > numberOfPlayers)
            await MiltyDraftSettingsService.DecreaseNumberOfPlayers();

        while (MiltyDraftSettingsService.NumberOfPlayers < numberOfPlayers)
            await MiltyDraftSettingsService.IncreaseNumberOfPlayers();

        StateHasChanged();
    }

    private async Task SetSliceCountAsync(int numberOfSlices)
    {
        while (MiltyDraftSettingsService.NumberOfSlices > numberOfSlices)
            await MiltyDraftSettingsService.DecreaseNumberOfSlices();

        while (MiltyDraftSettingsService.NumberOfSlices < numberOfSlices)
            await MiltyDraftSettingsService.IncreaseNumberOfSlices();

        StateHasChanged();
    }

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

    private Task SetWormholeDensityAsync(WormholeDensity wormholeDensity) => MiltyDraftSettingsService.UpdateWormholeDensity(wormholeDensity);

    private static IReadOnlyCollection<int> GetPlayerCountOptions() =>
        Enumerable.Range(MiltyDraftOptions.MinNumberOfPlayers, (MiltyDraftOptions.MaxNumberOfPlayers - MiltyDraftOptions.MinNumberOfPlayers) + 1).ToArray();

    private static IReadOnlyCollection<int> GetSliceCountOptions() =>
        Enumerable.Range(MiltyDraftOptions.MinNumberOfSlices, (MiltyDraftOptions.MaxNumberOfSlices - MiltyDraftOptions.MinNumberOfSlices) + 1).ToArray();

    private static IReadOnlyCollection<int> GetFactionCountOptions() => Enumerable.Range(MiltyDraftOptions.MinNumberOfPlayers, 60).ToArray();

    private IReadOnlyCollection<int> GetLegendaryPlanetOptions() =>
        Enumerable.Range(0, GetMaximumLegendaryPlanetCount() + 1).ToArray();

    private int GetMaximumLegendaryPlanetCount()
    {
        var gameVersions = MiltyDraftSettingsService.GameVersions;
        return (gameVersions.Contains(GameVersion.ProphecyOfKings) ? 2 : 0) +
               (gameVersions.Contains(GameVersion.UnchartedSpace) ? 5 : 0) +
               (gameVersions.Contains(GameVersion.AscendantSun) ? 12 : 0);
    }
}
