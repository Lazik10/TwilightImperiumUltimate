namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncGeneralStatsDto
{
    public AsyncGeneralStatsDto()
    {
    }

    public AsyncGeneralStatsDto(
        int games,
        int active,
        int cancelled,
        int finished,
        int eliminations,
        int players,
        int activePlayers,
        int inactivePlayers,
        int inactiveLessThanThreeMonths,
        int inactiveMoreThanThreeMonths,
        IReadOnlyCollection<GameDistributionByVpDto> distributionByVp,
        IReadOnlyCollection<PlayerDistributionByTimerDto> distributionByPlayerTimers,
        IReadOnlyCollection<GameDistributionByPlayerCountDto> distributionByPlayerCount,
        IReadOnlyCollection<GameDitributionByAverageTurnEndDto> distributionByAverageTurnEnd)
    {
        Games = games;
        Active = active;
        Cancelled = cancelled;
        Finished = finished;
        Eliminations = eliminations;
        Players = players;
        ActivePlayers = activePlayers;
        InactivePlayers = inactivePlayers;
        InactiveLessThanThreeMonths = inactiveLessThanThreeMonths;
        InactiveMoreThanThreeMonths = inactiveMoreThanThreeMonths;
        DistributionByPlayerTimers = distributionByPlayerTimers;
        DistributionByVp = distributionByVp;
        DistributionByPlayerCount = distributionByPlayerCount;
        DistributionByAverageTurnEnd = distributionByAverageTurnEnd;
    }

    public int Games { get; set; }

    public int Active { get; set; }

    public int Cancelled { get; set; }

    public int Finished { get; set; }

    public int Eliminations { get; set; }

    public int Players { get; set; }

    public int ActivePlayers { get; set; }

    public int InactivePlayers { get; set; }

    public int InactiveLessThanThreeMonths { get; set; }

    public int InactiveMoreThanThreeMonths { get; set; }

    public IReadOnlyCollection<GameDistributionByVpDto> DistributionByVp { get; set; } = new List<GameDistributionByVpDto>();

    public IReadOnlyCollection<PlayerDistributionByTimerDto> DistributionByPlayerTimers { get; set; } = new List<PlayerDistributionByTimerDto>();

    public IReadOnlyCollection<GameDistributionByPlayerCountDto> DistributionByPlayerCount { get; set; } = new List<GameDistributionByPlayerCountDto>();

    public IReadOnlyCollection<GameDitributionByAverageTurnEndDto> DistributionByAverageTurnEnd { get; set; } = new List<GameDitributionByAverageTurnEndDto>();
}
