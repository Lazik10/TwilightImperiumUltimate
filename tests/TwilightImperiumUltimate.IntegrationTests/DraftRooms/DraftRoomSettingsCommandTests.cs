using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomSettingsCommandTests : IClassFixture<DraftRoomsModelTestsFixture>
{
    private readonly DraftRoomsModelTestsFixture fixture;
    private readonly IDraftRoomUnitOfWork unitOfWork;
    private readonly IDraftRoomAuthorizer authorizer;
    private readonly RoomPasswordHasher passwordHasher;

    public DraftRoomSettingsCommandTests(DraftRoomsModelTestsFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        this.fixture = fixture;
        this.unitOfWork = new DraftRoomUnitOfWork(
            fixture.Database.ContextFactory,
            new NullRoomChangeNotifier());
        this.authorizer = new DraftRoomAuthorizer(new GuestSecretService());
        this.passwordHasher = new RoomPasswordHasher();
    }

    [Fact]
    public async Task UpdateRoomSettingsCommand_WhenHostUpdatesSettings_ShouldTrimNameAndSetLock()
    {
        var (room, hostCaller) = CreateRoom();
        await SaveRoomAsync(room);

        var result = await this.CreateSettingsHandler()
            .Handle(
                new UpdateRoomSettingsCommand(room.Code, "  Updated Room  ", true, hostCaller),
                CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var updatedRoom = await context.DraftRooms.SingleAsync(item => item.Id == room.Id);
        updatedRoom.Name.Should().Be("Updated Room");
        updatedRoom.IsLocked.Should().BeTrue();
        updatedRoom.Version.Should().Be(room.Version + 1);
        await DraftRoomLogAssertions.AssertEntriesAsync(
            this.fixture.Database.ContextFactory,
            room.Id,
            DraftLogEntryType.RoomRenamed,
            DraftLogEntryType.RoomLocked);
    }

    [Fact]
    public async Task UpdateRoomSettingsCommand_WhenHostUnlocksRoom_ShouldLogRoomUnlocked()
    {
        var (room, hostCaller) = CreateRoom();
        room.IsLocked = true;
        await SaveRoomAsync(room);

        var result = await this.CreateSettingsHandler()
            .Handle(
                new UpdateRoomSettingsCommand(room.Code, room.Name, false, hostCaller),
                CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await DraftRoomLogAssertions.AssertEntriesAsync(
            this.fixture.Database.ContextFactory,
            room.Id,
            DraftLogEntryType.RoomUnlocked);
    }

    [Theory]
    [InlineData("")]
    [InlineData("x")]
    public async Task UpdateRoomSettingsCommand_WhenNameIsTooShort_ShouldReject(string name)
    {
        var (room, hostCaller) = CreateRoom();
        await SaveRoomAsync(room);

        var result = await this.CreateSettingsHandler()
            .Handle(
                new UpdateRoomSettingsCommand(room.Code, name, true, hostCaller),
                CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.InvalidRoomName);
        (await GetRoomAsync(room.Id)).Name.Should().Be(room.Name);
    }

    [Fact]
    public async Task UpdateRoomSettingsCommand_WhenNameExceedsMaximum_ShouldReject()
    {
        var (room, hostCaller) = CreateRoom();
        await SaveRoomAsync(room);

        var result = await this.CreateSettingsHandler()
            .Handle(
                new UpdateRoomSettingsCommand(
                    room.Code,
                    new string('a', DraftRoomLimits.MaxRoomNameLength + 1),
                    true,
                    hostCaller),
                CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.InvalidRoomName);
        (await GetRoomAsync(room.Id)).Name.Should().Be(room.Name);
    }

    [Fact]
    public async Task UpdateRoomSettingsCommand_WhenCallerIsNotManager_ShouldReject()
    {
        var (room, _) = CreateRoom();
        var (_, memberCaller) = AddGuestParticipant(room, DraftRoomRole.Member, "Player");
        await SaveRoomAsync(room);

        var result = await this.CreateSettingsHandler()
            .Handle(
                new UpdateRoomSettingsCommand(room.Code, "Updated Room", true, memberCaller),
                CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.Forbidden);
        (await GetRoomAsync(room.Id)).Name.Should().Be(room.Name);
    }

    [Fact]
    public async Task ChangeRoomPasswordCommand_WhenHostChangesPassword_ShouldRehashWithoutChangingParticipants()
    {
        var (room, hostCaller) = CreateRoom();
        var (member, _) = AddGuestParticipant(room, DraftRoomRole.Member, "Player");
        await SaveRoomAsync(room);

        var result = await new ChangeRoomPasswordCommandHandler(
                this.unitOfWork,
                this.authorizer,
                this.passwordHasher,
                new DraftRoomLog())
            .Handle(
                new ChangeRoomPasswordCommand(room.Code, "new-password", hostCaller),
                CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var updatedRoom = await context.DraftRooms.SingleAsync(item => item.Id == room.Id);
        this.passwordHasher.Verify(updatedRoom, updatedRoom.PasswordHash, "new-password").Should().BeTrue();
        this.passwordHasher.Verify(updatedRoom, updatedRoom.PasswordHash, "old-password").Should().BeFalse();
        (await context.DraftRoomParticipants.SingleAsync(item => item.Id == member.Id))
            .Status.Should().Be(DraftParticipantStatus.Active);
        updatedRoom.Version.Should().Be(room.Version + 1);
        await DraftRoomLogAssertions.AssertEntriesAsync(
            this.fixture.Database.ContextFactory,
            room.Id,
            DraftLogEntryType.PasswordChanged);
        await DraftRoomLogAssertions.AssertDoesNotContainAsync(
            this.fixture.Database.ContextFactory,
            room.Id,
            "new-password",
            room.PasswordHash,
            updatedRoom.PasswordHash);
    }

    [Theory]
    [InlineData("")]
    [InlineData("short")]
    public async Task ChangeRoomPasswordCommand_WhenPasswordIsTooShort_ShouldReject(string password)
    {
        var (room, hostCaller) = CreateRoom();
        await SaveRoomAsync(room);

        var result = await new ChangeRoomPasswordCommandHandler(
                this.unitOfWork,
                this.authorizer,
                this.passwordHasher,
                new DraftRoomLog())
            .Handle(
                new ChangeRoomPasswordCommand(room.Code, password, hostCaller),
                CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.InvalidPassword);
        var persistedRoom = await GetRoomAsync(room.Id);
        this.passwordHasher.Verify(persistedRoom, persistedRoom.PasswordHash, "old-password").Should().BeTrue();
    }

    [Fact]
    public async Task ChangeRoomPasswordCommand_WhenPasswordExceedsMaximum_ShouldReject()
    {
        var (room, hostCaller) = CreateRoom();
        await SaveRoomAsync(room);

        var result = await new ChangeRoomPasswordCommandHandler(
                this.unitOfWork,
                this.authorizer,
                this.passwordHasher,
                new DraftRoomLog())
            .Handle(
                new ChangeRoomPasswordCommand(
                    room.Code,
                    new string('a', DraftRoomLimits.MaxPasswordLength + 1),
                    hostCaller),
                CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.InvalidPassword);
    }

    [Fact]
    public async Task ChangeRoomPasswordCommand_WhenCallerIsNotManager_ShouldReject()
    {
        var (room, _) = CreateRoom();
        var (_, memberCaller) = AddGuestParticipant(room, DraftRoomRole.Member, "Player");
        await SaveRoomAsync(room);

        var result = await new ChangeRoomPasswordCommandHandler(
                this.unitOfWork,
                this.authorizer,
                this.passwordHasher,
                new DraftRoomLog())
            .Handle(
                new ChangeRoomPasswordCommand(room.Code, "new-password", memberCaller),
                CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.Forbidden);
        var persistedRoom = await GetRoomAsync(room.Id);
        this.passwordHasher.Verify(persistedRoom, persistedRoom.PasswordHash, "old-password").Should().BeTrue();
    }

    [Fact]
    public async Task CloseRoomCommand_WhenHostClosesRoom_ShouldDeleteRoomParticipantsAndSeats()
    {
        var (room, hostCaller) = CreateRoom();
        var (member, _) = AddGuestParticipant(room, DraftRoomRole.Member, "Player");
        SetSeatParticipant(room, 2, member);
        room.Status = DraftRoomStatus.InProgress;
        await SaveRoomAsync(room);

        var result = await new CloseRoomCommandHandler(this.unitOfWork, this.authorizer)
            .Handle(new CloseRoomCommand(room.Code, hostCaller), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.DraftRooms.AnyAsync(item => item.Id == room.Id)).Should().BeFalse();
        (await context.DraftRoomParticipants.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await context.DraftRoomSeats.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
    }

    [Fact]
    public async Task CloseRoomCommand_WhenCallerIsNotManager_ShouldRejectAndPreserveRoom()
    {
        var (room, _) = CreateRoom();
        var (_, memberCaller) = AddGuestParticipant(room, DraftRoomRole.Member, "Player");
        await SaveRoomAsync(room);

        var result = await new CloseRoomCommandHandler(this.unitOfWork, this.authorizer)
            .Handle(new CloseRoomCommand(room.Code, memberCaller), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.Forbidden);
        (await GetRoomAsync(room.Id)).Should().NotBeNull();
    }

    private static (DraftRoom Room, DraftRoomCaller HostCaller) CreateRoom()
    {
        var now = DateTime.UtcNow;
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant(),
            Name = "Room settings test",
            Status = DraftRoomStatus.Lobby,
            DraftType = "test",
            Version = 1,
            CreatedAtUtc = now,
            LastActivityUtc = now,
        };
        room.PasswordHash = new RoomPasswordHasher().Hash(room, "old-password");

        var (host, hostCaller) = AddGuestParticipant(room, DraftRoomRole.Host, "Host");
        for (var index = 1; index <= DraftRoomLimits.DefaultSeatCount; index++)
        {
            var participant = index == 1 ? host : null;
            room.Seats.Add(new DraftRoomSeat
            {
                Id = Guid.NewGuid(),
                RoomId = room.Id,
                Index = index,
                ParticipantId = participant?.Id,
                ClaimedAtUtc = participant is null ? null : now,
                Room = room,
                Participant = participant,
            });
        }

        return (room, hostCaller);
    }

    private static (DraftRoomParticipant Participant, DraftRoomCaller Caller) AddGuestParticipant(
        DraftRoom room,
        DraftRoomRole role,
        string displayName)
    {
        var secret = $"test-secret-{Guid.NewGuid():N}";
        var participant = new DraftRoomParticipant
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            SecretHash = new GuestSecretService().Hash(secret),
            DisplayName = displayName,
            DisplayNameKey = displayName.ToUpperInvariant(),
            Role = role,
            Status = DraftParticipantStatus.Active,
            JoinedAtUtc = DateTime.UtcNow,
            LastSeenUtc = DateTime.UtcNow,
            Room = room,
        };
        room.Participants.Add(participant);

        return (participant, new DraftRoomCaller(null, null, participant.Id, secret));
    }

    private static void SetSeatParticipant(
        DraftRoom room,
        int seatIndex,
        DraftRoomParticipant participant)
    {
        var seat = room.Seats.Single(item => item.Index == seatIndex);
        seat.ParticipantId = participant.Id;
        seat.Participant = participant;
        seat.ClaimedAtUtc = DateTime.UtcNow;
    }

    private UpdateRoomSettingsCommandHandler CreateSettingsHandler() => new(
        this.unitOfWork,
        this.authorizer,
        new DraftRoomLog(),
        Moq.Mock.Of<IDraftTypeRegistry>(),
        Moq.Mock.Of<IDraftRoomDraftPersistence>());

    private async Task SaveRoomAsync(DraftRoom room)
    {
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.DraftRooms.Add(room);
        await context.SaveChangesAsync();
    }

    private async Task<DraftRoom> GetRoomAsync(Guid roomId)
    {
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();

        return await context.DraftRooms.SingleAsync(item => item.Id == roomId);
    }
}
