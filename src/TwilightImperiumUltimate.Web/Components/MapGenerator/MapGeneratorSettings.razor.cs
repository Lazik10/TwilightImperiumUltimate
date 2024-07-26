using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Enums;
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

    private void IncreaseMapScale()
    {
        MapGeneratorSettingsService.IncreaseMapScale();
        OnSelectedTemplateChange.InvokeAsync(SelectedMapTemplate);
    }

    private void DecreaseMapScale()
    {
        MapGeneratorSettingsService.DecreaseMapScale();
        OnSelectedTemplateChange.InvokeAsync(SelectedMapTemplate);
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

    private void HideSettings()
    {
        OnHideSettings.InvokeAsync(true);
        StateHasChanged();
    }
}
