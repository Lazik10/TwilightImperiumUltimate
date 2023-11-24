using MediatR;
using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Core.Entities.Rules;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.Business.Rules;

public class GetAllRulesCommandHandler : IRequestHandler<GetAllRulesCommand, List<Rule>>
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _dbContextFactory;

    public GetAllRulesCommandHandler(IDbContextFactory<TwilightImperiumDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<List<Rule>> Handle(GetAllRulesCommand request, CancellationToken cancellationToken)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await context.Rules.ToListAsync(cancellationToken);
    }
}
