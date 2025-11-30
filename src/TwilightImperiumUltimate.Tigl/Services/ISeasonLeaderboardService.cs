namespace TwilightImperiumUltimate.Tigl.Services;

public interface ISeasonLeaderboardService
{
    Task CreateLeaderboard(int seasonEnded, CancellationToken cancellationToken);
}
