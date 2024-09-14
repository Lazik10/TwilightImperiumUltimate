namespace TwilightImperiumUltimate.Web.Models.Cards;

public class StrategyCardModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public CardType CardType { get; set; }

    public GameVersion GameVersion { get; set; }

    public StrategyCardName StrategyCardName { get; set; }

    public InitiativeOrder InitiativeOrder { get; set; }
}
