using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.API.DTOs.Galaxy;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.API.API.Galaxy;

[Route("api/[controller]")]
[ApiController]
public class GalaxyController : ControllerBase
{
    private readonly ISystemTileRepository _systemTilesRepository;

    public GalaxyController(ISystemTileRepository systemTilesRepository)
    {
        _systemTilesRepository = systemTilesRepository;
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
}
