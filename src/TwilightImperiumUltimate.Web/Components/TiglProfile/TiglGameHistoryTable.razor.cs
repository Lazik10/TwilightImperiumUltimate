using Microsoft.AspNetCore.Components.Web;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Components.TiglProfile;

public partial class TiglGameHistoryTable
{
    private readonly HashSet<int> _expandedYears = [];
    private readonly HashSet<(int Year, int Month)> _expandedMonths = [];
    private IReadOnlyList<TiglProfileGameDto>? _lastGames;
    private bool _allExpanded;

    [Parameter]
    public IReadOnlyList<TiglProfileGameDto> Games { get; set; } = [];

    [Parameter]
    public EventCallback<int> OnGameSelected { get; set; }

    private IEnumerable<GamesByYear> GroupedGames => Games
        .GroupBy(game => DateTimeOffset.FromUnixTimeMilliseconds(game.StartTimestamp).Year)
        .OrderByDescending(yearGroup => yearGroup.Key)
        .Select(yearGroup => new GamesByYear(
            yearGroup.Key,
            yearGroup
                .GroupBy(game => DateTimeOffset.FromUnixTimeMilliseconds(game.StartTimestamp).Month)
                .OrderByDescending(monthGroup => monthGroup.Key)
                .Select(monthGroup => new GamesByMonth(monthGroup.Key, monthGroup.OrderByDescending(game => game.StartTimestamp).ToList()))
                .ToList()));

    protected override void OnParametersSet()
    {
        if (ReferenceEquals(_lastGames, Games))
            return;

        _lastGames = Games;
        InitializeDefaultExpansion();
    }

    private void InitializeDefaultExpansion()
    {
        _expandedYears.Clear();
        _expandedMonths.Clear();
        _allExpanded = false;

        var mostRecentYear = GroupedGames.FirstOrDefault();
        if (mostRecentYear is null)
            return;

        _expandedYears.Add(mostRecentYear.Year);

        var mostRecentMonth = mostRecentYear.Months.FirstOrDefault();
        if (mostRecentMonth is not null)
            _expandedMonths.Add((mostRecentYear.Year, mostRecentMonth.Month));
    }

    private void OnExpandAllChanged(bool isChecked)
    {
        if (!isChecked)
        {
            InitializeDefaultExpansion();
            return;
        }

        foreach (var year in GroupedGames)
        {
            _expandedYears.Add(year.Year);
            foreach (var month in year.Months)
                _expandedMonths.Add((year.Year, month.Month));
        }

        _allExpanded = true;
    }

    private bool IsYearExpanded(int year) => _expandedYears.Contains(year);

    private bool IsMonthExpanded(int year, int month) => _expandedMonths.Contains((year, month));

    private void ToggleYear(int year)
    {
        if (!_expandedYears.Remove(year))
            _expandedYears.Add(year);

        _allExpanded = AreAllGameGroupsExpanded();
    }

    private void ToggleMonth(int year, int month)
    {
        var key = (year, month);
        if (!_expandedMonths.Remove(key))
            _expandedMonths.Add(key);

        _allExpanded = AreAllGameGroupsExpanded();
    }

    private bool AreAllGameGroupsExpanded()
    {
        var groupedGames = GroupedGames.ToList();
        return groupedGames.Any()
            && groupedGames.All(year => _expandedYears.Contains(year.Year)
                && year.Months.All(month => _expandedMonths.Contains((year.Year, month.Month))));
    }

    private void OnYearKeyDown(KeyboardEventArgs eventArgs, int year)
    {
        if (eventArgs.Key is "Enter" or " ")
            ToggleYear(year);
    }

    private void OnMonthKeyDown(KeyboardEventArgs eventArgs, int year, int month)
    {
        if (eventArgs.Key is "Enter" or " ")
            ToggleMonth(year, month);
    }

    private async Task OnGameKeyDown(KeyboardEventArgs eventArgs, int matchReportId)
    {
        if (eventArgs.Key is "Enter" or " ")
            await OnGameSelected.InvokeAsync(matchReportId);
    }

    private static string GetMonthName(int month) => new DateOnly(2000, month, 1).ToString("MMMM");

    private static string GetDuration(TiglProfileGameDto game)
    {
        if (game.StartTimestamp <= 0 || game.EndTimestamp <= 0)
            return "N/A";

        var duration = DateTimeOffset.FromUnixTimeMilliseconds(game.EndTimestamp) - DateTimeOffset.FromUnixTimeMilliseconds(game.StartTimestamp);
        if (duration < TimeSpan.FromHours(1))
            return "-";

        return $"{(int)duration.TotalDays:D2} d {duration.Hours:D2} h";
    }

    private sealed record GamesByYear(int Year, IReadOnlyList<GamesByMonth> Months);

    private sealed record GamesByMonth(int Month, IReadOnlyList<TiglProfileGameDto> Games);
}
