using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapGeneratorSettings
{
    private readonly IReadOnlyCollection<KeyValuePair<bool, string>> _booleanOptions =
    [
        new(false, Strings.String_Disabled),
        new(true, Strings.String_Enabled),
    ];

    private IReadOnlyCollection<KeyValuePair<MapTemplate, string>> _mapTemplates = default!;

    private IReadOnlyCollection<KeyValuePair<SystemWeight, string>> _systemWeights = default!;

    private IReadOnlyCollection<KeyValuePair<PlacementStyle, string>> _placementStyles = default!;

    private IReadOnlyCollection<KeyValuePair<SystemTileOverlay, string>> _systemTileOverlays = default!;

    private IReadOnlyCollection<KeyValuePair<WormholeDensity, string>> _wormholeDensities = default!;

    [Parameter]
    public EventCallback<MapTemplate> OnSelectedTemplateChange { get; set; } = default!;

    [Parameter]
    public EventCallback OnSelectedOverlayChange { get; set; } = default!;

    [Parameter]
    public EventCallback OnHideSettings { get; set; } = default!;

    /// <summary>
    /// Gets or sets the callback invoked when faction selection is enabled or disabled.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnFactionPickChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when a faction is enabled or disabled.
    /// </summary>
    [Parameter]
    public EventCallback<FactionModel> OnFactionChanged { get; set; }

    private MapTemplate SelectedMapTemplate { get; set; }

    private SystemWeight SelectedSystemWeight { get; set; }

    private PlacementStyle SelectedPlacementStyle { get; set; }

    private SystemTileOverlay SelectedMapOverlay { get; set; }

    private SystemTileOverlay SelectedMenuOverlay { get; set; }

    [Inject]
    private IMapGeneratorSettingsService MapGeneratorSettingsService { get; set; } = default!;

    private IReadOnlyCollection<KeyValuePair<bool, string>> BooleanOptions => _booleanOptions;

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
        _wormholeDensities = EnumExtensions.GetEnumValuesWithDisplayNames<WormholeDensity>();
    }

    private void InitializeSettings()
    {
        SelectedMapTemplate = MapGeneratorSettingsService.MapTemplate;
        SelectedPlacementStyle = MapGeneratorSettingsService.PlacementStyle;
        SelectedSystemWeight = MapGeneratorSettingsService.SystemWeight;
        SelectedMapOverlay = MapGeneratorSettingsService.MapOverlay;
        SelectedMenuOverlay = MapGeneratorSettingsService.MenuOverlay;
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

    private void SetMapOverlay(SystemTileOverlay systemTileOverlay)
    {
        SelectedMapOverlay = systemTileOverlay;
        MapGeneratorSettingsService.MapOverlay = systemTileOverlay;
        OnSelectedOverlayChange.InvokeAsync();
    }

    private void SetMenuOverlay(SystemTileOverlay systemTileOverlay)
    {
        SelectedMenuOverlay = systemTileOverlay;
        MapGeneratorSettingsService.MenuOverlay = systemTileOverlay;
        OnSelectedOverlayChange.InvokeAsync();
    }

    private void SetGameVersion(GameVersion gameVersion, bool isEnabled)
    {
        if (MapGeneratorSettingsService.GameVersions.Contains(gameVersion) != isEnabled)
            MapGeneratorSettingsService.UpdateGameVersion(gameVersion);

        StateHasChanged();
    }

    private void SetNumberOfLegendaryPlanets(int numberOfLegendaryPlanets)
    {
        MapGeneratorSettingsService.NumberOfLegendaryPlanets = numberOfLegendaryPlanets;
        StateHasChanged();
    }

    private void SetLegendaryPriorityInEquidistant(bool legendaryPriorityInEquidistant)
    {
        MapGeneratorSettingsService.LegendaryPriorityInEquidistant = legendaryPriorityInEquidistant;
        StateHasChanged();
    }

    private void SetPlayerNamesEnabled(bool enablePlayerNames)
    {
        MapGeneratorSettingsService.EnablePlayerNames = enablePlayerNames;
        StateHasChanged();
    }

    private void SetWormholeDensity(WormholeDensity wormholeDensity)
    {
        MapGeneratorSettingsService.UpdateWormholeDensity(wormholeDensity);
    }

    private IReadOnlyCollection<int> GetLegendaryPlanetOptions() =>
        Enumerable.Range(0, GetMaximumLegendaryPlanetCount() + 1).ToArray();

    private int GetMaximumLegendaryPlanetCount()
    {
        var gameVersions = MapGeneratorSettingsService.GameVersions;
        return (gameVersions.Contains(GameVersion.ProphecyOfKings) ? 2 : 0) +
               (gameVersions.Contains(GameVersion.UnchartedSpace) ? 5 : 0) +
               (gameVersions.Contains(GameVersion.AscendantSun) ? 12 : 0);
    }
}
