using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.AutoMapper;

internal class AsyncProfile : Profile
{
    public AsyncProfile()
    {
        CreateMap<AsyncPlayerProfile, AsyncPlayerProfileDto>()
            .ConstructUsing(x => new AsyncPlayerProfileDto(
                x.Id,
                x.DiscordUserName));

        CreateMap<AsyncPlayerProfileSettings, AsyncPlayerProfileSettingsDto>()
            .ConstructUsing(x => new AsyncPlayerProfileSettingsDto(
                x.ExcludeFromAsyncStats,
                x.ShowWinRate,
                x.ShowTurnStats,
                x.ShowCombatStats,
                x.ShowVpStats,
                x.ShowFactionStats,
                x.ShowOpponents,
                x.ShowGames));

        CreateMap<GameStats, AsyncGameDto>()
            .ConstructUsing(x => new AsyncGameDto(
                x.Id,
                x.AsyncGameID,
                x.AsyncFunGameName,
                x.SetupTimestamp,
                x.EndedTimestamp ?? 0,
                x.EndedTimestamp != null,
                x.HasWinner,
                x.NumberOfPlayers,
                x.Round,
                x.Scoreboard,
                x.IsTigl));
    }
}
