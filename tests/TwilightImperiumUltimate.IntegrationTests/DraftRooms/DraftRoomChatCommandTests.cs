using FluentAssertions;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
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
public sealed class DraftRoomChatCommandTests : IClassFixture<DraftRoomsModelTestsFixture>
{
    private const string GuestSecret = "chat-test-guest-secret";

    private readonly DraftRoomsModelTestsFixture fixture;

    public DraftRoomChatCommandTests(DraftRoomsModelTestsFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        this.fixture = fixture;
    }

    [Fact]
    public async Task SendChatCommand_WhenSpectatorSendsMessage_ShouldPersistAndBroadcastWithoutChangingVersion()
    {
        var (room, participant, caller) = CreateRoom(DraftParticipantStatus.Active);
        await SaveRoomAsync(room);
        var messageText = new string('a', DraftRoomLimits.MaxChatMessageLength);

        var notifier = new Mock<IRoomChangeNotifier>(MockBehavior.Strict);
        var messageWasPersistedWhenNotified = false;
        notifier
            .Setup(service => service.NotifyChatMessageReceivedAsync(room.Id, It.IsAny<ChatMessageDto>()))
            .Returns<Guid, ChatMessageDto>((_, message) =>
                this.VerifyMessagePersistedAsync(message.Id, value => messageWasPersistedWhenNotified = value));
        var handler = CreateHandler(
            notifier.Object,
            new DraftRoomChatThrottle(TimeProvider.System));

        var result = await handler.Handle(
            new SendChatCommand(room.Code, $"  {messageText}  ", caller),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().BeGreaterThan(0);
        result.Value.ParticipantId.Should().Be(participant.Id);
        result.Value.DisplayName.Should().Be(participant.DisplayName);
        result.Value.Kind.Should().Be(ChatMessageKind.User);
        result.Value.Text.Should().Be(messageText);
        messageWasPersistedWhenNotified.Should().BeTrue();

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedRoom = await verificationContext.DraftRooms.SingleAsync(item => item.Id == room.Id);
        var persistedMessage = await verificationContext.DraftRoomChatMessages
            .SingleAsync(item => item.RoomId == room.Id);
        persistedRoom.Version.Should().Be(room.Version);
        persistedRoom.LastActivityUtc.Should().BeAfter(room.LastActivityUtc);
        persistedMessage.Text.Should().Be(messageText);
        notifier.Verify(
            service => service.NotifyChatMessageReceivedAsync(room.Id, It.IsAny<ChatMessageDto>()),
            Times.Once);
        notifier.Verify(
            service => service.NotifyRoomChangedAsync(room.Id, It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public async Task SendChatCommand_WhenParticipantIsKicked_ShouldRejectWithoutSavingOrBroadcasting()
    {
        var (room, _, caller) = CreateRoom(DraftParticipantStatus.Kicked);
        await SaveRoomAsync(room);

        var notifier = new Mock<IRoomChangeNotifier>(MockBehavior.Strict);
        var handler = CreateHandler(
            notifier.Object,
            new DraftRoomChatThrottle(TimeProvider.System));

        var result = await handler.Handle(
            new SendChatCommand(room.Code, "Hello table", caller),
            CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code
            .Should().Be(DraftRoomErrorCode.NotFoundOrWrongPassword);

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.DraftRoomChatMessages.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await context.DraftRooms.SingleAsync(item => item.Id == room.Id))
            .Version.Should().Be(room.Version);
        notifier.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SendChatCommand_WhenParticipantExceedsMessageWindow_ShouldRejectSixthMessage()
    {
        var (room, _, caller) = CreateRoom(DraftParticipantStatus.Active);
        await SaveRoomAsync(room);

        var timeProvider = new TestTimeProvider(DateTimeOffset.UtcNow);
        var notifier = new Mock<IRoomChangeNotifier>(MockBehavior.Strict);
        notifier
            .Setup(service => service.NotifyChatMessageReceivedAsync(room.Id, It.IsAny<ChatMessageDto>()))
            .Returns(Task.CompletedTask);
        var handler = CreateHandler(notifier.Object, new DraftRoomChatThrottle(timeProvider));
        var results = new List<Result<ChatMessageDto>>();
        for (var index = 0; index < DraftRoomLimits.MaxChatMessagesPerWindow + 1; index++)
        {
            results.Add(await handler.Handle(
                new SendChatCommand(room.Code, $"Message {index}", caller),
                CancellationToken.None));
        }

        results.Take(DraftRoomLimits.MaxChatMessagesPerWindow)
            .Should()
            .OnlyContain(result => result.IsSuccess);
        results[^1].IsFailed.Should().BeTrue();
        results[^1].Errors.OfType<DraftRoomError>().Single().Code
            .Should().Be(DraftRoomErrorCode.ChatRateLimited);

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.DraftRoomChatMessages.CountAsync(item => item.RoomId == room.Id))
            .Should().Be(DraftRoomLimits.MaxChatMessagesPerWindow);
        (await context.DraftRooms.SingleAsync(item => item.Id == room.Id))
            .Version.Should().Be(room.Version);
        notifier.Verify(
            service => service.NotifyChatMessageReceivedAsync(room.Id, It.IsAny<ChatMessageDto>()),
            Times.Exactly(DraftRoomLimits.MaxChatMessagesPerWindow));
        notifier.Verify(
            service => service.NotifyRoomChangedAsync(room.Id, It.IsAny<int>()),
            Times.Never);
    }

    private static (DraftRoom Room, DraftRoomParticipant Participant, DraftRoomCaller Caller) CreateRoom(
        DraftParticipantStatus participantStatus)
    {
        var now = DateTime.UtcNow;
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = Guid.NewGuid().ToString("N")[..DraftRoomLimits.CodeLength].ToUpperInvariant(),
            Name = "Chat test room",
            PasswordHash = "test-hash",
            Status = DraftRoomStatus.Lobby,
            DraftType = string.Empty,
            Version = 1,
            CreatedAtUtc = now,
            LastActivityUtc = now,
        };
        var participant = new DraftRoomParticipant
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            SecretHash = new GuestSecretService().Hash(GuestSecret),
            DisplayName = "Unseated player",
            DisplayNameKey = "UNSEATED PLAYER",
            Role = DraftRoomRole.Member,
            Status = participantStatus,
            JoinedAtUtc = now,
            LastSeenUtc = now,
            Room = room,
        };
        room.Participants.Add(participant);

        return (
            room,
            participant,
            new DraftRoomCaller(null, null, participant.Id, GuestSecret));
    }

    private SendChatCommandHandler CreateHandler(
        IRoomChangeNotifier notifier,
        IDraftRoomChatThrottle chatThrottle)
    {
        return new SendChatCommandHandler(
            new DraftRoomUnitOfWork(
                this.fixture.Database.ContextFactory,
                notifier),
            new DraftRoomAuthorizer(new GuestSecretService()),
            chatThrottle,
            notifier,
            NullLogger<SendChatCommandHandler>.Instance);
    }

    private async Task SaveRoomAsync(DraftRoom room)
    {
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.DraftRooms.Add(room);
        await context.SaveChangesAsync();
    }

    private async Task VerifyMessagePersistedAsync(long messageId, Action<bool> reportResult)
    {
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        reportResult(await context.DraftRoomChatMessages.AnyAsync(item => item.Id == messageId));
    }

    private sealed class TestTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow()
        {
            return utcNow;
        }
    }
}
