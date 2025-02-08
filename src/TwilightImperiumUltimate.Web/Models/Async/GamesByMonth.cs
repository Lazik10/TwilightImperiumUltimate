using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.GamesStats;

namespace TwilightImperiumUltimate.Web.Models.Async;

public class GamesByMonth
{
    public DateOnly Month { get; set; }

    public IEnumerable<AsyncPlayerGameDto> Games { get; set; } = new List<AsyncPlayerGameDto>();

    public IEnumerable<AsyncGameDto> AllGames { get; set; } = new List<AsyncGameDto>();
}
