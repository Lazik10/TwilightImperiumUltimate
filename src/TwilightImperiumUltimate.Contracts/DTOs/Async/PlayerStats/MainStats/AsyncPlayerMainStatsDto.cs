namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.MainStats;

public record AsyncPlayerMainStatsDto
{
    public AsyncPlayerMainStatsDto()
    {
        GamesCount = 0;
        Wins = 0;
        Eliminations = 0;
        Active = 0;
        Finished = 0;
    }

    public AsyncPlayerMainStatsDto(int numberOfGames, int wins, int eliminations, int active, int finished)
    {
        GamesCount = numberOfGames;
        Wins = wins;
        Eliminations = eliminations;
        Active = active;
        Finished = finished;
    }

    public int GamesCount { get; init; }

    public int Wins { get; init; }

    public int Eliminations { get; init; }

    public int Active { get; init; }

    public int Finished { get; init; }

    public float WinRate => Finished == 0 ? 0 : (float)Wins / Finished * 100;
}

