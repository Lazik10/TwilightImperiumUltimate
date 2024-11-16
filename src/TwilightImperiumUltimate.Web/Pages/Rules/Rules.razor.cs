namespace TwilightImperiumUltimate.Web.Pages.Rules;

public partial class Rules
{
    [Parameter]
    [SupplyParameterFromQuery(Name = "search")]
    public string SearchWord { get; set; } = string.Empty;
}
