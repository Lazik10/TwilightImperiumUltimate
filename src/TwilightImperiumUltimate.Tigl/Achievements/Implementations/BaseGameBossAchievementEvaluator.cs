using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win a game with each base game faction.
/// </summary>
[AchievementEvaluator(AchievementName.BaseGameBoss)]
public sealed class BaseGameBossAchievementEvaluator(
    ITiglUserRepository tiglUserRepository,
    IAchievementRepository achievementRepository)
    : IAchievementEvaluator
{
    private const int BaseGameFactionCount = 17;

    private static readonly HashSet<TiglFactionName> BaseGameFactions = Enum.GetValues<TiglFactionName>()
        .Where(f => f >= TiglFactionName.TheArborec && f <= TiglFactionName.TheYssarilTribes)
        .ToHashSet();

    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        var winners = matchReport.PlayerResults
            .Where(pr => pr.IsWinner && !usersAchievements.Any(ua => ua.TiglUserId == pr.TiglUserId && ua.Achievement.Name == achievementName))
            .ToList();

        foreach (var winner in winners)
        {
            var games = await tiglUserRepository.GetTiglUserMatchReports(winner.TiglUserId, matchReport.EndTimestamp, cancellationToken, true);
            var uniqueFactionWins = games.SelectMany(g => g.PlayerResults)
                .Where(pr => pr.TiglUserId == winner.TiglUserId && BaseGameFactions.Contains(pr.Faction))
                .Select(x => x.Faction)
                .Distinct()
                .ToHashSet();

            uniqueFactionWins.Add(winner.Faction);

            if (uniqueFactionWins.Count == BaseGameFactionCount && uniqueFactionWins.SetEquals(BaseGameFactions))
                await achievementRepository.AwardAchievement(winner.TiglUserId, matchReport, achievementName, cancellationToken);
        }
    }
}
