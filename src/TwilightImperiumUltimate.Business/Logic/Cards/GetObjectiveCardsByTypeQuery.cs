namespace TwilightImperiumUltimate.Business.Logic.Cards;

public class GetObjectiveCardsByTypeQuery(
    ObjectiveCardType objectiveCardType)
    : IRequest<ItemListDto<ObjectiveCardDto>>
{
    public ObjectiveCardType ObjectiveCardType { get; set; } = objectiveCardType;
}
