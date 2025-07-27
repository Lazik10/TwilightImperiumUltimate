using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Helpers;

namespace TwilightImperiumUltimate.Tigl.Services;

internal class TiglRankUpResolver(
    ITiglRepository tiglRepository)
    : ITiglRankUpResolver
{
    public async Task ResolveRankUpAsync(IReadOnlyCollection<TiglUser> tiglUsers, IReadOnlyCollection<AsyncPlayerMatchStats> matchStats, TiglLeague league, CancellationToken cancellationToken)
    {
        var rankingParameter = await tiglRepository.GetTiglParameter(TiglParameterName.RankingUp, cancellationToken);
        var allowRankingUp = rankingParameter?.Enabled ?? false;

        var lowestRank = tiglUsers.Select(x => x.GetCorrectAsyncStats(league)).Min(x => x?.Rank ?? TiglAsyncRank.Unranked);

        foreach (var player in tiglUsers)
        {
            var asyncStats = player.GetCorrectAsyncStats(league);
            var playerStats = matchStats.FirstOrDefault(x => x.DiscordUserId == player.DiscordId);

            if (asyncStats is not null && playerStats is not null)
            {
                var playerRank = asyncStats.Rank;

                playerStats.OldRank = playerRank;
                playerStats.NewRank = playerRank;

                if (playerRank == lowestRank && playerStats.Winner && allowRankingUp)
                {
                    var isRankUpGame = true;
                    var newRank = playerRank;

                    if (lowestRank < TiglAsyncRank.Commander)
                    {
                        var enumValues = Enum.GetValues<TiglAsyncRank>().ToList();
                        var currentIndex = enumValues.IndexOf(playerRank);

                        if (currentIndex < enumValues.Count - 1)
                        {
                            newRank = enumValues[currentIndex + 1];
                        }
                    }
                    else if (lowestRank == TiglAsyncRank.Commander)
                    {
                        newRank = TiglAsyncRank.Hero;
                    }
                    else if (lowestRank >= TiglAsyncRank.Hero)
                    {
                        var heroStats = asyncStats.MatchStats
                            .Where(x => x.NewRank >= TiglAsyncRank.Hero)
                            .ToList();

                        var heroCount = heroStats.DistinctBy(x => x.Faction).Count();

                        newRank = heroCount switch
                        {
                            < 5 => TiglAsyncRank.Hero,
                            < 10 => TiglAsyncRank.GalacticThreatOne,
                            < 15 => TiglAsyncRank.GalacticThreatTwo,
                            < 20 => TiglAsyncRank.GalacticThreatThree,
                            < 25 => TiglAsyncRank.GalacticThreatFour,
                            _ => TiglAsyncRank.GalacticThreatFive,
                        };
                    }

                    playerStats.IsRankUpGame = isRankUpGame;
                    asyncStats.Rank = newRank;
                    playerStats.NewRank = newRank;
                }
            }
        }
    }

    public async Task ResolveRankUpAsync(IReadOnlyCollection<TiglUser> tiglUsers, IReadOnlyCollection<GlickoPlayerMatchStats> matchStats, TiglLeague league, CancellationToken cancellationToken)
    {
        var rankingParameter = await tiglRepository.GetTiglParameter(TiglParameterName.RankingUp, cancellationToken);
        var allowRankingUp = rankingParameter?.Enabled ?? false;

        var lowestRank = tiglUsers.Select(x => x.GetCorrectGlickoStats(league)).Min(x => x?.Rank ?? TiglAsyncRank.Unranked);

        foreach (var player in tiglUsers)
        {
            var glickoStats = player.GetCorrectGlickoStats(league);
            var playerStats = matchStats.FirstOrDefault(x => x.DiscordUserId == player.DiscordId);

            if (glickoStats is not null && playerStats is not null)
            {
                var playerRank = glickoStats.Rank;

                playerStats.OldRank = playerRank;
                playerStats.NewRank = playerRank;

                if (playerRank == lowestRank && playerStats.Winner && allowRankingUp)
                {
                    var isRankUpGame = true;
                    var newRank = playerRank;

                    if (lowestRank < TiglAsyncRank.Commander)
                    {
                        var enumValues = Enum.GetValues<TiglAsyncRank>().ToList();
                        var currentIndex = enumValues.IndexOf(playerRank);

                        if (currentIndex < enumValues.Count - 1)
                        {
                            newRank = enumValues[currentIndex + 1];
                        }
                    }
                    else if (lowestRank == TiglAsyncRank.Commander)
                    {
                        newRank = TiglAsyncRank.Hero;
                    }
                    else if (lowestRank >= TiglAsyncRank.Hero)
                    {
                        var heroStats = glickoStats.MatchStats
                            .Where(x => x.NewRank >= TiglAsyncRank.Hero)
                            .ToList();

                        var heroCount = heroStats.DistinctBy(x => x.Faction).Count();

                        newRank = heroCount switch
                        {
                            < 5 => TiglAsyncRank.Hero,
                            < 10 => TiglAsyncRank.GalacticThreatOne,
                            < 15 => TiglAsyncRank.GalacticThreatTwo,
                            < 20 => TiglAsyncRank.GalacticThreatThree,
                            < 25 => TiglAsyncRank.GalacticThreatFour,
                            _ => TiglAsyncRank.GalacticThreatFive,
                        };
                    }

                    playerStats.IsRankUpGame = isRankUpGame;
                    glickoStats.Rank = newRank;
                    playerStats.NewRank = newRank;
                }
            }
        }
    }

    public async Task ResolveRankUpAsync(IReadOnlyCollection<TiglUser> tiglUsers, IReadOnlyCollection<TrueSkillPlayerMatchStats> matchStats, TiglLeague league, CancellationToken cancellationToken)
    {
        var rankingParameter = await tiglRepository.GetTiglParameter(TiglParameterName.RankingUp, cancellationToken);
        var allowRankingUp = rankingParameter?.Enabled ?? false;

        var lowestRank = tiglUsers.Select(x => x.GetCorrectTrueSkillStats(league)).Min(x => x?.Rank ?? TiglAsyncRank.Unranked);

        foreach (var player in tiglUsers)
        {
            var trueSkillStats = player.GetCorrectTrueSkillStats(league);
            var playerStats = matchStats.FirstOrDefault(x => x.DiscordUserId == player.DiscordId);

            if (trueSkillStats is not null && playerStats is not null)
            {
                var playerRank = trueSkillStats.Rank;

                playerStats.OldRank = playerRank;
                playerStats.NewRank = playerRank;

                if (playerRank == lowestRank && playerStats.Winner && allowRankingUp)
                {
                    var isRankUpGame = true;
                    var newRank = playerRank;

                    if (lowestRank < TiglAsyncRank.Commander)
                    {
                        var enumValues = Enum.GetValues<TiglAsyncRank>().ToList();
                        var currentIndex = enumValues.IndexOf(playerRank);

                        if (currentIndex < enumValues.Count - 1)
                        {
                            newRank = enumValues[currentIndex + 1];
                        }
                    }
                    else if (lowestRank == TiglAsyncRank.Commander)
                    {
                        newRank = TiglAsyncRank.Hero;
                    }
                    else if (lowestRank >= TiglAsyncRank.Hero)
                    {
                        var heroStats = trueSkillStats.MatchStats
                            .Where(x => x.NewRank >= TiglAsyncRank.Hero)
                            .ToList();

                        var heroCount = heroStats.DistinctBy(x => x.Faction).Count();

                        newRank = heroCount switch
                        {
                            < 5 => TiglAsyncRank.Hero,
                            < 10 => TiglAsyncRank.GalacticThreatOne,
                            < 15 => TiglAsyncRank.GalacticThreatTwo,
                            < 20 => TiglAsyncRank.GalacticThreatThree,
                            < 25 => TiglAsyncRank.GalacticThreatFour,
                            _ => TiglAsyncRank.GalacticThreatFive,
                        };
                    }

                    playerStats.IsRankUpGame = isRankUpGame;
                    trueSkillStats.Rank = newRank;
                    playerStats.NewRank = newRank;
                }
            }
        }
    }
}
