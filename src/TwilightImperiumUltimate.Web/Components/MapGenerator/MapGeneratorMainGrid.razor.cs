using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapGeneratorMainGrid
{
    private bool _showSettings;

    [Inject]
    private IMapGeneratorService MapGeneratorService { get; set; } = default!;

    [Inject]
    private IMapGeneratorSettingsService MapGeneratorSettingsService { get; set; } = default!;

    private void ToggleSettings()
    {
        _showSettings = !_showSettings;
    }

    private void UpdateMapTemplate(MapTemplate mapTemplate)
    {
        StateHasChanged();
    }

    private void HideSettings()
    {
        _showSettings = false;
    }

    private async Task GenerateMap()
    {
        await MapGeneratorService.GenerateMapAsync();
        StateHasChanged();
    }
}
