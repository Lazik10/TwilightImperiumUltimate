using TwilightImperiumUltimate.Business.Logic.SlicesArchive;
using TwilightImperiumUltimate.Contracts.ApiContracts.SliceDrafts;

namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/slice-draft-ratings")]
[ApiController]
public class SliceDraftRatingsController(
    IMediator mediator)
    : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // GET: api/slice-draft-ratings/user
    [Route("user")]
    [HttpPost]
    public async Task<ActionResult<SliceDraftRatingDto>> GetUserSliceDraftRating(UserSliceDraftRatingRequest request)
    {
        var response = await _mediator.Send(new GetUserSliceDraftRatingQuery(request.UserId, request.SliceDraftId));

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    // PUT: api/slice-draft-ratings/user
    [Route("user")]
    [HttpPut]
    public async Task<ActionResult<SliceDraftRatingDto>> UpdateUserSliceDraftRating(UserSliceDraftRatingRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new AddOrUpdateUserSliceDraftRatingCommand(request.UserId, request.SliceDraftId, request.Rating), cancellationToken);

        return Ok(response);
    }
}
