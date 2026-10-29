using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Core.Entities.Users;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomRepositoryTests : IClassFixture<DraftRoomRepositoryTestsFixture>
{
    private readonly DraftRoomRepository repository;
    private readonly DraftRoomRepositoryTestsFixture fixture;

    public DraftRoomRepositoryTests(DraftRoomRepositoryTestsFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        this.fixture = fixture;
        this.repository = new DraftRoomRepository(fixture.Database.ContextFactory);
    }

    [Fact]
    public async Task CodeExistsAsync_WhenCodeIsStored_ShouldReturnWhetherItExists()
    {
        var room = CreateRoom();

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            await context.SaveChangesAsync();
        }

        (await this.repository.CodeExistsAsync(room.Code, CancellationToken.None)).Should().BeTrue();
        (await this.repository.CodeExistsAsync("missing", CancellationToken.None)).Should().BeFalse();
    }

    [Fact]
    public async Task CreateRoomWithHostAsync_WhenRoomAndHostAreValid_ShouldPersistBoth()
    {
        var room = CreateRoom();
        var host = CreateParticipant(room, "Host", "host", role: DraftRoomRole.Host);

        await this.repository.CreateRoomWithHostAsync(room, host, CancellationToken.None, static _ => { });

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedRoom = await context.DraftRooms
            .Include(item => item.Participants)
            .SingleAsync(item => item.Id == room.Id);

        persistedRoom.Participants.Should().ContainSingle()
            .Which.Role.Should().Be(DraftRoomRole.Host);
    }

    [Fact]
    public async Task CreateDraftRoomCommandHandler_WhenGuestCreatesRoom_ShouldPersistDefaultSeatsAndHostSeat()
    {
        var codeGenerator = new Mock<IRoomCodeGenerator>(MockBehavior.Strict);
        codeGenerator.Setup(generator => generator.Generate()).Returns("NEW123");
        var displayNameValidator = new Mock<IDraftRoomDisplayNameValidator>(MockBehavior.Strict);
        displayNameValidator.Setup(validator => validator.Normalize("Host")).Returns("Host");
        displayNameValidator.Setup(validator => validator.ValidateGuestName("Host")).ReturnsAsync(true);
        displayNameValidator.Setup(validator => validator.MakeKey("Host")).Returns("HOST");

        var handler = new CreateDraftRoomCommandHandler(
            this.repository,
            codeGenerator.Object,
            new GuestSecretService(),
            new RoomPasswordHasher(),
            displayNameValidator.Object,
            new DraftRoomLog(),
            new DraftRoomDraftPersistence(new FactionPickRepository(), Mock.Of<IFactionPickPoolProvider>()),
            new NullRoomChangeNotifier(),
            TimeProvider.System,
            NullLogger<CreateDraftRoomCommandHandler>.Instance);
        var result = await handler.Handle(
            new CreateDraftRoomCommand(
                new CreateDraftRoomRequest("Test Room", "password", "Host"),
                new DraftRoomCaller(null, null, null, null)),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedRoom = await context.DraftRooms
            .Include(item => item.Participants)
            .Include(item => item.Seats)
            .SingleAsync(item => item.Id == result.Value.RoomId);

        persistedRoom.Seats.Should().HaveCount(DraftRoomLimits.DefaultSeatCount);
        persistedRoom.DraftType.Should().Be(nameof(DraftTypeId.FactionPick));
        var draftSettings = await context.FactionPickSettings.SingleAsync(item => item.RoomId == persistedRoom.Id);
        draftSettings.GameVersions.Should().Equal(GameVersion.BaseGame, GameVersion.ProphecyOfKings, GameVersion.ThundersEdge);
        draftSettings.PicksPerPlayer.Should().Be(1);
        persistedRoom.Seats.Select(seat => seat.Index)
            .Should()
            .BeEquivalentTo(Enumerable.Range(1, DraftRoomLimits.DefaultSeatCount));
        var host = persistedRoom.Participants.Should().ContainSingle().Subject;
        var hostSeat = persistedRoom.Seats.Single(seat => seat.Index == 1);
        hostSeat.ParticipantId.Should().Be(host.Id);
        hostSeat.ClaimedAtUtc.Should().NotBeNull();
        persistedRoom.Seats
            .Where(seat => seat.Index != 1)
            .Should()
            .OnlyContain(seat => !seat.ParticipantId.HasValue && !seat.ClaimedAtUtc.HasValue);
        await DraftRoomLogAssertions.AssertEntriesAsync(
            this.fixture.Database.ContextFactory,
            persistedRoom.Id,
            DraftLogEntryType.RoomCreated);
        var guestSecret = result.Value.GuestSecret
            ?? throw new InvalidOperationException("Guest room creation should return a guest secret.");
        await DraftRoomLogAssertions.AssertDoesNotContainAsync(
            this.fixture.Database.ContextFactory,
            persistedRoom.Id,
            guestSecret,
            persistedRoom.PasswordHash);
    }

    [Fact]
    public async Task JoinDraftRoomCommandHandler_WhenNewGuestJoins_ShouldPersistPublicJoinLog()
    {
        var room = CreateRoom();
        var passwordHasher = new RoomPasswordHasher();
        room.PasswordHash = passwordHasher.Hash(room, "friday1");
        var host = CreateParticipant(room, "Host", "HOST", role: DraftRoomRole.Host);
        await this.repository.CreateRoomWithHostAsync(room, host, CancellationToken.None, static _ => { });

        var userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        userRepository
            .Setup(repository => repository.GetUserByUserName("Guest"))
            .ReturnsAsync((TwilightImperiumUser?)null);
        var handler = new JoinDraftRoomCommandHandler(
            this.repository,
            new JoinAttemptThrottle(TimeProvider.System),
            new GuestSecretService(),
            passwordHasher,
            new DraftRoomDisplayNameValidator(userRepository.Object),
            new DraftRoomLog(),
            new NullRoomChangeNotifier(),
            TimeProvider.System,
            NullLogger<JoinDraftRoomCommandHandler>.Instance);

        var result = await handler.Handle(
            new JoinDraftRoomCommand(
                new JoinDraftRoomRequest(room.Code, "friday1", "Guest"),
                new DraftRoomCaller(null, null, null, null),
                "127.0.0.1"),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.DisplayName.Should().Be("Guest");
        result.Value.GuestSecret.Should().NotBeNullOrWhiteSpace();
        await DraftRoomLogAssertions.AssertEntriesAsync(
            this.fixture.Database.ContextFactory,
            room.Id,
            DraftLogEntryType.ParticipantJoined);
        var guestSecret = result.Value.GuestSecret
            ?? throw new InvalidOperationException("Guest room entry should return a guest secret.");
        await DraftRoomLogAssertions.AssertDoesNotContainAsync(
            this.fixture.Database.ContextFactory,
            room.Id,
            guestSecret,
            "friday1",
            room.PasswordHash);
    }

    [Fact]
    public async Task JoinDraftRoomCommandHandler_WhenKnownGuestReenters_ShouldPersistPublicJoinLog()
    {
        var room = CreateRoom();
        var guestSecretService = new GuestSecretService();
        var participant = CreateParticipant(
            room,
            "Guest",
            "GUEST",
            status: DraftParticipantStatus.Left);
        participant.SecretHash = guestSecretService.Hash("guest-secret");
        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            context.DraftRoomParticipants.Add(participant);
            await context.SaveChangesAsync();
        }

        var userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        var handler = new JoinDraftRoomCommandHandler(
            this.repository,
            new JoinAttemptThrottle(TimeProvider.System),
            guestSecretService,
            new RoomPasswordHasher(),
            new DraftRoomDisplayNameValidator(userRepository.Object),
            new DraftRoomLog(),
            new NullRoomChangeNotifier(),
            TimeProvider.System,
            NullLogger<JoinDraftRoomCommandHandler>.Instance);

        var result = await handler.Handle(
            new JoinDraftRoomCommand(
                new JoinDraftRoomRequest(room.Code, null, null),
                new DraftRoomCaller(null, null, participant.Id, "guest-secret"),
                "127.0.0.1"),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.ParticipantId.Should().Be(participant.Id);
        await DraftRoomLogAssertions.AssertEntriesAsync(
            this.fixture.Database.ContextFactory,
            room.Id,
            DraftLogEntryType.ParticipantJoined);
    }

    [Fact]
    public async Task GetRoomByCodeAsync_WhenRoomExists_ShouldReturnRoomAndParticipants()
    {
        var room = CreateRoom();
        var participant = CreateParticipant(room, "Guest", "guest");

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            context.DraftRoomParticipants.Add(participant);
            await context.SaveChangesAsync();
        }

        var result = await this.repository.GetRoomByCodeAsync(room.Code, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Participants.Should().ContainSingle(item => item.Id == participant.Id);
    }

    [Fact]
    public async Task GetRoomForViewByCodeAsync_WhenRoomExists_ShouldReturnParticipantsAndSeats()
    {
        var room = CreateRoom();
        var participant = CreateParticipant(room, "Guest", "guest");
        var seat = new DraftRoomSeat
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            Index = 1,
            ParticipantId = participant.Id,
            ClaimedAtUtc = DateTime.UtcNow,
        };

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            context.DraftRoomParticipants.Add(participant);
            context.DraftRoomSeats.Add(seat);
            await context.SaveChangesAsync();
        }

        var result = await this.repository.GetRoomForViewByCodeAsync(room.Code, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Participants.Should().ContainSingle(item => item.Id == participant.Id);
        result.Seats.Should().ContainSingle(item => item.Id == seat.Id)
            .Which.ParticipantId.Should().Be(participant.Id);
    }

    [Fact]
    public async Task FindParticipantByUserAsync_WhenAccountParticipantExists_ShouldReturnParticipant()
    {
        var user = CreateUser();
        var room = CreateRoom();
        var participant = CreateParticipant(room, "Player", "player", user.Id);

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.Users.Add(user);
            context.DraftRooms.Add(room);
            context.DraftRoomParticipants.Add(participant);
            await context.SaveChangesAsync();
        }

        var result = await this.repository.FindParticipantByUserAsync(room.Id, user.Id, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(participant.Id);
    }

    [Fact]
    public async Task FindParticipantByIdAsync_WhenParticipantBelongsToRoom_ShouldReturnParticipant()
    {
        var room = CreateRoom();
        var participant = CreateParticipant(room, "Guest", "guest");

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            context.DraftRoomParticipants.Add(participant);
            await context.SaveChangesAsync();
        }

        var result = await this.repository.FindParticipantByIdAsync(
            room.Id,
            participant.Id,
            CancellationToken.None);

        result.Should().NotBeNull();
        result!.RoomId.Should().Be(room.Id);
    }

    [Fact]
    public async Task AddParticipantAsync_WhenRoomExists_ShouldPersistParticipant()
    {
        var room = CreateRoom();

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            await context.SaveChangesAsync();
        }

        var participant = CreateParticipant(room, "Guest", "guest");

        var activityAtUtc = DateTime.UtcNow;
        await this.repository.AddParticipantAsync(
            participant,
            activityAtUtc,
            CancellationToken.None,
            static _ => { });

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedParticipant = await verificationContext.DraftRoomParticipants
            .SingleAsync(item => item.Id == participant.Id);
        var persistedRoom = await verificationContext.DraftRooms.SingleAsync(item => item.Id == room.Id);

        persistedParticipant.DisplayName.Should().Be("Guest");
        persistedRoom.LastActivityUtc.Should().Be(activityAtUtc);
        persistedRoom.Version.Should().Be(2);
    }

    [Fact]
    public async Task AddParticipantAsync_WhenLogCallbackFails_ShouldRollbackParticipantAndRoomActivity()
    {
        var room = CreateRoom();
        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            await context.SaveChangesAsync();
        }

        var participant = CreateParticipant(room, "Guest", "GUEST");
        var activityAtUtc = DateTime.UtcNow;
        var act = () => this.repository.AddParticipantAsync(
            participant,
            activityAtUtc,
            CancellationToken.None,
            scope =>
            {
                new DraftRoomLog().Append(
                    scope,
                    DraftLogEntryType.ParticipantJoined,
                    participant.Id,
                    new { participant.Id, participant.DisplayName });
                throw new InvalidOperationException("Test rollback.");
            });

        await act.Should().ThrowAsync<InvalidOperationException>();

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await verificationContext.DraftRooms.SingleAsync(item => item.Id == room.Id))
            .Version.Should().Be(room.Version);
        (await verificationContext.DraftRoomParticipants.AnyAsync(item => item.Id == participant.Id))
            .Should().BeFalse();
        (await verificationContext.DraftRoomLogEntries.AnyAsync(item => item.RoomId == room.Id))
            .Should().BeFalse();
    }

    [Fact]
    public async Task ReactivateParticipantAsync_WhenParticipantHasLeft_ShouldReactivateAndUpdateRoom()
    {
        var room = CreateRoom();
        var participant = CreateParticipant(
            room,
            "Guest",
            "guest",
            status: DraftParticipantStatus.Left);

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            context.DraftRoomParticipants.Add(participant);
            await context.SaveChangesAsync();
        }

        var activityAtUtc = DateTime.UtcNow;
        var reactivated = await this.repository.ReactivateParticipantAsync(
            room.Id,
            participant.Id,
            activityAtUtc,
            CancellationToken.None,
            static _ => { });

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedParticipant = await verificationContext.DraftRoomParticipants
            .SingleAsync(item => item.Id == participant.Id);
        var persistedRoom = await verificationContext.DraftRooms.SingleAsync(item => item.Id == room.Id);

        reactivated.Should().BeTrue();
        persistedParticipant.Status.Should().Be(DraftParticipantStatus.Active);
        persistedParticipant.LastSeenUtc.Should().Be(activityAtUtc);
        persistedRoom.LastActivityUtc.Should().Be(activityAtUtc);
        persistedRoom.Version.Should().Be(2);
    }

    [Fact]
    public async Task ReactivateParticipantAsync_WhenParticipantWasKicked_ShouldNotRestoreAccess()
    {
        var room = CreateRoom();
        var participant = CreateParticipant(
            room,
            "Guest",
            "guest",
            status: DraftParticipantStatus.Kicked);

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            context.DraftRoomParticipants.Add(participant);
            await context.SaveChangesAsync();
        }

        var act = () => this.repository.ReactivateParticipantAsync(
            room.Id,
            participant.Id,
            DateTime.UtcNow,
            CancellationToken.None,
            static _ => { });

        await act.Should().ThrowAsync<DbUpdateConcurrencyException>();

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedParticipant = await verificationContext.DraftRoomParticipants
            .SingleAsync(item => item.Id == participant.Id);

        persistedParticipant.Status.Should().Be(DraftParticipantStatus.Kicked);
    }

    [Fact]
    public async Task ReactivateParticipantAsync_WhenDisplayNameWasReclaimed_ShouldLeaveParticipantLeft()
    {
        var room = CreateRoom();
        var returningParticipant = CreateParticipant(
            room,
            "Guest",
            "guest",
            status: DraftParticipantStatus.Left);
        var activeParticipant = CreateParticipant(room, "Guest", "guest");

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            context.DraftRoomParticipants.AddRange(returningParticipant, activeParticipant);
            await context.SaveChangesAsync();
        }

        var reactivated = await this.repository.ReactivateParticipantAsync(
            room.Id,
            returningParticipant.Id,
            DateTime.UtcNow,
            CancellationToken.None,
            static _ => { });

        reactivated.Should().BeFalse();

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedParticipant = await verificationContext.DraftRoomParticipants
            .SingleAsync(item => item.Id == returningParticipant.Id);

        persistedParticipant.Status.Should().Be(DraftParticipantStatus.Left);
    }

    [Fact]
    public async Task CountOpenRoomsForUserAsync_WhenUserHasFinishedAndOpenRooms_ShouldCountOnlyOpenRooms()
    {
        var user = CreateUser();
        var openRoom = CreateRoom();
        var finishedRoom = CreateRoom(status: DraftRoomStatus.Finished);
        var openHost = CreateParticipant(openRoom, "Host", "host", user.Id, DraftRoomRole.Host);
        var finishedHost = CreateParticipant(finishedRoom, "Host", "host", user.Id, DraftRoomRole.Host);

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.Users.Add(user);
            context.DraftRooms.AddRange(openRoom, finishedRoom);
            context.DraftRoomParticipants.AddRange(openHost, finishedHost);
            await context.SaveChangesAsync();
        }

        var count = await this.repository.CountOpenRoomsForUserAsync(user.Id, CancellationToken.None);

        count.Should().Be(1);
    }

    [Fact]
    public async Task CountActiveParticipantsAsync_WhenRoomHasActiveAndLeftParticipants_ShouldCountOnlyActive()
    {
        var room = CreateRoom();
        var activeParticipant = CreateParticipant(room, "Active", "active");
        var leftParticipant = CreateParticipant(
            room,
            "Left",
            "left",
            status: DraftParticipantStatus.Left);

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            context.DraftRoomParticipants.AddRange(activeParticipant, leftParticipant);
            await context.SaveChangesAsync();
        }

        var count = await this.repository.CountActiveParticipantsAsync(room.Id, CancellationToken.None);

        count.Should().Be(1);
    }

    [Fact]
    public async Task IsDisplayNameTakenAsync_WhenKeyIsActive_ShouldReturnWhetherItIsTaken()
    {
        var room = CreateRoom();
        var activeParticipant = CreateParticipant(room, "Active", "active");
        var leftParticipant = CreateParticipant(
            room,
            "Left",
            "left",
            status: DraftParticipantStatus.Left);

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            context.DraftRoomParticipants.AddRange(activeParticipant, leftParticipant);
            await context.SaveChangesAsync();
        }

        (await this.repository.IsDisplayNameTakenAsync(room.Id, "active", CancellationToken.None)).Should().BeTrue();
        (await this.repository.IsDisplayNameTakenAsync(room.Id, "left", CancellationToken.None)).Should().BeFalse();
        (await this.repository.IsDisplayNameTakenAsync(room.Id, "other", CancellationToken.None)).Should().BeFalse();
    }

    [Fact]
    public async Task GetActiveParticipantsForUserAsync_WhenMembershipsExist_ShouldFilterOrderAndLimitResults()
    {
        var account = CreateUser();
        var otherAccount = CreateUser();
        var activityBase = DateTime.UtcNow.AddDays(-1);
        var activeRoomCodes = new List<string>();

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.Users.AddRange(account, otherAccount);

            for (var index = 0; index < 52; index++)
            {
                var room = CreateRoom(index == 51 ? DraftRoomStatus.Finished : DraftRoomStatus.Lobby);
                room.Code = $"T{index:D5}";
                room.Name = $"My room {index}";
                room.LastActivityUtc = activityBase.AddMinutes(index);
                var participant = CreateParticipant(
                    room,
                    $"Account {index}",
                    $"ACCOUNT-{index}",
                    account.Id);
                participant.Room = room;
                room.Participants.Add(participant);
                if (index == 51)
                {
                    room.Seats.Add(new DraftRoomSeat
                    {
                        Id = Guid.NewGuid(),
                        RoomId = room.Id,
                        Room = room,
                        ParticipantId = participant.Id,
                        Participant = participant,
                        Index = 7,
                        ClaimedAtUtc = room.LastActivityUtc,
                    });
                }

                activeRoomCodes.Add(room.Code);
                context.DraftRooms.Add(room);
            }

            var leftRoom = CreateRoom();
            leftRoom.Code = "LEFT01";
            leftRoom.LastActivityUtc = activityBase.AddMinutes(100);
            var leftParticipant = CreateParticipant(
                leftRoom,
                "Left account",
                "LEFT-ACCOUNT",
                account.Id,
                status: DraftParticipantStatus.Left);
            leftParticipant.Room = leftRoom;
            leftRoom.Participants.Add(leftParticipant);
            context.DraftRooms.Add(leftRoom);

            var kickedRoom = CreateRoom();
            kickedRoom.Code = "KICK01";
            kickedRoom.LastActivityUtc = activityBase.AddMinutes(150);
            var kickedParticipant = CreateParticipant(
                kickedRoom,
                "Kicked account",
                "KICKED-ACCOUNT",
                account.Id,
                status: DraftParticipantStatus.Kicked);
            kickedParticipant.Room = kickedRoom;
            kickedRoom.Participants.Add(kickedParticipant);
            context.DraftRooms.Add(kickedRoom);

            var otherAccountRoom = CreateRoom();
            otherAccountRoom.Code = "OTHER1";
            otherAccountRoom.LastActivityUtc = activityBase.AddMinutes(200);
            var otherParticipant = CreateParticipant(
                otherAccountRoom,
                "Other account",
                "OTHER-ACCOUNT",
                otherAccount.Id);
            otherParticipant.Room = otherAccountRoom;
            otherAccountRoom.Participants.Add(otherParticipant);
            context.DraftRooms.Add(otherAccountRoom);

            await context.SaveChangesAsync();
        }

        var participants = await this.repository.GetActiveParticipantsForUserAsync(
            account.Id,
            50,
            CancellationToken.None);

        participants.Should().HaveCount(50);
        participants.Select(participant => participant.Room.Code)
            .Should()
            .Equal(activeRoomCodes.Skip(2).Reverse());
        participants[0].Room.Status.Should().Be(DraftRoomStatus.Finished);
        participants[0].Room.Seats.Should().ContainSingle()
            .Which.Should().Match<DraftRoomSeat>(seat =>
                seat.ParticipantId == participants[0].Id
                && seat.Index == 7);
        participants
            .Select(participant => participant.Room.Code)
            .Should()
            .NotContain("LEFT01")
            .And
            .NotContain("KICK01")
            .And
            .NotContain("OTHER1");
    }

    private static DraftRoom CreateRoom(DraftRoomStatus status = DraftRoomStatus.Lobby)
    {
        var now = DateTime.UtcNow;

        return new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = Guid.NewGuid().ToString("N")[..6],
            Name = "Repository test room",
            PasswordHash = "test-hash",
            Status = status,
            DraftType = "faction-pick",
            Version = 1,
            CreatedAtUtc = now,
            LastActivityUtc = now,
        };
    }

    private static DraftRoomParticipant CreateParticipant(
        DraftRoom room,
        string displayName,
        string displayNameKey,
        string? userId = null,
        DraftRoomRole role = DraftRoomRole.Member,
        DraftParticipantStatus status = DraftParticipantStatus.Active)
    {
        return new DraftRoomParticipant
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            UserId = userId,
            SecretHash = userId is null ? new byte[32] : null,
            DisplayName = displayName,
            DisplayNameKey = displayNameKey,
            Role = role,
            Status = status,
            JoinedAtUtc = DateTime.UtcNow,
            LastSeenUtc = DateTime.UtcNow,
        };
    }

    private static TwilightImperiumUser CreateUser()
    {
        var userId = Guid.NewGuid().ToString();

        return new TwilightImperiumUser
        {
            Id = userId,
            UserName = userId,
            NormalizedUserName = userId.ToUpperInvariant(),
        };
    }
}
