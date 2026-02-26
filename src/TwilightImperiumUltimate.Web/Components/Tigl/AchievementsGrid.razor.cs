using TwilightImperiumUltimate.Contracts.DTOs.Rankings;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class AchievementsGrid
{
    private List<RankingsAchievementDto> _achievements = new();
    private bool _loading;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        await LoadAchievements();
        _loading = false;
    }

    private static string FormatDate(long ts)
    {
        if (ts <= 0) return "-";
        var dt = DateTimeOffset.FromUnixTimeMilliseconds(ts).ToLocalTime().DateTime;
        return dt.ToString("dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
    }

    private async Task LoadAchievements()
    {
        var (resp, status) = await HttpClient.GetAsync<ApiResponse<ItemListDto<RankingsAchievementDto>>>(Paths.ApiPath_RecentAchievements);
        if (status == HttpStatusCode.OK && resp?.Data?.Items is not null)
        {
            _achievements = resp.Data.Items
                .OrderByDescending(a => a.AchievedAt)
                .ToList();
        }
    }

    private void RedirectToGameDetail(int id)
    {
        var returnUrl = Uri.EscapeDataString(Pages.Pages.TiglAchievements);
        NavigationManager.NavigateTo($"{Pages.Pages.TiglGameDetail}?id={id}&returnUrl={returnUrl}");
    }

    private void NavigateToProfile(int tiglUserId)
    {
        if (tiglUserId == 0)
            return;

        var returnUrl = Uri.EscapeDataString(Pages.Pages.TiglRankings);
        NavigationManager.NavigateTo($"{Pages.Pages.TiglPlayerProfile}?playerId={tiglUserId}&returnUrl={returnUrl}");
    }
}