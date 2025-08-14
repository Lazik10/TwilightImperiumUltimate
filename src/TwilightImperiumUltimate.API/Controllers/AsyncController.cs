using System.Text;
using TwilightImperiumUltimate.API.Helpers;
using TwilightImperiumUltimate.Business.Logic.Async;
using TwilightImperiumUltimate.Contracts.ApiContracts.AsyncTI4;
using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Games;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;

namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/[controller]")]
[ApiKeyStatsAuth]
[ApiController]
public class AsyncController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // GET: api/async/player-profile/discordId/{discordId}
    [Route("player-profile/discordId/{asyncPlayerId:long}")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncPlayerProfileSummaryStatsDto>>> GetSpecificAsyncPlayerProfile(long asyncPlayerId)
    {
        var response = await _mediator.Send(new GetAsyncPlayerInfoByIdQuery(asyncPlayerId));
        return Ok(new ApiResponse<AsyncPlayerProfileSummaryStatsDto>() { Data = response });
    }

    // POST: api/async/player-profile
    [Route("player-profile")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<AsyncPlayerProfileSummaryStatsDto>>> GetSpecificAsyncPlayerProfile(AsyncPlayerProfileRequest playerProfile)
    {
        var response = await _mediator.Send(new GetAsyncPlayerInfoByPlayerProfileQuery(playerProfile));
        return Ok(new ApiResponse<AsyncPlayerProfileSummaryStatsDto>() { Data = response });
    }

    // GET: api/async/player-profiles
    [Route("player-profiles")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncPlayerProfileListDto>>> GetAllPlayerProfileNames()
    {
        var response = await _mediator.Send(new GetAllAsyncPlayerProfilesQuery());
        return Ok(new ApiResponse<AsyncPlayerProfileListDto>() { Success = true, Data = response });
    }

    // GET: api/async/player-profile-by-discordId
    [Route("player-profile-by-discordId")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncPlayerProfileDto>>> GetPlayerId([FromQuery] long discordId)
    {
        var response = await _mediator.Send(new GetPlayerIdByDiscordIdQuery(discordId));

        if (response.Id == -1)
        {
            return BadRequest(new ApiResponse<AsyncPlayerProfileDto>() { Success = false, Data = response, ProblemDetails = new ProblemDetailsDto() { Title = "Player not found!" } });
        }

        return Ok(new ApiResponse<AsyncPlayerProfileDto>() { Success = true, Data = response });
    }

    // GET: api/async/games
    [Route("games")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<AsyncGameDto>>>> GetAsyncGames()
    {
        var response = await _mediator.Send(new GetAllAsyncGamesQuery());
        return Ok(new ApiResponse<ItemListDto<AsyncGameDto>>() { Success = true, Data = new ItemListDto<AsyncGameDto>(response) });
    }

    // GET: api/async/game-by-discordId
    [Route("game-by-discordId")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncGameDto>>> GetAsyncGameByDiscordId([FromQuery] string discordId)
    {
        var response = await _mediator.Send(new GetGameByDiscordIdQuery(discordId));
        return Ok(new ApiResponse<AsyncGameDto>() { Success = true, Data = response });
    }

    // GET: api/async/game-by-fun-name
    [Route("game-by-fun-name")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncGameDto>>> GetAsyncGameByFunName([FromQuery] string funName)
    {
        var response = await _mediator.Send(new GetGameByFunNameQuery(funName));
        return Ok(new ApiResponse<AsyncGameDto>() { Success = true, Data = response });
    }

    // GET: api/async/games-by-date
    [Route("games-by-date")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<ItemListDto<AsyncGameDto>>>> GetAsyncGamesByYearAndMonth(AsyncGamesByYearAndMonthRequest request)
    {
        var response = await _mediator.Send(new GetAllAsyncGamesByYearAndMonthQuery(request.Year, request.Month));
        return Ok(new ApiResponse<ItemListDto<AsyncGameDto>>() { Success = true, Data = new ItemListDto<AsyncGameDto>(response) });
    }

    // GET: api/async/game-dates
    [Route("game-dates")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncGameDatesDto>>> GetAsyncGameDates()
    {
        var response = await _mediator.Send(new GetAllAsyncGameDatesQuery());
        return Ok(new ApiResponse<AsyncGameDatesDto>() { Success = true, Data = response });
    }

    // GET: api/async/game-names
    [Route("game-names")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncGameNamesDto>>> GetAsyncGameNames()
    {
        var gameNames = await _mediator.Send(new GetAllAsyncGameNamesQuery());
        var response = new AsyncGameNamesDto(gameNames);

        return Ok(new ApiResponse<AsyncGameNamesDto>() { Success = true, Data = response });
    }

    // GET: api/async/game-fun-names
    [Route("game-fun-names")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncGameNamesDto>>> GetAsyncGameFunNames()
    {
        var gameNames = await _mediator.Send(new GetAllAsyncGameFunNamesQuery());
        var response = new AsyncGameNamesDto(gameNames);

        return Ok(new ApiResponse<AsyncGameNamesDto>() { Success = true, Data = response });
    }

    // POST: api/async/
    [Route("player-settings")]
    [HttpPost]
    public async Task<ActionResult<IApiResponse<AsyncPlayerSettingsResponseDto>>> UpdateAsyncUserSettings(AsyncPlayerSettingsRequestDto request)
    {
        var result = await _mediator.Send(new UpdateAsyncPlayerSettingsCommand(request));

        if (result.IsFailed)
        {
            var sb = new StringBuilder();
            foreach (var error in result.Errors)
            {
                sb.AppendLine(error.Message);
            }

            var errorMessage = sb.ToString().TrimEnd();

            return BadRequest(new ApiResponse<AsyncPlayerSettingsResponseDto>() { Success = false, ProblemDetails = new ProblemDetailsDto() { Title = "Unable to update player settings!", Detail = errorMessage } });
        }

        var responseDto = new AsyncPlayerSettingsResponseDto(
            request.PlayerDiscordId,
            ExcludeFromAsyncStats: request.ExcludeFromAsyncStats,
            ShowWinRates: request.ShowWinRates,
            ShowTurnStats: request.ShowTurnStats,
            ShowCombatStats: request.ShowCombatStats,
            ShowVpStats: request.ShowVpStats,
            ShowFactionStats: request.ShowFactionStats,
            ShowOpponents: request.ShowOpponents,
            ShowGames: request.ShowGames);

        return Ok(new ApiResponse<AsyncPlayerSettingsResponseDto>() { Success = result.IsSuccess, Data = responseDto });
    }

    // GET: api/async/general-stats
    [Route("general-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncGeneralSummaryStatsDto>>> GetAsyncGeneralStats()
    {
        var generalStats = await _mediator.Send(new GetAsyncGeneralStatsSummaryQuery());
        return Ok(new ApiResponse<AsyncGeneralSummaryStatsDto>() { Success = true, Data = generalStats });
    }

    // GET: api/async/games-stats
    [Route("games-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncGamesSummaryStatsDto>>> GetAsyncGameStats([FromQuery] int limit)
    {
        var gameStats = await _mediator.Send(new GetAsyncGamesStatsSummaryQuery(limit));
        return Ok(new ApiResponse<AsyncGamesSummaryStatsDto>() { Success = true, Data = gameStats });
    }

    // GET: api/async/wins-stats
    [Route("wins-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncWinsSummaryStatsDto>>> GetWinsGameStats([FromQuery] int limit)
    {
        var winStats = await _mediator.Send(new GetAsyncWinsStatsSummaryQuery(limit));
        return Ok(new ApiResponse<AsyncWinsSummaryStatsDto>() { Success = true, Data = winStats });
    }

    // GET: api/async/vp-stats
    [Route("vp-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncVpSummaryStatsDto>>> GetVpGameStats([FromQuery] int limit)
    {
        var vpStats = await _mediator.Send(new GetAsyncVpStatsSummaryQuery(limit));
        return Ok(new ApiResponse<AsyncVpSummaryStatsDto>() { Success = true, Data = vpStats });
    }

    // GET: api/async/eliminations-stats
    [Route("eliminations-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncEliminationsSummaryStatsDto>>> GetEliminationsGameStats([FromQuery] int limit)
    {
        var eliminationsStats = await _mediator.Send(new GetAsyncEliminationsStatsSummaryQuery(limit));
        return Ok(new ApiResponse<AsyncEliminationsSummaryStatsDto>() { Success = true, Data = eliminationsStats });
    }

    // GET: api/async/turns-stats
    [Route("turns-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncTurnsSummaryStatsDto>>> GetTurnsGameStats([FromQuery] int limit)
    {
        var turnsStats = await _mediator.Send(new GetAsyncTurnsStatsSummaryQuery(limit));
        return Ok(new ApiResponse<AsyncTurnsSummaryStatsDto>() { Success = true, Data = turnsStats });
    }

    // GET: api/async/combat-stats
    [Route("combat-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncCombatSummaryStatsDto>>> GetCombatGameStats([FromQuery] int limit)
    {
        var combatStats = await _mediator.Send(new GetAsyncCombatStatsSummaryQuery(limit));
        return Ok(new ApiResponse<AsyncCombatSummaryStatsDto>() { Success = true, Data = combatStats });
    }

    // GET: api/async/durations-stats
    [Route("durations-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncDurationsSummaryStatsDto>>> GetDurationsGameStats([FromQuery] int limit)
    {
        var combatStats = await _mediator.Send(new GetAsyncDurationsStatsSummaryQuery(limit));
        return Ok(new ApiResponse<AsyncDurationsSummaryStatsDto>() { Success = true, Data = combatStats });
    }

    // GET: api/async/factions-stats
    [Route("factions-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncFactionsSummaryStatsDto>>> GetFactionStats()
    {
        var response = await _mediator.Send(new GetAsyncFactionsStatsQuery());
        return Ok(new ApiResponse<AsyncFactionsSummaryStatsDto>() { Data = response });
    }

    // GET: api/async/opponents-stats
    [Route("opponents-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncOpponentsSummaryStatsDto>>> GetOpponentsStats(int limit)
    {
        var response = await _mediator.Send(new GetAsyncOpponentsStatsQuery(limit));
        return Ok(new ApiResponse<AsyncOpponentsSummaryStatsDto>() { Data = response });
    }

    // GET: api/async/history-stats
    [Route("history-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncHistorySummaryStatsDto>>> GetHistoryStats()
    {
        var response = await _mediator.Send(new GetAsyncHistoryStatsQuery());
        return Ok(new ApiResponse<AsyncHistorySummaryStatsDto>() { Data = response });
    }
}
