using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Options.Async;

namespace TwilightImperiumUltimate.Web.Pages.Tigl;

public partial class TiglGameReportDetail
{
    private int _placement;
    private IJSObjectReference? _jsModule;
    private RankingSystem _selectedRankingSystem = RankingSystem.TrueSkill;

    [Parameter]
    [SupplyParameterFromQuery(Name = "id")]
    public int Id { get; set; }

    private MatchReportDto? MatchReport { get; set; }

    private List<PlayerResultDto> Winners => GetWinners();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private IConfiguration Configuration { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadGameReport();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Tigl/TiglGameReportDetail.razor.js");
        }
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

    private async Task RedirectToGameDetails(string gameId)
    {
        if (_jsModule is null)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Tigl/TiglGameReportDetail.razor.js");
        }

        var path = Configuration.GetSection(nameof(AsyncServerOptions))[nameof(AsyncServerOptions.BaseGameUrl)];
        _ = Task.Run(async () => await _jsModule.InvokeVoidAsync("openInNewTab", $"{path}{gameId}"));
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

    private string GetGameRankString()
    {
        return $"Game Rank: {GetGameRank().GetDisplayName()}";
    }

    private string GetGameLeagueString() => $"League: {MatchReport!.League.GetDisplayName()}";

    private void RedirectBack() => NavigationManager.NavigateTo($"{Pages.Tigl}?category=games");

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