using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Web.Services.Rankings;

namespace TwilightImperiumUltimate.Web.Pages.Tigl;

public partial class TiglLeaders
{
    private IReadOnlyCollection<RankingsLeaderDto>? _leaders;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IRankingsDataCache Cache { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        if (!Cache.IsLeadersExpired && Cache.Leaders is not null)
        {
            _leaders = Cache.Leaders;
            return;
        }

        var (leadersResponse, leadersStatus) = await HttpClient.GetAsync<ApiResponse<ItemListDto<RankingsLeaderDto>>>(Paths.ApiPath_RankingsLeaders, cancellationToken: default);
        var leaders = leadersStatus == HttpStatusCode.OK && leadersResponse?.Data is not null
            ? leadersResponse.Data.Items
            : new List<RankingsLeaderDto>();

        Cache.Update(leaders);
        _leaders = leaders;
    }
}
