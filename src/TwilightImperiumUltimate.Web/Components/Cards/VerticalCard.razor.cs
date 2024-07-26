namespace TwilightImperiumUltimate.Web.Components.Cards;

public partial class VerticalCard
{
    [Parameter]
    public string Name { get; set; } = string.Empty;

    [Parameter]
    public string TypeOfCard { get; set; } = string.Empty;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string GetCardImagePath()
    {
        return PathProvider.GetCardImagePath(Name, TypeOfCard);
    }
}