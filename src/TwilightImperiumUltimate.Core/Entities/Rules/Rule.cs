using TwilightImperiumUltimate.Core.Enums.Rules;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Rules;

public class Rule : IEntity
{
    public int Id { get; set; }

    public RuleCategory RuleCategory { get; set; }

    public string Content { get; set; } = string.Empty;
}
