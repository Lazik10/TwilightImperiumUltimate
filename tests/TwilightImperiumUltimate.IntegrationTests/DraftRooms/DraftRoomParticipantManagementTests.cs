using Microsoft.EntityFrameworkCore;
using Moq;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Core.Interfaces.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomParticipantManagementTests : IClassFixture<DraftRoomsModelTestsFixture>
{
    private readonly DraftRoomsModelTestsFixture fixture;
    private readonly IDraftRoomUnitOfWork unitOfWork;
    private readonly IDraftRoomAuthorizer authorizer;

    public DraftRoomParticipantManagementTests(DraftRoomsModelTestsFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        this.fixture = fixture;
        this.unitOfWork = new DraftRoomUnitOfWork(
            fixture.Database.ContextFactory,
            new NullRoomChangeNotifier());
        this.authorizer = new DraftRoomAuthorizer(new GuestSecretService());
    }

    [Fact]
    public async Task LeaveRoomCommand_WhenParticipantLeavesLobby_ShouldMarkLeftAndFreeSeat()
    {
        var (room, _) = CreateRoom();
        var (member, memberCaller) = AddGuestParticipant(room, DraftRoomRole.Member, "Player");
        SetSeatParticipant(room, 2, member);
        await SaveRoomAsync(room);
        var notifier = new Mock<IRoomChangeNotifier>(MockBehavior.Strict);
        notifier
            .Setup(service => service.NotifyChatMessageReceivedAsync(room.Id, It.IsAny<ChatMessageDto>()))
            .Returns(Task.CompletedTask);

        var result = await new LeaveRoomCommandHandler(
                this.unitOfWork,
                this.authorizer,
                new DraftRoomLog(),
                notifier.Object)
            .Handle(new LeaveRoomCommand(room.Code, memberCaller), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedMember = await context.DraftRoomParticipants.SingleAsync(item => item.Id == member.Id);
        persistedMember.Status.Should().Be(DraftParticipantStatus.Left);
        persistedMember.LastSeenUtc.Should().BeAfter(member.LastSeenUtc);
        var leaveMessage = await context.DraftRoomChatMessages.SingleAsync(item => item.RoomId == room.Id);
        leaveMessage.ParticipantId.Should().Be(member.Id);
        leaveMessage.Kind.Should().Be(ChatMessageKind.Leave);
        leaveMessage.Text.Should().Be("Player left");
        notifier.Verify(
            service => service.NotifyChatMessageReceivedAsync(room.Id, It.Is<ChatMessageDto>(
                message => message.Id == leaveMessage.Id && message.Kind == ChatMessageKind.Leave)),
            Times.Once);
        var persistedSeat = await context.DraftRoomSeats.SingleAsync(item => item.RoomId == room.Id && item.Index == 2);
        persistedSeat.ParticipantId.Should().BeNull();
        persistedSeat.ClaimedAtUtc.Should().BeNull();
        await DraftRoomLogAssertions.AssertEntriesAsync(
            this.fixture.Database.ContextFactory,
            room.Id,
            DraftLogEntryType.ParticipantLeft,
            DraftLogEntryType.SeatUnclaimed);
    }

    [Fact]
    public async Task LeaveRoomCommand_WhenRoomIsInProgress_ShouldRejectWithoutChangingParticipant()
    {
        var (room, _) = CreateRoom();
        var (member, memberCaller) = AddGuestParticipant(room, DraftRoomRole.Member, "Player");
        SetSeatParticipant(room, 2, member);
        room.Status = DraftRoomStatus.InProgress;
        await SaveRoomAsync(room);

        var result = await new LeaveRoomCommandHandler(
                this.unitOfWork,
                this.authorizer,
                new DraftRoomLog(),
                new NullRoomChangeNotifier())
            .Handle(new LeaveRoomCommand(room.Code, memberCaller), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.RoomChanged);

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.DraftRoomParticipants.SingleAsync(item => item.Id == member.Id))
            .Status.Should().Be(DraftParticipantStatus.Active);
    }

    [Fact]
    public async Task KickParticipantCommand_WhenHostKicksMember_ShouldMarkKickedAndFreeSeat()
    {
        var (room, hostCaller) = CreateRoom();
        var (member, _) = AddGuestParticipant(room, DraftRoomRole.Member, "Player");
        SetSeatParticipant(room, 2, member);
        await SaveRoomAsync(room);
        var notifier = new Mock<IRoomChangeNotifier>(MockBehavior.Strict);
        notifier
            .Setup(service => service.NotifyChatMessageReceivedAsync(room.Id, It.IsAny<ChatMessageDto>()))
            .Returns(Task.CompletedTask);

        var result = await new KickParticipantCommandHandler(
                this.unitOfWork,
                this.authorizer,
                new DraftRoomLog(),
                notifier.Object)
            .Handle(new KickParticipantCommand(room.Code, member.Id, hostCaller), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.DraftRoomParticipants.SingleAsync(item => item.Id == member.Id))
            .Status.Should().Be(DraftParticipantStatus.Kicked);
        (await context.DraftRoomSeats.SingleAsync(item => item.RoomId == room.Id && item.Index == 2))
            .ParticipantId.Should().BeNull();
        var leaveMessage = await context.DraftRoomChatMessages.SingleAsync(item => item.RoomId == room.Id);
        leaveMessage.ParticipantId.Should().Be(member.Id);
        leaveMessage.Kind.Should().Be(ChatMessageKind.Leave);
        leaveMessage.Text.Should().Be("Player left");
        notifier.Verify(
            service => service.NotifyChatMessageReceivedAsync(room.Id, It.Is<ChatMessageDto>(
                message => message.Id == leaveMessage.Id && message.Kind == ChatMessageKind.Leave)),
            Times.Once);
        await DraftRoomLogAssertions.AssertEntriesAsync(
            this.fixture.Database.ContextFactory,
            room.Id,
            DraftLogEntryType.ParticipantKicked,
            DraftLogEntryType.SeatUnclaimed);
    }

    [Theory]
    [InlineData(DraftRoomRole.Host)]
    [InlineData(DraftRoomRole.CoHost)]
    public async Task KickParticipantCommand_WhenCoHostTargetsHostOrCoHost_ShouldReject(
        DraftRoomRole targetRole)
    {
        var (room, _) = CreateRoom();
        var (_, coHostCaller) = AddGuestParticipant(room, DraftRoomRole.CoHost, "Co-host");
        var target = targetRole == DraftRoomRole.Host
            ? room.Participants.Single(participant => participant.Role == DraftRoomRole.Host)
            : AddGuestParticipant(room, DraftRoomRole.CoHost, "Second co-host").Participant;
        await SaveRoomAsync(room);

        var result = await new KickParticipantCommandHandler(
                this.unitOfWork,
                this.authorizer,
                new DraftRoomLog(),
                new NullRoomChangeNotifier())
            .Handle(new KickParticipantCommand(room.Code, target.Id, coHostCaller), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.Forbidden);
    }

    [Fact]
    public async Task SetParticipantRoleCommand_WhenHostPromotesMember_ShouldUpdateRole()
    {
        var (room, hostCaller) = CreateRoom();
        var (member, _) = AddGuestParticipant(room, DraftRoomRole.Member, "Player");
        await SaveRoomAsync(room);

        var result = await new SetParticipantRoleCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(
                new SetParticipantRoleCommand(room.Code, member.Id, DraftRoomRole.CoHost, hostCaller),
                CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.DraftRoomParticipants.SingleAsync(item => item.Id == member.Id))
            .Role.Should().Be(DraftRoomRole.CoHost);
        await DraftRoomLogAssertions.AssertEntriesAsync(
            this.fixture.Database.ContextFactory,
            room.Id,
            DraftLogEntryType.RoleChanged);
    }

    [Fact]
    public async Task SetParticipantRoleCommand_WhenCoHostAttemptsRoleChange_ShouldReject()
    {
        var (room, _) = CreateRoom();
        var (coHost, coHostCaller) = AddGuestParticipant(room, DraftRoomRole.CoHost, "Co-host");
        var (member, _) = AddGuestParticipant(room, DraftRoomRole.Member, "Player");
        await SaveRoomAsync(room);

        var result = await new SetParticipantRoleCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(
                new SetParticipantRoleCommand(room.Code, member.Id, DraftRoomRole.CoHost, coHostCaller),
                CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.Forbidden);

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.DraftRoomParticipants.SingleAsync(item => item.Id == coHost.Id))
            .Role.Should().Be(DraftRoomRole.CoHost);
        (await context.DraftRoomParticipants.SingleAsync(item => item.Id == member.Id))
            .Role.Should().Be(DraftRoomRole.Member);
    }

    [Fact]
    public async Task SetParticipantRoleCommand_WhenRequestWouldCreateAnotherHost_ShouldRejectInvalidRole()
    {
        var (room, hostCaller) = CreateRoom();
        var (member, _) = AddGuestParticipant(room, DraftRoomRole.Member, "Player");
        await SaveRoomAsync(room);

        var result = await new SetParticipantRoleCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(
                new SetParticipantRoleCommand(room.Code, member.Id, DraftRoomRole.Host, hostCaller),
                CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.InvalidRole);
    }

    private static (DraftRoom Room, DraftRoomCaller HostCaller) CreateRoom()
    {
        var now = DateTime.UtcNow;
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant(),
            Name = "Participant management test room",
            PasswordHash = "test-hash",
            Status = DraftRoomStatus.Lobby,
            DraftType = "test",
            Version = 1,
            CreatedAtUtc = now,
            LastActivityUtc = now,
        };
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

    private async Task SaveRoomAsync(DraftRoom room)
    {
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.DraftRooms.Add(room);
        await context.SaveChangesAsync();
    }
}
