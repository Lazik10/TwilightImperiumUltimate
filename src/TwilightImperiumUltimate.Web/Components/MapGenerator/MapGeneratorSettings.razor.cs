using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapGeneratorSettings
{
    private IReadOnlyCollection<KeyValuePair<MapTemplate, string>> _mapTemplates = default!;

    private IReadOnlyCollection<KeyValuePair<SystemWeight, string>> _systemWeights = default!;

    private IReadOnlyCollection<KeyValuePair<PlacementStyle, string>> _placementStyles = default!;

    private IReadOnlyCollection<KeyValuePair<SystemTileOverlay, string>> _systemTileOverlays = default!;

    [Parameter]
    public EventCallback<MapTemplate> OnSelectedTemplateChange { get; set; } = default!;

    [Parameter]
    public EventCallback OnSelectedSystemTileOverlayChange { get; set; } = default!;

    [Parameter]
    public EventCallback OnHideSettings { get; set; } = default!;

    private MapTemplate SelectedMapTemplate { get; set; }

    private SystemWeight SelectedSystemWeight { get; set; }

    private PlacementStyle SelectedPlacementStyle { get; set; }

    private SystemTileOverlay SelectedSystemTileOverlay { get; set; }

    [Inject]
    private IMapGeneratorSettingsService MapGeneratorSettingsService { get; set; } = default!;

    protected override void OnInitialized()
    {
        CreateEnumsWithDisplayNames();
        InitializeSettings();
    }

    private void CreateEnumsWithDisplayNames()
    {
        _mapTemplates = EnumExtensions.GetEnumValuesWithDisplayNames<MapTemplate>();
        _systemWeights = EnumExtensions.GetEnumValuesWithDisplayNames<SystemWeight>();
        _placementStyles = EnumExtensions.GetEnumValuesWithDisplayNames<PlacementStyle>();
        _systemTileOverlays = EnumExtensions.GetEnumValuesWithDisplayNames<SystemTileOverlay>();
    }

    private void InitializeSettings()
    {
        SelectedMapTemplate = MapGeneratorSettingsService.MapTemplate;
        SelectedPlacementStyle = MapGeneratorSettingsService.PlacementStyle;
        SelectedSystemWeight = MapGeneratorSettingsService.SystemWeight;
        SelectedSystemTileOverlay = MapGeneratorSettingsService.SystemTileOverlay;
    }

    private void SetMapTemplate(MapTemplate mapTemplate)
    {
        SelectedMapTemplate = mapTemplate;
        MapGeneratorSettingsService.MapTemplate = mapTemplate;
        OnSelectedTemplateChange.InvokeAsync(mapTemplate);
    }

    private void SetPlacementStyle(PlacementStyle placementStyle)
    {
        SelectedPlacementStyle = placementStyle;
        MapGeneratorSettingsService.PlacementStyle = placementStyle;
    }

    private void SetSystemWeight(SystemWeight systemWeight)
    {
        SelectedSystemWeight = systemWeight;
        MapGeneratorSettingsService.SystemWeight = systemWeight;
    }

    private void SetSystemTileOverlay(SystemTileOverlay systemTileOverlay)
    {
        SelectedSystemTileOverlay = systemTileOverlay;
        MapGeneratorSettingsService.SystemTileOverlay = systemTileOverlay;
        OnSelectedSystemTileOverlayChange.InvokeAsync();
    }

    private void UpdateGameVersion(GameVersion gameVersion)
    {
        MapGeneratorSettingsService.UpdateGameVersion(gameVersion);
    }

    private void IncreaseNumberOfLegendaryPlanets()
    {
        int maxNumberOfLegendaryPlanets = 0;
        maxNumberOfLegendaryPlanets += MapGeneratorSettingsService.GameVersions.Contains(GameVersion.ProphecyOfKings) ? 2 : 0;
        maxNumberOfLegendaryPlanets += MapGeneratorSettingsService.GameVersions.Contains(GameVersion.UnchartedSpace) ? 5 : 0;
        maxNumberOfLegendaryPlanets += MapGeneratorSettingsService.GameVersions.Contains(GameVersion.AscendantSun) ? 12 : 0;

        if (MapGeneratorSettingsService.NumberOfLegendaryPlanets < maxNumberOfLegendaryPlanets)
        {
            MapGeneratorSettingsService.NumberOfLegendaryPlanets++;
            StateHasChanged();
        }
    }

    private void DecreaseNumberOfLegendaryPlanets()
    {
        if (MapGeneratorSettingsService.NumberOfLegendaryPlanets > 0)
        {
            MapGeneratorSettingsService.NumberOfLegendaryPlanets--;
            StateHasChanged();
        }
    }

    private void ToggleLegendaryPriorityInEquidistant()
    {
        MapGeneratorSettingsService.LegendaryPriorityInEquidistant = !MapGeneratorSettingsService.LegendaryPriorityInEquidistant;
        StateHasChanged();
    }

    private void TogglePlayerNames()
    {
        MapGeneratorSettingsService.EnablePlayerNames = !MapGeneratorSettingsService.EnablePlayerNames;
        StateHasChanged();
    }
}
