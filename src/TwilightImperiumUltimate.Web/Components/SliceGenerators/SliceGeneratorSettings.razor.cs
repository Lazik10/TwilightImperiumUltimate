using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Services.SliceGenerators;

namespace TwilightImperiumUltimate.Web.Components.SliceGenerators;

public partial class SliceGeneratorSettings
{
    private IReadOnlyCollection<KeyValuePair<SystemTileOverlay, string>> _systemTileOverlays = default!;

    [Parameter]
    public EventCallback OnSettingsChange { get; set; } = default!;

    private SystemTileOverlay SelectedSystemTileOverlay { get; set; }

    [Inject]
    private ISliceGeneratorService SliceGeneratorService { get; set; } = default!;

    [Inject]
    private ISliceGeneratorSettingsService SliceGeneratorSettingsService { get; set; } = default!;

    protected override void OnInitialized()
    {
        CreateEnumsWithDisplayNames();
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        SelectedSystemTileOverlay = SliceGeneratorSettingsService.SystemTileOverlay;
    }

    private void CreateEnumsWithDisplayNames()
    {
        _systemTileOverlays = EnumExtensions.GetEnumValuesWithDisplayNames<SystemTileOverlay>();
    }

    private async Task SetSystemTileOverlay(SystemTileOverlay systemTileOverlay)
    {
        SelectedSystemTileOverlay = systemTileOverlay;
        await SliceGeneratorSettingsService.UpdateSystemTileOverlay(systemTileOverlay);
        await OnSettingsChange.InvokeAsync();
    }

    private async Task UpdateGameVersion(GameVersion gameVersion)
    {
        await SliceGeneratorSettingsService.UpdateGameVersion(gameVersion);
    }

    private async Task DecreaseSliceCount()
    {
        await SliceGeneratorSettingsService.DecreaseNumberOfSlices();
        await SliceGeneratorService.RemoveSlice();
        await OnSettingsChange.InvokeAsync();
    }

    private async Task IncreaseSliceCount()
    {
        await SliceGeneratorSettingsService.IncreaseNumberOfSlices();
        await SliceGeneratorService.AddSlice();
        await OnSettingsChange.InvokeAsync();
    }

    private int GetSliceCount()
    {
        return SliceGeneratorSettingsService.NumberOfSlices;
    }

    private void DecreaseNumberOfLegendaryPlanets()
    {
        SliceGeneratorSettingsService.DecreaseNumberOfLegendaries();
        StateHasChanged();
    }

    private void IncreaseNumberOfLegendaryPlanets()
    {
        SliceGeneratorSettingsService.IncreaseNumberOfLegendaries();
        StateHasChanged();
    }
}