using TwilightImperiumUltimate.Core.Entities.Rules;
using TwilightImperiumUltimate.Core.Enums.Rules;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class RulesData
{
    internal static List<Rule> Rules => GetRules();

    private static List<Rule> GetRules() => Enum.GetValues<RuleCategory>().Select((x, i) => new Rule { Id = i + 1, RuleCategory = x, Content = $"{nameof(RuleCategory)}_{x}" }).ToList();
}
