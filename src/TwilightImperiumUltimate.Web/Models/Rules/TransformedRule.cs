using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Web.Models.Rules;

public record TransformedRule(RuleCategory RuleCategory, string RuleTitle, string Content, Language Language);
