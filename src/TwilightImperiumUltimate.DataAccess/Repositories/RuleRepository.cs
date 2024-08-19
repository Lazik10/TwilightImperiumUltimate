namespace TwilightImperiumUltimate.DataAccess.Repositories;

internal class RuleRepository(
    IDbContextFactory<TwilightImperiumDbContext> context)
    : IRuleRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;

    public async Task<IReadOnlyCollection<Rule>> GetAllRules(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return dbContext.Rules
            .Where(x => x.RuleCategory != RuleCategory.None)
            .AsEnumerable()
            .OrderBy(x => x.RuleCategory)
            .ToList();
    }
}
