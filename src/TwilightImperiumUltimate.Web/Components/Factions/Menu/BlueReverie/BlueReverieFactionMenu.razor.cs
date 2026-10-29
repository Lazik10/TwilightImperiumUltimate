namespace TwilightImperiumUltimate.Web.Components.Factions.Menu.BlueReverie;

/// <summary>
/// Displays <see cref="FactionSource.BlueReverie"/> factions using the shared non-official
/// source layout defined in <see cref="SourceFactionMenu"/>.
/// </summary>
public partial class BlueReverieFactionMenu : TwilightImperiumBaseComponent
{
    private static readonly IReadOnlyDictionary<ResponsiveBreakpoint, (IReadOnlyList<int> Columns, IReadOnlyList<int> PlaceholderPositions)> _layout =
        new Dictionary<ResponsiveBreakpoint, (IReadOnlyList<int> Columns, IReadOnlyList<int> PlaceholderPositions)>
        {
            [ResponsiveBreakpoint.Desktop] = (new List<int> { 17 }, new List<int> { 0, 1, 2, 3, 14, 15, 16, 17 }),
            [ResponsiveBreakpoint.Tablet] = (new List<int> { 9 }, new List<int> { }),
            [ResponsiveBreakpoint.Mobile] = (new List<int> { 9 }, new List<int> { }),
        };

    private IReadOnlyCollection<FactionDto> _factions = Array.Empty<FactionDto>();

    [Inject]
    private IFactionProvider FactionProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _factions = await FactionProvider.GetFactionsBySource(FactionSource.BlueReverie);
    }

    private async Task HandleFactionClick(FactionDto faction)
    {
        FactionProvider.SetCurrentFactionName(faction.FactionName);
    }
}