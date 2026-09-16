using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class StatisticsGrid
{
    private readonly IReadOnlyCollection<KeyValuePair<QueryLimit, string>> _queryLimitOptions =
        EnumExtensions.GetEnumValuesWithDisplayNames<QueryLimit>()
            .Where(item => item.Key != QueryLimit.None)
            .ToList();

    private readonly IReadOnlyCollection<StatisticsFilterOption> _statisticsFilters =
        EnumExtensions.GetEnumValuesWithDisplayNames<PlayerStatisticsType>()
            .Select(item => new StatisticsFilterOption(
                item.Key,
                item.Key == PlayerStatisticsType.Custom ? "Casual" : item.Value))
            .ToList();

    private AsyncStatisticsTypeMenuItem _selectedMenuItem;
    private PlayerStatisticsType _selectedGamesType;
    private QueryLimit _selectedQueryLimit = QueryLimit.Twenty;

    private bool IsGeneralStatistics => _selectedMenuItem == AsyncStatisticsTypeMenuItem.General;

    private void UpdateSelectedMenuItem(AsyncStatisticsTypeMenuItem menuItem)
    {
        _selectedMenuItem = menuItem;
        StateHasChanged();
    }

    private void OnEnumChanged(PlayerStatisticsType statisticsType)
    {
        _selectedGamesType = statisticsType;
        StateHasChanged();
    }

    private void OnQueryLimitChanged(QueryLimit queryLimit)
    {
        _selectedQueryLimit = queryLimit;
        StateHasChanged();
    }

    private int GetQueryLimit()
    {
        return _selectedQueryLimit switch
        {
            QueryLimit.Fifty => 50,
            QueryLimit.Hundred => 100,
            QueryLimit.TwoHundred => 200,
            _ => 20,
        };
    }

    private sealed record StatisticsFilterOption(PlayerStatisticsType Value, string DisplayName);
}
