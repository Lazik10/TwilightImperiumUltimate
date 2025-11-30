using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Achievements;

namespace TwilightImperiumUltimate.Business.Logic.Tigl.Achievements;

public record AddTiglUserAchievementCommand(int TiglUserId, AchievementName AchievementName, TiglFactionName Faction) : IRequest<AddUserAchievementResponse>;
