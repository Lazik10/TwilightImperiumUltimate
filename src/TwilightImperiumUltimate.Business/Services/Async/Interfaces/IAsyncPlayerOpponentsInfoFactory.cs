using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.Opponents;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncPlayerOpponentsInfoFactory
{
    Task<AsyncPlayerOpponentsSummaryDto> CreateAsyncPlayerOpponentsInfo(AsyncPlayerProfile playerProfile);
}
