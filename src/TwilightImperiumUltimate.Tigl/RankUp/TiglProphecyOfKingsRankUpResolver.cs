using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Extensions;

namespace TwilightImperiumUltimate.Tigl.RankUp;

internal class TiglProphecyOfKingsRankUpResolver : RankUpResolverBase, ITiglRankUpResolver
{
    public TiglProphecyOfKingsRankUpResolver(
        ITiglRepository tiglRepository,
        ITiglUserRepository tiglUserRepository,
        IRankRepository rankRepository,
        IPrestigeRankService prestigeRankService,
        ILeaderUpdateService leaderUpdateService)
        : base(tiglRepository, tiglUserRepository, rankRepository, prestigeRankService, leaderUpdateService)
    {
    }

    protected override int MaxPrestigeRanks => 27;

    protected override int MaxGalaxyThreatRanks => 4;

    public virtual async Task ResolveRankUpAsync(int gameId, CancellationToken cancellationToken)
    {
        if (!await GameCanBeEvaluated(gameId, cancellationToken))
            return;

        await InitializePlayers(cancellationToken);

        foreach (var player in TiglUsers)
        {
            var playerMatchStats = player.GetPlayerGameMatchStats(MatchReport.Id, MatchReport.League);
            var faction = MatchReport.PlayerResults.First(x => x.TiglUserId == player.Id).Faction;

            if (playerMatchStats.Valid)
            {
                var asyncStats = playerMatchStats.AsyncStats!;
                var glickoStats = playerMatchStats.GlickoStats!;
                var trueSkillStats = playerMatchStats.TrueSkillStats!;

                var isWinner = asyncStats.Winner && glickoStats.Winner && trueSkillStats.Winner;

                var rankBeforeGame = PreMatchRanks.TryGetValue(player.Id, out var oldRank) ? oldRank : TiglRankName.Unranked;
                var newRank = rankBeforeGame;
                var isRankUpGame = false;
                var updatePlayerRank = false;

                if (rankBeforeGame <= LowestRank && isWinner && await IsRankUpEnabled(cancellationToken))
                {
                    newRank = GetNewRank(rankBeforeGame, player.ProphecyOfKingsRank);

                    if (newRank > player.ProphecyOfKingsRank)
                    {
                        isRankUpGame = true;
                        updatePlayerRank = true;
                    }
                    else
                    {
                        // If current rank is higher than new rank, set the pregame rank as new rank in player match stats
                        // It means player has ranked up in a previous game this session
                        isRankUpGame = false;
                        newRank = rankBeforeGame;
                        updatePlayerRank = false;
                    }

                    // If player reached Hero rank or is already at Hero rank, evaluate prestige ranks
                    if (newRank == TiglRankName.Hero || player.ProphecyOfKingsRank == TiglRankName.Hero)
                    {
                        await EvaluatePrestigeRanks(player.Id, MatchReport, faction, MatchReport.League, cancellationToken);
                        await LeaderUpdateService.UpdateLeader(player, MatchReport, faction, cancellationToken);
                    }
                }

                AssignRankAndSetRankUpStatus(player, asyncStats, glickoStats, trueSkillStats, rankBeforeGame, newRank, MatchReport.League, isRankUpGame, updatePlayerRank);

                if (rankBeforeGame < newRank)
                {
                    await RankRepository.AddRank(player.Id, newRank, MatchReport.League, MatchReport.EndTimestamp, MatchReport.Id, cancellationToken);
                }

                await RankRepository.UpdateUserAndMatchStats(MatchReport, player, asyncStats, glickoStats, trueSkillStats, cancellationToken);
            }
        }
    }
}
