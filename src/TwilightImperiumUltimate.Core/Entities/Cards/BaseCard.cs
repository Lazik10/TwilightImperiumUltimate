using TwilightImperiumUltimate.Core.Enums.Cards;
using TwilightImperiumUltimate.Core.Enums.Game;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public class BaseCard : IEntity, ICard, IGameVersion
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public CardType Type { get; set; }

    public string ImagePath { get; set; } = string.Empty;

    public GameVersion GameVersion { get; set; }
}
