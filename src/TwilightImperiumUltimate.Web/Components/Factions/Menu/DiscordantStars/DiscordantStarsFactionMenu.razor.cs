namespace TwilightImperiumUltimate.Web.Components.Factions.Menu.DiscordantStars;

public partial class DiscordantStarsFactionMenu : TwilightImperiumBaseComponent
{
    private static readonly IReadOnlyDictionary<ResponsiveBreakpoint, (IReadOnlyList<int> Columns, IReadOnlyList<int> PlaceholderPositions)> _layout =
        new Dictionary<ResponsiveBreakpoint, (IReadOnlyList<int> Columns, IReadOnlyList<int> PlaceholderPositions)>
        {
            [ResponsiveBreakpoint.Desktop] = (new List<int> { 17, 17 }, new List<int> { }),
            [ResponsiveBreakpoint.Tablet] = (new List<int> { 17, 17 }, new List<int> { }),
            [ResponsiveBreakpoint.Mobile] = (new List<int> { 9, 8, 9, 8 }, new List<int> { }),
        };

    private IReadOnlyCollection<FactionDto> _factions = Array.Empty<FactionDto>();

    [Inject]
    private IFactionProvider FactionProvider { get; set; } = default!;
    
    protected override async Task OnInitializedAsync()
    {
        _factions = await FactionProvider.GetFactionsBySource(FactionSource.DiscordantStars);
    }

    private async Task HandleFactionClick(FactionDto faction)
    {
        FactionProvider.SetCurrentFactionName(faction.FactionName);
    }
}
