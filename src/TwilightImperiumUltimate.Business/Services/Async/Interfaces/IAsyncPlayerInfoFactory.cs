using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.PlayerInfo;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncPlayerInfoFactory
{
    Task<AsyncPlayerInfoDto> CreateAsyncPlayerInfoAsync(AsyncPlayerProfile playerProfile);
}
