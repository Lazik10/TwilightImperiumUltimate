using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncFactionsStatsDto
{
    public AsyncFactionsStatsDto()
    {
    }

    public AsyncFactionsStatsDto(
        AsyncFactionName factionName,
        AsyncFactionStatsByGameVpDto all,
        AsyncFactionStatsByGameVpDto tenVp,
        AsyncFactionStatsByGameVpDto twelveVp,
        AsyncFactionStatsByGameVpDto fourteenVp)
    {
        FactionName = factionName;
        All = all;
        TenVp = tenVp;
        TwelveVp = twelveVp;
        FourteenVp = fourteenVp;
    }

    public AsyncFactionName FactionName { get; set; }

    public AsyncFactionStatsByGameVpDto All { get; set; } = new();

    public AsyncFactionStatsByGameVpDto TenVp { get; set; } = new();

    public AsyncFactionStatsByGameVpDto TwelveVp { get; set; } = new();

    public AsyncFactionStatsByGameVpDto FourteenVp { get; set; } = new();
}
