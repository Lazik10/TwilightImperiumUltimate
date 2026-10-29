using System.Security.Claims;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.RateLimiting;
using TwilightImperiumUltimate.API.Helpers;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.API.Controllers;

/// <summary>
/// Handles draft-room creation, membership, and management operations.
/// </summary>
[ApiController]
[AllowAnonymous]
[Route("api/draft-rooms")]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public sealed class DraftRoomsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator
        ?? throw new ArgumentNullException(nameof(mediator));

    /// <summary>
    /// Creates a draft room for an account or guest.
    /// </summary>
    /// <param name="request">The room name, password, and optional guest display name.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The created room and the caller's participant credentials.</returns>
    [HttpPost]
    [EnableRateLimiting("draft-room-create")]
    [ProducesResponseType(typeof(DraftRoomEntryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<DraftRoomEntryDto>> Create(
        [FromBody] CreateDraftRoomRequest request,
        CancellationToken cancellationToken)
    {
        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        var result = await this._mediator
            .Send(new CreateDraftRoomCommand(request, caller), cancellationToken)
            .ConfigureAwait(false);

        return result.IsSuccess
            ? this.Ok(result.Value)
            : this.ToProblem(result.Errors);
    }

    /// <summary>
    /// Joins a draft room or re-enters as a known participant.
    /// </summary>
    /// <param name="request">The room code, password, and optional guest display name.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The room and the caller's participant credentials.</returns>
    [HttpPost("join")]
    [ProducesResponseType(typeof(DraftRoomEntryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<DraftRoomEntryDto>> Join(
        [FromBody] JoinDraftRoomRequest request,
        CancellationToken cancellationToken)
    {
        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrWhiteSpace(clientIp))
        {
            return this.Problem(
                statusCode: StatusCodes.Status503ServiceUnavailable,
                title: "Unable to determine the client address.");
        }

        var result = await this._mediator
            .Send(new JoinDraftRoomCommand(request, caller, clientIp), cancellationToken)
            .ConfigureAwait(false);

        return result.IsSuccess
            ? this.Ok(result.Value)
            : this.ToProblem(result.Errors);
    }

    /// <summary>
    /// Links the authenticated account to the caller's verified guest participant.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>No content when the identities are linked.</returns>
    [HttpPost("{code}/link-account")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> LinkAccount(string code, CancellationToken cancellationToken)
    {
        if (User.Identity?.IsAuthenticated is not true)
        {
            return this.Unauthorized();
        }

        var caller = DraftRoomCallerResolver.ResolveForAccountLink(User, Request.Headers);
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        return await this.ExecuteCommand(
                new LinkAccountCommand(code, caller),
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the authenticated account's active draft-room memberships.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>Up to 50 active room memberships, ordered by latest room activity.</returns>
    [HttpGet("mine")]
    [ProducesResponseType(typeof(IReadOnlyList<MyDraftRoomDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<MyDraftRoomDto>>> GetMine(
        CancellationToken cancellationToken)
    {
        if (User.Identity?.IsAuthenticated is not true)
        {
            return this.Unauthorized();
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return this.InvalidCallerProblem();
        }

        var result = await this._mediator
            .Send(new GetMyRoomsQuery(userId), cancellationToken)
            .ConfigureAwait(false);

        return result.IsSuccess
            ? this.Ok(result.Value)
            : this.ToProblem(result.Errors);
    }

    /// <summary>
    /// Gets the personalized view of a draft room.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The room view for the requesting participant.</returns>
    [HttpGet("{code}")]
    [ProducesResponseType(typeof(DraftRoomViewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DraftRoomViewDto>> Get(
        string code,
        CancellationToken cancellationToken)
    {
        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        var result = await this._mediator
            .Send(new GetDraftRoomViewQuery(code, caller), cancellationToken)
            .ConfigureAwait(false);

        return result.IsSuccess
            ? this.Ok(result.Value)
            : this.ToProblem(result.Errors);
    }

    /// <summary>
    /// Gets the latest chat messages or messages after the supplied cursor.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="afterId">An optional exclusive message cursor.</param>
    /// <param name="limit">The maximum number of messages to return, up to 200.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>Chat messages visible to the requesting participant.</returns>
    [HttpGet("{code}/chat")]
    [ProducesResponseType(typeof(IReadOnlyList<ChatMessageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<ChatMessageDto>>> GetChat(
        string code,
        [FromQuery] long? afterId,
        [FromQuery] int limit = DraftRoomLimits.DefaultHistoryPageSize,
        CancellationToken cancellationToken = default)
    {
        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        var result = await this._mediator
            .Send(new GetDraftRoomChatHistoryQuery(code, afterId, limit, caller), cancellationToken)
            .ConfigureAwait(false);

        return result.IsSuccess
            ? this.Ok(result.Value)
            : this.ToProblem(result.Errors);
    }

    /// <summary>
    /// Sends a chat message to the draft room.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="request">The message text.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The persisted chat message.</returns>
    [HttpPost("{code}/chat")]
    [ProducesResponseType(typeof(ChatMessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<ChatMessageDto>> SendChat(
        string code,
        [FromBody] SendDraftRoomChatMessageRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        var result = await this._mediator
            .Send(new SendChatCommand(code, request.Text, caller), cancellationToken)
            .ConfigureAwait(false);

        return result.IsSuccess
            ? this.Ok(result.Value)
            : this.ToProblem(result.Errors);
    }

    /// <summary>
    /// Gets the latest activity entries or entries after the supplied cursor.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="afterId">An optional exclusive entry cursor.</param>
    /// <param name="limit">The maximum number of entries to return, up to 200.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>Activity entries visible to the requesting participant.</returns>
    [HttpGet("{code}/log")]
    [ProducesResponseType(typeof(IReadOnlyList<DraftLogEntryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<DraftLogEntryDto>>> GetLog(
        string code,
        [FromQuery] long? afterId,
        [FromQuery] int limit = DraftRoomLimits.DefaultHistoryPageSize,
        CancellationToken cancellationToken = default)
    {
        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        var result = await this._mediator
            .Send(new GetDraftRoomLogHistoryQuery(code, afterId, limit, caller), cancellationToken)
            .ConfigureAwait(false);

        return result.IsSuccess
            ? this.Ok(result.Value)
            : this.ToProblem(result.Errors);
    }

    /// <summary>
    /// Adds a seat to a draft room.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>No content when the seat was added.</returns>
    [HttpPost("{code}/seats")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddSeat(string code, CancellationToken cancellationToken)
    {
        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        return await this.ExecuteCommand(new AddSeatCommand(code, caller), cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a seat from a draft room.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="seatId">The identifier of the seat to remove.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>No content when the seat was removed.</returns>
    [HttpDelete("{code}/seats/{seatId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RemoveSeat(
        string code,
        Guid seatId,
        CancellationToken cancellationToken)
    {
        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        return await this.ExecuteCommand(new RemoveSeatCommand(code, seatId, caller), cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Claims or moves the requesting participant to a seat.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="seatId">The identifier of the seat to claim.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>No content when the seat was claimed.</returns>
    [HttpPost("{code}/seats/{seatId:guid}/claim")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ClaimSeat(
        string code,
        Guid seatId,
        CancellationToken cancellationToken)
    {
        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        return await this.ExecuteCommand(new ClaimSeatCommand(code, seatId, caller), cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Unclaims an occupied seat.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="seatId">The identifier of the seat to unclaim.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>No content when the seat was unclaimed.</returns>
    [HttpPost("{code}/seats/{seatId:guid}/unclaim")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UnclaimSeat(
        string code,
        Guid seatId,
        CancellationToken cancellationToken)
    {
        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        return await this.ExecuteCommand(new UnclaimSeatCommand(code, seatId, caller), cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Marks the requesting participant as having left a draft room.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>No content when the participant left the room.</returns>
    [HttpPost("{code}/leave")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Leave(string code, CancellationToken cancellationToken)
    {
        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        return await this.ExecuteCommand(new LeaveRoomCommand(code, caller), cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Kicks a participant from a draft room.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="participantId">The identifier of the participant to kick.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>No content when the participant was kicked.</returns>
    [HttpPost("{code}/participants/{participantId:guid}/kick")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> KickParticipant(
        string code,
        Guid participantId,
        CancellationToken cancellationToken)
    {
        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        return await this.ExecuteCommand(new KickParticipantCommand(code, participantId, caller), cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Changes a participant's role.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="participantId">The identifier of the participant to update.</param>
    /// <param name="request">The participant's requested new role.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>No content when the role was changed.</returns>
    [HttpPut("{code}/participants/{participantId:guid}/role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SetParticipantRole(
        string code,
        Guid participantId,
        [FromBody] SetParticipantRoleRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        return await this.ExecuteCommand(
                new SetParticipantRoleCommand(code, participantId, request.Role, caller),
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Updates a draft room's name and join lock.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="request">The new room settings.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>No content when the settings were changed.</returns>
    [HttpPut("{code}/settings")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateSettings(
        string code,
        [FromBody] UpdateRoomSettingsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        return await this.ExecuteCommand(
                new UpdateRoomSettingsCommand(code, request.Name, request.IsLocked, caller, request.DraftType),
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Replaces the password required for new room participants.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="request">The new room password.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>No content when the password was changed.</returns>
    [HttpPut("{code}/password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangePassword(
        string code,
        [FromBody] ChangeRoomPasswordRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        return await this.ExecuteCommand(
                new ChangeRoomPasswordCommand(code, request.Password, caller),
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Closes and deletes a draft room.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>No content when the room was closed.</returns>
    [HttpDelete("{code}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Close(string code, CancellationToken cancellationToken)
    {
        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        return await this.ExecuteCommand(new CloseRoomCommand(code, caller), cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>Assigns an eligible active participant to a vacant seat.</summary>
    /// <param name="code">The room code.</param>
    /// <param name="seatId">The vacant seat.</param>
    /// <param name="request">The replacement participant.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content, or Problem Details when assignment is refused.</returns>
    [HttpPost("{code}/seats/{seatId:guid}/assign")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AssignSeat(
        string code,
        Guid seatId,
        [FromBody] AssignSeatRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var caller = this.ResolveCaller();
        return caller is null
            ? this.InvalidCallerProblem()
            : await this.ExecuteCommand(new AssignSeatCommand(code, seatId, request.ParticipantId, caller), cancellationToken);
    }

    /// <summary>Updates the selected draft engine's rules in the lobby.</summary>
    /// <param name="code">The room code.</param>
    /// <param name="settings">The polymorphic settings payload.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content on success, or Problem Details with validation codes.</returns>
    [HttpPut("{code}/draft/settings")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateDraftSettings(
        string code,
        [FromBody] DraftSettingsDto settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);
        var caller = this.ResolveCaller();
        return caller is null
            ? this.InvalidCallerProblem()
            : await this.ExecuteCommand(new UpdateDraftSettingsCommand(code, settings, caller), cancellationToken);
    }

    /// <summary>Starts a ready lobby using its selected draft engine.</summary>
    /// <param name="code">The room code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content or Problem Details with blocker codes.</returns>
    [HttpPost("{code}/draft/start")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> StartDraft(string code, CancellationToken cancellationToken)
    {
        var caller = this.ResolveCaller();
        return caller is null
            ? this.InvalidCallerProblem()
            : await this.ExecuteCommand(new StartDraftCommand(code, caller), cancellationToken);
    }

    /// <summary>Resets a running draft without removing participants, settings or chat.</summary>
    /// <param name="code">The room code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content or Problem Details.</returns>
    [HttpPost("{code}/draft/reset")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ResetDraft(string code, CancellationToken cancellationToken)
    {
        var caller = this.ResolveCaller();
        return caller is null
            ? this.InvalidCallerProblem()
            : await this.ExecuteCommand(new ResetDraftCommand(code, caller), cancellationToken);
    }

    /// <summary>Applies a typed draft action using the authenticated participant's seat.</summary>
    /// <param name="code">The room code.</param>
    /// <param name="action">The polymorphic action.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content, or Problem Details on refusal.</returns>
    [HttpPost("{code}/draft/actions")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ApplyDraftAction(string code, [FromBody] DraftActionDto action, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(action);
        var caller = this.ResolveCaller();
        return caller is null
            ? this.InvalidCallerProblem()
            : await this.ExecuteCommand(new ApplyDraftActionCommand(code, action, caller), cancellationToken);
    }

    /// <summary>Discloses and replaces a seat's private choice as host or co-host.</summary>
    /// <param name="code">The room code.</param>
    /// <param name="request">The seat and explicit replacement choice.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content, or Problem Details on refusal.</returns>
    [HttpPost("{code}/draft/decide-for")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DecideFor(string code, [FromBody] DecideForRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var caller = this.ResolveCaller();
        return caller is null
            ? this.InvalidCallerProblem()
            : await this.ExecuteCommand(new DecideForCommand(code, request.SeatId, request.Action, caller), cancellationToken);
    }

    /// <summary>Undoes the last locked pick as host or co-host while picks are running.</summary>
    /// <param name="code">The room code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content, or Problem Details on refusal.</returns>
    [HttpPost("{code}/draft/undo")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UndoLastPick(string code, CancellationToken cancellationToken)
    {
        var caller = this.ResolveCaller();
        return caller is null
            ? this.InvalidCallerProblem()
            : await this.ExecuteCommand(new UndoLastPickCommand(code, caller), cancellationToken);
    }

    /// <summary>Reads a public result using its unguessable, case-sensitive share token.</summary>
    /// <param name="token">The share token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The public snapshot, or identical Problem Details for unknown and deleted results.</returns>
    [HttpGet("results/{token}")]
    [ProducesResponseType(typeof(DraftResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DraftResultDto>> GetResult(string token, CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(new GetResultByTokenQuery(token), cancellationToken).ConfigureAwait(false);
        return result.IsSuccess ? this.Ok(result.Value) : this.ToProblem(result.Errors);
    }

    /// <summary>Reads the finished room's snapshot for a verified participant.</summary>
    /// <param name="code">The room code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The public snapshot or Problem Details.</returns>
    [HttpGet("{code}/result")]
    [ProducesResponseType(typeof(DraftResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DraftResultDto>> GetRoomResult(string code, CancellationToken cancellationToken)
    {
        var caller = this.ResolveCaller();
        if (caller is null)
        {
            return this.InvalidCallerProblem();
        }

        var result = await this._mediator.Send(new GetRoomResultQuery(code, caller), cancellationToken).ConfigureAwait(false);
        return result.IsSuccess ? this.Ok(result.Value) : this.ToProblem(result.Errors);
    }

    /// <summary>Reads twenty finished drafts associated with the authenticated account.</summary>
    /// <param name="page">The one-based history page.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The account history and whether another page exists.</returns>
    [HttpGet("results/mine")]
    [ProducesResponseType(typeof(DraftResultHistoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<DraftResultHistoryDto>> GetMyResults([FromQuery] int page = 1, CancellationToken cancellationToken = default)
    {
        if (User.Identity?.IsAuthenticated is not true)
        {
            return this.Unauthorized();
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return this.InvalidCallerProblem();
        }

        var result = await this._mediator.Send(new GetMyResultsQuery(userId, page), cancellationToken).ConfigureAwait(false);
        return result.IsSuccess ? this.Ok(result.Value) : this.ToProblem(result.Errors);
    }

    /// <summary>Deletes a result as an active room manager, or as its owning account after room cleanup.</summary>
    /// <param name="id">The result identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content, or Problem Details on refusal.</returns>
    [HttpDelete("results/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteResult(Guid id, CancellationToken cancellationToken)
    {
        var caller = this.ResolveCaller();
        return caller is null
            ? this.InvalidCallerProblem()
            : await this.ExecuteCommand(new DeleteResultCommand(id, caller), cancellationToken);
    }

    private DraftRoomCaller? ResolveCaller()
    {
        return DraftRoomCallerResolver.Resolve(User, Request.Headers);
    }

    private ObjectResult InvalidCallerProblem()
    {
        return this.Problem(
            statusCode: StatusCodes.Status400BadRequest,
            title: "Invalid draft room caller.");
    }

    private async Task<IActionResult> ExecuteCommand(
        IRequest<Result> request,
        CancellationToken cancellationToken)
    {
        var result = await this._mediator.Send(request, cancellationToken).ConfigureAwait(false);
        return result.IsSuccess ? this.NoContent() : this.ToProblem(result.Errors);
    }

    private ObjectResult ToProblem(IEnumerable<IError> errors)
    {
        var roomErrors = errors.OfType<DraftRoomError>().ToArray();
        var draftRoomError = roomErrors.FirstOrDefault();
        var errorCode = draftRoomError?.Code;
        var statusCode = errorCode switch
        {
            DraftRoomErrorCode.InvalidRoomName
                or DraftRoomErrorCode.InvalidPassword
                or DraftRoomErrorCode.InvalidGuestDisplayName
                or DraftRoomErrorCode.InvalidCaller
                or DraftRoomErrorCode.InvalidRole
                or DraftRoomErrorCode.InvalidChatMessage
                or DraftRoomErrorCode.InvalidHistoryCursor
                or DraftRoomErrorCode.InvalidDraftType
                or DraftRoomErrorCode.InvalidDraftAction
                or DraftRoomErrorCode.SettingsInvalid
                or DraftRoomErrorCode.NotEnoughSeats
                or DraftRoomErrorCode.SeatsNotFull
                or DraftRoomErrorCode.InvalidHistoryLimit => StatusCodes.Status400BadRequest,
            DraftRoomErrorCode.NotFoundOrWrongPassword
                or DraftRoomErrorCode.ParticipantNotFound
                or DraftRoomErrorCode.ResultNotFound => StatusCodes.Status404NotFound,
            DraftRoomErrorCode.JoinAttemptsThrottled
                or DraftRoomErrorCode.ChatRateLimited => StatusCodes.Status429TooManyRequests,
            DraftRoomErrorCode.ParticipantKicked
                or DraftRoomErrorCode.Forbidden => StatusCodes.Status403Forbidden,
            DraftRoomErrorCode.OpenRoomLimitReached
                or DraftRoomErrorCode.RoomFull
                or DraftRoomErrorCode.RoomLocked
                or DraftRoomErrorCode.DuplicateDisplayName
                or DraftRoomErrorCode.AccountAlreadyInRoom
                or DraftRoomErrorCode.RoomChanged
                or DraftRoomErrorCode.SeatLimitReached
                or DraftRoomErrorCode.MinimumSeatCountReached
                or DraftRoomErrorCode.SeatNotFound
                or DraftRoomErrorCode.SeatOccupied
                or DraftRoomErrorCode.SeatNotOccupied => StatusCodes.Status409Conflict,
            DraftRoomErrorCode.RoomCodeGenerationFailed => StatusCodes.Status503ServiceUnavailable,
            _ => StatusCodes.Status500InternalServerError,
        };

        var title = statusCode >= StatusCodes.Status500InternalServerError
            ? "The draft room request could not be completed."
            : "The draft room request was rejected.";
        var problem = this.Problem(statusCode: statusCode, title: title);
        if (errorCode is not null && problem.Value is ProblemDetails problemDetails)
        {
            problemDetails.Extensions["code"] = errorCode.Value.ToString();
            problemDetails.Extensions["codes"] = roomErrors
                .SelectMany(error => new[] { error.Code.ToString() }.Concat(error.ValidationCodes))
                .Distinct(StringComparer.Ordinal)
                .ToArray();
        }

        return problem;
    }
}
