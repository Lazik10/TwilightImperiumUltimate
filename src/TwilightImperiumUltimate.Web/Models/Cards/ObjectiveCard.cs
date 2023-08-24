using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Models.Cards;

public class ObjectiveCard
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public CardType Type { get; set; }

    public string ImagePath { get; set; } = string.Empty;

    public GameVersion GameVersion { get; set; }
}
