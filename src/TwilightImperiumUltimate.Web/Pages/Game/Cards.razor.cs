using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Pages.Game;

public partial class Cards
{
    private string _currentTypeOfCard = Strings.StrategyCard;

    private void SetCurrentTypeOfCard(string typeOfCard)
    {
        _currentTypeOfCard = typeOfCard;
        StateHasChanged();
    }
}