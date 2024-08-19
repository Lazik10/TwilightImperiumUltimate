using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Web.Models.Rules;

public class RuleModel
{
    public RuleCategory RuleCategory { get; set; }

    public string Content { get; set; } = string.Empty;

    public Language Language { get; set; }
}
