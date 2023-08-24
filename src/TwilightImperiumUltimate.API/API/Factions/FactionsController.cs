using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.API.DTOs.Factions;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.API.API.Factions;

[Route("api/[controller]")]
[ApiController]
public class FactionsController : ControllerBase
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context;

    public FactionsController(IDbContextFactory<TwilightImperiumDbContext> context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FactionDto>>> GetAllFactionsInfo()
    {
        using var context = _context.CreateDbContext();

        var factions = await context.Factions
            .ToListAsync()
            .ConfigureAwait(false);

        var factionDtos = factions
            .Select(x => new FactionDto
            {
                FactionName = x.FactionName,
                Commodities = x.Commodities,
                ComplexityRating = (int)++x.ComplexityRating,
                GameVersion = x.GameVersion,
            })
            .ToList();

        return factionDtos;
    }
}
