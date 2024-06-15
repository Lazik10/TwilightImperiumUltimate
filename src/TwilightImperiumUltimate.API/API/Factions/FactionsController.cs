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
            .Include(x => x.FactionUnits)
            .Include(x => x.FactionTechnologies)
            .ToListAsync();

        var promissaryNotes = await context.PromissaryNoteCards
            .ToListAsync();

        var factionDtos = factions
            .Select(x => new FactionDto
            {
                FactionName = x.FactionName,
                Commodities = x.Commodities,
                ComplexityRating = (int)++x.ComplexityRating,
                Units = x.FactionUnits
                    .ToDictionary(fu => fu.UnitName, fu => fu.Count),
                FactionTechnologies = x.FactionTechnologies
                    .Select(ft => ft.TechnologyName)
                    .ToList(),
                PromissaryNotes = promissaryNotes.Where(pn => pn.Faction == x.FactionName)
                    .Select(pn => pn.PromissaryNoteCardName)
                    .ToList(),
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
            .Include(x => x.FactionUnits)
            .ToListAsync();

        var promissaryNotes = await context.PromissaryNoteCards
            .ToListAsync();

        var factionDtos = factions
            .Select(x => new FactionDto
            {
                FactionName = x.FactionName,
                Commodities = x.Commodities,
                ComplexityRating = (int)++x.ComplexityRating,
                Units = x.FactionUnits
                    .ToDictionary(fu => fu.UnitName, fu => fu.Count),
                PromissaryNotes = promissaryNotes.Where(pn => pn.Faction == x.FactionName)
                    .Select(pn => pn.PromissaryNoteCardName)
                    .ToList(),
                GameVersion = x.GameVersion,
            })
            .First();

        return factionDtos;
    }
}
