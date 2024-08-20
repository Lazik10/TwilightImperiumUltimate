using TwilightImperiumUltimate.Contracts.ApiContracts.Faqs;

namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FaqsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<FaqDto>>>> GetAllFaqs(CancellationToken cancellationToken)
    {
        var faqs = await _mediator.Send(new GetAllFaqsQuery(), cancellationToken);
        return Ok(new ApiResponse<ItemListDto<FaqDto>>() { Success = true, Data = faqs });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IApiResponse<FaqDto>>> GetFaqById(int id, CancellationToken cancellationToken)
    {
        var faq = await _mediator.Send(new GetFaqByIdQuery(id), cancellationToken);
        return Ok(new ApiResponse<FaqDto>() { Success = true, Data = faq });
    }

    [HttpPost]
    public async Task<ActionResult<IApiResponse<FaqDto>>> InsertNewFaq(InsertFaqRequest request, CancellationToken cancellationToken)
    {
        var faq = await _mediator.Send(new InsertNewFaqCommand(request.Faq), cancellationToken);
        return Ok(new ApiResponse<FaqDto>() { Success = true, Data = faq });
    }

    [HttpPut]
    public async Task<ActionResult<IApiResponse<FaqDto>>> UpdateFaq(UpdateFaqRequest request, CancellationToken cancellationToken)
    {
        var faq = await _mediator.Send(new UpdateFaqCommand(request.Faq), cancellationToken);
        return Ok(new ApiResponse<FaqDto>() { Success = true, Data = faq });
    }
}
