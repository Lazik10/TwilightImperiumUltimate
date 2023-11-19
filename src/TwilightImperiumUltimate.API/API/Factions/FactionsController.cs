using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.API.DTOs.Factions;
using TwilightImperiumUltimate.Core.Enums.Races;
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
        using var context = await _context.CreateDbContextAsync();

        var factions = await context.Factions
            .ToListAsync();

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

    [HttpGet("{id}")]
    public async Task<ActionResult<FactionDto>> GetFactionByFactionId(int id)
    {
        using var context = await _context.CreateDbContextAsync();

        var factions = await context.Factions
            .Where(x => x.FactionName == (FactionName)id)
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
            .AsEnumerable()
            .First();

        return factionDtos;
    }
}
