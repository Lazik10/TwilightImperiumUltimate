using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
namespace TwilightImperiumUltimate.Web.Components.TiglProfile;
public sealed record TiglProfileSeasonSummary(int Season, IReadOnlyList<TiglProfileGameDto> Games)
{
    public int GamesPlayed => Games.Count;
    public int Wins => Games.Count(game => game.IsWinner);
    public double WinRate => GamesPlayed == 0 ? 0 : Wins / (double)GamesPlayed * 100;
}
