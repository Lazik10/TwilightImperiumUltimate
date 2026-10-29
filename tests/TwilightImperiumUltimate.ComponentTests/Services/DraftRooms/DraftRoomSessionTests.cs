using System.Diagnostics.CodeAnalysis;
using Moq;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.ComponentTests.Services.DraftRooms;

[SuppressMessage(
    "Reliability",
    "CA2000:Dispose objects before losing scope",
    Justification = "Each test awaits session disposal; DraftRoomSession owns and disposes its injected connection.")]
public sealed class DraftRoomSessionTests
{
    [Fact]
    public async Task RefreshAsync_WhenApiReturnsRoomView_ShouldUpdateViewAndNotifyOnce()
    {
        var view = new DraftRoomViewDto { Code = "ABC234", Name = "Room One" };
        var apiService = new Mock<IDraftRoomApiService>();
        apiService
            .Setup(service => service.GetViewAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomViewDto>(DraftRoomApiResultKind.Success, view));
        await using var session = CreateSession(apiService);
        var changedCount = 0;
        session.Changed += () => changedCount++;
        session.Initialize("abc234");

        await session.RefreshAsync();

        session.View.Should().BeSameAs(view);
        session.Error.Should().BeNull();
        session.IsLoading.Should().BeFalse();
        changedCount.Should().Be(1);
        apiService.Verify(service => service.GetViewAsync("ABC234", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RefreshAsync_WhenApiReturnsForbidden_ShouldClearViewAndExposeError()
    {
        var apiService = new Mock<IDraftRoomApiService>();
        apiService
            .Setup(service => service.GetViewAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomViewDto>(DraftRoomApiResultKind.Forbidden, null));
        await using var session = CreateSession(apiService);
        session.Initialize("ABC234");

        await session.RefreshAsync();

        session.View.Should().BeNull();
        session.Error.Should().Be(DraftRoomApiResultKind.Forbidden);
        session.IsLoading.Should().BeFalse();
    }

    [Fact]
    public async Task ExecuteAsync_WhenCommandConflicts_ShouldRefreshViewAndNotifyOnce()
    {
        var view = new DraftRoomViewDto { Code = "ABC234", Name = "Latest Name" };
        var apiService = new Mock<IDraftRoomApiService>();
        apiService
            .Setup(service => service.GetViewAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomViewDto>(DraftRoomApiResultKind.Success, view));
        await using var session = CreateSession(apiService);
        var changedCount = 0;
        session.Changed += () => changedCount++;
        session.Initialize("ABC234");

        var result = await session.ExecuteAsync(
            () => Task.FromResult(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Conflict)));

        result.Kind.Should().Be(DraftRoomApiResultKind.Conflict);
        session.View.Should().BeSameAs(view);
        session.Error.Should().BeNull();
        changedCount.Should().Be(1);
        apiService.Verify(service => service.GetViewAsync("ABC234", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RefreshAsync_WhenSessionIsNotInitialized_ShouldThrow()
    {
        var apiService = new Mock<IDraftRoomApiService>();
        await using var session = CreateSession(apiService);

        var act = () => session.RefreshAsync();

        await act.Should().ThrowAsync<InvalidOperationException>();
        apiService.Verify(
            service => service.GetViewAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ConnectAsync_WhenRoomChangedIsNewer_ShouldRefreshOnceAndIgnoreOlderVersions()
    {
        var firstView = new DraftRoomViewDto { Code = "ABC234", Name = "Room One", Version = 1 };
        var latestView = new DraftRoomViewDto { Code = "ABC234", Name = "Room Two", Version = 2 };
        var apiService = new Mock<IDraftRoomApiService>();
        apiService
            .SetupSequence(service => service.GetViewAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomViewDto>(DraftRoomApiResultKind.Success, firstView))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomViewDto>(DraftRoomApiResultKind.Success, latestView));
        var connection = new TestDraftRoomConnection { SubscribeVersion = firstView.Version };
        await using var session = CreateSession(apiService, connection);
        session.Initialize("ABC234");

        await session.ConnectAsync();
        await connection.RaiseRoomChangedAsync(1);
        await connection.RaiseRoomChangedAsync(2);
        await connection.RaiseRoomChangedAsync(1);

        session.View.Should().BeSameAs(latestView);
        apiService.Verify(
            service => service.GetViewAsync("ABC234", It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }

    [Fact]
    public async Task Reconnected_WhenConnectionResubscribes_ShouldRefreshRoomView()
    {
        var view = new DraftRoomViewDto { Code = "ABC234", Name = "Room One", Version = 1 };
        var apiService = new Mock<IDraftRoomApiService>();
        apiService
            .Setup(service => service.GetViewAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomViewDto>(DraftRoomApiResultKind.Success, view));
        var connection = new TestDraftRoomConnection { SubscribeVersion = view.Version };
        await using var session = CreateSession(apiService, connection);
        session.Initialize("ABC234");

        await session.ConnectAsync();
        await connection.RaiseReconnectedAsync();

        apiService.Verify(
            service => service.GetViewAsync("ABC234", It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }

    [Fact]
    public async Task ChatMessageReceived_WhenHubNotifiesSession_ShouldForwardMessageToSubscribers()
    {
        var connection = new TestDraftRoomConnection();
        await using var session = CreateSession(new Mock<IDraftRoomApiService>(), connection);
        var message = new ChatMessageDto { Id = 4, Text = "A new message" };
        ChatMessageDto? receivedMessage = null;
        session.ChatMessageReceived += received =>
        {
            receivedMessage = received;
            return Task.CompletedTask;
        };

        await connection.RaiseChatMessageReceivedAsync(message);

        receivedMessage.Should().BeSameAs(message);
    }

    [Fact]
    public async Task LogEntryAdded_WhenHubNotifiesSession_ShouldForwardEntryToSubscribers()
    {
        var connection = new TestDraftRoomConnection();
        await using var session = CreateSession(new Mock<IDraftRoomApiService>(), connection);
        var entry = new DraftLogEntryDto { Id = 4, DataJson = "{}", Type = DraftLogEntryType.RoomCreated };
        DraftLogEntryDto? receivedEntry = null;
        session.LogEntryAdded += received =>
        {
            receivedEntry = received;
            return Task.CompletedTask;
        };

        await connection.RaiseLogEntryAddedAsync(entry);

        receivedEntry.Should().BeSameAs(entry);
    }

    [Fact]
    public async Task ConnectAsync_WhenGuestIdentityIsSaved_ShouldPassGuestCredentialsToConnection()
    {
        var identity = new GuestIdentity(
            Guid.NewGuid(),
            "ABC234",
            Guid.NewGuid(),
            "guest-secret",
            "Player",
            DateTimeOffset.UtcNow);
        var apiService = new Mock<IDraftRoomApiService>();
        apiService
            .Setup(service => service.GetViewAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new DraftRoomApiResult<DraftRoomViewDto>(
                    DraftRoomApiResultKind.Success,
                    new DraftRoomViewDto { Code = "ABC234", Version = 1 }));
        var guestIdentityStore = new Mock<IGuestIdentityStore>();
        guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(identity);
        var connection = new TestDraftRoomConnection { SubscribeVersion = 1 };
        await using var session = new DraftRoomSession(apiService.Object, connection, guestIdentityStore.Object);
        session.Initialize("ABC234");

        await session.ConnectAsync();

        connection.Credentials.Should().NotBeNull();
        connection.Credentials!.GuestParticipantId.Should().Be(identity.ParticipantId);
        connection.Credentials.GuestSecret.Should().Be(identity.Secret);
    }

    [Fact]
    public async Task PresenceChanged_WhenReceived_ShouldForwardTheUpdate()
    {
        var apiService = new Mock<IDraftRoomApiService>();
        var connection = new TestDraftRoomConnection();
        await using var session = CreateSession(apiService, connection);
        var update = new DraftRoomPresenceUpdateDto { ParticipantId = Guid.NewGuid(), IsOnline = true };
        DraftRoomPresenceUpdateDto? receivedUpdate = null;
        session.PresenceChanged += value => receivedUpdate = value;

        await connection.RaisePresenceChangedAsync(update);

        receivedUpdate.Should().BeSameAs(update);
    }

    [Fact]
    public async Task PresenceChanged_WhenParticipantComesOnline_ShouldUpdateViewAndNotify()
    {
        var participant = new DraftRoomViewParticipantDto
        {
            Id = Guid.NewGuid(),
            DisplayName = "Player",
        };
        var apiService = new Mock<IDraftRoomApiService>();
        apiService
            .Setup(service => service.GetViewAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new DraftRoomApiResult<DraftRoomViewDto>(
                    DraftRoomApiResultKind.Success,
                    new DraftRoomViewDto { Code = "ABC234", Participants = [participant] }));
        var connection = new TestDraftRoomConnection();
        await using var session = CreateSession(apiService, connection);
        var changedCount = 0;
        session.Changed += () => changedCount++;
        session.Initialize("ABC234");
        await session.RefreshAsync();
        changedCount = 0;

        await connection.RaisePresenceChangedAsync(
            new DraftRoomPresenceUpdateDto { ParticipantId = participant.Id, IsOnline = true });

        participant.IsOnline.Should().BeTrue();
        changedCount.Should().Be(1);
    }

    [Fact]
    public async Task Closed_WhenRoomCloses_ShouldRefreshAndExposeNotFound()
    {
        var apiService = new Mock<IDraftRoomApiService>();
        apiService
            .Setup(service => service.GetViewAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomViewDto>(DraftRoomApiResultKind.NotFound, null));
        var connection = new TestDraftRoomConnection();
        await using var session = CreateSession(apiService, connection);
        session.Initialize("ABC234");

        await connection.RaiseClosedAsync();

        session.Error.Should().Be(DraftRoomApiResultKind.NotFound);
        apiService.Verify(
            service => service.GetViewAsync("ABC234", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task RemovedFromRoom_WhenKicked_ShouldRefreshAndExposeForbidden()
    {
        var apiService = new Mock<IDraftRoomApiService>();
        apiService
            .Setup(service => service.GetViewAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomViewDto>(DraftRoomApiResultKind.Forbidden, null));
        var connection = new TestDraftRoomConnection();
        await using var session = CreateSession(apiService, connection);
        session.Initialize("ABC234");

        await connection.RaiseRemovedFromRoomAsync();

        session.Error.Should().Be(DraftRoomApiResultKind.Forbidden);
        apiService.Verify(
            service => service.GetViewAsync("ABC234", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static DraftRoomSession CreateSession(
        Mock<IDraftRoomApiService> apiService,
        TestDraftRoomConnection? connection = null)
    {
        var guestIdentityStore = new Mock<IGuestIdentityStore>();
        guestIdentityStore
            .Setup(store => store.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GuestIdentity?)null);

        return new DraftRoomSession(
            apiService.Object,
            connection ?? new TestDraftRoomConnection(),
            guestIdentityStore.Object);
    }

    private sealed class TestDraftRoomConnection : IDraftRoomConnection
    {
        public event Func<int, Task>? RoomChanged;

        public event Func<DraftRoomPresenceUpdateDto, Task>? PresenceChanged;

        public event Func<ChatMessageDto, Task>? ChatMessageReceived;

        public event Func<DraftLogEntryDto, Task>? LogEntryAdded;

        public event Func<Task>? Reconnected;

        public event Func<Task>? Closed;

        public event Func<Task>? RemovedFromRoom;

        public event Action<DraftRoomConnectionState>? StateChanged;

        public DraftRoomConnectionState State { get; private set; } = DraftRoomConnectionState.Offline;

        public int SubscribeVersion { get; init; }

        public DraftRoomCredentials? Credentials { get; private set; }

        public bool IsDisposed { get; private set; }

        public Task<int> ConnectAsync(
            string code,
            DraftRoomCredentials credentials,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentNullException.ThrowIfNull(credentials);
            cancellationToken.ThrowIfCancellationRequested();

            this.Credentials = credentials;
            this.State = DraftRoomConnectionState.Connected;
            this.StateChanged?.Invoke(this.State);
            return Task.FromResult(this.SubscribeVersion);
        }

        public ValueTask DisposeAsync()
        {
            this.IsDisposed = true;
            this.State = DraftRoomConnectionState.Offline;
            return ValueTask.CompletedTask;
        }

        public Task RaiseRoomChangedAsync(int version)
        {
            return this.RoomChanged?.Invoke(version) ?? Task.CompletedTask;
        }

        public Task RaiseReconnectedAsync()
        {
            return this.Reconnected?.Invoke() ?? Task.CompletedTask;
        }

        public Task RaisePresenceChangedAsync(DraftRoomPresenceUpdateDto presenceUpdate)
        {
            return this.PresenceChanged?.Invoke(presenceUpdate) ?? Task.CompletedTask;
        }

        public Task RaiseChatMessageReceivedAsync(ChatMessageDto message)
        {
            return this.ChatMessageReceived?.Invoke(message) ?? Task.CompletedTask;
        }

        public Task RaiseLogEntryAddedAsync(DraftLogEntryDto entry)
        {
            return this.LogEntryAdded?.Invoke(entry) ?? Task.CompletedTask;
        }

        public Task RaiseClosedAsync()
        {
            return this.Closed?.Invoke() ?? Task.CompletedTask;
        }

        public Task RaiseRemovedFromRoomAsync()
        {
            return this.RemovedFromRoom?.Invoke() ?? Task.CompletedTask;
        }
    }
}
