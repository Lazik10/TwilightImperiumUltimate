namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncDurationDto
{
    public AsyncDurationDto()
    {
    }

    public AsyncDurationDto(
        string id,
        string funName,
        long duration,
        int scoreboard,
        int playerCount)
    {
        Id = id;
        FunName = funName;
        Duration = duration;
        Scoreboard = scoreboard;
        PlayerCount = playerCount;
    }

    public string Id { get; init; } = string.Empty;

    public string FunName { get; init; } = string.Empty;

    public long Duration { get; init; }

    public int Scoreboard { get; init; }

    public int PlayerCount { get; init; }
}
