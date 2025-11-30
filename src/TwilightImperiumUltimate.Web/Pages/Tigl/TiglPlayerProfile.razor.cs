namespace TwilightImperiumUltimate.Web.Pages.Tigl;

public partial class TiglPlayerProfile
{
    [Parameter]
    [SupplyParameterFromQuery(Name = "playerId")]
    public int PlayerId { get; set; }

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override void OnInitialized()
    {
    }
}