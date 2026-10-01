using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Web.Components.Statistics.DataGrid;

public partial class DurationStatisticsTable
{
    [Parameter] public IReadOnlyCollection<AsyncDurationDto> Data { get; set; } = [];
    [Parameter] public string TableTitle { get; set; } = string.Empty;
    [Parameter] public string TableDescription { get; set; } = string.Empty;
    [Parameter] public bool IsLongest { get; set; }
    [Parameter] public bool IsLoading { get; set; }
    [Parameter] public EventCallback<string> GameClick { get; set; }

    private Task OnRowClick(AsyncDurationDto game) => GameClick.InvokeAsync(game.Id);

    private Task OnRowKeyDown(KeyboardEventArgs eventArgs, AsyncDurationDto game) =>
        eventArgs.Key is "Enter" or " " ? OnRowClick(game) : Task.CompletedTask;

    private static string GetDurationTime(long timestamp)
    {
        var duration = TimeSpan.FromSeconds(timestamp);
        var parts = new List<string>();

        if (duration.Days > 0)
            parts.Add($"{duration.Days:D2} d");
        if (duration.Hours > 0)
            parts.Add($"{duration.Hours:D2} h");
        if (duration.Minutes > 0)
            parts.Add($"{duration.Minutes:D2} m");

        return parts.Count > 0 ? string.Join(" ", parts) : "0 m";
    }

    private static string GetFunGameName(string? funName)
    {
        if (string.IsNullOrEmpty(funName) || funName.Length <= 20)
            return funName ?? string.Empty;

        return $"{funName[..20]}...";
    }
}