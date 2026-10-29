using FluentAssertions;
using FluentResults;
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
public sealed class CreateDraftRoomCommandHandlerTests
{
    private static readonly DateTimeOffset FixedUtcNow =
        new(2026, 10, 16, 0, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task Handle_WhenGuestRequestIsValid_ShouldCreateRoomAndReturnGuestCredentials()
    {
        var context = new HandlerTestContext();
        DraftRoom? savedRoom = null;
        DraftRoomParticipant? savedHost = null;

        context.RoomRepository
            .Setup(repository => repository.CodeExistsAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        context.RoomRepository
            .Setup(repository => repository.CreateRoomWithHostAsync(
                It.IsAny<DraftRoom>(),
                It.IsAny<DraftRoomParticipant>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Action<DraftRoomScope>>()))
            .Callback<DraftRoom, DraftRoomParticipant, CancellationToken, Action<DraftRoomScope>>(
                (room, host, _, appendLog) =>
            {
                savedRoom = room;
                savedHost = host;
                appendLog(null!);
            })
            .Returns(Task.CompletedTask);
        context.DisplayNameValidator
            .Setup(validator => validator.Normalize("  Alice   Smith "))
            .Returns("Alice Smith");
        context.DisplayNameValidator
            .Setup(validator => validator.ValidateGuestName("  Alice   Smith "))
            .ReturnsAsync(true);
        context.DisplayNameValidator
            .Setup(validator => validator.MakeKey("Alice Smith"))
            .Returns("ALICE SMITH");
        context.GuestSecretService
            .Setup(service => service.Generate())
            .Returns("generated-guest-secret");
        context.GuestSecretService
            .Setup(service => service.Hash("generated-guest-secret"))
            .Returns([1, 2, 3]);
        context.PasswordHasher
            .Setup(hasher => hasher.Hash(It.IsAny<DraftRoom>(), "friday1"))
            .Returns("hashed-password");
        var result = await context.Handler.Handle(
            new CreateDraftRoomCommand(
                new CreateDraftRoomRequest(" Friday TI ", "friday1", "  Alice   Smith "),
                new DraftRoomCaller(null, null, null, null)),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        savedRoom.Should().NotBeNull();
        savedHost.Should().NotBeNull();
        savedRoom!.Name.Should().Be("Friday TI");
        savedRoom.Code.Should().Be("ABC234");
        savedRoom.PasswordHash.Should().Be("hashed-password");
        savedRoom.Status.Should().Be(DraftRoomStatus.Lobby);
        savedRoom.DraftType.Should().Be(nameof(DraftTypeId.FactionPick));
        savedRoom.Version.Should().Be(1);
        savedRoom.IsLocked.Should().BeFalse();
        savedRoom.CreatedAtUtc.Should().Be(FixedUtcNow.UtcDateTime);
        savedRoom.LastActivityUtc.Should().Be(FixedUtcNow.UtcDateTime);
        savedHost!.RoomId.Should().Be(savedRoom.Id);
        savedHost.UserId.Should().BeNull();
        savedHost.SecretHash.Should().Equal(1, 2, 3);
        savedHost.DisplayName.Should().Be("Alice Smith");
        savedHost.DisplayNameKey.Should().Be("ALICE SMITH");
        savedHost.Role.Should().Be(DraftRoomRole.Host);
        savedHost.Status.Should().Be(DraftParticipantStatus.Active);
        savedHost.JoinedAtUtc.Should().Be(FixedUtcNow.UtcDateTime);
        savedHost.LastSeenUtc.Should().Be(FixedUtcNow.UtcDateTime);
        savedRoom.Seats.Should().HaveCount(DraftRoomLimits.DefaultSeatCount);
        savedRoom.Seats.Select(seat => seat.Index)
            .Should()
            .Equal(Enumerable.Range(1, DraftRoomLimits.DefaultSeatCount));
        var hostSeat = savedRoom.Seats.Single(seat => seat.Index == 1);
        hostSeat.ParticipantId.Should().Be(savedHost.Id);
        hostSeat.Participant.Should().BeSameAs(savedHost);
        hostSeat.ClaimedAtUtc.Should().Be(FixedUtcNow.UtcDateTime);
        savedRoom.Seats
            .Where(seat => seat.Index != 1)
            .Should()
            .OnlyContain(seat => !seat.ParticipantId.HasValue && !seat.ClaimedAtUtc.HasValue);

        result.Value.Should().BeEquivalentTo(new DraftRoomEntryDto
        {
            RoomId = savedRoom.Id,
            Code = "ABC234",
            RoomName = "Friday TI",
            ParticipantId = savedHost.Id,
            DisplayName = "Alice Smith",
            Role = DraftRoomRole.Host,
            IsGuest = true,
            GuestSecret = "generated-guest-secret",
        });
    }

    [Fact]
    public async Task Handle_WhenAccountRequestIsValid_ShouldCreateAccountHostWithoutGuestSecret()
    {
        var context = new HandlerTestContext();
        DraftRoomParticipant? savedHost = null;

        context.RoomRepository
            .Setup(repository => repository.CountOpenRoomsForUserAsync("user-id", It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        context.RoomRepository
            .Setup(repository => repository.CodeExistsAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        context.RoomRepository
            .Setup(repository => repository.CreateRoomWithHostAsync(
                It.IsAny<DraftRoom>(),
                It.IsAny<DraftRoomParticipant>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Action<DraftRoomScope>>()))
            .Callback<DraftRoom, DraftRoomParticipant, CancellationToken, Action<DraftRoomScope>>(
                (_, host, _, appendLog) =>
                {
                    savedHost = host;
                    appendLog(null!);
                })
            .Returns(Task.CompletedTask);
        context.PasswordHasher
            .Setup(hasher => hasher.Hash(It.IsAny<DraftRoom>(), "friday1"))
            .Returns("hashed-password");
        context.DisplayNameValidator
            .Setup(validator => validator.MakeKey("AccountHost"))
            .Returns("ACCOUNTHOST");

        var result = await context.Handler.Handle(
            new CreateDraftRoomCommand(
                new CreateDraftRoomRequest("Friday TI", "friday1", null),
                new DraftRoomCaller("user-id", "AccountHost", null, null)),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        savedHost.Should().NotBeNull();
        savedHost!.UserId.Should().Be("user-id");
        savedHost.DisplayName.Should().Be("AccountHost");
        savedHost.SecretHash.Should().BeNull();
        result.Value.IsGuest.Should().BeFalse();
        result.Value.GuestSecret.Should().BeNull();
        context.DisplayNameValidator.Verify(
            validator => validator.ValidateGuestName(It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenAccountAlreadyHostsMaximumOpenRooms_ShouldReturnRoomLimitError()
    {
        var context = new HandlerTestContext();
        context.RoomRepository
            .Setup(repository => repository.CountOpenRoomsForUserAsync("user-id", It.IsAny<CancellationToken>()))
            .ReturnsAsync(DraftRoomLimits.MaxOpenRoomsPerUser);

        var result = await context.Handler.Handle(
            new CreateDraftRoomCommand(
                new CreateDraftRoomRequest("Friday TI", "friday1", null),
                new DraftRoomCaller("user-id", "AccountHost", null, null)),
            CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        GetErrorCode(result).Should().Be(DraftRoomErrorCode.OpenRoomLimitReached);
        context.RoomCodeGenerator.Verify(generator => generator.Generate(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenGeneratedRoomCodeExists_ShouldRetryUntilAnUnusedCodeIsFound()
    {
        var context = new HandlerTestContext();
        context.RoomCodeGenerator
            .SetupSequence(generator => generator.Generate())
            .Returns("ABC234")
            .Returns("DEF567");
        context.RoomRepository
            .Setup(repository => repository.CodeExistsAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        context.RoomRepository
            .Setup(repository => repository.CodeExistsAsync("DEF567", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        context.RoomRepository
            .Setup(repository => repository.CreateRoomWithHostAsync(
                It.IsAny<DraftRoom>(),
                It.IsAny<DraftRoomParticipant>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Action<DraftRoomScope>>()))
            .Callback<DraftRoom, DraftRoomParticipant, CancellationToken, Action<DraftRoomScope>>(
                (_, _, _, appendLog) => appendLog(null!))
            .Returns(Task.CompletedTask);
        context.DisplayNameValidator
            .Setup(validator => validator.Normalize("Alice"))
            .Returns("Alice");
        context.DisplayNameValidator
            .Setup(validator => validator.ValidateGuestName("Alice"))
            .ReturnsAsync(true);
        context.DisplayNameValidator
            .Setup(validator => validator.MakeKey("Alice"))
            .Returns("ALICE");
        context.GuestSecretService
            .Setup(service => service.Generate())
            .Returns("guest-secret");
        context.GuestSecretService
            .Setup(service => service.Hash("guest-secret"))
            .Returns([4, 5, 6]);
        context.PasswordHasher
            .Setup(hasher => hasher.Hash(It.IsAny<DraftRoom>(), "friday1"))
            .Returns("hashed-password");

        var result = await context.Handler.Handle(
            new CreateDraftRoomCommand(
                new CreateDraftRoomRequest("Friday TI", "friday1", "Alice"),
                new DraftRoomCaller(null, null, null, null)),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be("DEF567");
        context.RoomCodeGenerator.Verify(generator => generator.Generate(), Times.Exactly(2));
    }

    [Fact]
    public async Task Handle_WhenNoUniqueRoomCodeIsGeneratedAfterFiveAttempts_ShouldReturnCodeGenerationError()
    {
        var context = new HandlerTestContext();
        context.RoomCodeGenerator
            .Setup(generator => generator.Generate())
            .Returns("ABC234");
        context.RoomRepository
            .Setup(repository => repository.CodeExistsAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        context.DisplayNameValidator
            .Setup(validator => validator.Normalize("Alice"))
            .Returns("Alice");
        context.DisplayNameValidator
            .Setup(validator => validator.ValidateGuestName("Alice"))
            .ReturnsAsync(true);
        context.DisplayNameValidator
            .Setup(validator => validator.MakeKey("Alice"))
            .Returns("ALICE");
        context.GuestSecretService
            .Setup(service => service.Generate())
            .Returns("guest-secret");
        context.GuestSecretService
            .Setup(service => service.Hash("guest-secret"))
            .Returns([4, 5, 6]);

        var result = await context.Handler.Handle(
            new CreateDraftRoomCommand(
                new CreateDraftRoomRequest("Friday TI", "friday1", "Alice"),
                new DraftRoomCaller(null, null, null, null)),
            CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        GetErrorCode(result).Should().Be(DraftRoomErrorCode.RoomCodeGenerationFailed);
        context.RoomCodeGenerator.Verify(generator => generator.Generate(), Times.Exactly(5));
        context.RoomRepository.Verify(
            repository => repository.CreateRoomWithHostAsync(
                It.IsAny<DraftRoom>(),
                It.IsAny<DraftRoomParticipant>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Action<DraftRoomScope>>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenGuestDisplayNameIsInvalid_ShouldReturnDisplayNameError()
    {
        var context = new HandlerTestContext();
        context.DisplayNameValidator
            .Setup(validator => validator.Normalize("Bad!Name"))
            .Returns("Bad!Name");
        context.DisplayNameValidator
            .Setup(validator => validator.ValidateGuestName("Bad!Name"))
            .ReturnsAsync(false);

        var result = await context.Handler.Handle(
            new CreateDraftRoomCommand(
                new CreateDraftRoomRequest("Friday TI", "friday1", "Bad!Name"),
                new DraftRoomCaller(null, null, null, null)),
            CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        GetErrorCode(result).Should().Be(DraftRoomErrorCode.InvalidGuestDisplayName);
        context.RoomCodeGenerator.Verify(generator => generator.Generate(), Times.Never);
    }

    [Theory]
    [InlineData("TI", "friday1", DraftRoomErrorCode.InvalidRoomName)]
    [InlineData("Friday TI", "short", DraftRoomErrorCode.InvalidPassword)]
    [InlineData("Friday TI", "12345678901234567890123456789012345678901234567890123456789012345", DraftRoomErrorCode.InvalidPassword)]
    public async Task Handle_WhenRoomNameOrPasswordIsInvalid_ShouldReturnValidationError(
        string roomName,
        string password,
        DraftRoomErrorCode expectedErrorCode)
    {
        var context = new HandlerTestContext();

        var result = await context.Handler.Handle(
            new CreateDraftRoomCommand(
                new CreateDraftRoomRequest(roomName, password, "Alice"),
                new DraftRoomCaller(null, null, null, null)),
            CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        GetErrorCode(result).Should().Be(expectedErrorCode);
    }

    private static DraftRoomErrorCode GetErrorCode(Result<DraftRoomEntryDto> result)
    {
        return result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<DraftRoomError>()
            .Which.Code;
    }

    private sealed class HandlerTestContext
    {
        public HandlerTestContext()
        {
            this.RoomCodeGenerator
                .Setup(generator => generator.Generate())
                .Returns("ABC234");

            this.Handler = new CreateDraftRoomCommandHandler(
                this.RoomRepository.Object,
                this.RoomCodeGenerator.Object,
                this.GuestSecretService.Object,
                this.PasswordHasher.Object,
                this.DisplayNameValidator.Object,
                this.DraftRoomLog.Object,
                Mock.Of<IDraftRoomDraftPersistence>(),
                new NullRoomChangeNotifier(),
                new FixedTimeProvider(FixedUtcNow),
                NullLogger<CreateDraftRoomCommandHandler>.Instance);
        }

        public Mock<IDraftRoomRepository> RoomRepository { get; } = new(MockBehavior.Strict);

        public Mock<IRoomCodeGenerator> RoomCodeGenerator { get; } = new(MockBehavior.Strict);

        public Mock<IGuestSecretService> GuestSecretService { get; } = new(MockBehavior.Strict);

        public Mock<IRoomPasswordHasher> PasswordHasher { get; } = new(MockBehavior.Strict);

        public Mock<IDraftRoomDisplayNameValidator> DisplayNameValidator { get; } = new(MockBehavior.Strict);

        public Mock<IDraftRoomLog> DraftRoomLog { get; } = CreateDraftRoomLogMock();

        public CreateDraftRoomCommandHandler Handler { get; }

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
