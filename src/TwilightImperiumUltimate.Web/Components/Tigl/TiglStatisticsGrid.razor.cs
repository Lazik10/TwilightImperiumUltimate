namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglStatisticsGrid
{
    private TiglStatisticsType _selectedStatisticsType = TiglStatisticsType.Factions;
    private int _selectedSeasonNumber = -1;
    private TiglLeague _selectedLeague = TiglLeague.ProphecyOfKings;

    private void ChangeStatisticsType(TiglStatisticsType statisticType)
    {
        _selectedStatisticsType = statisticType;
        StateHasChanged();
    }

    private void OnSeasonChanged(int seasonNumber)
    {
        _selectedSeasonNumber = seasonNumber;
        StateHasChanged();
    }

    private void OnLeagueChanged(TiglLeague league)
    {
        _selectedLeague = league;
        StateHasChanged();
    }
}