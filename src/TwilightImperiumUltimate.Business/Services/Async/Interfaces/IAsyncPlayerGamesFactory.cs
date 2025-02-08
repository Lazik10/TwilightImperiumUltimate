using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.GamesStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncPlayerGamesFactory
{
    Task<AsyncPlayerGamesSummaryDto> CreateAsyncPlayerGames(AsyncPlayerProfile playerProfile);
}
