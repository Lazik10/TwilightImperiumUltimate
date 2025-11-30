using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Achievements;

namespace TwilightImperiumUltimate.Business.Logic.Tigl.Achievements;

public record RemoveTiglUserAchievementCommand(int TiglUserId, AchievementName AchievementName, TiglFactionName Faction) : IRequest<RemoveUserAchievementResponse>;
