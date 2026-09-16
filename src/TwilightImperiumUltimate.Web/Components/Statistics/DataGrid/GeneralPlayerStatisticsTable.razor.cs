using Microsoft.AspNetCore.Components;

namespace TwilightImperiumUltimate.Web.Components.Statistics.DataGrid;

public partial class GeneralPlayerStatisticsTable
{
    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public int Players { get; set; }

    [Parameter]
    public int ActivePlayers { get; set; }

    [Parameter]
    public int InactivePlayers { get; set; }

    [Parameter]
    public int InactiveLessThanThreeMonths { get; set; }

    [Parameter]
    public int InactiveMoreThanThreeMonths { get; set; }

    [Parameter]
    public bool IsLoading { get; set; }
}
