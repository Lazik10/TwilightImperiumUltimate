using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using Radzen;

namespace TwilightImperiumUltimate.Web.Components.TiglProfile;
public partial class TiglFactionStatisticsTable
{
    [Parameter] public IReadOnlyList<TiglProfileFactionStatsDto> Stats { get; set; } = [];
    [Parameter] public bool ShowFilters { get; set; }
    [Parameter] public bool AreFiltersVisible { get; set; }
    [Parameter] public string FilterButtonLabel { get; set; } = string.Empty;
    [Parameter] public string FilterIconPath { get; set; } = string.Empty;
    [Parameter] public EventCallback OnToggleFilters { get; set; }
    [Parameter] public RenderFragment? FilterContent { get; set; }

    private static void OnCellRender(DataGridCellRenderEventArgs<TiglProfileFactionStatsDto> args)
    {
        if (args.Data is not { Games: 0 })
            return;

        if (args.Column?.Property == nameof(TiglProfileFactionStatsDto.Games))
            args.Attributes["colspan"] = 4;
        else if (args.Column?.Property is nameof(TiglProfileFactionStatsDto.Wins)
            or nameof(TiglProfileFactionStatsDto.WinRate)
            or nameof(TiglProfileFactionStatsDto.AverageVp))
            args.Attributes["style"] = "display: none";
    }
}
