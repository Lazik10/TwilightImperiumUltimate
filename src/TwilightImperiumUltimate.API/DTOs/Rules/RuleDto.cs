using TwilightImperiumUltimate.Core.Enums.Languages;
using TwilightImperiumUltimate.Core.Enums.Rules;

namespace TwilightImperiumUltimate.API.DTOs.Rules;

public class RuleDto
{
    public RuleCategory RuleCategory { get; set; }

    public string Content { get; set; } = string.Empty;

    public Language Language { get; set; }
}
