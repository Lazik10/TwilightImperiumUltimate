using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Services.SliceGenerators;

namespace TwilightImperiumUltimate.Web.Components.SliceGenerators;

public partial class SliceGeneratorSettings
{
    private readonly IReadOnlyCollection<KeyValuePair<bool, string>> _booleanOptions =
    [
        new(false, Strings.String_Disabled),
        new(true, Strings.String_Enabled),
    ];

    private IReadOnlyCollection<KeyValuePair<SystemTileOverlay, string>> _systemTileOverlays = default!;

    private IReadOnlyCollection<KeyValuePair<WormholeDensity, string>> _wormholeDensities = default!;

    [Parameter]
    public EventCallback OnSettingsChange { get; set; } = default!;

    private SystemTileOverlay SelectedMenuOverlay { get; set; }

    private SystemTileOverlay SelectedSliceOverlay { get; set; }

    [Inject]
    private ISliceGeneratorService SliceGeneratorService { get; set; } = default!;

    [Inject]
    private ISliceGeneratorSettingsService SliceGeneratorSettingsService { get; set; } = default!;

    private IReadOnlyCollection<KeyValuePair<bool, string>> BooleanOptions => _booleanOptions;

    protected override void OnInitialized()
    {
        CreateEnumsWithDisplayNames();
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        SelectedMenuOverlay = SliceGeneratorSettingsService.MenuOverlay;
        SelectedSliceOverlay = SliceGeneratorSettingsService.SliceOverlay;
    }

    private void CreateEnumsWithDisplayNames()
    {
        _systemTileOverlays = EnumExtensions.GetEnumValuesWithDisplayNames<SystemTileOverlay>();
        _wormholeDensities = EnumExtensions.GetEnumValuesWithDisplayNames<WormholeDensity>();
    }

    private async Task SetSliceOverlay(SystemTileOverlay systemTileOverlay)
    {
        SelectedSliceOverlay = systemTileOverlay;
        await SliceGeneratorSettingsService.UpdateSliceOverlay(systemTileOverlay);
        await OnSettingsChange.InvokeAsync();
    }

    private async Task SetMenuOverlay(SystemTileOverlay systemTileOverlay)
    {
        SelectedMenuOverlay = systemTileOverlay;
        await SliceGeneratorSettingsService.UpdateMenuOverlay(systemTileOverlay);
        await OnSettingsChange.InvokeAsync();
    }

    private async Task SetGameVersionAsync(GameVersion gameVersion, bool isEnabled)
    {
        if (SliceGeneratorSettingsService.GameVersions.Contains(gameVersion) != isEnabled)
            await SliceGeneratorSettingsService.UpdateGameVersion(gameVersion);

        await OnSettingsChange.InvokeAsync();
        StateHasChanged();
    }

    private async Task SetSliceCountAsync(int numberOfSlices)
    {
        while (SliceGeneratorSettingsService.NumberOfSlices > numberOfSlices)
        {
            var previousCount = SliceGeneratorSettingsService.NumberOfSlices;
            await SliceGeneratorSettingsService.DecreaseNumberOfSlices();
            if (SliceGeneratorSettingsService.NumberOfSlices == previousCount)
                break;

            await SliceGeneratorService.RemoveSlice();
        }

        while (SliceGeneratorSettingsService.NumberOfSlices < numberOfSlices)
        {
            var previousCount = SliceGeneratorSettingsService.NumberOfSlices;
            await SliceGeneratorSettingsService.IncreaseNumberOfSlices();
            if (SliceGeneratorSettingsService.NumberOfSlices == previousCount)
                break;

            await SliceGeneratorService.AddSlice();
        }

        await OnSettingsChange.InvokeAsync();
        StateHasChanged();
    }

    private async Task SetNumberOfLegendaryPlanetsAsync(int numberOfLegendaries)
    {
        while (SliceGeneratorSettingsService.NumberOfLegendaries > numberOfLegendaries)
            await SliceGeneratorSettingsService.DecreaseNumberOfLegendaries();

        while (SliceGeneratorSettingsService.NumberOfLegendaries < numberOfLegendaries)
        {
            var previousCount = SliceGeneratorSettingsService.NumberOfLegendaries;
            await SliceGeneratorSettingsService.IncreaseNumberOfLegendaries();
            if (SliceGeneratorSettingsService.NumberOfLegendaries == previousCount)
                break;
        }

        StateHasChanged();
    }

    private async Task SetWormholeDensityAsync(WormholeDensity wormholeDensity)
    {
        await SliceGeneratorSettingsService.UpdateWormholeDensity(wormholeDensity);
        await OnSettingsChange.InvokeAsync();
    }

    private static IReadOnlyCollection<int> GetSliceCountOptions() => Enumerable.Range(0, 10).ToArray();

    private IReadOnlyCollection<int> GetLegendaryPlanetOptions() =>
        Enumerable.Range(0, GetMaximumLegendaryPlanetCount() + 1).ToArray();

    private int GetMaximumLegendaryPlanetCount()
    {
        var gameVersions = SliceGeneratorSettingsService.GameVersions;
        return (gameVersions.Contains(GameVersion.ProphecyOfKings) ? 2 : 0) +
               (gameVersions.Contains(GameVersion.UnchartedSpace) ? 5 : 0) +
               (gameVersions.Contains(GameVersion.AscendantSun) ? 12 : 0);
    }
}
