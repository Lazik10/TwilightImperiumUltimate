namespace TwilightImperiumUltimate.Web.Models.Cards;

public class CardModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public CardType CardType { get; set; }

    public GameVersion GameVersion { get; set; }
}
