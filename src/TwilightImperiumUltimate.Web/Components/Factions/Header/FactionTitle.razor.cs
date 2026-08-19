namespace TwilightImperiumUltimate.Web.Components.Factions.Header;

public partial class FactionTitle
{
    [Parameter]
    [EditorRequired]
    public FactionName FactionName { get; set; } = default!;

    [Parameter]
    [EditorRequired]
    public int Width { get; set; } = 100;

    /// <summary>
    /// Gets or sets a value indicating whether the title text is prevented from wrapping
    /// onto multiple lines.
    /// </summary>
    [Parameter]
    public bool NoWrap { get; set; } = true;
}
