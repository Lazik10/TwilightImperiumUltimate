using TwilightImperiumUltimate.Business.Logic.MapArchive;
using TwilightImperiumUltimate.Contracts.ApiContracts.Maps;

namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/map-ratings")]
[ApiController]
public class MapRatingsController(
    IMediator mediator)
    : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // GET: api/map-ratings/user
    [Route("user")]
    [HttpPost]
    public async Task<ActionResult<MapRatingDto>> GetUserMapRating(UserMapRatingRequest request)
    {
        var response = await _mediator.Send(new GetUserMapRatingQuery(request.UserId, request.MapId));

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    // POST: api/map-ratings/user
    [Route("user")]
    [HttpPut]
    public async Task<ActionResult<MapRatingDto>> UpdateUserMapRating(UserMapRatingRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new AddOrUpdateUserMapRatingCommand(request.UserId, request.MapId, request.Rating), cancellationToken);

        return Ok(response);
    }
}
