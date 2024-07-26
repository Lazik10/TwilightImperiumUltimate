using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Cards;

public abstract class BaseCard<TCardNameEnum> : IEntity, ICard<TCardNameEnum>, IGameVersion
    where TCardNameEnum : Enum
{
    public int Id { get; set; }

    public abstract TCardNameEnum EnumName { get; set; }

    public string Name => EnumName.ToString();

    public string Text { get; set; } = string.Empty;

    public abstract CardType Type { get; set; }

    public GameVersion GameVersion { get; set; }
}
