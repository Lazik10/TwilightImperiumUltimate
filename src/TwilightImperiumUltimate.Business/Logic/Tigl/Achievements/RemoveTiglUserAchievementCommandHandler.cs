using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Achievements;

namespace TwilightImperiumUltimate.Business.Logic.Tigl.Achievements;

public class RemoveTiglUserAchievementCommandHandler(IAchievementRepository achievementRepository)
    : IRequestHandler<RemoveTiglUserAchievementCommand, RemoveUserAchievementResponse>
{
    public async Task<RemoveUserAchievementResponse> Handle(RemoveTiglUserAchievementCommand request, CancellationToken cancellationToken)
    {
        var success = await achievementRepository.RemoveAchievement(request.TiglUserId, request.AchievementName, request.Faction, cancellationToken);
        if (!success)
            return new RemoveUserAchievementResponse { Success = false, TiglUserId = request.TiglUserId, AchievementName = request.AchievementName, Faction = request.Faction, ErrorTitle = "Remove Achievement Failed", ErrorMessage = "Unable to remove achievement." };
        return new RemoveUserAchievementResponse { Success = true, TiglUserId = request.TiglUserId, AchievementName = request.AchievementName, Faction = request.Faction };
    }
}
