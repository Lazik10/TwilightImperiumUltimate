namespace TwilightImperiumUltimate.Web.Components.Factions.Menu;

/// <summary>
/// Displays <see cref="FactionSource.TwilightsFall"/> factions using the shared non-official
/// source layout defined in <see cref="SourceFactionMenu"/>.
/// </summary>
public partial class TwilightsFallFactionMenu : TwilightImperiumBaseComponent
{
    [Parameter]
    [EditorRequired]
    public required IReadOnlyCollection<FactionModel> Factions { get; set; }

    [Parameter]
    public EventCallback<FactionModel> OnFactionClick { get; set; }
}
