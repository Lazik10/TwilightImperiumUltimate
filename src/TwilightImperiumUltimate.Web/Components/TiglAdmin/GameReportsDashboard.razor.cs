using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Options.Async;

namespace TwilightImperiumUltimate.Web.Components.TiglAdmin;

public partial class GameReportsDashboard
{
    private bool _loading;
    private bool _confirming;
    private IJSObjectReference? _jsModule;

    private List<MatchReportDto> _matchReportsInQueue = new();
    private List<MatchReportDto> _detailedReports = new();
    private Dictionary<int, (string Title, string Message)> _evaluationErrors = new();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private IConfiguration Configuration { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadReportsAsync();
    }

    private async Task LoadReportsAsync()
    {
        _loading = true;
        _detailedReports.Clear();
        _evaluationErrors.Clear();

        var allMatchReportsResponse = await HttpClient.GetAsync<ApiResponse<ItemListDto<MatchReportDto>>>(Paths.ApiPath_GameReports);
        if (allMatchReportsResponse.StatusCode == HttpStatusCode.OK && allMatchReportsResponse.Response?.Data?.Items is not null)
        {
            _matchReportsInQueue = allMatchReportsResponse.Response.Data.Items.Where(r => r.State == MatchState.AddedInQueue).ToList();

            foreach (var matchReport in _matchReportsInQueue)
            {
                var detailResult = await HttpClient.GetAsync<ApiResponse<MatchReportDto>>($"{Paths.ApiPath_GameReport}{matchReport.Id}");
                if (detailResult.StatusCode == HttpStatusCode.OK && detailResult.Response?.Data is not null)
                {
                    _detailedReports.Add(detailResult.Response.Data);
                }
            }
        }

        _loading = false;
    }

    private static string GetUserName(PlayerResultDto player)
    {
        if (player.TiglUserName == player.DiscordUserName)
            return player.TiglUserName;

        return $"{player.TiglUserName} ({player.DiscordUserName})";
    }

    private async Task ConfirmAsync(int matchReportId)
    {
        if (_confirming)
            return;

        _confirming = true;

        var evaluationRequest = new EvaluateGameReportRequest { MatchReportId = matchReportId };
        var evaluateResult = await HttpClient.PostAsync<EvaluateGameReportRequest, ApiResponse<GameReportResult>>(Paths.ApiPath_EvaluateGameReport, evaluationRequest);

        if (evaluateResult.StatusCode == HttpStatusCode.OK && evaluateResult.Response?.Data?.Success == true)
        {
            _evaluationErrors.Remove(matchReportId);
            await LoadReportsAsync();
        }
        else
        {
            var title = evaluateResult.Response?.ProblemDetails?.Title ?? "Evaluation Failed";
            var message = evaluateResult.Response?.ProblemDetails?.Detail ?? "Unknown error while evaluating game report";
            _evaluationErrors[matchReportId] = (title, message);
        }

        _confirming = false;
    }

    private async Task RedirectToGameDetails(string gameId)
    {
        if (_jsModule is null)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/TiglAdmin/GameReportsDashboard.razor.js");
        }

        var path = Configuration.GetSection(nameof(AsyncServerOptions))[nameof(AsyncServerOptions.BaseGameUrl)];
        _ = Task.Run(async () => await _jsModule.InvokeVoidAsync("openInNewTab", $"{path}{gameId}"));
    }
}
