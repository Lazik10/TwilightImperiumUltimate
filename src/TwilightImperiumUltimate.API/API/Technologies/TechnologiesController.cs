using MediatR;
using Microsoft.AspNetCore.Mvc;
using TwilightImperiumUltimate.API.DTOs.Technologies;
using TwilightImperiumUltimate.Business.Technologies;

namespace TwilightImperiumUltimate.API.API.Technologies;

[Route("api/[controller]")]
[ApiController]
public class TechnologiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TechnologiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<List<TechnologyDto>> GetAllNews()
    {
        var result = await _mediator.Send(new GetAllTechnologiesCommand());

        return result.Select(x => new TechnologyDto
        {
            Id = x.Id,
            TechnologyName = x.TechnologyName,
            Type = x.Type,
            Level = x.Level,
            GameVersion = x.GameVersion,
            IsFactionTechnology = x.IsFactionTechnology,
            FactionName = x.FactionName,
        })
        .ToList();
    }
}
