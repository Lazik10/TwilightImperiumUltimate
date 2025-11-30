using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglStatisticsFactionsGrid
{
    private bool _loading;
    private List<FactionSeasonStatsDto> _rows = new();

    [CascadingParameter(Name = "SelectedSeasonNumber")]
    public int Season { get; set; }

    [CascadingParameter(Name = "SelectedLeague")]
    public TiglLeague League { get; set; }

    [Inject] private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        _loading = true;
        StateHasChanged();

        var (resp, status) = await HttpClient.GetAsync<ApiResponse<ItemListDto<FactionSeasonStatsDto>>>(
            $"api/tigl/faction-stats/{Season}/{League}");

        if (status == HttpStatusCode.OK && resp?.Data?.Items is not null)
        {
            _rows = resp.Data.Items.ToList();
        }
        else
        {
            _rows = new List<FactionSeasonStatsDto>();
        }

        _loading = false;
        StateHasChanged();
    }

    public static TextColor GetWinrateColor(double winrate)
    {
        if (winrate > 16.67f) return TextColor.Green;
        if (winrate > 12.0f) return TextColor.Yellow;
        if (winrate > 8.0f) return TextColor.Orange;
        return TextColor.Red;
    }
}