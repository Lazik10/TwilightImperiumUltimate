using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Achievements;

public class AddUserAchievementRequest
{
    public int TiglUserId { get; set; }

    public AchievementName AchievementName { get; set; }

    public TiglFactionName Faction { get; set; } = TiglFactionName.None;
}
