namespace TwilightImperiumUltimate.Web.Components.Factions.Menu.WhispersFromTheVoid;

/// <summary>
/// Displays <see cref="FactionSource.WhispersFromTheVoid"/> factions using the shared non-official
/// source layout defined in <see cref="SourceFactionMenu"/>.
/// </summary>
public partial class WhispersFromTheVoidFactionMenu : TwilightImperiumBaseComponent
{
    private static readonly IReadOnlyDictionary<ResponsiveBreakpoint, (IReadOnlyList<int> Columns, IReadOnlyList<int> PlaceholderPositions)> _layout =
        new Dictionary<ResponsiveBreakpoint, (IReadOnlyList<int> Columns, IReadOnlyList<int> PlaceholderPositions)>
        {
            [ResponsiveBreakpoint.Desktop] = (new List<int> { 16 }, new List<int> { 0, 1, 2, 3, 13, 14, 15, 16 }),
            [ResponsiveBreakpoint.Tablet] = (new List<int> { 8 }, new List<int> { }),
            [ResponsiveBreakpoint.Mobile] = (new List<int> { 8 }, new List<int> { }),
        };

    private IReadOnlyCollection<FactionDto> _factions = Array.Empty<FactionDto>();

    [Inject]
    private IFactionProvider FactionProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _factions = await FactionProvider.GetFactionsBySource(FactionSource.WhispersFromTheVoid);
    }

    private async Task HandleFactionClick(FactionDto faction)
    {
        FactionProvider.SetCurrentFactionName(faction.FactionName);
    }
}
