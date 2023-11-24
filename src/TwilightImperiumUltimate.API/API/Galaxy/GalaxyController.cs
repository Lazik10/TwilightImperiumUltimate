using MediatR;
using Microsoft.AspNetCore.Mvc;
using TwilightImperiumUltimate.API.DTOs.Galaxy;
using TwilightImperiumUltimate.Business.Galaxy;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.API.API.Galaxy;

[Route("api/[controller]")]
[ApiController]
public class GalaxyController : ControllerBase
{
    private readonly ISystemTileRepository _systemTilesRepository;
    private readonly IMediator _mediator;

    public GalaxyController(ISystemTileRepository systemTilesRepository, IMediator mediator)
    {
        _systemTilesRepository = systemTilesRepository;
        _mediator = mediator;
    }

    [Route("tiles")]
    [HttpGet]
    public ActionResult<IEnumerable<SystemTileDto>> GetAllSystemTiles()
    {
        var systemTiles = _systemTilesRepository.GetAllSystemTiles();

        var systemTilesDtos = systemTiles
            .Select(x => new SystemTileDto
            {
                Id = x.Id,
                Name = x.SystemTileName,
                TileCategory = x.TileCategory,
                Resources = x.Resources,
                Influence = x.Influence,
                HasPlanets = x.HasPlanets,
            })
            .ToList();

        return systemTilesDtos;
    }

    [Route("planets")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlanetDto>>> GetAllPlanets()
    {
        var planets = await _mediator.Send(new GetAllPlanetsCommand());

        var planetDtos = planets.Select(x => new PlanetDto
        {
            PlanetName = x.PlanetName,
            Resources = x.Resources,
            Influence = x.Influence,
            IsLegendary = x.IsLegendary,
            TechnologySkip = x.TechnologySkip,
            PlanetTrait = x.PlanetTrait,
            GameVersion = x.GameVersion,
        })
        .ToList();

        return planetDtos;
    }
}
