namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class StatisticsGrid
{
    private AsyncStatisticsTypeMenuItem _selectedMenuItem;
    private PlayerStatisticsType _selectedGamesType;
    private QueryLimit _selectedQueryLimit = QueryLimit.Twenty;
    private List<QueryLimit> _excludedQueryLimits = new List<QueryLimit>() { QueryLimit.None };

    private void UpdateSelectedMenuItem(AsyncStatisticsTypeMenuItem menuItem)
    {
        _selectedMenuItem = menuItem;
        StateHasChanged();
    }

    private void OnEnumChanged(PlayerStatisticsType statisticsType)
    {
        _selectedGamesType = statisticsType;
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
}