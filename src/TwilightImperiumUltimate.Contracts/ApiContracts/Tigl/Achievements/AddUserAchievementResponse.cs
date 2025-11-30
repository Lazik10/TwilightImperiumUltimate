using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Achievements;

public class AddUserAchievementResponse
{
    public bool Success { get; set; }

    public int TiglUserId { get; set; }

    public AchievementName AchievementName { get; set; }

    public TiglFactionName Faction { get; set; }

    public string ErrorTitle { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;
}
