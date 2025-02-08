using System.Globalization;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.FactionStats;

public record AsyncPlayerFactionStatsDto
{
    public AsyncPlayerFactionStatsDto()
    {
    }

    public AsyncPlayerFactionStatsDto(AsyncFactionName factionName)
    {
        FactionName = factionName;
    }

    public AsyncFactionName FactionName { get; set; }

    public AsyncPlayerFactionStatsByGameVp All { get; set; } = new();

    public AsyncPlayerFactionStatsByGameVp TenVp { get; set; } = new();

    public AsyncPlayerFactionStatsByGameVp TwelveVp { get; set; } = new();

    public AsyncPlayerFactionStatsByGameVp FourteenVp { get; set; } = new();
}