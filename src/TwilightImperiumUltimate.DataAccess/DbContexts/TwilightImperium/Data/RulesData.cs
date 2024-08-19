namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class RulesData
{
    internal static List<Rule> Rules => GetRules();

    private static List<Rule> GetRules() => Enum.GetValues<RuleCategory>()
        .Select(x => new Rule { RuleCategory = x, Content = $"{nameof(RuleCategory)}_{x}" })
        .ToList();
}
