namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionBreakthrough
{
    [Parameter]
    public FactionName FactionName { get; set; }

    [Parameter]
    [EditorRequired]
    public IReadOnlyList<BreakthroughCardModel> BreakthroughCards { get; set; } = [];

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private IReadOnlyList<string> GetBreakthroughImagePaths() => FactionName == FactionName.TheFirmamentTheObsidian
        ? [PathProvider.GetBreakthroughImagePath("TheSowing"), PathProvider.GetBreakthroughImagePath("TheReaping")]
        : BreakthroughCards.Select(card => PathProvider.GetBreakthroughImagePath(card.BreakthroughName)).ToList();

    private int GetDesktopColumns() => Math.Clamp(GetBreakthroughImagePaths().Count, 1, 2);
}
