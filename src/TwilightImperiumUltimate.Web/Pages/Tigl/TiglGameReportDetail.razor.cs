using System.Globalization;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Options.Async;

namespace TwilightImperiumUltimate.Web.Pages.Tigl;

public partial class TiglGameReportDetail
{
    private bool _isRankingSystemFilterVisible;
    private RankingSystem _selectedRankingSystem = RankingSystem.TrueSkill;

    [Parameter]
    [SupplyParameterFromQuery(Name = "id")]
    public int Id { get; set; }

    private MatchReportDto? MatchReport { get; set; }

    private string PageTitle => Strings.Page_TiglGameReportDetail_PageTitle.FormatWith(MatchReport?.GameId ?? Strings.Page_TiglGameReports);

    private List<PlayerResultDto> Winners => GetWinners();

    private IEnumerable<PlayerResultDto> OrderedPlayerResults => MatchReport!.PlayerResults
        .OrderBy(player => !player.IsWinner)
        .ThenByDescending(player => player.Score);

    private string RankingSystemFilterButtonLabel => _isRankingSystemFilterVisible
        ? "Hide ranking system filter"
        : "Show ranking system filter";

    private string RankingSystemFilterIconPath => PathProvider.GetIconPath(_isRankingSystemFilterVisible ? IconType.FilterClicked : IconType.Filter);

    private IReadOnlyCollection<KeyValuePair<RankingSystem, string>> RankingSystemOptions =>
        EnumExtensions.GetEnumValuesWithDisplayNames<RankingSystem>();

    [Inject]
    private IConfiguration Configuration { get; set; } = default!;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadGameReport();
    }

    private static string FormatTimestamp(long timestamp)
    {
        if (timestamp == 0)
            return "Unknown";

        return DateTimeOffset.FromUnixTimeMilliseconds(timestamp)
            .ToLocalTime()
            .ToString("yyyy-MM-dd - HH:mm", CultureInfo.InvariantCulture);
    }

    private static string GetUserName(PlayerResultDto player)
    {
        if (player.TiglUserName == player.DiscordUserName)
            return player.TiglUserName;

        return $"{player.TiglUserName} ({player.DiscordUserName})";
    }

    private async Task LoadGameReport()
    {
        var (lbResponse, lbStatus) = await HttpClient.GetAsync<ApiResponse<MatchReportDto>>($"{Paths.ApiPath_GameReport}{Id}");
        if (lbStatus == HttpStatusCode.OK && lbResponse?.Data is not null)
        {
            MatchReport = lbResponse.Data;
        }
    }

    private TiglRankName GetGameRank()
    {
        if (MatchReport is not null && MatchReport.PlayerMatchAsyncStats is not null && MatchReport.PlayerMatchAsyncStats.Count > 0)
            return MatchReport!.PlayerMatchAsyncStats.Min(x => x.OldRank);

        return TiglRankName.Unranked;
    }

    private List<PlayerResultDto> GetWinners() => MatchReport!.PlayerResults.Where(x => x.IsWinner).ToList();

    private void ChangeRankingSystem(RankingSystem rankingSystem)
    {
        _selectedRankingSystem = rankingSystem;
        StateHasChanged();
    }

    private void ToggleRankingSystemFilter()
    {
        _isRankingSystemFilterVisible = !_isRankingSystemFilterVisible;
    }

    private void NavigateToAsyncGame()
    {
        var baseGameUrl = Configuration.GetSection(nameof(AsyncServerOptions))[nameof(AsyncServerOptions.BaseGameUrl)];
        if (!string.IsNullOrWhiteSpace(baseGameUrl))
        {
            NavigationManager.NavigateTo($"{baseGameUrl}{MatchReport!.GameId}", forceLoad: true);
        }
    }

    private AsyncPlayerMatchStatsDto GetPlayersAsyncData(int playerId)
    {
        return MatchReport!.PlayerMatchAsyncStats.FirstOrDefault(x => x.TiglUserId == playerId) ?? new AsyncPlayerMatchStatsDto();
    }

    private GlickoPlayerMatchStatsDto GetPlayersGlickoData(int playerId)
    {
        return MatchReport!.PlayerMatchGlickoStats.FirstOrDefault(x => x.TiglUserId == playerId) ?? new GlickoPlayerMatchStatsDto();
    }

    private TrueSkillPlayerMatchStatsDto GetPlayersTrueSkillData(int playerId)
    {
        return MatchReport!.PlayerMatchTrueSkillStats.FirstOrDefault(x => x.TiglUserId == playerId) ?? new TrueSkillPlayerMatchStatsDto();
    }

    private void NavigateToPlayerProfile(PlayerResultDto player)
    {
        NavigationManager.NavigateTo($"{Pages.TiglPlayerProfile}?playerId={player.TiglUserId}");
    }

    private TextColor GetChangeColor(double value)
    {
        return value switch
        {
            > 0 => TextColor.Green,
            < 0 => TextColor.Red,
            _ => TextColor.Yellow,
        };
    }
}
