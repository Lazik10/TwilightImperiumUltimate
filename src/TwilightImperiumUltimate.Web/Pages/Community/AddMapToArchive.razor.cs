namespace TwilightImperiumUltimate.Web.Pages.Community;

public partial class AddMapToArchive
{
    [Parameter]
    [SupplyParameterFromQuery(Name = "template")]
    public string MapTemplateImported { get; set; } = string.Empty;

    [Parameter]
    [SupplyParameterFromQuery(Name = "tiles")]
    public string TilesImported { get; set; } = string.Empty;
}