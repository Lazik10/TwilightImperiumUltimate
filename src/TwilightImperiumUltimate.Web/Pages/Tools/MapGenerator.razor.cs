namespace TwilightImperiumUltimate.Web.Pages.Tools;

public partial class MapGenerator
{
    [Parameter]
    [SupplyParameterFromQuery(Name = "template")]
    public string MapTemplateImported { get; set; } = string.Empty;

    [Parameter]
    [SupplyParameterFromQuery(Name = "tiles")]
    public string TilesImported { get; set; } = string.Empty;
}