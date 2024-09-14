using TwilightImperiumUltimate.Web.Models.GameTracker;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class StrategyCardPicker
{
    [Parameter]
    public required GameTrackerPlayerModel Player { get; set; }

    [Parameter]
    public IReadOnlyCollection<StrategyCardModel> StrategyCards { get; set; } = new List<StrategyCardModel>();

    [Parameter]
    public EventCallback<StrategyCardName> OnCardChange { get; set; }

    public StrategyCardName Value { get; set; }

    [Parameter]
    public int MaxHeight { get; set; } = 100;

    [Parameter]
    public EventCallback OnDecrease { get; set; }

    [Parameter]
    public EventCallback OnIncrease { get; set; }

    [Parameter]
    public MarkupString LeftArrowSymbol { get; set; } = (MarkupString)Strings.ButtonLeftArrow;

    [Parameter]
    public MarkupString RightArrowSymbol { get; set; } = (MarkupString)Strings.ButtonRightArrow;

    [Parameter]
    public int Width { get; set; } = 100;

    protected override void OnInitialized()
    {
        Value = Player.StrategyCard;
    }

    private void DecreaseStrategyCard()
    {
        var strategyCard = StrategyCards.First(x => x.StrategyCardName == Value);
        var initiativeOrder = strategyCard.InitiativeOrder;

        if (initiativeOrder == InitiativeOrder.First)
        {
            initiativeOrder = InitiativeOrder.Eighth;
            Value = StrategyCardName.Imperial;
        }
        else
        {
            initiativeOrder = --initiativeOrder;
            Value = StrategyCards.First(x => x.InitiativeOrder == initiativeOrder).StrategyCardName;
        }

        Player.StrategyCard = Value;

        if (Player.FactionName != FactionName.TheNaaluCollective)
            Player.Initiative = initiativeOrder;
    }

    private void IncreaseStrategyCard()
    {
        var strategyCard = StrategyCards.First(x => x.StrategyCardName == Value);
        var initiativeOrder = strategyCard.InitiativeOrder;

        if (initiativeOrder == InitiativeOrder.Eighth)
        {
            initiativeOrder = InitiativeOrder.First;
            Value = StrategyCardName.Leadership;
        }
        else
        {
            initiativeOrder = ++initiativeOrder;
            Value = StrategyCards.First(x => x.InitiativeOrder == initiativeOrder).StrategyCardName;
        }

        Player.StrategyCard = Value;

        if (Player.FactionName != FactionName.TheNaaluCollective)
            Player.Initiative = initiativeOrder;
    }
}