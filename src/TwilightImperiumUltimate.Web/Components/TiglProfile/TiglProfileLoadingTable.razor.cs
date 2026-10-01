namespace TwilightImperiumUltimate.Web.Components.TiglProfile;

public partial class TiglProfileLoadingTable
{
    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public int ColumnCount { get; set; } = 1;
}