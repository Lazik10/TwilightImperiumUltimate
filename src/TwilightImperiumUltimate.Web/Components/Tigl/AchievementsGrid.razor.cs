using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using Microsoft.AspNetCore.Components.Web;

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
        return dt.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
    }

    private static string GetGameDetailUrl(int id)
    {
        var returnUrl = Uri.EscapeDataString(Pages.Pages.TiglAchievements);
        return $"{Pages.Pages.TiglGameDetail}?id={id}&returnUrl={returnUrl}";
    }

    private async Task LoadAchievements()
    {
        var (resp, status) = await HttpClient.GetAsync<ApiResponse<ItemListDto<RankingsAchievementDto>>>(Paths.ApiPath_RecentAchievements);
        if (status == HttpStatusCode.OK && resp?.Data?.Items is not null)
        {
            _achievements = resp.Data.Items
                .OrderByDescending(a => a.AchievedAt)
                .Take(100)
                .ToList();
        }
    }

    private void NavigateToProfile(int tiglUserId)
    {
        if (tiglUserId == 0)
            return;

        var returnUrl = Uri.EscapeDataString(Pages.Pages.TiglRankings);
        NavigationManager.NavigateTo($"{Pages.Pages.TiglPlayerProfile}?playerId={tiglUserId}&returnUrl={returnUrl}");
    }

    private void OnRowKeyDown(KeyboardEventArgs eventArgs, int tiglUserId)
    {
        if (eventArgs.Key is "Enter" or " ")
            NavigateToProfile(tiglUserId);
    }

    private static TextColor GetRarityTextColor(double rarityPercent)
    {
        return rarityPercent switch
        {
            < 0.1 => TextColor.Pink,
            < 0.5 => TextColor.Orange,
            < 5.0 => TextColor.Purple,
            < 15.0 => TextColor.LightBlue,
            < 30.0 => TextColor.Green,
            _ => TextColor.White,
        };
    }
}
