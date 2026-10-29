using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.API.Hubs;

/// <summary>
/// Provides the real-time connection endpoint for Draft Rooms.
/// </summary>
public sealed class DraftRoomHub(
    IMediator mediator,
    IPresenceTracker presenceTracker)
    : Hub
{
    private readonly IMediator _mediator = mediator
        ?? throw new ArgumentNullException(nameof(mediator));

    private readonly IPresenceTracker _presenceTracker = presenceTracker
        ?? throw new ArgumentNullException(nameof(presenceTracker));

    /// <summary>
    /// Returns the current server time to verify hub connectivity.
    /// </summary>
    /// <returns>The current UTC time.</returns>
    public DateTimeOffset Ping() => DateTimeOffset.UtcNow;

    /// <summary>
    /// Subscribes an authorized account or guest participant to room updates.
    /// </summary>
    /// <param name="code">The public room code.</param>
    /// <param name="credentials">Guest credentials; authenticated accounts are resolved from claims.</param>
    /// <returns>The room version observed while subscribing.</returns>
    /// <exception cref="HubException">The room is unavailable or the caller is not an active participant.</exception>
    public async Task<int> Subscribe(string code, DraftRoomCredentials credentials)
    {
        if (string.IsNullOrWhiteSpace(code) || credentials is null)
        {
            throw new HubException("Room not found or credentials invalid.");
        }

        var normalizedCode = code.Trim().ToUpperInvariant();
        var caller = this.ResolveCaller(credentials);
        if (caller is null)
        {
            throw new HubException("Room not found or credentials invalid.");
        }

        var roomResult = await this._mediator
            .Send(new GetDraftRoomViewQuery(normalizedCode, caller), this.Context.ConnectionAborted)
            .ConfigureAwait(false);
        if (roomResult.IsFailed)
        {
            throw new HubException("Room not found or credentials invalid.");
        }

        var room = roomResult.Value;
        await this.Groups
            .AddToGroupAsync(this.Context.ConnectionId, GetRoomGroupName(room.RoomId), this.Context.ConnectionAborted)
            .ConfigureAwait(false);
        await this.Groups
            .AddToGroupAsync(
                this.Context.ConnectionId,
                GetParticipantGroupName(room.RoomId, room.Me.ParticipantId),
                this.Context.ConnectionAborted)
            .ConfigureAwait(false);
        this._presenceTracker.AddConnection(room.RoomId, room.Me.ParticipantId, this.Context.ConnectionId);

        return room.Version;
    }

    /// <inheritdoc />
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        this._presenceTracker.RemoveConnection(this.Context.ConnectionId);
        await base.OnDisconnectedAsync(exception).ConfigureAwait(false);
    }

    internal static string GetRoomGroupName(Guid roomId) => $"room:{roomId}";

    internal static string GetParticipantGroupName(Guid roomId, Guid participantId)
    {
        return $"participant:{roomId}:{participantId}";
    }

    private DraftRoomCaller? ResolveCaller(DraftRoomCredentials credentials)
    {
        if (credentials.GuestParticipantId is not null || credentials.GuestSecret is not null)
        {
            return new DraftRoomCaller(
                null,
                null,
                credentials.GuestParticipantId,
                credentials.GuestSecret);
        }

        var user = this.Context.User;
        if (user?.Identity?.IsAuthenticated is true)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = user.FindFirst(ClaimTypes.Name)?.Value;

            return string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(userName)
                ? null
                : new DraftRoomCaller(userId, userName, null, null);
        }

        return new DraftRoomCaller(null, null, null, null);
    }
}
