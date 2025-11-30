using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win a game with each Keleres variant.
/// </summary>
[AchievementEvaluator(AchievementName.TheTribunii)]
public sealed class TheTribuniiAchievementEvaluator(
    IAchievementRepository achievementRepository,
    ITiglUserRepository tiglUserRepository)
    : IAchievementEvaluator
{
    private const int KeleresFactionsCount = 3;

    private static readonly HashSet<TiglFactionName> KeleresFactions = Enum.GetValues<TiglFactionName>()
        .Where(f => f >= TiglFactionName.TheCouncilKeleresMentak && f <= TiglFactionName.TheCouncilKeleresArgent)
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
                .Where(pr => pr.TiglUserId == winner.TiglUserId && KeleresFactions.Contains(pr.Faction))
                .Select(x => x.Faction)
                .Distinct()
                .ToHashSet();

            uniqueFactionWins.Add(winner.Faction);

            if (uniqueFactionWins.Count == KeleresFactionsCount && uniqueFactionWins.SetEquals(KeleresFactions))
                await achievementRepository.AwardAchievement(winner.TiglUserId, matchReport, achievementName, cancellationToken);
        }
    }
}
