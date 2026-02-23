namespace TwilightImperiumUltimate.Web.Models.Cards;

public class SpecialComponentCardModel : CardModel
{
    public SpecialComponentName SpecialComponentName { get; set; }

    public FactionName FactionName { get; set; }

    public int Count { get; set; }

    public SpecialComponentType SpecialType { get; set; }
}
