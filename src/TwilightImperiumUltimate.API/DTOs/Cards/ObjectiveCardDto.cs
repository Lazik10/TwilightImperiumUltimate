using TwilightImperiumUltimate.Core.Enums.Cards;
using TwilightImperiumUltimate.Core.Enums.Game;

namespace TwilightImperiumUltimate.API.DTOs.Cards;

public class ObjectiveCardDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public CardType Type { get; set; }

    public string ImagePath { get; set; } = string.Empty;

    public GameVersion GameVersion { get; set; }

    public ObjectiveCardName ObjectiveCardName { get; set; }

    public ObjectiveCardType ObjectiveCardType { get; set; }
}
