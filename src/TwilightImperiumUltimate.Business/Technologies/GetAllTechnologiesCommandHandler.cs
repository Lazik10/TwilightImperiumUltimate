using MediatR;
using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Core.Entities.Technologies;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.Business.Technologies;

public class GetAllTechnologiesCommandHandler(IDbContextFactory<TwilightImperiumDbContext> dbContextFactory)
    : IRequestHandler<GetAllTechnologiesCommand, List<Technology>>
{
    public async Task<List<Technology>> Handle(GetAllTechnologiesCommand request, CancellationToken cancellationToken)
    {
        using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var technologies = await context.Technologies
            .ToListAsync(cancellationToken);

        return technologies;
    }
}
