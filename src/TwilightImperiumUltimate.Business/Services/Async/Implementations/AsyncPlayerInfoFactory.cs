using TwilightImperiumUltimate.Business.Helpers;
using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.PlayerInfo;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncPlayerInfoFactory : IAsyncPlayerInfoFactory
{
    public Task<AsyncPlayerInfoDto> CreateAsyncPlayerInfoAsync(AsyncPlayerProfile playerProfile)
    {
        var username = string.Empty;

        if (playerProfile is not null && playerProfile.ProfileSettings is not null && !playerProfile.ProfileSettings.ExcludeFromAsyncStats)
            username = playerProfile.DiscordUserName;
        else
            username = StringConstants.PrivateProfile;

        return Task.FromResult(new AsyncPlayerInfoDto(playerProfile?.Id ?? 0, username));
    }
}
