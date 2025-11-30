using FluentResults;
using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface ISeasonRepository
{
    Task<bool> CreateNewSeason(CancellationToken cancellationToken);

    Task<Result<Season>> AddNewSeason(int seasonNumber, string seasonName, CancellationToken cancellationToken);

    Task<int> GetCurrentSeason(CancellationToken cancellationToken);

    Task<Result<Season>> SetActiveSeason(int seasonNumber, CancellationToken cancellationToken);

    Task<Result<Season>> EndCurrentSeason(CancellationToken cancellationToken);

    Task<MatchReport?> GetFastestGameInSeason(int season, TiglLeague league, CancellationToken cancellationToken);

    Task<MatchReport?> GetSlowestGameInSeason(int season, TiglLeague league, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Season>> GetAllSeasons(CancellationToken cancellationToken);

    Task<Season?> GetSeasonByNumber(int seasonNumber, CancellationToken cancellationToken);

    Task<bool> UpdateSeason(Season season, CancellationToken cancellationToken);
}
