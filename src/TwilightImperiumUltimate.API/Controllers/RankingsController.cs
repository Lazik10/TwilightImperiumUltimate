using TwilightImperiumUltimate.Business.Logic.Rankings;
using TwilightImperiumUltimate.Business.Logic.Tigl.Achievements;
using TwilightImperiumUltimate.Business.Logic.Tigl.Prestige;
using TwilightImperiumUltimate.Contracts.ApiContracts.Rankings;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Achievements;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Prestige;
using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RankingsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // GET: api/rankings/overview
    [Route("overview")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<RankingsUserDto>>>> GetRankingsOverview(CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(new GetTiglRankingsOverviewQuery(), cancellationToken);
        return Ok(new ApiResponse<ItemListDto<RankingsUserDto>>() { Success = true, Data = data });
    }

    // GET: api/rankings/leaders
    [Route("leaders")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<RankingsLeaderDto>>>> GetLeadersOverview(CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(new GetTiglLeadersOverviewQuery(), cancellationToken);
        return Ok(new ApiResponse<ItemListDto<RankingsLeaderDto>>() { Success = true, Data = data });
    }

    // GET: api/rankings/recent-achievements
    [Route("recent-achievements")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<RankingsAchievementDto>>>> GetRecentAchievements(CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(new GetRecentAchievementsQuery(), cancellationToken);
        return Ok(new ApiResponse<ItemListDto<RankingsAchievementDto>>() { Success = true, Data = data });
    }

    // GET: api/rankings/user-rank-overview/{tiglUserId}
    [Route("user-rank-overview/{tiglUserId}")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<RankingsUserDto>>> GetUserOverview(int tiglUserId, CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(new GetUserRankOverviewQuery(tiglUserId), cancellationToken);
        return Ok(new ApiResponse<RankingsUserDto>() { Success = true, Data = data });
    }

    // GET: api/rankings/user-rank-history/{tiglUserId}
    [Route("user-rank-history/{tiglUserId}")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<RankHistoryDto>>>> GetUserHistory(int tiglUserId, CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(new GetUserRankHistoryQuery(tiglUserId), cancellationToken);
        return Ok(new ApiResponse<ItemListDto<RankHistoryDto>>() { Success = true, Data = data });
    }

    // GET: api/rankings/user-prestige-rank-history/{tiglUserId}
    [Route("user-prestige-rank-history/{tiglUserId}")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<PrestigeRankHistoryDto>>>> GetUserPrestigeHistory(int tiglUserId, CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(new GetUserPrestigeRankHistoryQuery(tiglUserId), cancellationToken);
        return Ok(new ApiResponse<ItemListDto<PrestigeRankHistoryDto>>() { Success = true, Data = data });
    }

    // POST: api/rankings/add-rank-history
    [Route("add-rank-history")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<AddRankHistoryResponse>>> AddRankHistory(AddRankHistoryRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddRankHistoryCommand(request.TiglUserId, request.League, request.Rank, request.AchievedAt), cancellationToken);
        if (!result.Success)
        {
            return BadRequest(new ApiResponse<AddRankHistoryResponse>() { Success = false, Data = result, ProblemDetails = new ProblemDetailsDto() { Title = result.ErrorTitle, Detail = result.ErrorMessage } });
        }

        return Ok(new ApiResponse<AddRankHistoryResponse>() { Success = true, Data = result });
    }

    // DELETE: api/rankings/remove-rank-history
    [Route("remove-rank-history")]
    [HttpDelete]
    public async Task<ActionResult<IApiResponse<RemoveRankHistoryResponse>>> RemoveRankHistory([FromBody] RemoveRankHistoryRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveRankHistoryCommand(request.RankHistoryId), cancellationToken);
        if (!result.Success)
        {
            return BadRequest(new ApiResponse<RemoveRankHistoryResponse>() { Success = false, Data = result, ProblemDetails = new ProblemDetailsDto() { Title = result?.ErrorTitle ?? string.Empty, Detail = result?.ErrorMessage ?? string.Empty } });
        }

        return Ok(new ApiResponse<RemoveRankHistoryResponse>() { Success = true, Data = result });
    }

    // GET: api/rankings/user-achievements/{tiglUserId}
    [Route("user-achievements/{tiglUserId}")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<TiglUserAchievementDto>>>> GetUserAchievements(int tiglUserId, CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(new GetTiglUserAchievementsQuery(tiglUserId), cancellationToken);
        return Ok(new ApiResponse<ItemListDto<TiglUserAchievementDto>>() { Success = true, Data = data });
    }

    // POST: api/rankings/add-achievement
    [Route("add-achievement")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<AddUserAchievementResponse>>> AddAchievement(AddUserAchievementRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddTiglUserAchievementCommand(request.TiglUserId, request.AchievementName, request.Faction), cancellationToken);
        if (!result.Success)
        {
            return BadRequest(new ApiResponse<AddUserAchievementResponse>() { Success = false, Data = result, ProblemDetails = new ProblemDetailsDto() { Title = result.ErrorTitle, Detail = result.ErrorMessage } });
        }

        return Ok(new ApiResponse<AddUserAchievementResponse>() { Success = true, Data = result });
    }

    // DELETE: api/rankings/remove-achievement
    [Route("remove-achievement")]
    [HttpDelete]
    public async Task<ActionResult<IApiResponse<RemoveUserAchievementResponse>>> RemoveAchievement([FromBody] RemoveUserAchievementRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveTiglUserAchievementCommand(request.TiglUserId, request.AchievementName, request.Faction), cancellationToken);
        if (!result.Success)
        {
            return BadRequest(new ApiResponse<RemoveUserAchievementResponse>() { Success = false, Data = result, ProblemDetails = new ProblemDetailsDto() { Title = result.ErrorTitle, Detail = result.ErrorMessage } });
        }

        return Ok(new ApiResponse<RemoveUserAchievementResponse>() { Success = true, Data = result });
    }

    // POST: api/rankings/award-prestige-rank
    [Route("award-prestige-rank")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<AwardPrestigeRankResponse>>> AwardPrestigeRank(AwardPrestigeRankRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AwardPrestigeRankCommand(request), cancellationToken);
        if (!result.Success)
        {
            return BadRequest(new ApiResponse<AwardPrestigeRankResponse>() { Success = false, Data = result, ProblemDetails = new ProblemDetailsDto() { Title = result.ErrorTitle, Detail = result.ErrorMessage } });
        }

        return Ok(new ApiResponse<AwardPrestigeRankResponse>() { Success = true, Data = result });
    }

    // DELETE: api/rankings/remove-prestige-rank
    [Route("remove-prestige-rank")]
    [HttpDelete]
    public async Task<ActionResult<IApiResponse<RemovePrestigeRankResponse>>> RemovePrestigeRank([FromBody] RemovePrestigeRankRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemovePrestigeRankCommand(request), cancellationToken);
        if (!result.Success)
        {
            return BadRequest(new ApiResponse<RemovePrestigeRankResponse>() { Success = false, Data = result, ProblemDetails = new ProblemDetailsDto() { Title = result.ErrorTitle, Detail = result.ErrorMessage } });
        }

        return Ok(new ApiResponse<RemovePrestigeRankResponse>() { Success = true, Data = result });
    }
}
