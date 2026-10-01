namespace TwilightImperiumUltimate.Web.Components.Factions.Menu;

/// <summary>
/// Displays <see cref="FactionSource.BlueRiverie"/> factions using the shared non-official
/// source layout defined in <see cref="SourceFactionMenu"/>.
/// </summary>
public partial class BlueRiverieFactionMenu : TwilightImperiumBaseComponent
{
    [Parameter]
    [EditorRequired]
    public required IReadOnlyCollection<FactionModel> Factions { get; set; }

    [Parameter]
    public EventCallback<FactionModel> OnFactionClick { get; set; }
}
