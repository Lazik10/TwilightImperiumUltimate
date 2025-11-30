using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Services;

namespace TwilightImperiumUltimate.Tigl.TrueSkillRating;

public class TrueSkillRatingProcessor(
    ITiglUserRepository tiglUserRepository,
    ISeasonRepository seasonRepository,
    ITrueSkillPlayerMatchStatsService trueSkillPlayerMatchStatsService,
    ITrueSkillRatingCalculatorService trueSkillRatingCalculatorService,
    ITiglLeagueResolver tiglLeagueResolver)
    : ITrueSkillRatingProcessor
{
    public async Task<bool> ProcessGameReport(IGameReport report, int gameId, CancellationToken cancellationToken)
    {
        var players = await tiglUserRepository.GetUsersFromGameReport(report, cancellationToken);
        var league = tiglLeagueResolver.Resolve(report);
        var season = await seasonRepository.GetCurrentSeason(cancellationToken);
        var matchStats = await trueSkillPlayerMatchStatsService.InitializePlayerMatchStats(report, gameId, players, league);

        await trueSkillRatingCalculatorService.UpdatePlayerMatchStats(matchStats, season);
        await trueSkillRatingCalculatorService.UpdatePlayerRatings(players, matchStats, league);

        await tiglUserRepository.UpdateTiglPlayers(players, cancellationToken);
        await tiglUserRepository.InsertPlayerMatchStats(matchStats, cancellationToken);

        return true;
    }
}
