using TwilightImperiumUltimate.Contracts.ApiContracts.AsyncTI4;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class UpdateAsyncPlayerSettingsCommand : IRequest<bool>
{
    public UpdateAsyncPlayerSettingsCommand(AsyncPlayerSettingsRequestDto request)
    {
        PlayerDiscordId = request.PlayerDiscordId;
        PlayerProfileSettings = new AsyncPlayerProfileSettings
        {
            ExcludeFromAsyncStats = request.ExcludeFromAsyncStats,
            ShowWinRate = request.ShowWinRates,
            ShowTurnStats = request.ShowTurnStats,
            ShowCombatStats = request.ShowCombatStats,
            ShowVpStats = request.ShowVpStats,
            ShowFactionStats = request.ShowFactionStats,
            ShowOpponents = request.ShowOpponents,
            ShowGames = request.ShowGames,
        };
    }

    public AsyncPlayerProfileSettings PlayerProfileSettings { get; set; }

    public long PlayerDiscordId { get; set; }
}
