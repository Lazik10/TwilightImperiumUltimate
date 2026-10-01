using Microsoft.AspNetCore.Components;

namespace TwilightImperiumUltimate.Web.Components.Statistics.DataGrid;

public partial class GeneralGameStatisticsTable
{
    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public int Games { get; set; }

    [Parameter]
    public int Active { get; set; }

    [Parameter]
    public int Cancelled { get; set; }

    [Parameter]
    public int Finished { get; set; }

    [Parameter]
    public int Eliminations { get; set; }

    [Parameter]
    public bool IsLoading { get; set; }
}
