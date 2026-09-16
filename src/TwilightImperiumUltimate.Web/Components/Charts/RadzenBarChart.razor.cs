using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace TwilightImperiumUltimate.Web.Components.Charts;

public partial class RadzenBarChart : IAsyncDisposable
{
    private ElementReference _chartElement;
    private IJSObjectReference? _jsModule;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string CategoryTitle { get; set; } = string.Empty;

    [Parameter]
    public string ValueTitle { get; set; } = string.Empty;

    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public string Subtitle { get; set; } = string.Empty;

    [Parameter]
    public bool ShowLegend { get; set; }

    [Parameter]
    public bool ShowValueAxis { get; set; } = true;

    [Parameter]
    public bool IsLoading { get; set; }

    [Parameter]
    public double? ValueAxisStep { get; set; }

    [Parameter]
    public double? ValueAxisMin { get; set; } = 0;

    [Parameter]
    public double? ValueAxisMax { get; set; }

    [Parameter]
    public Func<object, string>? ValueAxisFormatter { get; set; }

    [Parameter]
    public int ChartHeight { get; set; } = 520;

    /// <summary>
    /// Gets or sets a formatter converting each data point's raw (unique) category value into
    /// the text shown on the category axis, letting callers keep category values unique for
    /// Radzen's category scale while still displaying a shorter label (e.g. just a month name).
    /// </summary>
    [Parameter]
    public Func<object, string>? CategoryFormatter { get; set; }

    [Parameter]
    public IReadOnlyCollection<string> PrivateCategoryLabels { get; set; } = [];

    [Parameter]
    public EventCallback<SeriesClickEventArgs> SeriesClick { get; set; }

    private string ChartHeightPx => $"{ChartHeight}px";

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private string ChartStyle => $"height: {ChartHeight}px; width: 100%;";

    public async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("disposePrivateCategories", _chartElement);
            await _jsModule.DisposeAsync();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Charts/RadzenBarChart.razor.js");

        if (_jsModule is not null)
            await _jsModule.InvokeVoidAsync("observePrivateCategories", _chartElement, PrivateCategoryLabels);
    }
}
