using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Web.Services.Rankings;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class RankingsMainGrid
{
    private IReadOnlyCollection<RankingsUserDto>? _rankings;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IRankingsDataCache Cache { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        if (!Cache.IsRankingsExpired && Cache.Rankings is not null)
        {
            _rankings = Cache.Rankings;
            return;
        }

        var (ranksResponse, ranksStatus) = await HttpClient.GetAsync<ApiResponse<ItemListDto<RankingsUserDto>>>(Paths.ApiPath_Rankings, cancellationToken: default);
        var rankings = ranksStatus == HttpStatusCode.OK && ranksResponse?.Data is not null
            ? ranksResponse.Data.Items
            : new List<RankingsUserDto>();

        Cache.Update(rankings);
        _rankings = rankings;
    }
}