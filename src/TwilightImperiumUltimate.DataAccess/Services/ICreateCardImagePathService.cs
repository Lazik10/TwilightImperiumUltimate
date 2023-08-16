using TwilightImperiumUltimate.Core.Enums.Cards;

namespace TwilightImperiumUltimate.DataAccess.Services;

public interface ICreateCardImagePathService
{
    string GetCardImagePath<TEnumName, TEnumType>(TEnumName enumName, TEnumType cardType)
        where TEnumName : Enum
        where TEnumType : Enum;
}
