using TwilightImperiumUltimate.Business.Logic.GameStatistics;
using TwilightImperiumUltimate.Business.Logic.MapArchive;
using TwilightImperiumUltimate.Contracts.ApiContracts.Statistics;
using TwilightImperiumUltimate.Contracts.DTOs.GameTracker;

namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/game-statistics")]
[ApiController]
public class GameStatisticsController(
    IMediator mediator)
    : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // POST: api/game-statistics/round
    [Route("round")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<StatisticsResponse>>> RegisterRoundStatistics(RoundStatisticsDto roundStatistics)
    {
        var result = await _mediator.Send(new AddNewRoundStatisticsCommand(roundStatistics));
        return Ok(new ApiResponse<StatisticsResponse>() { Success = result });
    }

    // POST: api/game-statistics/game
    [Route("game")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<StatisticsResponse>>> AddEndGameStatistics(GameStatisticsDto gameStatistics)
    {
        var result = await _mediator.Send(new AddNewGameStatisticsCommand(gameStatistics));
        return Ok(new ApiResponse<StatisticsResponse>() { Success = result });
    }
}
