using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Core.Interfaces.DraftRooms;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomUnitOfWorkTests : IClassFixture<DraftRoomsModelTestsFixture>
{
    private readonly DraftRoomsModelTestsFixture fixture;
    private readonly DraftRoomUnitOfWork unitOfWork;

    public DraftRoomUnitOfWorkTests(DraftRoomsModelTestsFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        this.fixture = fixture;
        this.unitOfWork = new DraftRoomUnitOfWork(
            fixture.Database.ContextFactory,
            new NullRoomChangeNotifier());
    }

    [Fact]
    public async Task ExecuteAsync_WhenOperationsOverlap_ShouldSerializeAndIncrementVersion()
    {
        var room = CreateRoom();
        await SaveRoomAsync(this.fixture.Database.ContextFactory, room);

        var firstOperationEntered = NewSignal();
        var firstOperationRelease = NewSignal();
        var secondOperationEntered = NewSignal();

        var firstOperation = this.unitOfWork.ExecuteAsync(
            room.Code,
            async scope =>
            {
                firstOperationEntered.TrySetResult();
                await firstOperationRelease.Task;

                scope.Room.Name = "First serialized operation";

                return true;
            },
            CancellationToken.None);
        await firstOperationEntered.Task.WaitAsync(TimeSpan.FromSeconds(10));

        var secondOperation = this.unitOfWork.ExecuteAsync(
            room.Code,
            scope =>
            {
                scope.Room.Name = "Second serialized operation";
                secondOperationEntered.TrySetResult();

                return Task.FromResult(true);
            },
            CancellationToken.None);

        await Task.Delay(TimeSpan.FromMilliseconds(100));
        var secondOperationEnteredBeforeRelease = secondOperationEntered.Task.IsCompleted;
        firstOperationRelease.TrySetResult();

        await Task.WhenAll(firstOperation, secondOperation).WaitAsync(TimeSpan.FromSeconds(10));

        var firstResult = await firstOperation;
        var secondResult = await secondOperation;
        secondOperationEnteredBeforeRelease.Should().BeFalse();
        firstResult.Should().BeTrue();
        secondResult.Should().BeTrue();

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedRoom = await verificationContext.DraftRooms.SingleAsync(item => item.Id == room.Id);
        persistedRoom.Version.Should().Be(2);
        persistedRoom.LastActivityUtc.Should().BeAfter(room.LastActivityUtc);
    }

    [Fact]
    public async Task ExecuteAsync_WhenWorkMakesNoChanges_ShouldNotIncrementRoomVersion()
    {
        var room = CreateRoom();
        await SaveRoomAsync(this.fixture.Database.ContextFactory, room);

        _ = await this.unitOfWork.ExecuteAsync(
            room.Code,
            _ => Task.FromResult(true),
            CancellationToken.None);

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedRoom = await verificationContext.DraftRooms.SingleAsync(item => item.Id == room.Id);
        persistedRoom.Version.Should().Be(room.Version);
        persistedRoom.LastActivityUtc.Should().Be(room.LastActivityUtc);
    }

    [Fact]
    public async Task ExecuteAsync_WhenChangesStateIsFalse_ShouldUpdateActivityWithoutVersionOrRoomNotification()
    {
        var room = CreateRoom();
        await SaveRoomAsync(this.fixture.Database.ContextFactory, room);

        var notifier = new RecordingRoomChangeNotifier();
        var notifyingUnitOfWork = new DraftRoomUnitOfWork(this.fixture.Database.ContextFactory, notifier);

        _ = await notifyingUnitOfWork.ExecuteAsync(
            room.Code,
            scope =>
            {
                scope.Context.DraftRoomChatMessages.Add(new DraftRoomChatMessage
                {
                    RoomId = scope.Room.Id,
                    Kind = ChatMessageKind.User,
                    Text = "Hello",
                    CreatedAtUtc = scope.Now,
                });

                return Task.FromResult(true);
            },
            CancellationToken.None,
            changesState: false);

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedRoom = await verificationContext.DraftRooms.SingleAsync(item => item.Id == room.Id);
        persistedRoom.Version.Should().Be(room.Version);
        persistedRoom.LastActivityUtc.Should().BeAfter(room.LastActivityUtc);
        (await verificationContext.DraftRoomChatMessages.CountAsync(item => item.RoomId == room.Id))
            .Should().Be(1);
        notifier.RoomChanges.Should().BeEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_WhenOperationAddsLogEntry_ShouldNotifyAfterCommit()
    {
        var room = CreateRoom();
        await SaveRoomAsync(this.fixture.Database.ContextFactory, room);

        var notifier = new RecordingRoomChangeNotifier();
        var logWasPersistedWhenNotified = false;
        notifier.OnLogEntryAddedAsync = async (roomId, entry) =>
        {
            await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
            logWasPersistedWhenNotified = await context.DraftRoomLogEntries
                .AnyAsync(item => item.Id == entry.Id && item.RoomId == roomId);
        };
        var notifyingUnitOfWork = new DraftRoomUnitOfWork(this.fixture.Database.ContextFactory, notifier);

        _ = await notifyingUnitOfWork.ExecuteAsync(
            room.Code,
            scope =>
            {
                scope.Context.DraftRoomLogEntries.Add(new DraftRoomLogEntry
                {
                    RoomId = scope.Room.Id,
                    ActorParticipantId = null,
                    Type = DraftLogEntryType.RoomRenamed,
                    DataJson = "{}",
                    CreatedAtUtc = scope.Now,
                });

                return Task.FromResult(true);
            },
            CancellationToken.None);

        notifier.LogEntries.Should().ContainSingle()
            .Which.Entry.Type.Should().Be(DraftLogEntryType.RoomRenamed);
        notifier.LogEntries.Single().Entry.Id.Should().BeGreaterThan(0);
        logWasPersistedWhenNotified.Should().BeTrue();
    }

    [Fact]
    public async Task ExecuteAsync_WhenChangesCommit_ShouldNotifyAfterTheNewVersionIsVisible()
    {
        var room = CreateRoom();
        await SaveRoomAsync(this.fixture.Database.ContextFactory, room);

        var notifier = new RecordingRoomChangeNotifier();
        notifier.OnRoomChangedAsync = async (roomId, _) =>
        {
            await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
            notifier.VersionObservedDuringNotification = await context.DraftRooms
                .Where(item => item.Id == roomId)
                .Select(item => (int?)item.Version)
                .SingleOrDefaultAsync();
        };
        var notifyingUnitOfWork = new DraftRoomUnitOfWork(this.fixture.Database.ContextFactory, notifier);

        _ = await notifyingUnitOfWork.ExecuteAsync(
            room.Code,
            scope =>
            {
                scope.Room.Name = "Committed before notification";
                return Task.FromResult(true);
            },
            CancellationToken.None);

        notifier.RoomChanges.Should().ContainSingle().Which.Should().Be((room.Id, 1));
        notifier.VersionObservedDuringNotification.Should().Be(1);
    }

    [Fact]
    public async Task ExecuteAsync_WhenRoomIsDeleted_ShouldNotifyRoomClosed()
    {
        var room = CreateRoom();
        await SaveRoomAsync(this.fixture.Database.ContextFactory, room);

        var notifier = new RecordingRoomChangeNotifier();
        var notifyingUnitOfWork = new DraftRoomUnitOfWork(this.fixture.Database.ContextFactory, notifier);

        _ = await notifyingUnitOfWork.ExecuteAsync(
            room.Code,
            scope =>
            {
                scope.Context.DraftRooms.Remove(scope.Room);
                return Task.FromResult(true);
            },
            CancellationToken.None);

        notifier.ClosedRooms.Should().ContainSingle().Which.Should().Be(room.Id);
        notifier.RoomChanges.Should().BeEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_WhenParticipantIsKicked_ShouldNotifyRoomAndRemovedParticipant()
    {
        var room = CreateRoom();
        var participant = new DraftRoomParticipant
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            DisplayName = "Kicked player",
            DisplayNameKey = "kicked player",
            Role = DraftRoomRole.Member,
            Status = DraftParticipantStatus.Active,
            JoinedAtUtc = room.CreatedAtUtc,
            LastSeenUtc = room.CreatedAtUtc,
            Room = room,
        };
        room.Participants.Add(participant);
        await SaveRoomAsync(this.fixture.Database.ContextFactory, room);

        var notifier = new RecordingRoomChangeNotifier();
        var notifyingUnitOfWork = new DraftRoomUnitOfWork(this.fixture.Database.ContextFactory, notifier);

        _ = await notifyingUnitOfWork.ExecuteAsync(
            room.Code,
            scope =>
            {
                scope.Room.Participants.Single().Status = DraftParticipantStatus.Kicked;
                return Task.FromResult(true);
            },
            CancellationToken.None);

        notifier.RoomChanges.Should().ContainSingle().Which.Should().Be((room.Id, 1));
        notifier.RemovedParticipants.Should().ContainSingle().Which.Should().Be((room.Id, participant.Id));
    }

    [Fact]
    public async Task ExecuteAsync_WhenRoomVersionChangesDuringOperation_ShouldThrowConflict()
    {
        var room = CreateRoom();
        await SaveRoomAsync(this.fixture.Database.ContextFactory, room);

        var notifier = new RecordingRoomChangeNotifier();
        var notifyingUnitOfWork = new DraftRoomUnitOfWork(this.fixture.Database.ContextFactory, notifier);
        var act = () => notifyingUnitOfWork.ExecuteAsync(
            room.Code,
            async scope =>
            {
                scope.Room.Name = "Updated by this operation";

                await using var concurrentContext =
                    await this.fixture.Database.ContextFactory.CreateDbContextAsync();
                var concurrentRoom = await concurrentContext.DraftRooms
                    .SingleAsync(item => item.Id == room.Id);
                concurrentRoom.Version++;
                await concurrentContext.SaveChangesAsync();

                return true;
            },
            CancellationToken.None);

        await act.Should().ThrowAsync<DraftRoomConflictException>();
        notifier.RoomChanges.Should().BeEmpty();
    }

    private static DraftRoom CreateRoom()
    {
        var now = DateTime.UtcNow;

        return new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant(),
            Name = "Unit of work test room",
            PasswordHash = "test-hash",
            Status = DraftRoomStatus.Lobby,
            DraftType = "test",
            IsLocked = false,
            Version = 0,
            CreatedAtUtc = now,
            LastActivityUtc = now,
        };
    }

    private static async Task SaveRoomAsync(
        IDbContextFactory<TwilightImperiumDbContext> contextFactory,
        DraftRoom room)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.DraftRooms.Add(room);
        await context.SaveChangesAsync();
    }

    private static TaskCompletionSource NewSignal()
    {
        return new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    private sealed class RecordingRoomChangeNotifier : IRoomChangeNotifier
    {
        public List<(Guid RoomId, int Version)> RoomChanges { get; } = [];

        public List<Guid> ClosedRooms { get; } = [];

        public List<(Guid RoomId, Guid ParticipantId)> RemovedParticipants { get; } = [];

        public List<(Guid RoomId, ChatMessageDto Message)> ChatMessages { get; } = [];

        public List<(Guid RoomId, DraftLogEntryDto Entry)> LogEntries { get; } = [];

        public int? VersionObservedDuringNotification { get; set; }

        public Func<Guid, int, Task>? OnRoomChangedAsync { get; set; }

        public Func<Guid, DraftLogEntryDto, Task>? OnLogEntryAddedAsync { get; set; }

        public Task NotifyRoomChangedAsync(Guid roomId, int version)
        {
            this.RoomChanges.Add((roomId, version));
            return this.OnRoomChangedAsync?.Invoke(roomId, version) ?? Task.CompletedTask;
        }

        public Task NotifyRoomClosedAsync(Guid roomId)
        {
            this.ClosedRooms.Add(roomId);
            return Task.CompletedTask;
        }

        public Task NotifyParticipantRemovedAsync(Guid roomId, Guid participantId)
        {
            this.RemovedParticipants.Add((roomId, participantId));
            return Task.CompletedTask;
        }

        public Task NotifyChatMessageReceivedAsync(Guid roomId, ChatMessageDto message)
        {
            this.ChatMessages.Add((roomId, message));
            return Task.CompletedTask;
        }

        public Task NotifyLogEntryAddedAsync(Guid roomId, DraftLogEntryDto entry)
        {
            this.LogEntries.Add((roomId, entry));
            return this.OnLogEntryAddedAsync?.Invoke(roomId, entry) ?? Task.CompletedTask;
        }
    }
}
