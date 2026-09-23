using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
namespace TwilightImperiumUltimate.Web.Components.TiglProfile;
public partial class TiglFactionStatisticsTable
{
    [Parameter] public IReadOnlyList<TiglProfileFactionStatsDto> Stats { get; set; } = [];
}
