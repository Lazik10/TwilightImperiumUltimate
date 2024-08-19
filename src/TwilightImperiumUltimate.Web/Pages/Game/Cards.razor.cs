namespace TwilightImperiumUltimate.Web.Pages.Game;

public partial class Cards
{
    private string _currentTypeOfCard = Paths.ResourcePath_StrategyCard;

    private void SetCurrentTypeOfCard(string typeOfCard)
    {
        _currentTypeOfCard = typeOfCard;
        StateHasChanged();
    }
}