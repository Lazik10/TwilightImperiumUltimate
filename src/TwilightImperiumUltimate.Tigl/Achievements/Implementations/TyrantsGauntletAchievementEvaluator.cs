using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win a game with each Mahact Tyrant faction.
/// </summary>
[AchievementEvaluator(AchievementName.TyrantsGauntlet)]
public sealed class TyrantsGauntletAchievementEvaluator(
    IAchievementRepository achievementRepository,
    ITiglUserRepository tiglUserRepository)
    : IAchievementEvaluator
{
    private const int MahactTyrantsFactionsCount = 8;

    private static readonly HashSet<TiglFactionName> MahactTyrantFactions = Enum.GetValues<TiglFactionName>()
        .Where(f => f >= TiglFactionName.TheRubyMonarch && f <= TiglFactionName.ASickeningLurch)
        .ToHashSet();

    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        if (matchReport.League == TiglLeague.Fractured)
        {
            var winners = matchReport.PlayerResults
                .Where(pr => pr.IsWinner && !usersAchievements.Any(ua => ua.TiglUserId == pr.TiglUserId && ua.Achievement.Name == achievementName))
                .ToList();

            foreach (var winner in winners)
            {
                var games = await tiglUserRepository.GetTiglUserMatchReports(winner.TiglUserId, matchReport.EndTimestamp, cancellationToken, true);
                var uniqueFactionWins = games.SelectMany(g => g.PlayerResults)
                    .Where(pr => pr.TiglUserId == winner.TiglUserId && MahactTyrantFactions.Contains(pr.Faction))
                    .Select(x => x.Faction)
                    .Distinct()
                    .ToHashSet();

                uniqueFactionWins.Add(winner.Faction);

                if (uniqueFactionWins.Count == MahactTyrantsFactionsCount && uniqueFactionWins.SetEquals(MahactTyrantFactions))
                    await achievementRepository.AwardAchievement(winner.TiglUserId, matchReport, achievementName, cancellationToken);
            }
        }
    }
}
