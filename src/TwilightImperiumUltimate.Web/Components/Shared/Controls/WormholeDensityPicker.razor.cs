using System.Globalization;
using TwilightImperiumUltimate.Web.Services.MapGenerators;
using TwilightImperiumUltimate.Web.Services.SliceGenerators;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class WormholeDensityPicker
{
    [Parameter]
    public WormholeDensity WormholeDensity { get; set; }

    [Parameter]
    public EventCallback OnDecrease { get; set; }

    [Parameter]
    public EventCallback OnIncrease { get; set; }

    [Parameter]
    public MarkupString LeftArrowSymbol { get; set; } = (MarkupString)Strings.ButtonLeftArrow;

    [Parameter]
    public MarkupString RightArrowSymbol { get; set; } = (MarkupString)Strings.ButtonRightArrow;

    [Parameter]
    public int Width { get; set; } = 100;

    [Inject]
    private IMapGeneratorSettingsService MapGeneratorSettingsService { get; set; } = default!;

    [Inject]
    private ISliceGeneratorSettingsService SliceGeneratorSettingsService { get; set; } = default!;

    private int WormholeDensityValue => (int)WormholeDensity;

    private async Task Decrease()
    {
        if (Enum.TryParse<WormholeDensity>((WormholeDensityValue - 1).ToString(CultureInfo.InvariantCulture), out var wormholeDensity)
            && Enum.GetValues<WormholeDensity>().Contains(wormholeDensity))
        {
            WormholeDensity = MapGeneratorSettingsService.WormholeDensity = wormholeDensity;
            await SliceGeneratorSettingsService.UpdateWormholeDensity(wormholeDensity);
        }
    }

    private async Task Increase()
    {
        if (Enum.TryParse<WormholeDensity>((WormholeDensityValue + 1).ToString(CultureInfo.InvariantCulture), out var wormholeDensity)
            && Enum.GetValues<WormholeDensity>().Contains(wormholeDensity))
        {
            WormholeDensity = MapGeneratorSettingsService.WormholeDensity = wormholeDensity;
            await SliceGeneratorSettingsService.UpdateWormholeDensity(wormholeDensity);
        }
    }
}