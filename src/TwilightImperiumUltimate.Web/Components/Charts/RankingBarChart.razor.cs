using System.Globalization;
using Microsoft.AspNetCore.Components;
using Radzen;
using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Charts;

/// <summary>
/// Reusable horizontal ranked bar chart built on <see cref="RadzenBarChart"/>. Renders one row per
/// <see cref="RankingBarPoint"/>, replacing the project's earlier progress-bar/table pattern for
/// ranked statistics lists (e.g. top players, top factions, top games).
/// </summary>
public partial class RankingBarChart
{
    [Parameter]
    [EditorRequired]
    public IReadOnlyCollection<RankingBarPoint> Data { get; set; } = [];

    [Parameter]
    public string YAxisLabel { get; set; } = string.Empty;

    [Parameter]
    public string XAxisLabel { get; set; } = string.Empty;

    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public string Subtitle { get; set; } = string.Empty;

    [Parameter]
    public string SeriesTitle { get; set; } = string.Empty;

    [Parameter]
    public bool ShowLegend { get; set; }

    [Parameter]
    public bool ShowValueAxis { get; set; } = true;

    [Parameter]
    public bool IsLoading { get; set; }

    [Parameter]
    public int? ChartHeight { get; set; }

    [Parameter]
    public Func<object, string>? CategoryFormatter { get; set; }

    [Parameter]
    public double? ValueAxisStep { get; set; }

    [Parameter]
    public double? ValueAxisMin { get; set; } = 0;

    [Parameter]
    public double? ValueAxisMax { get; set; }

    [Parameter]
    public Func<object, string>? ValueAxisFormatter { get; set; }

    [Parameter]
    public string BarColor { get; set; } = "#5aa9e6";

    [Parameter]
    public IReadOnlyCollection<string>? BarColors { get; set; }

    /// <summary>Gets or sets a formatter converting each point's raw value into its data-label text.</summary>
    [Parameter]
    public Func<double, string>? ValueFormatter { get; set; }

    /// <summary>Gets or sets the callback invoked with a clicked point's <see cref="RankingBarPoint.Tag"/>.</summary>
    [Parameter]
    public EventCallback<object?> TagClick { get; set; }

    private int EffectiveChartHeight => ChartHeight ?? Math.Max(220, 60 + (Data.Count * 28));

    private double AxisStep => ValueAxisStep ?? CalculateAxisStep();

    private IReadOnlyCollection<string> GetPrivateCategoryLabels() => Data
        .Where(point => point.IsPrivateProfile)
        .Select(point => point.Label)
        .ToList();

    private IReadOnlyCollection<string> GetFills()
    {
        if (BarColors is { Count: > 0 })
            return BarColors;

        return Enumerable.Repeat(BarColor, Data.Count).ToList();
    }

    private double CalculateAxisStep()
    {
        static double GetNiceStep(double normalizedStep)
        {
            if (normalizedStep <= 1)
                return 1;
            if (normalizedStep <= 2)
                return 2;
            if (normalizedStep <= 5)
                return 5;

            return 10;
        }

        var maximum = Data.Count == 0 ? 0 : Data.Max(point => point.Value);
        if (maximum <= 0)
            return 1;

        var rawStep = maximum / 5;
        var magnitude = Math.Pow(10, Math.Floor(Math.Log10(rawStep)));
        var normalizedStep = rawStep / magnitude;
        var niceStep = GetNiceStep(normalizedStep);

        return niceStep * magnitude;
    }

    private string FormatCategory(object value)
    {
        if (CategoryFormatter is not null)
            return CategoryFormatter(value);

        var key = value?.ToString() ?? string.Empty;
        return Data.FirstOrDefault(point => point.CategoryKey == key)?.Label ?? key;
    }

    private string FormatValue(object value)
    {
        var numeric = Convert.ToDouble(value, CultureInfo.InvariantCulture);
        return ValueFormatter?.Invoke(numeric) ?? numeric.ToString(CultureInfo.InvariantCulture);
    }

    private Task OnSeriesClick(SeriesClickEventArgs args)
    {
        return args.Data is RankingBarPoint point && TagClick.HasDelegate
            ? TagClick.InvokeAsync(point.Tag)
            : Task.CompletedTask;
    }
}
