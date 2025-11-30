using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Tigl.RankUp;

internal abstract class RankUpResolverBase(
    ITiglRepository tiglRepository,
    ITiglUserRepository tiglUserRepository,
    IRankRepository rankRepository,
    IPrestigeRankService prestigeRankService,
    ILeaderUpdateService leaderUpdateService)
    : IRankUpResolver
{
    protected int RankUpStep { get; set; } = 5;

    protected abstract int MaxPrestigeRanks { get; }

    protected abstract int MaxGalaxyThreatRanks { get; }

    protected ITiglUserRepository TiglRepository => tiglUserRepository;

    protected ITiglRepository TiglUserRepository => tiglRepository;

    protected IRankRepository RankRepository => rankRepository;

    protected ILeaderUpdateService LeaderUpdateService => leaderUpdateService;

    protected IPrestigeRankService PrestigeRankService => prestigeRankService;

    protected MatchReport MatchReport { get; set; } = default!;

    protected List<TiglUser> TiglUsers { get; set; } = [];

    protected Dictionary<int, TiglRankName> PreMatchRanks { get; set; } = [];

    protected TiglRankName LowestRank => GetLowestRank();

    public virtual Task ResolveRankUp(int gameId, TiglLeague league, CancellationToken cancellationToken) => Task.CompletedTask;

    protected static void AssignRankAndSetRankUpStatus(
        TiglUser player,
        AsyncPlayerMatchStats asyncPlayerMatchStats,
        GlickoPlayerMatchStats glickoPlayerMatchStats,
        TrueSkillPlayerMatchStats trueSkillPlayerMatchStats,
        TiglRankName rankBeforeGame,
        TiglRankName newRank,
        TiglLeague league,
        bool isRankUpGame,
        bool updatePlayerRank,
        bool forcedReset = false)
    {
        asyncPlayerMatchStats.IsRankUpGame = isRankUpGame;
        glickoPlayerMatchStats.IsRankUpGame = isRankUpGame;
        trueSkillPlayerMatchStats.IsRankUpGame = isRankUpGame;

        asyncPlayerMatchStats.OldRank = rankBeforeGame;
        asyncPlayerMatchStats.NewRank = newRank;
        asyncPlayerMatchStats.ForcedReset = forcedReset;

        glickoPlayerMatchStats.OldRank = rankBeforeGame;
        glickoPlayerMatchStats.NewRank = newRank;
        glickoPlayerMatchStats.ForcedReset = forcedReset;

        trueSkillPlayerMatchStats.OldRank = rankBeforeGame;
        trueSkillPlayerMatchStats.NewRank = newRank;
        trueSkillPlayerMatchStats.ForcedReset = forcedReset;

        if (updatePlayerRank)
        {
            if (league == TiglLeague.ProphecyOfKings)
                player.ProphecyOfKingsRank = newRank;
            else if (league == TiglLeague.ThundersEdge)
                player.ThundersEdgeRank = newRank;
            else if (league == TiglLeague.Fractured)
                player.ShatteredRank = newRank;
        }
    }

    protected async Task<bool> GameCanBeEvaluated(int gameId, CancellationToken cancellationToken)
        => await MatchExists(gameId, cancellationToken) && await IsRankUpEnabled(cancellationToken);

    protected TiglRankName GetLowestRank() => PreMatchRanks.Min(x => x.Value);

    protected async Task<bool> IsRankUpEnabled(CancellationToken cancellationToken)
    {
        var rankingParameter = await tiglRepository.GetTiglParameter(TiglParameterName.RankingUp, cancellationToken);
        return rankingParameter?.Enabled ?? false;
    }

    protected async Task InitializePlayers(CancellationToken cancellationToken)
    {
        foreach (var playerResult in MatchReport.PlayerResults)
        {
            var tiglUser = await tiglUserRepository.GetTiglUserById(playerResult.TiglUserId, MatchReport.League, cancellationToken);
            if (tiglUser is not null)
            {
                var result = await rankRepository.GetPreMatchRank(tiglUser.Id, MatchReport.StartTimestamp, MatchReport.League, cancellationToken);
                var preMatchRank = result.IsSuccess ? result.Value : TiglRankName.Unranked;

                PreMatchRanks.Add(tiglUser.Id, preMatchRank);
                TiglUsers.Add(tiglUser);
            }
        }
    }

    protected async Task EvaluatePrestigeRanks(int playerId, MatchReport matchReport, TiglFactionName faction, TiglLeague league, CancellationToken cancellationToken)
    {
        // Add Prestige rank for the faction player won with
        if (!await PrestigeRankService.HasFactionPrestigeRank(playerId, matchReport, faction, league, cancellationToken))
        {
            await PrestigeRankService.AddFactionPrestigeRank(playerId, matchReport, faction, league, cancellationToken);
        }

        // Get total prestige rank count for all unique factions
        var prestigeRankCount = await PrestigeRankService.GetFactionPrestigeRankCount(playerId, MatchReport, league, cancellationToken);

        var isGalacticRankUp = prestigeRankCount >= RankUpStep && prestigeRankCount % RankUpStep == 0;
        var rank = prestigeRankCount / RankUpStep;

        if (isGalacticRankUp && rank <= MaxGalaxyThreatRanks)
        {
            await PrestigeRankService.AddGalacticThreatRank(playerId, matchReport, rank, league, cancellationToken);
        }

        if (prestigeRankCount == MaxPrestigeRanks && rank >= MaxGalaxyThreatRanks)
        {
            await PrestigeRankService.AddPaxMagnificaBellumGloriosumRank(playerId, matchReport, league, cancellationToken);
        }
    }

    protected virtual TiglRankName GetNewRank(TiglRankName rankBeforeGame, TiglRankName currentRank)
    {
        var newRank = rankBeforeGame switch
        {
            TiglRankName.Unranked => TiglRankName.Minister,
            TiglRankName.Minister => TiglRankName.Agent,
            TiglRankName.Agent => TiglRankName.Commander,
            TiglRankName.Commander or TiglRankName.Hero => TiglRankName.Hero,
            _ => currentRank,
        };

        return newRank;
    }

    private async Task<bool> MatchExists(int gameId, CancellationToken cancellationToken)
    {
        var matchReport = await tiglRepository.GetMatchReportById(gameId, cancellationToken);
        if (matchReport == null)
            return false;

        MatchReport = matchReport;
        return true;
    }
}
