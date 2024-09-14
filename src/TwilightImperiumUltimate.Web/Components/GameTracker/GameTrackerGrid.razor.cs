using System.Globalization;
using System.Timers;
using TwilightImperiumUltimate.Contracts.ApiContracts.Statistics;
using TwilightImperiumUltimate.Contracts.DTOs.GameTracker;
using TwilightImperiumUltimate.Web.Services.GameTracker;
using Timer = System.Timers.Timer;

namespace TwilightImperiumUltimate.Web.Components.GameTracker;

public partial class GameTrackerGrid : IDisposable
{
    private GameTrackerPhase _currentPhase;
    private GameTrackerWindow _selectedWindow = GameTrackerWindow.StrategyCardPick;
    private Timer _timer = new Timer();
    private TimeSpan elapsedTime = TimeSpan.Zero;
    private bool isTimerRunning;
    private bool currentRoundStatisticsSent;
    private OverviewWindow _overviewWindow = default!;

    [Inject]
    private IGameTrackerService GameTrackerService { get; set; } = default!;

    [Inject]
    private IGameTrackerSettingsService GameTrackerSettingsService { get; set; } = default!;

    [Inject]
    private IObjectiveCardTracker ObjectiveCardTracker { get; set; } = default!;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    public Task Refresh()
    {
        _currentPhase = GameTrackerService.CurrentPhase;
        StateHasChanged();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected override async void OnInitialized()
    {
        _currentPhase = GameTrackerService.CurrentPhase;

        await GameTrackerService.InitializePlayers();

        _timer = new Timer(1000);
        _timer.Elapsed += UpdateElapsedTime;

        StateHasChanged();
    }

    protected virtual void Dispose(bool disposing)
    {
        _timer?.Dispose();
    }

    private void SetSelectedWindow(GameTrackerWindow window)
    {
        _selectedWindow = window;
        StateHasChanged();
    }

    private void StartTimer()
    {
        _timer.Start();
        isTimerRunning = true;
    }

    private void StopTimer()
    {
        _timer?.Stop();
        isTimerRunning = false;
    }

    private async Task ResetTimer()
    {
        StopTimer();
        elapsedTime = TimeSpan.Zero;

        foreach (var player in GameTrackerService.Players)
        {
            player.ElapsedTime = TimeSpan.Zero;
        }

        await _overviewWindow.Refresh();
    }

    private void UpdateElapsedTime(object? sender, ElapsedEventArgs e)
    {
        InvokeAsync(() =>
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
            GameTrackerService.ActivePlayer.ElapsedTime += TimeSpan.FromSeconds(1);
        });

        _overviewWindow.Refresh().ConfigureAwait(true);
        StateHasChanged();
    }

    private void Agenda()
    {
        GameTrackerService.SetActivePlayerForAgenda();
    }

    private async Task ProcessNextRound()
    {
        await SendCurrentFinishedRoundStatistics();
        GameTrackerService.CurrentRound++;
        _selectedWindow = GameTrackerWindow.StrategyCardPick;
        currentRoundStatisticsSent = false;
        StateHasChanged();
    }

    private bool ShowFinishGameButton()
    {
        var maxPlayerPoints = GameTrackerService.Players.Max(x => x.Score);
        return maxPlayerPoints >= GameTrackerSettingsService.NumberOfPoints;
    }

    private async Task FinishGame()
    {
        if (GameTrackerService.CurrentPhase == GameTrackerPhase.GameEnded)
            return;

        await SendCurrentFinishedRoundStatistics();

        var statisitcs = new GameStatisticsDto(
            GameTrackerService.GameId,
            GameTrackerSettingsService.NumberOfPoints,
            GameTrackerSettingsService.NumberOfPlayers,
            GameTrackerService.CurrentRound,
            elapsedTime.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture),
            GameTrackerService.Players.OrderByDescending(x => x.Score).ThenBy(x => x.Initiative).Select(x => x.FactionName).First());

        await HttpClient.PostAsync<GameStatisticsDto, ApiResponse<StatisticsResponse>>(Paths.ApiPath_EndGameStatistics, statisitcs);

        StopTimer();
        await GameTrackerService.SetGamePhase(GameTrackerPhase.GameEnded);
        _selectedWindow = GameTrackerWindow.Winner;
        StateHasChanged();
    }

    private async Task SendCurrentFinishedRoundStatistics()
    {
        if (currentRoundStatisticsSent)
            return;

        var factionStats = GameTrackerService.Players.Select(x =>
            new FactionStatsDto(
                x.FactionName,
                x.Score,
                x.StrategyCard))
            .ToList();

        var statisitcs = new RoundStatisticsDto(
            GameTrackerService.GameId,
            GameTrackerService.CurrentRound,
            factionStats);

        await HttpClient.PostAsync<RoundStatisticsDto, ApiResponse<StatisticsResponse>>(Paths.ApiPath_RoundStatistics, statisitcs);

        currentRoundStatisticsSent = true;
    }
}