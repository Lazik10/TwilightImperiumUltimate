using FluentAssertions;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class JoinDraftRoomCommandHandlerTests
{
    private static readonly DateTimeOffset FixedUtcNow =
        new(2026, 10, 16, 0, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task Handle_WhenRoomCodeDoesNotExist_ShouldReturnSameErrorAsWrongPassword()
    {
        var context = new HandlerTestContext();
        context.RoomRepository
            .Setup(repository => repository.GetRoomByCodeAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync((DraftRoom?)null);
        context.PasswordHasher
            .Setup(hasher => hasher.VerifyDummyPassword("wrong"))
            .Returns(false);

        var result = await context.Handler.Handle(
            CreateCommand("ABC234", "wrong"),
            CancellationToken.None);

        GetErrorCode(result).Should().Be(DraftRoomErrorCode.NotFoundOrWrongPassword);
        context.PasswordHasher.Verify(hasher => hasher.VerifyDummyPassword("wrong"), Times.Once);
        context.Throttle.Verify(throttle => throttle.RegisterFailure("127.0.0.1", "ABC234"), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenPasswordIsWrong_ShouldReturnSameErrorAsUnknownRoomCode()
    {
        var context = new HandlerTestContext();
        var room = CreateRoom();
        context.RoomRepository
            .Setup(repository => repository.GetRoomByCodeAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);
        context.PasswordHasher
            .Setup(hasher => hasher.Verify(room, room.PasswordHash, "wrong"))
            .Returns(false);

        var result = await context.Handler.Handle(
            CreateCommand("ABC234", "wrong"),
            CancellationToken.None);

        GetErrorCode(result).Should().Be(DraftRoomErrorCode.NotFoundOrWrongPassword);
    }

    [Fact]
    public async Task Handle_WhenJoinAttemptsAreBlocked_ShouldNotLookUpRoom()
    {
        var context = new HandlerTestContext();
        context.Throttle
            .Setup(throttle => throttle.IsBlocked("127.0.0.1", "ABC234"))
            .Returns(true);

        var result = await context.Handler.Handle(
            CreateCommand(" abc234 ", "friday1"),
            CancellationToken.None);

        GetErrorCode(result).Should().Be(DraftRoomErrorCode.JoinAttemptsThrottled);
        context.RoomRepository.Verify(
            repository => repository.GetRoomByCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRoomCodeIsBlank_ShouldReturnGenericAuthenticationFailure()
    {
        var context = new HandlerTestContext();
        context.PasswordHasher
            .Setup(hasher => hasher.VerifyDummyPassword(string.Empty))
            .Returns(false);

        var result = await context.Handler.Handle(
            CreateCommand(" ", null),
            CancellationToken.None);

        GetErrorCode(result).Should().Be(DraftRoomErrorCode.NotFoundOrWrongPassword);
        context.RoomRepository.Verify(
            repository => repository.GetRoomByCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenAccountParticipantReenters_ShouldNotRequirePassword()
    {
        var context = new HandlerTestContext();
        var room = CreateRoom();
        var participant = CreateParticipant(room, "AccountHost", "user-id");
        room.Participants.Add(participant);
        context.RoomRepository
            .Setup(repository => repository.GetRoomByCodeAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);
        context.RoomRepository
            .Setup(repository => repository.FindParticipantByUserAsync(
                room.Id,
                "user-id",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(participant);

        var result = await context.Handler.Handle(
            CreateCommand("ABC234", null, new DraftRoomCaller("user-id", "AccountHost", null, null)),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.ParticipantId.Should().Be(participant.Id);
        result.Value.IsGuest.Should().BeFalse();
        result.Value.GuestSecret.Should().BeNull();
        context.PasswordHasher.Verify(
            hasher => hasher.Verify(It.IsAny<DraftRoom>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
        context.Throttle.Verify(throttle => throttle.RegisterSuccess("127.0.0.1", "ABC234"), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenLeftGuestReentersLockedRoom_ShouldReactivateWithoutPassword()
    {
        var context = new HandlerTestContext();
        var room = CreateRoom();
        room.IsLocked = true;
        var participant = CreateParticipant(room, "Guest", null, DraftParticipantStatus.Left);
        context.RoomRepository
            .Setup(repository => repository.GetRoomByCodeAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);
        context.RoomRepository
            .Setup(repository => repository.FindParticipantByIdAsync(
                room.Id,
                participant.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(participant);
        context.GuestSecretService
            .Setup(service => service.Verify("guest-secret", It.IsAny<byte[]>()))
            .Returns(true);
        context.RoomRepository
            .Setup(repository => repository.ReactivateParticipantAsync(
                room.Id,
                participant.Id,
                FixedUtcNow.UtcDateTime,
                It.IsAny<CancellationToken>(),
                It.IsAny<Action<DraftRoomScope>>()))
            .Callback<Guid, Guid, DateTime, CancellationToken, Action<DraftRoomScope>>(
                (_, _, _, _, appendLog) => appendLog(null!))
            .ReturnsAsync(true);

        var result = await context.Handler.Handle(
            CreateCommand(
                "ABC234",
                null,
                new DraftRoomCaller(null, null, participant.Id, "guest-secret")),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.ParticipantId.Should().Be(participant.Id);
        result.Value.GuestSecret.Should().BeNull();
        participant.Status.Should().Be(DraftParticipantStatus.Active);
        context.RoomRepository.Verify(
            repository => repository.ReactivateParticipantAsync(
                room.Id,
                participant.Id,
                FixedUtcNow.UtcDateTime,
                It.IsAny<CancellationToken>(),
                It.IsAny<Action<DraftRoomScope>>()),
            Times.Once);
        context.PasswordHasher.Verify(
            hasher => hasher.Verify(It.IsAny<DraftRoom>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
        context.RoomRepository.Verify(
            repository => repository.CountActiveParticipantsAsync(room.Id, It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenGuestReentrySecretIsWrong_ShouldReturnGenericAuthenticationFailure()
    {
        var context = new HandlerTestContext();
        var room = CreateRoom();
        var participant = CreateParticipant(room, "Guest", null);
        context.RoomRepository
            .Setup(repository => repository.GetRoomByCodeAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);
        context.RoomRepository
            .Setup(repository => repository.FindParticipantByIdAsync(
                room.Id,
                participant.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(participant);
        context.GuestSecretService
            .Setup(service => service.Verify("wrong-secret", participant.SecretHash!))
            .Returns(false);
        context.PasswordHasher
            .Setup(hasher => hasher.VerifyDummyPassword("wrong"))
            .Returns(false);

        var result = await context.Handler.Handle(
            CreateCommand(
                "ABC234",
                "wrong",
                new DraftRoomCaller(null, null, participant.Id, "wrong-secret")),
            CancellationToken.None);

        GetErrorCode(result).Should().Be(DraftRoomErrorCode.NotFoundOrWrongPassword);
        context.RoomRepository.Verify(
            repository => repository.ReactivateParticipantAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Action<DraftRoomScope>>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenKnownParticipantWasKicked_ShouldRefuseReentry()
    {
        var context = new HandlerTestContext();
        var room = CreateRoom();
        var participant = CreateParticipant(room, "AccountHost", "user-id", DraftParticipantStatus.Kicked);
        context.RoomRepository
            .Setup(repository => repository.GetRoomByCodeAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);
        context.RoomRepository
            .Setup(repository => repository.FindParticipantByUserAsync(
                room.Id,
                "user-id",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(participant);

        var result = await context.Handler.Handle(
            CreateCommand("ABC234", null, new DraftRoomCaller("user-id", "AccountHost", null, null)),
            CancellationToken.None);

        GetErrorCode(result).Should().Be(DraftRoomErrorCode.ParticipantKicked);
    }

    [Fact]
    public async Task Handle_WhenRoomHasMaximumActiveParticipants_ShouldRefuseNewParticipant()
    {
        var context = new HandlerTestContext();
        var room = CreateRoom();
        context.RoomRepository
            .Setup(repository => repository.GetRoomByCodeAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);
        context.PasswordHasher
            .Setup(hasher => hasher.Verify(room, room.PasswordHash, "friday1"))
            .Returns(true);
        context.RoomRepository
            .Setup(repository => repository.CountActiveParticipantsAsync(room.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(DraftRoomLimits.MaxActiveParticipants);

        var result = await context.Handler.Handle(
            CreateCommand("ABC234", "friday1"),
            CancellationToken.None);

        GetErrorCode(result).Should().Be(DraftRoomErrorCode.RoomFull);
    }

    [Fact]
    public async Task Handle_WhenGuestDisplayNameIsAlreadyTaken_ShouldReturnDuplicateNameError()
    {
        var context = new HandlerTestContext();
        var room = CreateRoom();
        SetupNewRoomAndPassword(context, room);
        context.RoomRepository
            .Setup(repository => repository.CountActiveParticipantsAsync(room.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        context.DisplayNameValidator
            .Setup(validator => validator.Normalize("Bob"))
            .Returns("Bob");
        context.DisplayNameValidator
            .Setup(validator => validator.ValidateGuestName("Bob"))
            .ReturnsAsync(true);
        context.DisplayNameValidator
            .Setup(validator => validator.MakeKey("Bob"))
            .Returns("BOB");
        context.RoomRepository
            .Setup(repository => repository.IsDisplayNameTakenAsync(
                room.Id,
                "BOB",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await context.Handler.Handle(
            CreateCommand("ABC234", "friday1"),
            CancellationToken.None);

        GetErrorCode(result).Should().Be(DraftRoomErrorCode.DuplicateDisplayName);
    }

    [Fact]
    public async Task Handle_WhenGuestDisplayNameIsInvalid_ShouldRejectJoinBeforeCreatingCredentials()
    {
        var context = new HandlerTestContext();
        var room = CreateRoom();
        SetupNewRoomAndPassword(context, room);
        context.RoomRepository
            .Setup(repository => repository.CountActiveParticipantsAsync(room.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        context.DisplayNameValidator
            .Setup(validator => validator.Normalize("Invalid Name"))
            .Returns("Invalid Name");
        context.DisplayNameValidator
            .Setup(validator => validator.ValidateGuestName("Invalid Name"))
            .ReturnsAsync(false);

        var result = await context.Handler.Handle(
            new JoinDraftRoomCommand(
                new JoinDraftRoomRequest("ABC234", "friday1", "Invalid Name"),
                new DraftRoomCaller(null, null, null, null),
                "127.0.0.1"),
            CancellationToken.None);

        GetErrorCode(result).Should().Be(DraftRoomErrorCode.InvalidGuestDisplayName);
        context.GuestSecretService.Verify(service => service.Generate(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenGuestJoinsSuccessfully_ShouldCreateParticipantAndReturnSecret()
    {
        var context = new HandlerTestContext();
        var room = CreateRoom();
        DraftRoomParticipant? savedParticipant = null;
        SetupNewRoomAndPassword(context, room);
        context.RoomRepository
            .Setup(repository => repository.CountActiveParticipantsAsync(room.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        context.DisplayNameValidator
            .Setup(validator => validator.Normalize("Bob"))
            .Returns("Bob");
        context.DisplayNameValidator
            .Setup(validator => validator.ValidateGuestName("Bob"))
            .ReturnsAsync(true);
        context.DisplayNameValidator
            .Setup(validator => validator.MakeKey("Bob"))
            .Returns("BOB");
        context.RoomRepository
            .Setup(repository => repository.IsDisplayNameTakenAsync(
                room.Id,
                "BOB",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        context.GuestSecretService
            .Setup(service => service.Generate())
            .Returns("new-guest-secret");
        context.GuestSecretService
            .Setup(service => service.Hash("new-guest-secret"))
            .Returns([1, 2, 3]);
        context.RoomRepository
            .Setup(repository => repository.AddParticipantAsync(
                It.IsAny<DraftRoomParticipant>(),
                FixedUtcNow.UtcDateTime,
                It.IsAny<CancellationToken>(),
                It.IsAny<Action<DraftRoomScope>>()))
            .Callback<DraftRoomParticipant, DateTime, CancellationToken, Action<DraftRoomScope>>(
                (participant, _, _, appendLog) =>
                {
                    savedParticipant = participant;
                    appendLog(null!);
                })
            .Returns(Task.CompletedTask);

        var result = await context.Handler.Handle(
            CreateCommand("ABC234", "friday1"),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.IsGuest.Should().BeTrue();
        result.Value.GuestSecret.Should().Be("new-guest-secret");
        result.Value.DisplayName.Should().Be("Bob");
        savedParticipant.Should().NotBeNull();
        savedParticipant!.SecretHash.Should().Equal(1, 2, 3);
        savedParticipant.Role.Should().Be(DraftRoomRole.Member);
        savedParticipant.Status.Should().Be(DraftParticipantStatus.Active);
        context.Throttle.Verify(throttle => throttle.RegisterSuccess("127.0.0.1", "ABC234"), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenAccountNameCollides_ShouldAppendNumericSuffix()
    {
        var context = new HandlerTestContext();
        var room = CreateRoom();
        DraftRoomParticipant? savedParticipant = null;
        SetupNewRoomAndPassword(context, room);
        context.RoomRepository
            .Setup(repository => repository.FindParticipantByUserAsync(
                room.Id,
                "user-id",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((DraftRoomParticipant?)null);
        context.RoomRepository
            .Setup(repository => repository.CountActiveParticipantsAsync(room.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        context.DisplayNameValidator
            .Setup(validator => validator.MakeKey("AccountHost"))
            .Returns("ACCOUNTHOST");
        context.RoomRepository
            .Setup(repository => repository.IsDisplayNameTakenAsync(
                room.Id,
                "ACCOUNTHOST",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        context.DisplayNameValidator
            .Setup(validator => validator.MakeKey("AccountHost2"))
            .Returns("ACCOUNTHOST2");
        context.RoomRepository
            .Setup(repository => repository.IsDisplayNameTakenAsync(
                room.Id,
                "ACCOUNTHOST2",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        context.RoomRepository
            .Setup(repository => repository.AddParticipantAsync(
                It.IsAny<DraftRoomParticipant>(),
                FixedUtcNow.UtcDateTime,
                It.IsAny<CancellationToken>(),
                It.IsAny<Action<DraftRoomScope>>()))
            .Callback<DraftRoomParticipant, DateTime, CancellationToken, Action<DraftRoomScope>>(
                (participant, _, _, appendLog) =>
                {
                    savedParticipant = participant;
                    appendLog(null!);
                })
            .Returns(Task.CompletedTask);

        var result = await context.Handler.Handle(
            CreateCommand(
                "ABC234",
                "friday1",
                new DraftRoomCaller("user-id", "AccountHost", null, null)),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        savedParticipant!.DisplayName.Should().Be("AccountHost2");
        result.Value.IsGuest.Should().BeFalse();
        result.Value.GuestSecret.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenRoomChangesBeforeParticipantIsAdded_ShouldReturnConflictError()
    {
        var context = new HandlerTestContext();
        var room = CreateRoom();
        SetupNewRoomAndPassword(context, room);
        context.RoomRepository
            .Setup(repository => repository.CountActiveParticipantsAsync(room.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        context.DisplayNameValidator
            .Setup(validator => validator.Normalize("Bob"))
            .Returns("Bob");
        context.DisplayNameValidator
            .Setup(validator => validator.ValidateGuestName("Bob"))
            .ReturnsAsync(true);
        context.DisplayNameValidator
            .Setup(validator => validator.MakeKey("Bob"))
            .Returns("BOB");
        context.RoomRepository
            .Setup(repository => repository.IsDisplayNameTakenAsync(
                room.Id,
                "BOB",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        context.GuestSecretService
            .Setup(service => service.Generate())
            .Returns("new-guest-secret");
        context.GuestSecretService
            .Setup(service => service.Hash("new-guest-secret"))
            .Returns([1, 2, 3]);
        context.RoomRepository
            .Setup(repository => repository.AddParticipantAsync(
                It.IsAny<DraftRoomParticipant>(),
                FixedUtcNow.UtcDateTime,
                It.IsAny<CancellationToken>(),
                It.IsAny<Action<DraftRoomScope>>()))
            .Returns(Task.FromException(new DbUpdateConcurrencyException("The room changed.")));

        var result = await context.Handler.Handle(
            CreateCommand("ABC234", "friday1"),
            CancellationToken.None);

        GetErrorCode(result).Should().Be(DraftRoomErrorCode.RoomChanged);
        context.Throttle.Verify(
            throttle => throttle.RegisterFailure(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRoomIsLocked_ShouldRefuseNewParticipantAfterPasswordVerification()
    {
        var context = new HandlerTestContext();
        var room = CreateRoom();
        room.IsLocked = true;
        SetupNewRoomAndPassword(context, room);

        var result = await context.Handler.Handle(
            CreateCommand("ABC234", "friday1"),
            CancellationToken.None);

        GetErrorCode(result).Should().Be(DraftRoomErrorCode.RoomLocked);
        context.RoomRepository.Verify(
            repository => repository.CountActiveParticipantsAsync(room.Id, It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static JoinDraftRoomCommand CreateCommand(
        string code,
        string? password,
        DraftRoomCaller? caller = null)
    {
        return new JoinDraftRoomCommand(
            new JoinDraftRoomRequest(code, password, caller?.UserId is null ? "Bob" : null),
            caller ?? new DraftRoomCaller(null, null, null, null),
            "127.0.0.1");
    }

    private static DraftRoom CreateRoom()
    {
        return new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = "ABC234",
            Name = "Friday TI",
            PasswordHash = "hashed-password",
            Status = DraftRoomStatus.Lobby,
            DraftType = string.Empty,
            Version = 1,
        };
    }

    private static DraftRoomParticipant CreateParticipant(
        DraftRoom room,
        string displayName,
        string? userId,
        DraftParticipantStatus status = DraftParticipantStatus.Active)
    {
        return new DraftRoomParticipant
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            UserId = userId,
            SecretHash = userId is null ? [1, 2, 3] : null,
            DisplayName = displayName,
            DisplayNameKey = displayName.ToUpperInvariant(),
            Role = DraftRoomRole.Host,
            Status = status,
        };
    }

    private static DraftRoomErrorCode GetErrorCode(Result<DraftRoomEntryDto> result)
    {
        return result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<DraftRoomError>()
            .Which.Code;
    }

    private static void SetupNewRoomAndPassword(HandlerTestContext context, DraftRoom room)
    {
        context.RoomRepository
            .Setup(repository => repository.GetRoomByCodeAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);
        context.PasswordHasher
            .Setup(hasher => hasher.Verify(room, room.PasswordHash, "friday1"))
            .Returns(true);
    }

    private sealed class HandlerTestContext
    {
        public HandlerTestContext()
        {
            this.Throttle
                .Setup(throttle => throttle.IsBlocked("127.0.0.1", It.IsAny<string>()))
                .Returns(false);
            this.Throttle
                .Setup(throttle => throttle.RegisterFailure("127.0.0.1", It.IsAny<string>()));
            this.Throttle
                .Setup(throttle => throttle.RegisterSuccess("127.0.0.1", It.IsAny<string>()));

            this.Handler = new JoinDraftRoomCommandHandler(
                this.RoomRepository.Object,
                this.Throttle.Object,
                this.GuestSecretService.Object,
                this.PasswordHasher.Object,
                this.DisplayNameValidator.Object,
                this.DraftRoomLog.Object,
                new NullRoomChangeNotifier(),
                new FixedTimeProvider(FixedUtcNow),
                NullLogger<JoinDraftRoomCommandHandler>.Instance);
        }

        public Mock<IDraftRoomRepository> RoomRepository { get; } = new(MockBehavior.Strict);

        public Mock<IJoinAttemptThrottle> Throttle { get; } = new(MockBehavior.Strict);

        public Mock<IGuestSecretService> GuestSecretService { get; } = new(MockBehavior.Strict);

        public Mock<IRoomPasswordHasher> PasswordHasher { get; } = new(MockBehavior.Strict);

        public Mock<IDraftRoomDisplayNameValidator> DisplayNameValidator { get; } = new(MockBehavior.Strict);

        public Mock<IDraftRoomLog> DraftRoomLog { get; } = CreateDraftRoomLogMock();

        public JoinDraftRoomCommandHandler Handler { get; }

        private static Mock<IDraftRoomLog> CreateDraftRoomLogMock()
        {
            var draftRoomLog = new Mock<IDraftRoomLog>(MockBehavior.Strict);
            draftRoomLog
                .Setup(log => log.Append(
                    It.IsAny<DraftRoomScope>(),
                    It.IsAny<DraftLogEntryType>(),
                    It.IsAny<Guid?>(),
                    It.IsAny<object>()))
                .Returns(new DraftRoomLogEntry
                {
                    Id = 1,
                    DataJson = "{}",
                    CreatedAtUtc = FixedUtcNow.UtcDateTime,
                });
            return draftRoomLog;
        }
    }

    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow()
        {
            return utcNow;
        }
    }
}
