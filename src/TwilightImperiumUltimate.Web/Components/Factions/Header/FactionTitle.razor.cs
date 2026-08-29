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
    /// onto multiple lines. Defaults to <see langword="false"/> so long faction names shrink
    /// via the responsive font-size in FactionHeader.razor.css instead of overflowing past the
    /// surrounding faction icons.
    /// </summary>
    [Parameter]
    public bool NoWrap { get; set; }
}
