using System.Diagnostics;
using System.Text;
using TwilightImperiumUltimate.API.Helpers;
using TwilightImperiumUltimate.API.Services;
using TwilightImperiumUltimate.Business.Logic.Async;
using TwilightImperiumUltimate.Contracts.ApiContracts.AsyncTI4;
using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Games;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;
using TwilightImperiumUltimate.Core.Helpers;

namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/[controller]")]
[ApiKeyStatsAuth]
[ApiController]
public class AsyncController(
    IMediator mediator,
    IAsyncStatisticsSnapshotOperations snapshotOperations,
    ILogger<AsyncController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IAsyncStatisticsSnapshotOperations _snapshotOperations = snapshotOperations;
    private readonly ILogger<AsyncController> _logger = logger;
    private long? _snapshotVersion;

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
        ArgumentNullException.ThrowIfNull(request);

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
        ArgumentNullException.ThrowIfNull(request);

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

        await _snapshotOperations.RequestRefreshAsync(HttpContext.RequestAborted);

        return Ok(new ApiResponse<AsyncPlayerSettingsResponseDto>() { Success = result.IsSuccess, Data = responseDto });
    }

    // GET: api/async/general-stats
    [Route("general-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncGeneralSummaryStatsDto>>> GetAsyncGeneralStats()
    {
        using var telemetry = new EndpointTelemetry(_logger, "general", "all", null, () => _snapshotVersion);
        if (await IsSnapshotNotModifiedAsync())
            return StatusCode(Response.StatusCode);

        var generalStats = await _mediator.Send(new GetAsyncGeneralStatsSummaryQuery());
        return Ok(new ApiResponse<AsyncGeneralSummaryStatsDto>() { Success = true, Data = generalStats });
    }

    // GET: api/async/games-stats
    [Route("games-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncGamesSummaryStatsDto>>> GetAsyncGameStats([FromQuery] int limit)
    {
        using var telemetry = new EndpointTelemetry(_logger, "games", "all", limit, () => _snapshotVersion);
        if (await IsSnapshotNotModifiedAsync())
            return StatusCode(Response.StatusCode);

        var gameStats = await _mediator.Send(new GetAsyncGamesStatsSummaryQuery(limit));
        return Ok(new ApiResponse<AsyncGamesSummaryStatsDto>() { Success = true, Data = gameStats });
    }

    // GET: api/async/wins-stats
    [Route("wins-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncWinsSummaryStatsDto>>> GetWinsGameStats([FromQuery] int limit)
    {
        using var telemetry = new EndpointTelemetry(_logger, "wins", "all", limit, () => _snapshotVersion);
        if (await IsSnapshotNotModifiedAsync())
            return StatusCode(Response.StatusCode);

        var winStats = await _mediator.Send(new GetAsyncWinsStatsSummaryQuery(limit));
        return Ok(new ApiResponse<AsyncWinsSummaryStatsDto>() { Success = true, Data = winStats });
    }

    // GET: api/async/vp-stats
    [Route("vp-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncVpSummaryStatsDto>>> GetVpGameStats([FromQuery] int limit)
    {
        using var telemetry = new EndpointTelemetry(_logger, "victory-points", "all", limit, () => _snapshotVersion);
        if (await IsSnapshotNotModifiedAsync())
            return StatusCode(Response.StatusCode);

        var vpStats = await _mediator.Send(new GetAsyncVpStatsSummaryQuery(limit));
        return Ok(new ApiResponse<AsyncVpSummaryStatsDto>() { Success = true, Data = vpStats });
    }

    // GET: api/async/eliminations-stats
    [Route("eliminations-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncEliminationsSummaryStatsDto>>> GetEliminationsGameStats([FromQuery] int limit)
    {
        using var telemetry = new EndpointTelemetry(_logger, "eliminations", "all", limit, () => _snapshotVersion);
        if (await IsSnapshotNotModifiedAsync())
            return StatusCode(Response.StatusCode);

        var eliminationsStats = await _mediator.Send(new GetAsyncEliminationsStatsSummaryQuery(limit));
        return Ok(new ApiResponse<AsyncEliminationsSummaryStatsDto>() { Success = true, Data = eliminationsStats });
    }

    // GET: api/async/turns-stats
    [Route("turns-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncTurnsSummaryStatsDto>>> GetTurnsGameStats([FromQuery] int limit)
    {
        using var telemetry = new EndpointTelemetry(_logger, "turns", "all", limit, () => _snapshotVersion);
        if (await IsSnapshotNotModifiedAsync())
            return StatusCode(Response.StatusCode);

        var turnsStats = await _mediator.Send(new GetAsyncTurnsStatsSummaryQuery(limit));
        return Ok(new ApiResponse<AsyncTurnsSummaryStatsDto>() { Success = true, Data = turnsStats });
    }

    // GET: api/async/combat-stats
    [Route("combat-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncCombatSummaryStatsDto>>> GetCombatGameStats([FromQuery] int limit)
    {
        using var telemetry = new EndpointTelemetry(_logger, "combat", "all", limit, () => _snapshotVersion);
        if (await IsSnapshotNotModifiedAsync())
            return StatusCode(Response.StatusCode);

        var combatStats = await _mediator.Send(new GetAsyncCombatStatsSummaryQuery(limit));
        return Ok(new ApiResponse<AsyncCombatSummaryStatsDto>() { Success = true, Data = combatStats });
    }

    // GET: api/async/durations-stats
    [Route("durations-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncDurationsSummaryStatsDto>>> GetDurationsGameStats([FromQuery] int limit)
    {
        using var telemetry = new EndpointTelemetry(_logger, "durations", "all", limit, () => _snapshotVersion);
        if (await IsSnapshotNotModifiedAsync())
            return StatusCode(Response.StatusCode);

        var combatStats = await _mediator.Send(new GetAsyncDurationsStatsSummaryQuery(limit));
        return Ok(new ApiResponse<AsyncDurationsSummaryStatsDto>() { Success = true, Data = combatStats });
    }

    // GET: api/async/factions-stats
    [Route("factions-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncFactionsSummaryStatsDto>>> GetFactionStats()
    {
        using var telemetry = new EndpointTelemetry(_logger, "factions", "all", null, () => _snapshotVersion);
        if (await IsSnapshotNotModifiedAsync())
            return StatusCode(Response.StatusCode);

        var response = await _mediator.Send(new GetAsyncFactionsStatsQuery());
        return Ok(new ApiResponse<AsyncFactionsSummaryStatsDto>() { Data = response });
    }

    // GET: api/async/opponents-stats
    [Route("opponents-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncOpponentsSummaryStatsDto>>> GetOpponentsStats(int limit)
    {
        using var telemetry = new EndpointTelemetry(_logger, "opponents", "all", limit, () => _snapshotVersion);
        if (await IsSnapshotNotModifiedAsync())
            return StatusCode(Response.StatusCode);

        var response = await _mediator.Send(new GetAsyncOpponentsStatsQuery(limit));
        return Ok(new ApiResponse<AsyncOpponentsSummaryStatsDto>() { Data = response });
    }

    // GET: api/async/history-stats
    [Route("history-stats")]
    [HttpGet]
    public async Task<ActionResult<IApiResponse<AsyncHistorySummaryStatsDto>>> GetHistoryStats()
    {
        using var telemetry = new EndpointTelemetry(_logger, "history", "all", null, () => _snapshotVersion);
        if (await IsSnapshotNotModifiedAsync())
            return StatusCode(Response.StatusCode);

        var response = await _mediator.Send(new GetAsyncHistoryStatsQuery());
        return Ok(new ApiResponse<AsyncHistorySummaryStatsDto>() { Data = response });
    }

    private async Task<bool> IsSnapshotNotModifiedAsync()
    {
        var snapshot = await _snapshotOperations.GetPublishedAsync(HttpContext.RequestAborted);
        if (snapshot is null)
        {
            Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            return true;
        }

        _snapshotVersion = snapshot.SnapshotVersion;
        var etag = $"\"{snapshot.SnapshotVersion}\"";
        var lastModified = snapshot.GeneratedAtUtc.ToUniversalTime();
        var snapshotAge = Math.Max(0, (DateTimeOffset.UtcNow - lastModified).TotalSeconds);
        Response.Headers.ETag = etag;
        Response.Headers.LastModified = lastModified.ToString("R", System.Globalization.CultureInfo.InvariantCulture);
        Response.Headers["X-Async-Snapshot-Generated-At"] = lastModified.ToString("O", System.Globalization.CultureInfo.InvariantCulture);
        Response.Headers["X-Async-Snapshot-Age-Seconds"] = snapshotAge.ToString("F0", System.Globalization.CultureInfo.InvariantCulture);

        var ifNoneMatch = Request.Headers.IfNoneMatch.ToString();
        var modifiedSince = Request.GetTypedHeaders().IfModifiedSince;

        // If-None-Match takes precedence over If-Modified-Since when both are sent.
        var isNotModified = AsyncSnapshotHttpValidator.IsNotModified(ifNoneMatch, modifiedSince, etag, lastModified);
        if (isNotModified)
            Response.StatusCode = StatusCodes.Status304NotModified;

        return isNotModified;
    }

    private sealed class EndpointTelemetry(
        ILogger logger,
        string category,
        string filter,
        int? limit,
        Func<long?> snapshotVersion) : IDisposable
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public void Dispose()
        {
            logger.LogInformation("Async statistics endpoint completed; SnapshotVersion={SnapshotVersion}, Category={Category}, Filter={Filter}, Limit={Limit}, DurationMilliseconds={DurationMilliseconds}", snapshotVersion(), category, filter, limit, _stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
