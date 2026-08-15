using TwilightImperiumUltimate.Web.Services.Factions;

namespace TwilightImperiumUltimate.Web.Components.Factions.Menu.Official;

public partial class OfficialFactionMenu : TwilightImperiumBaseComponent
{
    private static readonly IReadOnlyDictionary<ResponsiveBreakpoint, (IReadOnlyList<int> Columns, IReadOnlyList<int> PlaceholderPositions)> _layout =
        new Dictionary<ResponsiveBreakpoint, (IReadOnlyList<int> Columns, IReadOnlyList<int> PlaceholderPositions)>
        {
            [ResponsiveBreakpoint.Desktop] = (new List<int> { 17, 17 }, new List<int> { 17, 25, 27, 33 }),
            [ResponsiveBreakpoint.Tablet] = (new List<int> { 17, 17 }, new List<int> { 17, 25, 27, 33 }),
            [ResponsiveBreakpoint.Mobile] = (new List<int> { 9, 8, 9, 9 }, new List<int> { 23, 25 }),
        };

    private IReadOnlyCollection<FactionDto> _factions = Array.Empty<FactionDto>();

    [Parameter]
    public EventCallback<FactionModel> OnFactionClick { get; set; }

    [Inject]
    private IFactionProvider FactionProvider { get; set; } = default!;
    
    protected override async Task OnInitializedAsync()
    {
        _factions = await FactionProvider.GetFactionsBySource(FactionSource.Official);
    }
}
