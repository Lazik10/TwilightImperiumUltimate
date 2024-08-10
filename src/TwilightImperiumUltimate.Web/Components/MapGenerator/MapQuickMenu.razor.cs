using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapQuickMenu
{
    private bool _showImportMapString;

    private MapTemplate _selectedMapTemplate = MapTemplate.SixPlayersMediumMap;

    private string _importedTtsString = string.Empty;

    private IReadOnlyCollection<KeyValuePair<MapTemplate, string>> _mapTemplates = default!;

    [Parameter]
    public EventCallback<IconType> OnMenuIconClick { get; set; }

    [Inject]
    private IMapToStringConverter MapToStringConverter { get; set; } = default!;

    [Inject]
    private IMapGeneratorService MapGeneratorService { get; set; } = default!;

    [Inject]
    private IMapGeneratorSettingsService MapGeneratorSettingsService { get; set; } = default!;

    protected override void OnInitialized()
    {
        CreateEnumsWithDisplayNames();
    }

    private void CreateEnumsWithDisplayNames()
    {
        _mapTemplates = EnumExtensions.GetEnumValuesWithDisplayNames<MapTemplate>();
    }

    private async Task HandleMenuIconClick(IconType iconType)
    {
        if (iconType == IconType.ImportMapString)
        {
            _showImportMapString = !_showImportMapString;
            return;
        }

        await OnMenuIconClick.InvokeAsync(iconType);
    }

    private async Task LoadMapFromTtsString()
    {
        await MapToStringConverter.ConvertTtsStringToMap(_selectedMapTemplate, _importedTtsString);
        await OnMenuIconClick.InvokeAsync(IconType.ImportMapString);
    }

    private async Task HandleMapTemplateChange()
    {
        MapGeneratorSettingsService.MapTemplate = _selectedMapTemplate;
        await MapGeneratorService.GenerateMapAsync(true, default);
    }
}