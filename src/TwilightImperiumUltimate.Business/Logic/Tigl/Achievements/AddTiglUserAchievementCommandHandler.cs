using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Achievements;

namespace TwilightImperiumUltimate.Business.Logic.Tigl.Achievements;

public class AddTiglUserAchievementCommandHandler(IAchievementRepository achievementRepository)
    : IRequestHandler<AddTiglUserAchievementCommand, AddUserAchievementResponse>
{
    public async Task<AddUserAchievementResponse> Handle(AddTiglUserAchievementCommand request, CancellationToken cancellationToken)
    {
        var success = await achievementRepository.AddManualAchievement(request.TiglUserId, request.AchievementName, request.Faction, cancellationToken);
        if (!success)
            return new AddUserAchievementResponse { Success = false, TiglUserId = request.TiglUserId, AchievementName = request.AchievementName, Faction = request.Faction, ErrorTitle = "Add Achievement Failed", ErrorMessage = "Unable to add achievement." };
        return new AddUserAchievementResponse { Success = true, TiglUserId = request.TiglUserId, AchievementName = request.AchievementName, Faction = request.Faction };
    }
}
