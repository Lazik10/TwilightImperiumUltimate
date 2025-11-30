using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Tigl.Achievements.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class AchievementEvaluatorAttribute(AchievementName name) : Attribute
{
    public AchievementName Name { get; } = name;
}
