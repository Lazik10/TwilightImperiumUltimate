using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Services;

namespace TwilightImperiumUltimate.Tigl.Glicko2Rating;

public class GlickoRatingProcessor(
    ITiglUserRepository tiglUserRepository,
    ISeasonRepository seasonRepository,
    IGlickoPlayerMatchStatsService glickoPlayerMatchStatsService,
    IGlickoRatingCalculatorService glickoRatingCalculatorService,
    ITiglLeagueResolver tiglLeagueResolver)
    : IGlickoRatingProcessor
{
    public async Task<bool> ProcessGameReport(IGameReport report, int gameId, CancellationToken cancellationToken)
    {
        var players = await tiglUserRepository.GetUsersFromGameReport(report, cancellationToken);
        var league = tiglLeagueResolver.Resolve(report);
        var season = await seasonRepository.GetCurrentSeason(cancellationToken);
        var matchStats = await glickoPlayerMatchStatsService.InitializePlayerMatchStats(report, gameId, players, league);

        await glickoRatingCalculatorService.UpdatePlayerMatchStats(matchStats, season);
        await glickoRatingCalculatorService.UpdatePlayerRatings(players, matchStats, league);

        await tiglUserRepository.UpdateTiglPlayers(players, cancellationToken);
        await tiglUserRepository.InsertPlayerMatchStats(matchStats, cancellationToken);

        return true;
    }
}
