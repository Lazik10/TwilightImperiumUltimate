using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win a game with all 30 factions.
/// </summary>
[AchievementEvaluator(AchievementName.GottaCatchEmAllPartOne)]
public sealed class GottaCatchEmAllPartOneAchievementEvaluator(
    IAchievementRepository achievementRepository,
    ITiglUserRepository tiglUserRepository)
    : IAchievementEvaluator
{
    private const int StandardFactionsCount = 29; // No Keleres

    private static readonly HashSet<TiglFactionName> StandardFactions = Enum.GetValues<TiglFactionName>()
        .Where(f => f >= TiglFactionName.TheArborec && f <= TiglFactionName.TheRalNelConsortium)
        .ToHashSet();

    private static readonly HashSet<TiglFactionName> KeleresFactions = new()
    {
        TiglFactionName.TheCouncilKeleresArgent,
        TiglFactionName.TheCouncilKeleresMentak,
        TiglFactionName.TheCouncilKeleresXxcha,
    };

    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        var winners = matchReport.PlayerResults
            .Where(pr => pr.IsWinner && !usersAchievements.Any(ua => ua.TiglUserId == pr.TiglUserId && ua.Achievement.Name == achievementName))
            .ToList();

        var standardFactionsWithoutKeleres = StandardFactions.Except(KeleresFactions).ToHashSet();

        foreach (var winner in winners)
        {
            var games = await tiglUserRepository.GetTiglUserMatchReports(winner.TiglUserId, matchReport.EndTimestamp, cancellationToken, true);

            if (games.Count < standardFactionsWithoutKeleres.Count)
                continue;

            var uniqueFactionWins = games.SelectMany(g => g.PlayerResults)
                .Where(pr => pr.TiglUserId == winner.TiglUserId && standardFactionsWithoutKeleres.Contains(pr.Faction))
                .Select(x => x.Faction)
                .Distinct()
                .ToHashSet();

            bool hasKeleresWin = games.SelectMany(g => g.PlayerResults)
                .Where(pr => pr.TiglUserId == winner.TiglUserId && KeleresFactions.Contains(pr.Faction))
                .Any();

            if (!KeleresFactions.Contains(winner.Faction))
                uniqueFactionWins.Add(winner.Faction);

            var uniqueFactionWinsWithoutKeleres = uniqueFactionWins.Except(KeleresFactions).ToHashSet();

            if (hasKeleresWin && uniqueFactionWinsWithoutKeleres.Count == StandardFactionsCount && uniqueFactionWinsWithoutKeleres.SetEquals(standardFactionsWithoutKeleres))
                await achievementRepository.AwardAchievement(winner.TiglUserId, matchReport, achievementName, cancellationToken);
        }
    }
}
