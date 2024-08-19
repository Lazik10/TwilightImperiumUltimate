namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface IRuleRepository
{
    Task<IReadOnlyCollection<Rule>> GetAllRules(CancellationToken cancellationToken);
}
