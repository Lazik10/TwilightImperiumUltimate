namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class ResourceLinkRow
{
    [Parameter]
    [EditorRequired]
    public string Link { get; set; } = string.Empty;

    [Parameter]
    [EditorRequired]
    public string LinkText { get; set; } = string.Empty;

    [Parameter]
    [EditorRequired]
    public string DownloadName { get; set; } = string.Empty;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;
}
