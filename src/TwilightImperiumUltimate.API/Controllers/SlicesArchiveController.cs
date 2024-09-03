using TwilightImperiumUltimate.Business.Logic.SlicesArchive;

namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/slices-archive")]
[ApiController]
public class SlicesArchiveController(
    IMediator mediator)
    : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // GET: api/slices-archive/drafts
    [Route("drafts")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<SliceDraftDto>>>> GetAllMaps()
    {
        var maps = await _mediator.Send(new GetAllSliceDraftsQuery());
        return Ok(new ApiResponse<ItemListDto<SliceDraftDto>>() { Data = maps });
    }

    // GET: api/slices-archive/slice-draft
    [Route("slice-draft/{id:int}")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<SliceDraftDto>>> GetSpecificSliceDraft(int id)
    {
        var response = await _mediator.Send(new GetSliceDraftByIdQuery(id));
        return Ok(response);
    }

    // POST: api/slices-archive/add-slice-draft
    [Route("add-slice-draft")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<SliceDraftDto>>> AddSliceDraft(SliceDraftDto sliceDraftDto)
    {
        var result = await _mediator.Send(new AddNewSliceDraftCommand(sliceDraftDto));
        return Ok(new ApiResponse<SliceDraftDto>() { Success = result });
    }
}
