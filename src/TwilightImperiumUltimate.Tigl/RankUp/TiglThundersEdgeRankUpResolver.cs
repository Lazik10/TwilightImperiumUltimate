using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Extensions;

namespace TwilightImperiumUltimate.Tigl.RankUp;

internal class TiglThundersEdgeRankUpResolver : RankUpResolverBase, ITiglRankUpResolver
{
    public TiglThundersEdgeRankUpResolver(
        ITiglRepository tiglRepository,
        ITiglUserRepository tiglUserRepository,
        IRankRepository rankRepository,
        IPrestigeRankService prestigeRankService,
        ILeaderUpdateService leaderUpdateService)
        : base(tiglRepository, tiglUserRepository, rankRepository, prestigeRankService, leaderUpdateService)
    {
    }

    protected override int MaxPrestigeRanks => 32;

    protected override int MaxGalaxyThreatRanks => 5;

    public virtual async Task ResolveRankUpAsync(int gameId, CancellationToken cancellationToken)
    {
        if (!await GameCanBeEvaluated(gameId, cancellationToken))
            return;

        await InitializePlayers(cancellationToken);

        foreach (var player in TiglUsers)
        {
            var playerMatchStats = player.GetPlayerGameMatchStats(MatchReport.Id, MatchReport.League);
            if (playerMatchStats.Valid)
            {
                var asyncStats = playerMatchStats.AsyncStats!;
                var glickoStats = playerMatchStats.GlickoStats!;
                var trueSkillStats = playerMatchStats.TrueSkillStats!;

                var isWinner = asyncStats.Winner && glickoStats.Winner && trueSkillStats.Winner;
                var faction = MatchReport.PlayerResults.First(x => x.TiglUserId == player.Id).Faction;

                var rankBeforeGame = PreMatchRanks.TryGetValue(player.Id, out var oldRank) ? oldRank : TiglRankName.Unranked;
                var currentRankResult = await RankRepository.GetCurrentRank(player.Id, MatchReport.League, cancellationToken);
                var currentRank = currentRankResult.Value!;
                var newRank = rankBeforeGame;
                var isRankUpGame = false;
                var updatePlayerRank = false;
                var forcedReset = false;

                // If current rank was achieved after the game started, do not evaluate rank up
                // beacause player has already ranked up in another game
                if (currentRank.AchievedAt <= MatchReport.StartTimestamp
                    && rankBeforeGame <= LowestRank && isWinner && await IsRankUpEnabled(cancellationToken))
                {
                    // Get next rank based on the rank before the game, not the current rank
                    newRank = GetNewRank(rankBeforeGame, player.ThundersEdgeRank);

                    // Steal leader from current holder
                    if (newRank == TiglRankName.Hero || player.ThundersEdgeRank == TiglRankName.Hero)
                        await LeaderUpdateService.UpdateLeader(player, MatchReport, faction, cancellationToken);

                    if (newRank > player.ThundersEdgeRank)
                    {
                        isRankUpGame = true;
                        updatePlayerRank = true;
                    }
                    else
                    {
                        // If current rank is higher than new rank, set the pregame rank as new rank in player match stats
                        // It means player has ranked up in a previous game this season
                        isRankUpGame = false;
                        newRank = rankBeforeGame;
                        updatePlayerRank = false;
                    }

                    // If player is already at Hero rank, evaluate prestige ranks
                    if (player.ThundersEdgeRank == TiglRankName.Hero)
                    {
                        await EvaluatePrestigeRanks(player.Id, MatchReport, faction, MatchReport.League, cancellationToken);

                        // Handle rank reset if player is at Hero rank and collected enough prestige ranks
                        forcedReset = await ShouldResetRank(player, MatchReport, cancellationToken);
                        if (forcedReset)
                            isRankUpGame = true;
                    }
                }

                AssignRankAndSetRankUpStatus(player, asyncStats, glickoStats, trueSkillStats, rankBeforeGame, newRank, MatchReport.League, isRankUpGame, updatePlayerRank, forcedReset);

                // Add rank or add unranked rank if player was forced to reset
                if (rankBeforeGame < newRank || forcedReset)
                {
                    if (forcedReset)
                    {
                        newRank = TiglRankName.Unranked;
                        player.ThundersEdgeRank = TiglRankName.Unranked;
                    }

                    await RankRepository.AddRank(player.Id, newRank, MatchReport.League, MatchReport.EndTimestamp, MatchReport.Id, cancellationToken);
                }

                await RankRepository.UpdateUserAndMatchStats(MatchReport, player, asyncStats, glickoStats, trueSkillStats, cancellationToken);
            }
        }
    }

    private async Task<bool> ShouldResetRank(TiglUser player, MatchReport matchReport, CancellationToken cancellationToken)
    {
        var prestigeRanksCount = await PrestigeRankService.GetFactionPrestigeRankCount(player.Id, matchReport, MatchReport.League, cancellationToken);

        return prestigeRanksCount >= RankUpStep && prestigeRanksCount % RankUpStep == 0 && prestigeRanksCount < MaxGalaxyThreatRanks * RankUpStep;
    }
}
