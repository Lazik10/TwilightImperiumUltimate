using System.Text;
using TwilightImperiumUltimate.API.Discord;
using TwilightImperiumUltimate.API.Helpers;
using TwilightImperiumUltimate.Business.Logic.Rankings;
using TwilightImperiumUltimate.Business.Logic.Tigl;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Parameters;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/[controller]")]
[ApiKeyStatsAuth]
[ApiController]
public class TiglController(
    IMediator mediator,
    IDiscordClient discordClient,
    ITiglRepository tiglRepository)
    : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ITiglRepository _tiglRepository = tiglRepository;

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

    // POST: api/tigl/manual-report-game
    [Route("manual-report-game")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<GameReportResult>>> ManualReportGame(GameReport gameReport, CancellationToken cancellationToken)
    {
        var manualReportValidation = await _tiglRepository.GetTiglParameter(TiglParameterName.ManualGameReview, cancellationToken);

        GameReportResult result;

        if (manualReportValidation is not null && manualReportValidation.Enabled)
        {
            result = await _mediator.Send(new ManualReportGameCommand(gameReport), cancellationToken);
        }
        else
        {
            result = await _mediator.Send(new ReportGameCommand(gameReport), cancellationToken);
        }

        if (result.Success)
        {
            if (manualReportValidation is not null && manualReportValidation.Enabled)
                await discordClient.PostTiglAdminAsync($"A new game report has been submitted for manual review: Game ID {gameReport.GameId}", cancellationToken);
            return Ok(new ApiResponse<GameReportResult>() { Success = result.Success, Data = result });
        }
        else
        {
            return BadRequest(new ApiResponse<GameReportResult>()
            {
                Success = result.Success,
                Data = result,
                ProblemDetails = new ProblemDetailsDto()
                {
                    Title = result.ErrorTitle,
                    Detail = result.ErrorMessage,
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
            sb.AppendJoin(",", result.Errors.Select(x => x.Message));
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

    // POST: api/tigl/add-season
    [Route("add-season")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<AddSeasonResponse>>> AddSeason(AddSeasonRequest request)
    {
        var result = await _mediator.Send(new AddSeasonCommand(request.SeasonNumber, request.SeasonName));
        if (!result.Success)
        {
            return BadRequest(new ApiResponse<AddSeasonResponse>() { Success = result.Success, Data = result, ProblemDetails = new ProblemDetailsDto() { Title = result.ErrorTitle, Detail = result.ErrorMessage } });
        }
        else
        {
            return Ok(new ApiResponse<AddSeasonResponse>() { Success = result.Success, Data = result, });
        }
    }

    // POST: api/tigl/update-season
    [Route("update-season")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<UpdateSeasonResponse>>> UpdateSeason(UpdateSeasonRequest request)
    {
        var result = await _mediator.Send(new UpdateSeasonCommand(request.SeasonNumber, request.SeasonName, request.StartDate, request.EndDate));
        if (!result.Success)
        {
            return BadRequest(new ApiResponse<UpdateSeasonResponse>() { Success = result.Success, Data = result, ProblemDetails = new ProblemDetailsDto() { Title = result.ErrorTitle, Detail = result.ErrorMessage } });
        }
        else
        {
            return Ok(new ApiResponse<UpdateSeasonResponse>() { Success = result.Success, Data = result });
        }
    }

    // GET: api/tigl/end-season
    [Route("end-season")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<EndSeasonResponse>>> EndSeason()
    {
        var result = await _mediator.Send(new EndSeasonCommand());
        if (result.IsFailed)
        {
            var errorMessages = string.Join(", ", result.Errors.Select(e => e.Message));
            return BadRequest(new ApiResponse<EndSeasonResponse>() { Success = false, ProblemDetails = new ProblemDetailsDto() { Title = "Failed to end season.", Detail = errorMessages } });
        }

        return Ok(new ApiResponse<EndSeasonResponse>() { Success = result.IsSuccess });
    }

    // POST: api/tigl/set-active-season
    [Route("set-active-season")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<SetActiveSeasonResponse>>> SetActiveSeason(SetActiveSeasonRequest request)
    {
        var result = await _mediator.Send(new SetActiveSeasonCommand(request.SeasonNumber));
        if (!result.Success)
        {
            return BadRequest(new ApiResponse<SetActiveSeasonResponse>() { Success = false, ProblemDetails = new ProblemDetailsDto() { Title = result.ErrorTitle, Detail = result.ErrorMessage } });
        }

        return Ok(new ApiResponse<SetActiveSeasonResponse>() { Success = result.Success, Data = result });
    }

    // GET: api/tigl/game-reports
    [Route("game-reports")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<MatchReportDto>>>> GetAllMatchReports()
    {
        var result = await _mediator.Send(new GetAllMatchReportsQuery());
        return Ok(new ApiResponse<ItemListDto<MatchReportDto>>() { Success = true, Data = new ItemListDto<MatchReportDto>(result) });
    }

    // GET: api/tigl/game-report/{id}
    [Route("game-report/{id}")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<MatchReportDto>>>> GetMatchReportById(int id)
    {
        var result = await _mediator.Send(new GetMatchReportByIdQuery(id));
        return Ok(new ApiResponse<MatchReportDto>() { Success = true, Data = result });
    }

    // GET: api/tigl/season-leaderboard/{season}
    [Route("season-leaderboard/{season}")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<PlayerSeasonResultDto>>>> GetSeasonLeaderboard(int season, CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(new GetSeasonLeaderboardQuery(season), cancellationToken);
        return Ok(new ApiResponse<ItemListDto<PlayerSeasonResultDto>>() { Success = true, Data = data });
    }

    // GET: api/tigl/seasons
    [Route("seasons")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<SeasonDto>>>> GetSeasons(CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(new GetAllSeasonsQuery(), cancellationToken);
        return Ok(new ApiResponse<ItemListDto<SeasonDto>>() { Success = true, Data = data });
    }

    // GET: api/tigl/users
    [Route("users")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<TiglUserLiteDto>>>> GetAllTiglUsers(CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(new GetAllTiglUsersQuery(), cancellationToken);
        return Ok(new ApiResponse<ItemListDto<TiglUserLiteDto>>() { Success = true, Data = data });
    }

    // GET: api/tigl/faction-stats/{season}/{league}
    [Route("faction-stats/{season}/{league}")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<FactionSeasonStatsDto>>>> GetFactionStats(int season, TiglLeague league, CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(new GetFactionSeasonStatsQuery(season, league), cancellationToken);
        return Ok(new ApiResponse<ItemListDto<FactionSeasonStatsDto>>() { Success = true, Data = data });
    }

    // POST: api/tigl/evaluate-game-report
    [Route("evaluate-game-report")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<GameReportResult>>> EvaluateGameReport(EvaluateGameReportRequest request)
    {
        var evalResult = await _mediator.Send(new EvaluateGameReportCommand(request.MatchReportId));
        if (!evalResult.Success)
        {
            return BadRequest(new ApiResponse<GameReportResult>()
            {
                Success = false,
                Data = evalResult,
                ProblemDetails = new ProblemDetailsDto() { Title = evalResult.ErrorTitle, Detail = evalResult.ErrorMessage },
            });
        }

        return Ok(new ApiResponse<GameReportResult>() { Success = true, Data = evalResult });
    }

    // GET: api/tigl/parameters
    [Route("parameters")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<TiglParameterDto>>>> GetAllParameters(CancellationToken cancellationToken)
    {
        var parameters = await _tiglRepository.GetAllTiglParameters(cancellationToken);
        var dtos = parameters.Select(p => new TiglParameterDto { Name = p.Name, Enabled = p.Enabled }).ToList();
        return Ok(new ApiResponse<ItemListDto<TiglParameterDto>>() { Success = true, Data = new ItemListDto<TiglParameterDto>(dtos) });
    }

    // POST: api/tigl/update-parameter
    [Route("update-parameter")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<UpdateTiglParameterResponse>>> UpdateParameter(UpdateTiglParameterRequest request, CancellationToken cancellationToken)
    {
        var result = await _tiglRepository.UpdateTiglParameter(request.Name, request.Enabled, cancellationToken);
        if (result.IsFailed)
        {
            return BadRequest(new ApiResponse<UpdateTiglParameterResponse>() { Success = false, ProblemDetails = new ProblemDetailsDto() { Title = "Failed to update parameter", Detail = string.Join(",", result.Errors.Select(e => e.Message)) } });
        }

        return Ok(new ApiResponse<UpdateTiglParameterResponse>() { Success = true, Data = new UpdateTiglParameterResponse() { Success = true } });
    }

    // GET: api/tigl/tigl-player-profile/{tiglUserId}
    [Route("tigl-player-profile/{tiglUserId}")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<TiglPlayerProfileDto>>> GetTiglPlayerProfile(int tiglUserId, CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(new GetTiglPlayerProfileQuery(tiglUserId), cancellationToken);
        return Ok(new ApiResponse<TiglPlayerProfileDto>() { Success = true, Data = data });
    }
}
