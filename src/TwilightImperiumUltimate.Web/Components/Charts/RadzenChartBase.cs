namespace TwilightImperiumUltimate.Web.Components.Charts;

public abstract class RadzenChartBase : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string CategoryTitle { get; set; } = string.Empty;

    [Parameter]
    public string ValueTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional numeric format for value-axis labels.
    /// </summary>
    [Parameter]
    public string? ValueFormatString { get; set; }

    [Parameter]
    public double Min { get; set; }

    [Parameter]
    public double Max { get; set; }

    [Parameter]
    public double Step { get; set; }
}
