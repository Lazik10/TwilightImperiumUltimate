namespace TwilightImperiumUltimate.Web.Pages.Community;

public partial class AddSliceDraftToArchive
{
    [Parameter]
    [SupplyParameterFromQuery(Name = "tiles")]
    public string TilesImported { get; set; } = string.Empty;
}