using System.Globalization;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Core.Interfaces.DraftRooms;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Applies the rules for joining or re-entering a draft room.
/// </summary>
public sealed class JoinDraftRoomCommandHandler(
    IDraftRoomRepository draftRoomRepository,
    IJoinAttemptThrottle joinAttemptThrottle,
    IGuestSecretService guestSecretService,
    IRoomPasswordHasher roomPasswordHasher,
    IDraftRoomDisplayNameValidator displayNameValidator,
    IDraftRoomLog draftRoomLog,
    IRoomChangeNotifier roomChangeNotifier,
    TimeProvider timeProvider,
    ILogger<JoinDraftRoomCommandHandler> logger)
    : IRequestHandler<JoinDraftRoomCommand, Result<DraftRoomEntryDto>>
{
    private readonly IDraftRoomRepository _draftRoomRepository = draftRoomRepository
        ?? throw new ArgumentNullException(nameof(draftRoomRepository));

    private readonly IJoinAttemptThrottle _joinAttemptThrottle = joinAttemptThrottle
        ?? throw new ArgumentNullException(nameof(joinAttemptThrottle));

    private readonly IGuestSecretService _guestSecretService = guestSecretService
        ?? throw new ArgumentNullException(nameof(guestSecretService));

    private readonly IRoomPasswordHasher _roomPasswordHasher = roomPasswordHasher
        ?? throw new ArgumentNullException(nameof(roomPasswordHasher));

    private readonly IDraftRoomDisplayNameValidator _displayNameValidator = displayNameValidator
        ?? throw new ArgumentNullException(nameof(displayNameValidator));

    private readonly IDraftRoomLog _draftRoomLog = draftRoomLog
        ?? throw new ArgumentNullException(nameof(draftRoomLog));

    private readonly IRoomChangeNotifier _roomChangeNotifier = roomChangeNotifier
        ?? throw new ArgumentNullException(nameof(roomChangeNotifier));

    private readonly TimeProvider _timeProvider = timeProvider
        ?? throw new ArgumentNullException(nameof(timeProvider));

    private readonly ILogger<JoinDraftRoomCommandHandler> _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public async Task<Result<DraftRoomEntryDto>> Handle(
        JoinDraftRoomCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Request);
        ArgumentNullException.ThrowIfNull(request.Caller);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.ClientIp);

        var roomCode = request.Request.Code?.Trim().ToUpperInvariant() ?? string.Empty;
        if (this._joinAttemptThrottle.IsBlocked(request.ClientIp, roomCode))
        {
            this._logger.LogWarning(
                "DraftRoomJoinThrottled RoomId={RoomId} Code={Code}",
                (Guid?)null,
                roomCode);

            return this.Failure(DraftRoomErrorCode.JoinAttemptsThrottled);
        }

        if (!DraftRoomCallerValidator.IsValid(request.Caller))
        {
            return this.FailedAttempt(request.ClientIp, roomCode, DraftRoomErrorCode.InvalidCaller);
        }

        if (string.IsNullOrWhiteSpace(roomCode))
        {
            this._roomPasswordHasher.VerifyDummyPassword(request.Request.Password ?? string.Empty);

            return this.FailedAttempt(
                request.ClientIp,
                roomCode,
                DraftRoomErrorCode.NotFoundOrWrongPassword);
        }

        var room = await this._draftRoomRepository
            .GetRoomByCodeAsync(roomCode, cancellationToken)
            .ConfigureAwait(false);

        if (room is null)
        {
            this._roomPasswordHasher.VerifyDummyPassword(request.Request.Password ?? string.Empty);

            return this.FailedAttempt(
                request.ClientIp,
                roomCode,
                DraftRoomErrorCode.NotFoundOrWrongPassword);
        }

        var knownParticipant = await this.FindKnownParticipantAsync(
            room.Id,
            request.Caller,
            cancellationToken).ConfigureAwait(false);

        if (knownParticipant is not null)
        {
            if (request.Caller.UserId is null
                && (knownParticipant.SecretHash is null
                    || !this._guestSecretService.Verify(
                        request.Caller.GuestSecret!,
                        knownParticipant.SecretHash)))
            {
                this._roomPasswordHasher.VerifyDummyPassword(request.Request.Password ?? string.Empty);

                return this.FailedAttempt(
                    request.ClientIp,
                    roomCode,
                    DraftRoomErrorCode.NotFoundOrWrongPassword,
                    room.Id);
            }

            if (knownParticipant.Status == DraftParticipantStatus.Kicked)
            {
                return this.FailedAttempt(
                    request.ClientIp,
                    roomCode,
                    DraftRoomErrorCode.ParticipantKicked,
                    room.Id);
            }

            if (knownParticipant.Status == DraftParticipantStatus.Left)
            {
                var reenteredAtUtc = this._timeProvider.GetUtcNow().UtcDateTime;
                DraftRoomLogEntry? joinLogEntry = null;
                try
                {
                    var reactivated = await this._draftRoomRepository
                        .ReactivateParticipantAsync(
                            room.Id,
                            knownParticipant.Id,
                            reenteredAtUtc,
                            cancellationToken,
                            scope =>
                            {
                                joinLogEntry = this._draftRoomLog.Append(
                                    scope,
                                    DraftLogEntryType.ParticipantJoined,
                                    knownParticipant.Id,
                                    new
                                    {
                                        knownParticipant.Id,
                                        knownParticipant.DisplayName,
                                        Role = knownParticipant.Role.ToString(),
                                        Reentered = true,
                                    });
                            })
                        .ConfigureAwait(false);
                    if (!reactivated)
                    {
                        return this.FailedRoomOperation(
                            room.Id,
                            roomCode,
                            DraftRoomErrorCode.DuplicateDisplayName);
                    }

                    var createdLogEntry = joinLogEntry
                        ?? throw new InvalidOperationException(
                            "Participant re-entry did not produce its required activity entry.");
                    await this.NotifyLogEntryAddedAsync(room.Id, createdLogEntry).ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return this.FailedRoomOperation(
                        room.Id,
                        roomCode,
                        DraftRoomErrorCode.RoomChanged);
                }

                knownParticipant.Status = DraftParticipantStatus.Active;
                knownParticipant.LastSeenUtc = reenteredAtUtc;
            }

            this._joinAttemptThrottle.RegisterSuccess(request.ClientIp, roomCode);
            this._logger.LogInformation(
                "DraftRoomJoined RoomId={RoomId} Code={Code}",
                room.Id,
                roomCode);

            return Result.Ok(ToEntry(room, knownParticipant, null));
        }

        if (request.Caller.UserId is null && request.Caller.GuestParticipantId is not null)
        {
            this._roomPasswordHasher.VerifyDummyPassword(request.Request.Password ?? string.Empty);

            return this.FailedAttempt(
                request.ClientIp,
                roomCode,
                DraftRoomErrorCode.NotFoundOrWrongPassword,
                room.Id);
        }

        var password = request.Request.Password ?? string.Empty;
        if (!this._roomPasswordHasher.Verify(room, room.PasswordHash, password))
        {
            return this.FailedAttempt(
                request.ClientIp,
                roomCode,
                DraftRoomErrorCode.NotFoundOrWrongPassword,
                room.Id);
        }

        if (room.IsLocked)
        {
            return this.FailedAttempt(
                request.ClientIp,
                roomCode,
                DraftRoomErrorCode.RoomLocked,
                room.Id);
        }

        if (await this._draftRoomRepository
                .CountActiveParticipantsAsync(room.Id, cancellationToken)
                .ConfigureAwait(false) >= DraftRoomLimits.MaxActiveParticipants)
        {
            return this.FailedAttempt(
                request.ClientIp,
                roomCode,
                DraftRoomErrorCode.RoomFull,
                room.Id);
        }

        var displayNameResult = await this.GetDisplayNameAsync(
            room.Id,
            request.Caller,
            request.Request.DisplayName,
            cancellationToken).ConfigureAwait(false);
        if (displayNameResult.ErrorCode is not null)
        {
            return this.FailedAttempt(
                request.ClientIp,
                roomCode,
                displayNameResult.ErrorCode.Value,
                room.Id);
        }

        var now = this._timeProvider.GetUtcNow().UtcDateTime;
        var guestSecret = request.Caller.UserId is null
            ? this._guestSecretService.Generate()
            : null;
        var participant = new DraftRoomParticipant
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            UserId = request.Caller.UserId,
            SecretHash = guestSecret is null ? null : this._guestSecretService.Hash(guestSecret),
            DisplayName = displayNameResult.DisplayName!,
            DisplayNameKey = this._displayNameValidator.MakeKey(displayNameResult.DisplayName!),
            Role = DraftRoomRole.Member,
            Status = DraftParticipantStatus.Active,
            JoinedAtUtc = now,
            LastSeenUtc = now,
        };

        try
        {
            DraftRoomLogEntry? joinLogEntry = null;
            await this._draftRoomRepository
                .AddParticipantAsync(
                    participant,
                    now,
                    cancellationToken,
                    scope =>
                    {
                        joinLogEntry = this._draftRoomLog.Append(
                            scope,
                            DraftLogEntryType.ParticipantJoined,
                            participant.Id,
                            new
                            {
                                participant.Id,
                                participant.DisplayName,
                                Role = participant.Role.ToString(),
                                Reentered = false,
                            });
                    })
                .ConfigureAwait(false);
            var createdLogEntry = joinLogEntry
                ?? throw new InvalidOperationException("Participant join did not produce its required activity entry.");
            await this.NotifyLogEntryAddedAsync(room.Id, createdLogEntry).ConfigureAwait(false);
        }
        catch (DbUpdateConcurrencyException)
        {
            return this.FailedRoomOperation(
                room.Id,
                roomCode,
                DraftRoomErrorCode.RoomChanged);
        }

        this._joinAttemptThrottle.RegisterSuccess(request.ClientIp, roomCode);
        this._logger.LogInformation(
            "DraftRoomJoined RoomId={RoomId} Code={Code}",
            room.Id,
            roomCode);

        return Result.Ok(ToEntry(room, participant, guestSecret));
    }

    private static DraftRoomEntryDto ToEntry(
        DraftRoom room,
        DraftRoomParticipant participant,
        string? guestSecret)
    {
        return new DraftRoomEntryDto
        {
            RoomId = room.Id,
            Code = room.Code,
            RoomName = room.Name,
            ParticipantId = participant.Id,
            DisplayName = participant.DisplayName,
            Role = participant.Role,
            IsGuest = participant.UserId is null,
            GuestSecret = guestSecret,
        };
    }

    private Task NotifyLogEntryAddedAsync(Guid roomId, DraftRoomLogEntry entry)
    {
        return this._roomChangeNotifier.NotifyLogEntryAddedAsync(roomId, new DraftLogEntryDto
        {
            Id = entry.Id,
            ActorParticipantId = entry.ActorParticipantId,
            Type = entry.Type,
            DataJson = entry.DataJson,
            CreatedAtUtc = entry.CreatedAtUtc,
        });
    }

    private Result<DraftRoomEntryDto> FailedAttempt(
        string clientIp,
        string roomCode,
        DraftRoomErrorCode errorCode,
        Guid? roomId = null)
    {
        this._joinAttemptThrottle.RegisterFailure(clientIp, roomCode);
        this._logger.LogWarning(
            "DraftRoomJoinFailed RoomId={RoomId} Code={Code}",
            roomId,
            roomCode);

        return this.Failure(errorCode);
    }

    private Result<DraftRoomEntryDto> FailedRoomOperation(
        Guid roomId,
        string roomCode,
        DraftRoomErrorCode errorCode)
    {
        this._logger.LogWarning(
            "DraftRoomJoinFailed RoomId={RoomId} Code={Code}",
            roomId,
            roomCode);

        return this.Failure(errorCode);
    }

    private async Task<DraftRoomParticipant?> FindKnownParticipantAsync(
        Guid roomId,
        DraftRoomCaller caller,
        CancellationToken cancellationToken)
    {
        if (caller.UserId is not null)
        {
            return await this._draftRoomRepository
                .FindParticipantByUserAsync(roomId, caller.UserId, cancellationToken)
                .ConfigureAwait(false);
        }

        if (caller.GuestParticipantId is not null)
        {
            return await this._draftRoomRepository
                .FindParticipantByIdAsync(roomId, caller.GuestParticipantId.Value, cancellationToken)
                .ConfigureAwait(false);
        }

        return null;
    }

    private async Task<(string? DisplayName, DraftRoomErrorCode? ErrorCode)> GetDisplayNameAsync(
        Guid roomId,
        DraftRoomCaller caller,
        string? requestedDisplayName,
        CancellationToken cancellationToken)
    {
        if (caller.UserId is null)
        {
            var guestName = this._displayNameValidator.Normalize(requestedDisplayName ?? string.Empty);
            if (!await this._displayNameValidator
                    .ValidateGuestName(requestedDisplayName ?? string.Empty)
                    .ConfigureAwait(false))
            {
                return (null, DraftRoomErrorCode.InvalidGuestDisplayName);
            }

            var guestKey = this._displayNameValidator.MakeKey(guestName);
            if (await this._draftRoomRepository
                    .IsDisplayNameTakenAsync(roomId, guestKey, cancellationToken)
                    .ConfigureAwait(false))
            {
                return (null, DraftRoomErrorCode.DuplicateDisplayName);
            }

            return (guestName, null);
        }

        var accountName = caller.UserName!;
        for (var suffix = 0; suffix <= DraftRoomLimits.MaxActiveParticipants; suffix++)
        {
            var candidate = suffix == 0
                ? accountName
                : string.Concat(accountName, (suffix + 1).ToString(CultureInfo.InvariantCulture));
            var key = this._displayNameValidator.MakeKey(candidate);
            if (!await this._draftRoomRepository
                    .IsDisplayNameTakenAsync(roomId, key, cancellationToken)
                    .ConfigureAwait(false))
            {
                return (candidate, null);
            }
        }

        return (null, DraftRoomErrorCode.DuplicateDisplayName);
    }

    private Result<DraftRoomEntryDto> Failure(DraftRoomErrorCode code)
    {
        return Result.Fail<DraftRoomEntryDto>(new DraftRoomError(code));
    }
}
