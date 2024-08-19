using System.Globalization;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

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

    private int WormholeDensityValue => (int)WormholeDensity;

    private void Decrease()
    {
        if (Enum.TryParse<WormholeDensity>((WormholeDensityValue - 1).ToString(CultureInfo.InvariantCulture), out var wormholeDensity)
            && Enum.GetValues<WormholeDensity>().Contains(wormholeDensity))
        {
            WormholeDensity = MapGeneratorSettingsService.WormholeDensity = wormholeDensity;
        }
    }

    private void Increase()
    {
        if (Enum.TryParse<WormholeDensity>((WormholeDensityValue + 1).ToString(CultureInfo.InvariantCulture), out var wormholeDensity)
            && Enum.GetValues<WormholeDensity>().Contains(wormholeDensity))
        {
            WormholeDensity = MapGeneratorSettingsService.WormholeDensity = wormholeDensity;
        }
    }
}