using TwilightImperiumUltimate.Business.Logic.Drafts.Slices;
using TwilightImperiumUltimate.Contracts.DTOs.SliceGeneration;

namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/slice-generators")]
[ApiController]
public class SliceGeneratorsController(
    IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // POST: api/slice-generators/generate-slices
    [Route("generate-slices")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<ItemListDto<SliceDto>>>> GenerateSlices(SliceDraftRequest request)
    {
        var draftedSlices = await _mediator.Send(new GenerateSlicesCommand(request));
        return Ok(new ApiResponse<ItemListDto<SliceDto>>() { Success = true, Data = draftedSlices });
    }
}
