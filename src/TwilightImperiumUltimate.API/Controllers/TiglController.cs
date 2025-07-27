using System.Text;
using TwilightImperiumUltimate.API.Helpers;
using TwilightImperiumUltimate.Business.Logic.Tigl;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/[controller]")]
[ApiKeyStatsAuth]
[ApiController]
public class TiglController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // POST: api/tigl/report-game
    [Route("report-game")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<GameReportResult>>> ReportGame(GameReport gameReport)
    {
        var response = await _mediator.Send(new ReportGameCommand(gameReport));

        if (response.Success)
        {
            return Ok(new ApiResponse<GameReportResult>() { Success = response.Success, Data = response });
        }
        else
        {
            return BadRequest(new ApiResponse<GameReportResult>()
            {
                Success = response.Success,
                Data = response,
                ProblemDetails = new ProblemDetailsDto()
                {
                    Title = response.ErrorTitle,
                    Detail = response.ErrorMessage,
                },
            });
        }
    }

    // POST: api/tigl/register-user
    [Route("register-user")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<NewTiglUserResponse>>> RegisterUser(NewTiglUserRequest newUserRequest)
    {
        var result = await _mediator.Send(new RegisterNewTiglUserCommand(newUserRequest));
        if (result.IsFailed)
        {
            var sb = new StringBuilder();
            sb.AppendJoin("\n", result.Errors.Select(x => x.Message));
            var detailedErrorMessage = sb.ToString();
            return BadRequest(new ApiResponse<NewTiglUserResponse>() { Success = false, ProblemDetails = new ProblemDetailsDto() { Title = "Failed to register new user.", Detail = detailedErrorMessage } });
        }

        return Ok(new ApiResponse<NewTiglUserResponse>() { Success = true, Data = new NewTiglUserResponse() { Success = true } });
    }

    // POST: api/tigl/change-username
    [Route("change-username")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<NewTiglUserResponse>>> ChangeUsername(ChangeTiglUserNameRequest newUserRequest)
    {
        var result = await _mediator.Send(new ChangeUserNameCommand(newUserRequest));
        if (!result.Success)
        {
            return BadRequest(new ApiResponse<NewTiglUserResponse>() { Success = false, ProblemDetails = new ProblemDetailsDto() { Title = result.ErrorTitle, Detail = result.ErrorMessage } });
        }

        return Ok(new ApiResponse<NewTiglUserResponse>() { Success = true, Data = new NewTiglUserResponse() { Success = true } });
    }

    // End season
    [HttpGet("end-season")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public ActionResult<bool> EndSeason()
    {
        var result = _mediator.Send(new EndSeasonCommand());
        return Ok(true);
    }
}
