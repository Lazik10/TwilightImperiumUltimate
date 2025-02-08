using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.GamesStats;

public record AsyncPlayerGameDto
{
    public AsyncPlayerGameDto(
        string asyncGameID,
        string gameFunName,
        int scoreboard,
        int playerScore,
        long startDate,
        long endDate,
        bool ended,
        bool validEnd,
        bool isTigl,
        int round,
        bool isWinner,
        AsyncFactionName faction)
    {
        AsyncGameID = asyncGameID;
        GameFunName = gameFunName;
        Scoreboard = scoreboard;
        PlayerScore = playerScore;
        StartDate = startDate;
        EndDate = endDate;
        Ended = ended;
        ValidEnd = validEnd;
        IsTigl = isTigl;
        Round = round;
        IsWinner = isWinner;
        Faction = faction;
    }

    public string AsyncGameID { get; init; } = string.Empty;

    public string GameFunName { get; init; } = string.Empty;

    public int Scoreboard { get; init; }

    public int PlayerScore { get; init; }

    public long StartDate { get; init; }

    public long EndDate { get; init; }

    public bool Ended { get; init; }

    public bool ValidEnd { get; init; }

    public bool IsTigl { get; init; }

    public int Round { get; init; }

    public bool IsWinner { get; init; }

    public AsyncFactionName Faction { get; init; }
}
