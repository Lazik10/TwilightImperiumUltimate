namespace TwilightImperiumUltimate.Web.Components.Factions.Header;

public partial class FactionTitle
{
    [Parameter]
    [EditorRequired]
    public FactionName FactionName { get; set; } = default!;

    [Parameter]
    [EditorRequired]
    public int Width { get; set; } = 100;
}
