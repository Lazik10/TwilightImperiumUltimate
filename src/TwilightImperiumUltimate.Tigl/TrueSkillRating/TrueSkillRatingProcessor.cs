using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Services;

namespace TwilightImperiumUltimate.Tigl.TrueSkillRating;

public class TrueSkillRatingProcessor(
    ITiglRepository tiglRepository,
    ITrueSkillPlayerMatchStatsService trueSkillPlayerMatchStatsService,
    ITrueSkillRatingCalculatorService trueSkillRatingCalculatorService,
    ITiglLeagueResolver tiglLeagueResolver,
    ITiglRankUpResolver tiglRankUpResolver)
    : ITrueSkillRatingProcessor
{
    public async Task<bool> ProcessGameReport(IGameReport report, int gameId, CancellationToken cancellationToken)
    {
        var players = await tiglRepository.GetUsersFromGameReport(report, cancellationToken);
        var league = tiglLeagueResolver.Resolve(report);
        var season = await tiglRepository.GetCurrentSeason(cancellationToken);
        var matchStats = await trueSkillPlayerMatchStatsService.InitializePlayerMatchStats(report, gameId, players, league);

        await trueSkillRatingCalculatorService.UpdatePlayerMatchStats(matchStats, season);
        await trueSkillRatingCalculatorService.UpdatePlayerRatings(players, matchStats, league);

        if (report.Source == ResultSource.Async)
            await tiglRankUpResolver.ResolveRankUpAsync(players, matchStats, league, cancellationToken);

        await tiglRepository.UpdateTiglPlayers(players, cancellationToken);
        await tiglRepository.InsertPlayerMatchStats(matchStats, cancellationToken);

        return true;
    }
}
