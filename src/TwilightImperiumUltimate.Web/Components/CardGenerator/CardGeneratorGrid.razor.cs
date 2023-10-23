using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Components.CardGenerator;

public partial class CardGeneratorGrid
{
    public CardSettings? CardSettings { get; set; }

    public void SetCardType(CardGenerationType cardType)
    {
        CardSettings!.SelectedCardType = cardType;
        Refresh();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
            StateHasChanged();
    }

    private void Refresh()
    {
        StateHasChanged();
    }
}
