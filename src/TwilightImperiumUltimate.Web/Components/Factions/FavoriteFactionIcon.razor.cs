namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FavoriteFactionIcon
{
    [Parameter]
    public required FactionName FactionName { get; set; }

    [Parameter]
    public int Height { get; set; } = 50;

    public string Name => FactionName.GetFactionUIText(FactionResourceType.Title);

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string GetFactionIconPath() => PathProvider.GetFactionIconPath(FactionName);
}