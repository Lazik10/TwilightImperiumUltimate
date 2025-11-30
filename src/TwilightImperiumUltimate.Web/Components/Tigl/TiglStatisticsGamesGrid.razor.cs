namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglStatisticsGamesGrid
{
    private bool _loading;

    [CascadingParameter(Name = "SelectedSeasonNumber")]
    public int Season { get; set; }

    [CascadingParameter(Name = "SelectedLeague")]
    public TiglLeague League { get; set; }

    [Inject] private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
    }
}