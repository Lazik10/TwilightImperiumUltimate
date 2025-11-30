using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Extensions;

namespace TwilightImperiumUltimate.Tigl.RankUp;

internal class TiglFracturedRankUpResolver : RankUpResolverBase, ITiglRankUpResolver
{
    public TiglFracturedRankUpResolver(
        ITiglRepository tiglRepository,
        ITiglUserRepository tiglUserRepository,
        IRankRepository rankRepository,
        IPrestigeRankService prestigeRankService,
        ILeaderUpdateService leaderUpdateService)
        : base(tiglRepository, tiglUserRepository, rankRepository, prestigeRankService, leaderUpdateService)
    {
    }

    protected override int MaxPrestigeRanks => 80;

    protected override int MaxGalaxyThreatRanks => 0;

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
                    newRank = GetNewRank(rankBeforeGame, player.ShatteredRank);

                    if (newRank > player.ShatteredRank || player.ShatteredRank == TiglRankName.Archon)
                    {
                        isRankUpGame = true;
                        updatePlayerRank = true;
                    }
                    else
                    {
                        // If current rank is higher or equal to the new rank, set the pregame rank as new rank in player match stats
                        isRankUpGame = false;
                        newRank = rankBeforeGame;
                        updatePlayerRank = false;
                    }

                    // If player is already at Archon rank reset rank to Unranked
                    if (player.ShatteredRank == TiglRankName.Archon)
                    {
                        forcedReset = true;
                        await PrestigeRankService.AddImperiumRank(player.Id, MatchReport, cancellationToken);
                    }

                    if (isRankUpGame)
                    {
                        if (!await PrestigeRankService.HasFactionPrestigeRank(player.Id, MatchReport, faction, MatchReport.League, cancellationToken))
                        {
                            await PrestigeRankService.AddFactionPrestigeRank(player.Id, MatchReport, faction, MatchReport.League, cancellationToken);
                        }

                        await LeaderUpdateService.UpdateLeader(player, MatchReport, faction, cancellationToken);
                    }
                }

                AssignRankAndSetRankUpStatus(player, asyncStats, glickoStats, trueSkillStats, rankBeforeGame, newRank, MatchReport.League, isRankUpGame, updatePlayerRank, forcedReset);

                // Add rank or add unranked rank if player was forced to reset
                if (rankBeforeGame < newRank || forcedReset)
                {
                    if (forcedReset)
                    {
                        newRank = TiglRankName.Unranked;
                        player.ShatteredRank = TiglRankName.Unranked;
                    }

                    await RankRepository.AddRank(player.Id, newRank, MatchReport.League, MatchReport.EndTimestamp, MatchReport.Id, cancellationToken);
                }

                await RankRepository.UpdateUserAndMatchStats(MatchReport, player, asyncStats, glickoStats, trueSkillStats, cancellationToken);
            }
        }
    }

    protected override TiglRankName GetNewRank(TiglRankName rankBeforeGame, TiglRankName currentRank)
    {
        var newRank = rankBeforeGame switch
        {
            TiglRankName.Unranked => TiglRankName.Thrall,
            TiglRankName.Thrall => TiglRankName.Acolyte,
            TiglRankName.Acolyte => TiglRankName.Legionnaire,
            TiglRankName.Legionnaire => TiglRankName.Starlancer,
            TiglRankName.Starlancer => TiglRankName.GeneSorcerer,
            TiglRankName.GeneSorcerer => TiglRankName.IxthLord,
            TiglRankName.IxthLord or TiglRankName.Archon => TiglRankName.Archon,
            _ => currentRank,
        };

        return newRank;
    }
}
