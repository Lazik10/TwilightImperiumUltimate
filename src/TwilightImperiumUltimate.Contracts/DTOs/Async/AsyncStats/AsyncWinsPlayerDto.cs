namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncWinsPlayerDto(
    int Id,
    string UserName,
    int Wins,
    int Games)
{
    public float WinPercentage => Games == 0 ? 0 : (float)Wins / Games * 100;

    public double WinDeviation => CalculatePercentageDeviation();

    private double CalculatePercentageDeviation()
    {
        double winProbability = 1.0 / 6.0;
        double expectedWins = Games * winProbability;
        double winDeviation = Wins - expectedWins;
        const double epsilon = 1e-10;

        return Math.Abs(expectedWins) < epsilon ? 0 : (winDeviation / expectedWins) * 100;
    }
}
