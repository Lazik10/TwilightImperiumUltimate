namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CardsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // GET: api/cards/action
    [Route("action")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<ActionCardDto>>>> GetAllActionCards(CancellationToken ct)
    {
        var actionCards = await _mediator.Send(new GetAllActionCardsQuery(), ct);
        return Ok(new ApiResponse<ItemListDto<ActionCardDto>>() { Success = true, Data = actionCards });
    }

    // GET: api/cards/agenda
    [Route("agenda")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<AgendaCardDto>>>> GetAllAgendaCards(CancellationToken ct)
    {
        var agendaCards = await _mediator.Send(new GetAllAgendaCardsQuery(), ct);
        return Ok(new ApiResponse<ItemListDto<AgendaCardDto>>() { Success = true, Data = agendaCards });
    }

    // GET: api/cards/exploration
    [Route("exploration")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<ExplorationCardDto>>>> GetAllExplorationCards(CancellationToken ct)
    {
        var explorationCards = await _mediator.Send(new GetAllExplorationCardsQuery(), ct);
        return Ok(new ApiResponse<ItemListDto<ExplorationCardDto>>() { Success = true, Data = explorationCards });
    }

    // GET: api/cards/frontier
    [Route("frontier")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<FrontierCardDto>>>> GetAllFrontierCards(CancellationToken ct)
    {
        var frontierCards = await _mediator.Send(new GetAllFrontierCardsQuery(), ct);
        return Ok(new ApiResponse<ItemListDto<FrontierCardDto>>() { Success = true, Data = frontierCards });
    }

    // GET: api/cards/objective
    [Route("objective")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<ObjectiveCardDto>>>> GetAllObjectiveCards(CancellationToken ct)
    {
        var publicStageOneObjectiveCards = await _mediator.Send(new GetAllObjectiveCardsQuery(), ct);
        return Ok(new ApiResponse<ItemListDto<ObjectiveCardDto>>() { Success = true, Data = publicStageOneObjectiveCards });
    }

    // GET: api/cards/secret
    [Route("secret")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<ObjectiveCardDto>>>> GetAllSecretCards(CancellationToken ct)
    {
        var secretObjectiveCards = await _mediator.Send(new GetObjectiveCardsByTypeQuery(ObjectiveCardType.Secret), ct);
        return Ok(new ApiResponse<ItemListDto<ObjectiveCardDto>>() { Success = true, Data = secretObjectiveCards });
    }

    // GET: api/cards/stage-one-objective
    [Route("stage-one-objective")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<ObjectiveCardDto>>>> GetAllStageOneObjectiveCards(CancellationToken ct)
    {
        var publicStageOneObjectiveCards = await _mediator.Send(new GetObjectiveCardsByTypeQuery(ObjectiveCardType.StageOne), ct);
        return Ok(new ApiResponse<ItemListDto<ObjectiveCardDto>>() { Success = true, Data = publicStageOneObjectiveCards });
    }

    // GET: api/cards/stage-two-objective
    [Route("stage-two-objective")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<ObjectiveCardDto>>>> GetAllStageTwoObjectiveCards(CancellationToken ct)
    {
        var publicStageTwoObjectiveCards = await _mediator.Send(new GetObjectiveCardsByTypeQuery(ObjectiveCardType.StageTwo), ct);
        return Ok(new ApiResponse<ItemListDto<ObjectiveCardDto>>() { Success = true, Data = publicStageTwoObjectiveCards });
    }

    // GET: api/cards/promissory-note
    [Route("promissory-note")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<PromissoryNoteCardDto>>>> GetAllPromissaryCards(CancellationToken ct)
    {
        var promissoryNoteCards = await _mediator.Send(new GetAllPromissoryNoteCardsQuery(), ct);
        return Ok(new ApiResponse<ItemListDto<PromissoryNoteCardDto>>() { Success = true, Data = promissoryNoteCards });
    }

    // GET: api/cards/relic
    [Route("relic")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<RelicCardDto>>>> GetAllRelicCards(CancellationToken ct)
    {
        var relicCards = await _mediator.Send(new GetAllRelicCardsQuery(), ct);
        return Ok(new ApiResponse<ItemListDto<RelicCardDto>>() { Success = true, Data = relicCards });
    }

    // GET: api/cards/strategy
    [Route("strategy")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<StrategyCardDto>>>> GetAllStrategyCards(CancellationToken ct)
    {
        var strategyCards = await _mediator.Send(new GetAllStrategyCardsQuery(), ct);
        return Ok(new ApiResponse<ItemListDto<StrategyCardDto>>() { Success = true, Data = strategyCards });
    }
}
