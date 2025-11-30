using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.Tigl.RankUp;

public interface IPrestigeRankService
{
    Task<bool> HasFactionPrestigeRank(int userId, MatchReport matchReport, TiglFactionName faction, TiglLeague league, CancellationToken cancellationToken);

    Task<bool> HasGalacticThreatRank(int userId, MatchReport matchReport, int rank, TiglLeague league, CancellationToken cancellationToken);

    Task<int> AddFactionPrestigeRank(int userId, MatchReport matchReport, TiglFactionName faction, TiglLeague league, CancellationToken cancellationToken);

    Task<int> AddGalacticThreatRank(int userId, MatchReport matchReport, int rank, TiglLeague league, CancellationToken cancellationToken);

    Task<int> AddPaxMagnificaBellumGloriosumRank(int userId, MatchReport matchReport, TiglLeague league, CancellationToken cancellationToken);

    Task<int> AddImperiumRank(int userId, MatchReport matchReport, CancellationToken cancellationToken);

    Task<int> GetFactionPrestigeRankCount(int userId, MatchReport matchReport, TiglLeague league, CancellationToken cancellationToken);
}
