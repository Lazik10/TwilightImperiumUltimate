namespace TwilightImperiumUltimate.Web.Components.Cards;

public partial class StrategyCard
{
    [Parameter]
    public int MaxHeight { get; set; } = 100;

    [Parameter]
    public StrategyCardName Name { get; set; }

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string GetCardImagePath()
    {
        return PathProvider.GetCardImagePath(Name.ToString(), Paths.ResourcePath_StrategyCard);
    }
}