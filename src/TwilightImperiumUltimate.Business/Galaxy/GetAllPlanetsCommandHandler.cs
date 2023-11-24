using MediatR;
using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.Business.Galaxy;

public class GetAllPlanetsCommandHandler : IRequestHandler<GetAllPlanetsCommand, List<Planet>>
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _contextFactory;

    public GetAllPlanetsCommandHandler(IDbContextFactory<TwilightImperiumDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<Planet>> Handle(GetAllPlanetsCommand request, CancellationToken cancellationToken)
    {
        using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        return await context.Planets
            .Where(x => x.PlanetName != Core.Enums.Galaxy.PlanetName.MalliceInactive)
            .ToListAsync(cancellationToken);
    }
}
