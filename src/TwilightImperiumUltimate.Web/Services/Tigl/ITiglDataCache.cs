using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Services.Tigl;

public interface ITiglDataCache
{
    IReadOnlyList<PlayerSeasonResultDto>? GetLeaderboard(int seasonNumber);

    void UpdateLeaderboard(int seasonNumber, IReadOnlyList<PlayerSeasonResultDto> data);

    IReadOnlyList<FactionSeasonStatsDto>? GetFactionStats(int season, TiglLeague league);

    void UpdateFactionStats(int season, TiglLeague league, IReadOnlyList<FactionSeasonStatsDto> data);
}
