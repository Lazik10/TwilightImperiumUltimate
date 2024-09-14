namespace TwilightImperiumUltimate.Web.Models.Cards;

public class ObjectiveCardModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public CardType CardType { get; set; }

    public GameVersion GameVersion { get; set; }

    public ObjectiveCardName ObjectiveCardName { get; set; }

    public ObjectiveCardType ObjectiveCardType { get; set; }

    public TimingWindow TimingWindow { get; set; }
}
