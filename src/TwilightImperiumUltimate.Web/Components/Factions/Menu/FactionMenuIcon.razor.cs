namespace TwilightImperiumUltimate.Web.Components.Factions.Menu;

/// <summary>
/// Single clickable faction icon cell used inside <see cref="FactionMenuGrid{TItem}"/> item
/// templates. Renders a light-blue underline that is always shown for the faction matching
/// <see cref="IFactionProvider.CurrentFactionName"/> (active state) and grows in from the
/// bottom-middle on hover for every other icon (see FactionMenuIcon.razor.css).
/// </summary>
public partial class FactionMenuIcon
{
    [Parameter]
    [EditorRequired]
    public FactionName FactionName { get; set; }

    [Parameter]
    public EventCallback OnClick { get; set; }

    [Inject]
    private IFactionProvider FactionProvider { get; set; } = default!;

    private bool IsActive => FactionProvider.CurrentFactionName == FactionName;
}
