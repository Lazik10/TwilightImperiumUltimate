using FluentResults;
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
/// Applies the rules for creating a draft room and its host participant.
/// </summary>
public sealed class CreateDraftRoomCommandHandler(
    IDraftRoomRepository draftRoomRepository,
    IRoomCodeGenerator roomCodeGenerator,
    IGuestSecretService guestSecretService,
    IRoomPasswordHasher roomPasswordHasher,
    IDraftRoomDisplayNameValidator displayNameValidator,
    IDraftRoomLog draftRoomLog,
    IDraftRoomDraftPersistence draftPersistence,
    IRoomChangeNotifier roomChangeNotifier,
    TimeProvider timeProvider,
    ILogger<CreateDraftRoomCommandHandler> logger)
    : IRequestHandler<CreateDraftRoomCommand, Result<DraftRoomEntryDto>>
{
    private const int MaxRoomCodeAttempts = 5;

    private readonly IDraftRoomRepository _draftRoomRepository = draftRoomRepository
        ?? throw new ArgumentNullException(nameof(draftRoomRepository));

    private readonly IRoomCodeGenerator _roomCodeGenerator = roomCodeGenerator
        ?? throw new ArgumentNullException(nameof(roomCodeGenerator));

    private readonly IGuestSecretService _guestSecretService = guestSecretService
        ?? throw new ArgumentNullException(nameof(guestSecretService));

    private readonly IRoomPasswordHasher _roomPasswordHasher = roomPasswordHasher
        ?? throw new ArgumentNullException(nameof(roomPasswordHasher));

    private readonly IDraftRoomDisplayNameValidator _displayNameValidator = displayNameValidator
        ?? throw new ArgumentNullException(nameof(displayNameValidator));

    private readonly IDraftRoomLog _draftRoomLog = draftRoomLog
        ?? throw new ArgumentNullException(nameof(draftRoomLog));

    private readonly IDraftRoomDraftPersistence _draftPersistence = draftPersistence
        ?? throw new ArgumentNullException(nameof(draftPersistence));

    private readonly IRoomChangeNotifier _roomChangeNotifier = roomChangeNotifier
        ?? throw new ArgumentNullException(nameof(roomChangeNotifier));

    private readonly TimeProvider _timeProvider = timeProvider
        ?? throw new ArgumentNullException(nameof(timeProvider));

    private readonly ILogger<CreateDraftRoomCommandHandler> _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public async Task<Result<DraftRoomEntryDto>> Handle(
        CreateDraftRoomCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Request);
        ArgumentNullException.ThrowIfNull(request.Caller);

        var roomName = request.Request.Name?.Trim();
        if (string.IsNullOrWhiteSpace(roomName)
            || roomName.Length < DraftRoomLimits.MinRoomNameLength
            || roomName.Length > DraftRoomLimits.MaxRoomNameLength)
        {
            return Failure(DraftRoomErrorCode.InvalidRoomName);
        }

        var password = request.Request.Password;
        if (string.IsNullOrEmpty(password)
            || password.Length < DraftRoomLimits.MinPasswordLength
            || password.Length > DraftRoomLimits.MaxPasswordLength)
        {
            return Failure(DraftRoomErrorCode.InvalidPassword);
        }

        var isAccount = request.Caller.UserId is not null;
        if (isAccount
            && (string.IsNullOrWhiteSpace(request.Caller.UserId)
                || string.IsNullOrWhiteSpace(request.Caller.UserName)))
        {
            return Failure(DraftRoomErrorCode.InvalidCaller);
        }

        if (!isAccount
            && (request.Caller.UserName is not null
                || request.Caller.GuestParticipantId is not null
                || request.Caller.GuestSecret is not null))
        {
            return Failure(DraftRoomErrorCode.InvalidCaller);
        }

        if (isAccount
            && await this._draftRoomRepository
                .CountOpenRoomsForUserAsync(request.Caller.UserId!, cancellationToken)
                .ConfigureAwait(false) >= DraftRoomLimits.MaxOpenRoomsPerUser)
        {
            return Failure(DraftRoomErrorCode.OpenRoomLimitReached);
        }

        var displayName = isAccount
            ? request.Caller.UserName!
            : this._displayNameValidator.Normalize(request.Request.DisplayName ?? string.Empty);

        if (!isAccount
            && !await this._displayNameValidator
                .ValidateGuestName(request.Request.DisplayName ?? string.Empty)
                .ConfigureAwait(false))
        {
            return Failure(DraftRoomErrorCode.InvalidGuestDisplayName);
        }

        string? guestSecret = null;
        Guid? guestParticipantId = null;
        byte[]? guestSecretHash = null;

        if (!isAccount)
        {
            guestParticipantId = Guid.NewGuid();
            guestSecret = this._guestSecretService.Generate();
            guestSecretHash = this._guestSecretService.Hash(guestSecret);
        }

        var roomCode = await this.GenerateUniqueRoomCodeAsync(cancellationToken).ConfigureAwait(false);
        if (roomCode is null)
        {
            return Failure(DraftRoomErrorCode.RoomCodeGenerationFailed);
        }

        var now = this._timeProvider.GetUtcNow().UtcDateTime;
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = roomCode,
            Name = roomName,
            Status = DraftRoomStatus.Lobby,
            DraftType = nameof(DraftTypeId.FactionPick),
            IsLocked = false,
            Version = 1,
            CreatedAtUtc = now,
            LastActivityUtc = now,
        };

        room.PasswordHash = this._roomPasswordHasher.Hash(room, password);

        var hostParticipant = new DraftRoomParticipant
        {
            Id = guestParticipantId ?? Guid.NewGuid(),
            RoomId = room.Id,
            UserId = isAccount ? request.Caller.UserId : null,
            SecretHash = guestSecretHash,
            DisplayName = displayName,
            DisplayNameKey = this._displayNameValidator.MakeKey(displayName),
            Role = DraftRoomRole.Host,
            Status = DraftParticipantStatus.Active,
            JoinedAtUtc = now,
            LastSeenUtc = now,
        };

        for (var seatIndex = 1; seatIndex <= DraftRoomLimits.DefaultSeatCount; seatIndex++)
        {
            var seatParticipant = seatIndex == 1 ? hostParticipant : null;
            room.Seats.Add(new DraftRoomSeat
            {
                Id = Guid.NewGuid(),
                RoomId = room.Id,
                Index = seatIndex,
                ParticipantId = seatParticipant?.Id,
                ClaimedAtUtc = seatParticipant is null ? null : now,
                Room = room,
                Participant = seatParticipant,
            });
        }

        DraftRoomLogEntry? roomCreatedLogEntry = null;
        await this._draftRoomRepository
            .CreateRoomWithHostAsync(
                room,
                hostParticipant,
                cancellationToken,
                scope =>
                {
                    this._draftPersistence.InitializeDefaults(scope, DraftTypeId.FactionPick);
                    roomCreatedLogEntry = this._draftRoomLog.Append(
                        scope,
                        DraftLogEntryType.RoomCreated,
                        hostParticipant.Id,
                        new
                        {
                            room.Id,
                            room.Code,
                            room.Name,
                            HostDisplayName = hostParticipant.DisplayName,
                        });
                })
            .ConfigureAwait(false);

        var createdLogEntry = roomCreatedLogEntry
            ?? throw new InvalidOperationException("Room creation did not produce its required activity entry.");
        await this._roomChangeNotifier
            .NotifyLogEntryAddedAsync(room.Id, new DraftLogEntryDto
            {
                Id = createdLogEntry.Id,
                ActorParticipantId = createdLogEntry.ActorParticipantId,
                Type = createdLogEntry.Type,
                DataJson = createdLogEntry.DataJson,
                CreatedAtUtc = createdLogEntry.CreatedAtUtc,
            })
            .ConfigureAwait(false);

        this._logger.LogInformation(
            "DraftRoomCreated RoomId={RoomId} Code={Code}",
            room.Id,
            room.Code);

        return Result.Ok(new DraftRoomEntryDto
        {
            RoomId = room.Id,
            Code = room.Code,
            RoomName = room.Name,
            ParticipantId = hostParticipant.Id,
            DisplayName = hostParticipant.DisplayName,
            Role = hostParticipant.Role,
            IsGuest = !isAccount,
            GuestSecret = guestSecret,
        });
    }

    private Result<DraftRoomEntryDto> Failure(DraftRoomErrorCode code)
    {
        return Result.Fail<DraftRoomEntryDto>(new DraftRoomError(code));
    }

    private async Task<string?> GenerateUniqueRoomCodeAsync(CancellationToken cancellationToken)
    {
        for (var attempt = 0; attempt < MaxRoomCodeAttempts; attempt++)
        {
            var code = this._roomCodeGenerator.Generate();
            if (!await this._draftRoomRepository.CodeExistsAsync(code, cancellationToken).ConfigureAwait(false))
            {
                return code;
            }
        }

        return null;
    }
}
