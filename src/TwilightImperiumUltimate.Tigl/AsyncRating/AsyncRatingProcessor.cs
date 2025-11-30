using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Services;

namespace TwilightImperiumUltimate.Tigl.AsyncRating;

public class AsyncRatingProcessor(
    ITiglUserRepository tiglUserRepository,
    ISeasonRepository seasonRepository,
    IAsyncPlayerMatchStatsService asyncPlayerMatchStatsService,
    IAsyncRatingCalculatorService asyncRatingCalculatorService,
    ITiglLeagueResolver tiglLeagueResolver)
    : IAsyncRatingProcessor
{
    public async Task<bool> ProcessGameReport(IGameReport report, int gameId, CancellationToken cancellationToken)
    {
        var players = await tiglUserRepository.GetUsersFromGameReport(report, cancellationToken);
        var league = tiglLeagueResolver.Resolve(report);
        var season = await seasonRepository.GetCurrentSeason(cancellationToken);
        var matchStats = await asyncPlayerMatchStatsService.InitializePlayerMatchStats(report, gameId, players, league);

        await asyncRatingCalculatorService.UpdatePlayerMatchStats(matchStats, season);
        await asyncRatingCalculatorService.UpdatePlayerRatings(players, matchStats, league);

        await tiglUserRepository.UpdateTiglPlayers(players, cancellationToken);
        await tiglUserRepository.InsertPlayerMatchStats(matchStats, cancellationToken);

        return true;
    }
}
