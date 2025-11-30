namespace TwilightImperiumUltimate.Web.Pages.Community;

public partial class Rankings
{
    [Parameter]
    [SupplyParameterFromQuery(Name = "category")]
    public string Category { get; set; } = string.Empty;
}