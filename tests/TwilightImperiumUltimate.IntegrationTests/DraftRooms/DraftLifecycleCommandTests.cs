using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Moq;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;
using TwilightImperiumUltimate.Draft.DraftRooms.FactionPick;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftLifecycleCommandTests : IClassFixture<DraftRoomsModelTestsFixture>
{
    private readonly DraftRoomsModelTestsFixture fixture;
    private readonly StartDraftCommandHandler start;
    private readonly ResetDraftCommandHandler reset;
    private readonly UpdateDraftSettingsCommandHandler updateSettings;
    private readonly Mock<IFactionPickPoolProvider> pool = new();
    private readonly Mock<IDraftSeatOccupantChangeHandler> occupantHook = new();
    private readonly ClaimSeatCommandHandler claim;
    private readonly UnclaimSeatCommandHandler unclaim;
    private readonly AssignSeatCommandHandler assign;
    private readonly ApplyDraftActionCommandHandler actions;
    private readonly DraftRoomAuthorizer roomAuthorizer;
    private readonly DraftRoomDraftViewProvider views;

    public DraftLifecycleCommandTests(DraftRoomsModelTestsFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        this.fixture = fixture;
        this.pool.Setup(provider => provider.GetPoolAsync(It.IsAny<IReadOnlyCollection<GameVersion>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enum.GetValues<FactionName>().Where(faction => faction != FactionName.None).Take(30).ToArray());
        var persistence = new DraftRoomDraftPersistence(new FactionPickRepository(), this.pool.Object);
        this.occupantHook.Setup(hook => hook.HandleAsync(
                It.IsAny<DraftRoomScope>(), It.IsAny<DraftRoomSeat>(), It.IsAny<Guid?>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var unitOfWork = new DraftRoomUnitOfWork(
            fixture.Database.ContextFactory,
            new NullRoomChangeNotifier(),
            [this.occupantHook.Object, new FactionPickOccupantChangeHandler(new DraftRoomLog())]);
        var authorizer = new DraftRoomAuthorizer(new GuestSecretService());
        var registry = new DraftTypeRegistry(
        [
            new DraftTypeModule<FactionPickSettingsDto, FactionPickDraftState>(new FactionPickDraftType()),
        ]);
        this.start = new StartDraftCommandHandler(unitOfWork, authorizer, registry, persistence, new CryptoRandomnessSource(), new DraftRoomLog());
        this.reset = new ResetDraftCommandHandler(unitOfWork, authorizer, persistence, new DraftRoomLog());
        this.updateSettings = new UpdateDraftSettingsCommandHandler(unitOfWork, authorizer, registry, persistence, new DraftRoomLog());
        this.claim = new ClaimSeatCommandHandler(unitOfWork, authorizer, new DraftRoomLog());
        this.unclaim = new UnclaimSeatCommandHandler(unitOfWork, authorizer, new DraftRoomLog());
        this.assign = new AssignSeatCommandHandler(unitOfWork, authorizer, new DraftRoomLog());
        this.actions = new ApplyDraftActionCommandHandler(
            unitOfWork,
            authorizer,
            registry,
            persistence,
            new DraftRoomLog(),
            new DraftRoomResultWriter(new DraftResultRepository(fixture.Database.ContextFactory)));
        this.roomAuthorizer = authorizer;
        this.views = new DraftRoomDraftViewProvider(registry, this.pool.Object, authorizer);
    }

    [Fact]
    public async Task Finish_WhenLastPickLocked_ShouldPersistExactlyOnePublicSnapshotInSameRoomTransaction()
    {
        var (room, caller) = await this.CreateRoomAsync();
        (await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None)).IsSuccess.Should().BeTrue();
        for (var turn = 0; turn < 2; turn++)
        {
            var view = await this.GetEngineViewAsync(room, caller);
            var faction = view.Pool.First(candidate => !view.LockedPicks.Any(pick => pick.Faction == candidate));
            (await this.actions.Handle(
                new DecideForCommand(room.Code, view.ActiveSeatId!.Value, new DecidePickAction { Faction = faction, TurnIndex = turn }, caller),
                CancellationToken.None)).IsSuccess.Should().BeTrue();
        }

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var savedRoom = await context.DraftRooms.SingleAsync(item => item.Id == room.Id);
        savedRoom.Status.Should().Be(DraftRoomStatus.Finished);
        var result = await context.DraftResults.Include(item => item.Players).SingleAsync(item => item.SourceRoomId == room.Id);
        savedRoom.ResultId.Should().Be(result.Id);
        result.ShareToken.Should().MatchRegex("^[A-Za-z0-9_-]{22}$");
        result.PayloadVersion.Should().Be(1);
        result.Players.Should().HaveCount(2);
        var payload = JsonSerializer.Deserialize<FactionPickResultDto>(result.PayloadJson)!;
        payload.Seats.SelectMany(seat => seat.Picks).Should().HaveCount(2);
        result.PayloadJson.Should().NotContain("ParticipantId").And.NotContain("UserId");
        (await this.actions.Handle(new UndoLastPickCommand(room.Code, caller), CancellationToken.None)).IsFailed.Should().BeTrue();
        (await this.actions.Handle(new DecideForCommand(room.Code, room.Seats.First().Id, new DecidePickAction(), caller), CancellationToken.None)).IsFailed.Should().BeTrue();
        (await context.DraftResults.CountAsync(item => item.SourceRoomId == room.Id)).Should().Be(1);
    }

    [Fact]
    public async Task Result_WhenFinishingTransactionThrows_ShouldRollbackSnapshotRoomPointerAndPlayerRows()
    {
        var (room, _) = await this.CreateRoomAsync();
        var writer = new DraftRoomResultWriter(new DraftResultRepository(this.fixture.Database.ContextFactory));
        var module = new DraftTypeModule<FactionPickSettingsDto, FactionPickDraftState>(new FactionPickDraftType());
        var seats = room.Seats.OrderBy(seat => seat.Index).ToArray();
        var factions = Enum.GetValues<FactionName>().Where(faction => faction != FactionName.None).Take(2).ToArray();
        var state = new FactionPickDraftState
        {
            IsStarted = true,
            Phase = FactionPickPhase.Finished,
            BansRevealed = true,
            SeatIds = seats.Select(seat => seat.Id).ToArray(),
            TurnOrder = seats.Select(seat => seat.Id).ToArray(),
            TurnIndex = 2,
            ResultSeats = seats.Select(seat => new FactionPickResultSeat(seat.Id, seat.Index, "Public player")).ToArray(),
            Choices = seats.Select((seat, index) => new FactionPickChoice(seat.Id, FactionPickSelectionKind.Pick, factions[index], FactionPickSelectionState.Locked)).ToArray(),
        };
        var unitOfWork = new DraftRoomUnitOfWork(this.fixture.Database.ContextFactory, new NullRoomChangeNotifier());

        var finish = async () => await unitOfWork.ExecuteAsync<FluentResults.Result>(
            room.Code,
            scope =>
            {
                writer.Create(scope, module, new FactionPickSettingsDto(), state);
                scope.Room.Status = DraftRoomStatus.Finished;
                throw new InvalidOperationException("Failure after snapshot creation");
            },
            CancellationToken.None);

        await finish.Should().ThrowAsync<InvalidOperationException>();
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var stored = await context.DraftRooms.SingleAsync(item => item.Id == room.Id);
        stored.ResultId.Should().BeNull();
        stored.Status.Should().Be(DraftRoomStatus.Lobby);
        (await context.DraftResults.AnyAsync(item => item.SourceRoomId == room.Id)).Should().BeFalse();
    }

    [Fact]
    public async Task Picks_WhenTwoSeatsRaceToLock_ShouldAcceptOnlyActiveSeat()
    {
        var (room, caller) = await this.CreateRoomAsync();
        var order = room.Seats.Select(seat => seat.Id).ToArray();
        var settings = new FactionPickSettingsDto { PickOrderMode = FactionPickOrderMode.HostDefined, SeatOrder = order, PicksPerPlayer = 2 };
        await this.updateSettings.Handle(new UpdateDraftSettingsCommand(room.Code, settings, caller), CancellationToken.None);
        await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None);
        var other = new DraftRoomCaller(null, null, room.Participants.Last().Id, "test-secret");
        var faction = (await this.GetEngineViewAsync(room, caller)).Pool[0];
        await this.actions.Handle(new ApplyDraftActionCommand(room.Code, new SelectPickAction { Faction = faction }, caller), CancellationToken.None);

        var results = await Task.WhenAll(
            this.actions.Handle(new ApplyDraftActionCommand(room.Code, new LockInPickAction(), caller), CancellationToken.None),
            this.actions.Handle(new ApplyDraftActionCommand(room.Code, new LockInPickAction(), other), CancellationToken.None));

        results[0].IsSuccess.Should().BeTrue();
        results[1].IsFailed.Should().BeTrue();
        var view = await this.GetEngineViewAsync(room, caller);
        view.TurnIndex.Should().Be(1);
        view.LockedPicks.Should().ContainSingle().Which.SeatId.Should().Be(order[0]);
    }

    [Fact]
    public async Task Picks_WhenSeatReleased_ShouldDiscardPendingWithoutDisclosureAndPreserveLockedPicks()
    {
        var (room, caller) = await this.CreateRoomAsync();
        var order = room.Seats.Select(seat => seat.Id).ToArray();
        var settings = new FactionPickSettingsDto { PickOrderMode = FactionPickOrderMode.HostDefined, SeatOrder = order };
        await this.updateSettings.Handle(new UpdateDraftSettingsCommand(room.Code, settings, caller), CancellationToken.None);
        await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None);
        var faction = (await this.GetEngineViewAsync(room, caller)).Pool[0];
        await this.actions.Handle(new ApplyDraftActionCommand(room.Code, new SelectPickAction { Faction = faction }, caller), CancellationToken.None);

        (await this.unclaim.Handle(new UnclaimSeatCommand(room.Code, order[0], caller), CancellationToken.None)).IsSuccess.Should().BeTrue();

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.FactionPickSelections.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        var discarded = await context.DraftRoomLogEntries.SingleAsync(item => item.RoomId == room.Id && item.Type == DraftLogEntryType.SeatChoicesDiscarded);
        discarded.DataJson.Should().NotContain("Faction");
        (await this.claim.Handle(new ClaimSeatCommand(room.Code, order[0], caller), CancellationToken.None)).IsSuccess.Should().BeTrue();
        await this.actions.Handle(new ApplyDraftActionCommand(room.Code, new SelectPickAction { Faction = faction }, caller), CancellationToken.None);
        await this.actions.Handle(new ApplyDraftActionCommand(room.Code, new LockInPickAction(), caller), CancellationToken.None);
        (await this.unclaim.Handle(new UnclaimSeatCommand(room.Code, order[0], caller), CancellationToken.None)).IsSuccess.Should().BeTrue();
        (await context.FactionPickSelections.SingleAsync(item => item.RoomId == room.Id)).State.Should().Be(FactionPickSelectionState.Locked);
    }

    [Fact]
    public async Task Settings_WhenHostOrderSavedAndReset_ShouldPersistConfigurationAndUseItOnEveryStart()
    {
        var (room, caller) = await this.CreateRoomAsync();
        var order = room.Seats.Select(seat => seat.Id).Reverse().ToArray();
        var settings = new FactionPickSettingsDto { PickOrderMode = FactionPickOrderMode.HostDefined, SeatOrder = order };
        (await this.updateSettings.Handle(new UpdateDraftSettingsCommand(room.Code, settings, caller), CancellationToken.None)).IsSuccess.Should().BeTrue();
        (await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None)).IsSuccess.Should().BeTrue();
        (await this.GetEngineViewAsync(room, caller)).TurnOrder.Should().Equal(order);

        (await this.reset.Handle(new ResetDraftCommand(room.Code, caller), CancellationToken.None)).IsSuccess.Should().BeTrue();

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.FactionPickSettings.SingleAsync(item => item.RoomId == room.Id)).SeatOrder.Should().Equal(order);
        (await context.FactionPickTurnOrder.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None)).IsSuccess.Should().BeTrue();
        (await this.GetEngineViewAsync(room, caller)).TurnOrder.Should().Equal(order);
        (await context.DraftRoomLogEntries.CountAsync(item => item.RoomId == room.Id && item.Type == DraftLogEntryType.DraftOrderConfigured)).Should().Be(2);
    }

    [Fact]
    public async Task Settings_WhenOrderContainsForeignSeat_ShouldRefuseWithoutSaving()
    {
        var (room, caller) = await this.CreateRoomAsync();
        var settings = new FactionPickSettingsDto
        {
            PickOrderMode = FactionPickOrderMode.HostDefined,
            SeatOrder = [room.Seats.First().Id, Guid.NewGuid()],
        };

        var result = await this.updateSettings.Handle(new UpdateDraftSettingsCommand(room.Code, settings, caller), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        await this.AssertUnchangedAsync(room);
    }

    [Fact]
    public async Task Apply_WhenSecretBanSelected_ShouldPersistWithoutPublicContentsAndProjectOnlyToOwner()
    {
        var (room, caller) = await this.CreateRoomAsync(mode: FactionPickBansMode.SimultaneousPlayerBans);
        await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None);
        var faction = Enum.GetValues<FactionName>().First(value => value != FactionName.None);

        var result = await this.actions.Handle(
            new ApplyDraftActionCommand(room.Code, new SelectBanAction { Faction = faction }, caller),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        var own = await this.GetEngineViewAsync(room, caller);
        var other = await this.GetEngineViewAsync(room, new DraftRoomCaller(null, null, room.Participants.Last().Id, "test-secret"));
        own.MyBans.Should().Equal(faction);
        other.MyBans.Should().BeEmpty();
        other.PublicBans.Should().BeEmpty();
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.DraftRooms.SingleAsync(item => item.Id == room.Id)).Version.Should().Be(2);
        (await context.FactionPickSelections.CountAsync(item => item.RoomId == room.Id)).Should().Be(1);
        (await context.DraftRoomLogEntries.CountAsync(item => item.RoomId == room.Id)).Should().Be(2);
    }

    [Fact]
    public async Task Lock_WhenSeatsRace_ShouldRevealExactlyOnceAndPublishOnlyAfterBothLocks()
    {
        var (room, caller) = await this.CreateRoomAsync(mode: FactionPickBansMode.SimultaneousPlayerBans);
        await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None);
        var other = new DraftRoomCaller(null, null, room.Participants.Last().Id, "test-secret");
        var faction = Enum.GetValues<FactionName>().First(value => value != FactionName.None);
        await this.actions.Handle(new ApplyDraftActionCommand(room.Code, new SelectBanAction { Faction = faction }, caller), CancellationToken.None);
        await this.actions.Handle(new ApplyDraftActionCommand(room.Code, new SelectBanAction { Faction = faction }, other), CancellationToken.None);

        var results = await Task.WhenAll(
            this.actions.Handle(new ApplyDraftActionCommand(room.Code, new LockInBansAction(), caller), CancellationToken.None),
            this.actions.Handle(new ApplyDraftActionCommand(room.Code, new LockInBansAction(), other), CancellationToken.None));

        results.Should().OnlyContain(result => result.IsSuccess);
        var view = await this.GetEngineViewAsync(room, other);
        view.Phase.Should().Be(FactionPickPhase.Picks);
        view.PublicBans.Should().Equal(faction);
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.DraftRoomLogEntries.CountAsync(item => item.RoomId == room.Id && item.Type == DraftLogEntryType.BansRevealed)).Should().Be(1);
        (await context.DraftRooms.SingleAsync(item => item.Id == room.Id)).Version.Should().Be(5);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Unclaim_WhenBanIsUnrevealed_ShouldDiscardPendingOrLockedChoiceWithoutDisclosure(bool locked)
    {
        var (room, caller) = await this.CreateRoomAsync(mode: FactionPickBansMode.SimultaneousPlayerBans);
        await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None);
        var faction = Enum.GetValues<FactionName>().First(value => value != FactionName.None);
        await this.actions.Handle(new ApplyDraftActionCommand(room.Code, new SelectBanAction { Faction = faction }, caller), CancellationToken.None);
        if (locked)
        {
            await this.actions.Handle(new ApplyDraftActionCommand(room.Code, new LockInBansAction(), caller), CancellationToken.None);
        }

        var result = await this.unclaim.Handle(new UnclaimSeatCommand(room.Code, room.Seats.First().Id, caller), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.FactionPickSelections.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await context.FactionPickStates.SingleAsync(item => item.RoomId == room.Id)).LockedBanSeatIds.Should().BeEmpty();
        var log = await context.DraftRoomLogEntries.SingleAsync(item => item.RoomId == room.Id && item.Type == DraftLogEntryType.SeatChoicesDiscarded);
        using var document = JsonDocument.Parse(log.DataJson);
        document.RootElement.EnumerateObject().Select(property => property.Name).Should().BeEquivalentTo("SeatId", "Index");
    }

    [Fact]
    public async Task Unclaim_WhenBansWereRevealed_ShouldRetainTheirPublicHistory()
    {
        var (room, caller) = await this.CreateRoomAsync(mode: FactionPickBansMode.SimultaneousPlayerBans);
        await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None);
        var other = new DraftRoomCaller(null, null, room.Participants.Last().Id, "test-secret");
        var faction = Enum.GetValues<FactionName>().First(value => value != FactionName.None);
        await this.actions.Handle(new ApplyDraftActionCommand(room.Code, new SelectBanAction { Faction = faction }, caller), CancellationToken.None);
        await this.actions.Handle(new ApplyDraftActionCommand(room.Code, new LockInBansAction(), caller), CancellationToken.None);
        await this.actions.Handle(new ApplyDraftActionCommand(room.Code, new LockInBansAction(), other), CancellationToken.None);

        await this.unclaim.Handle(new UnclaimSeatCommand(room.Code, room.Seats.First().Id, caller), CancellationToken.None);

        var view = await this.GetEngineViewAsync(room, other);
        view.PublicBans.Should().Equal(faction);
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.FactionPickSelections.SingleAsync(item => item.RoomId == room.Id)).State.Should().Be(FactionPickSelectionState.Locked);
    }

    [Fact]
    public async Task DecideFor_WhenManagerActs_ShouldDiscloseBeforeLockAndRefuseMembersWithoutEffects()
    {
        var (room, caller) = await this.CreateRoomAsync(mode: FactionPickBansMode.SimultaneousPlayerBans);
        await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None);
        var other = new DraftRoomCaller(null, null, room.Participants.Last().Id, "test-secret");
        var faction = Enum.GetValues<FactionName>().First(value => value != FactionName.None);
        await this.actions.Handle(new ApplyDraftActionCommand(room.Code, new SelectBanAction { Faction = faction }, other), CancellationToken.None);
        var targetSeat = room.Seats.Last().Id;

        var refused = await this.actions.Handle(new DecideForCommand(room.Code, targetSeat, new DecideBansAction(), other), CancellationToken.None);
        var accepted = await this.actions.Handle(new DecideForCommand(room.Code, targetSeat, new DecideBansAction(), caller), CancellationToken.None);

        refused.IsFailed.Should().BeTrue();
        accepted.IsSuccess.Should().BeTrue();
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var logs = await context.DraftRoomLogEntries.Where(item => item.RoomId == room.Id).OrderBy(item => item.Id).ToArrayAsync();
        logs.TakeLast(2).Select(item => item.Type).Should().Equal(DraftLogEntryType.HostDecidedForSeat, DraftLogEntryType.SeatBansLocked);
        using var document = JsonDocument.Parse(logs[^2].DataJson);
        document.RootElement.GetProperty("Factions")[0].GetInt32().Should().Be((int)faction);
        (await context.FactionPickSelections.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await context.DraftRooms.SingleAsync(item => item.Id == room.Id)).Version.Should().Be(3);
    }

    [Theory]
    [InlineData(FactionPickBansMode.None, FactionPickPhase.Picks)]
    [InlineData(FactionPickBansMode.HostBans, FactionPickPhase.Bans)]
    [InlineData(FactionPickBansMode.SimultaneousPlayerBans, FactionPickPhase.Bans)]
    public async Task Start_WhenLobbyIsReady_ShouldPersistPhaseOrderLogAndSingleVersion(
        FactionPickBansMode mode,
        FactionPickPhase expectedPhase)
    {
        var (room, caller) = await this.CreateRoomAsync(mode: mode);

        var result = await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var savedRoom = await context.DraftRooms.SingleAsync(item => item.Id == room.Id);
        savedRoom.Status.Should().Be(DraftRoomStatus.InProgress);
        savedRoom.Version.Should().Be(1);
        var state = await context.FactionPickStates.SingleAsync(item => item.RoomId == room.Id);
        state.Phase.Should().Be(expectedPhase);
        state.TurnIndex.Should().Be(0);
        var order = await context.FactionPickTurnOrder.Where(item => item.RoomId == room.Id).OrderBy(item => item.Position).ToArrayAsync();
        order.Select(entry => entry.SeatId).Should().BeEquivalentTo(room.Seats.Select(seat => seat.Id));
        var logs = await context.DraftRoomLogEntries.Where(item => item.RoomId == room.Id).OrderBy(item => item.Id).ToArrayAsync();
        logs.Select(entry => entry.Type).Should().Equal(DraftLogEntryType.DraftRandomized, DraftLogEntryType.DraftStarted);
        using var data = JsonDocument.Parse(logs[0].DataJson);
        var loggedOrder = data.RootElement.GetProperty("Data").GetProperty("SeatIds").EnumerateArray().Select(id => id.GetGuid());
        loggedOrder.Should().Equal(order.Select(entry => entry.SeatId));
    }

    [Theory]
    [InlineData(DraftRoomRole.Member, DraftRoomStatus.Lobby, DraftRoomErrorCode.Forbidden)]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.InProgress, DraftRoomErrorCode.RoomChanged)]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.Finished, DraftRoomErrorCode.RoomChanged)]
    public async Task Start_WhenRoleOrStatusForbidsIt_ShouldRefuseWithoutSideEffects(
        DraftRoomRole role,
        DraftRoomStatus status,
        DraftRoomErrorCode expected)
    {
        var (room, caller) = await this.CreateRoomAsync(role: role, status: status);

        var result = await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None);

        result.Errors.OfType<DraftRoomError>().Should().Contain(error => error.Code == expected);
        await this.AssertUnchangedAsync(room);
    }

    [Theory]
    [InlineData(false, 2, DraftRoomErrorCode.SeatsNotFull)]
    [InlineData(true, 1, DraftRoomErrorCode.NotEnoughSeats)]
    public async Task Start_WhenSeatsAreNotReady_ShouldReturnSpecificBlocker(
        bool occupied,
        int seatCount,
        DraftRoomErrorCode expected)
    {
        var (room, caller) = await this.CreateRoomAsync(occupied: occupied, seatCount: seatCount);

        var result = await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None);

        result.Errors.OfType<DraftRoomError>().Should().Contain(error => error.Code == expected);
        await this.AssertUnchangedAsync(room);
    }

    [Fact]
    public async Task Start_WhenPoolIsTooSmall_ShouldReturnSettingsValidationCodes()
    {
        var (room, caller) = await this.CreateRoomAsync();
        this.pool.Setup(provider => provider.GetPoolAsync(It.IsAny<IReadOnlyCollection<GameVersion>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([FactionName.TheArborec]);

        var result = await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None);

        var error = result.Errors.OfType<DraftRoomError>().Single();
        error.Code.Should().Be(DraftRoomErrorCode.SettingsInvalid);
        error.ValidationCodes.Should().Contain("InsufficientFactionPool");
        await this.AssertUnchangedAsync(room);
    }

    [Fact]
    public async Task Start_WhenCallerHasWrongSecret_ShouldRefuseBeforeLoadingPool()
    {
        var (room, caller) = await this.CreateRoomAsync();

        var result = await this.start.Handle(
            new StartDraftCommand(room.Code, caller with { GuestSecret = "wrong-secret" }), CancellationToken.None);

        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.NotFoundOrWrongPassword);
        this.pool.Verify(provider => provider.GetPoolAsync(It.IsAny<IReadOnlyCollection<GameVersion>>(), It.IsAny<CancellationToken>()), Times.Never);
        await this.AssertUnchangedAsync(room);
    }

    [Fact]
    public async Task Reset_WhenRunning_ShouldPreserveMembershipSettingsAndChatAndAllowRestart()
    {
        var (room, caller) = await this.CreateRoomAsync(role: DraftRoomRole.CoHost);
        (await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None)).IsSuccess.Should().BeTrue();
        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRoomChatMessages.Add(new DraftRoomChatMessage
            {
                RoomId = room.Id,
                Text = "Preserve this message",
                CreatedAtUtc = DateTime.UtcNow,
            });
            await context.SaveChangesAsync();
        }

        var result = await this.reset.Handle(new ResetDraftCommand(room.Code, caller), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            var saved = await context.DraftRooms.SingleAsync(item => item.Id == room.Id);
            saved.Status.Should().Be(DraftRoomStatus.Lobby);
            saved.Version.Should().Be(2);
            (await context.FactionPickStates.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
            (await context.FactionPickTurnOrder.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
            (await context.FactionPickSettings.SingleAsync(item => item.RoomId == room.Id)).Snake.Should().BeTrue();
            (await context.DraftRoomParticipants.CountAsync(item => item.RoomId == room.Id)).Should().Be(2);
            (await context.DraftRoomSeats.CountAsync(item => item.RoomId == room.Id && item.ParticipantId != null)).Should().Be(2);
            (await context.DraftRoomChatMessages.CountAsync(item => item.RoomId == room.Id)).Should().Be(1);
            (await context.DraftRoomLogEntries.CountAsync(item => item.RoomId == room.Id && item.Type == DraftLogEntryType.DraftReset))
                .Should().Be(1);
        }

        (await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None)).IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(DraftRoomRole.Member, DraftRoomStatus.InProgress, DraftRoomErrorCode.Forbidden)]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.Lobby, DraftRoomErrorCode.RoomChanged)]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.Finished, DraftRoomErrorCode.RoomChanged)]
    public async Task Reset_WhenRoleOrStatusForbidsIt_ShouldRefuseWithoutSideEffects(
        DraftRoomRole role,
        DraftRoomStatus status,
        DraftRoomErrorCode expected)
    {
        var (room, caller) = await this.CreateRoomAsync(role: role, status: status);

        var result = await this.reset.Handle(new ResetDraftCommand(room.Code, caller), CancellationToken.None);

        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(expected);
        await this.AssertUnchangedAsync(room);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Handle_WhenRoomDoesNotExist_ShouldReturnNotFound(bool resetting)
    {
        var caller = new DraftRoomCaller(null, null, Guid.NewGuid(), "test-secret");

        var result = resetting
            ? await this.reset.Handle(new ResetDraftCommand("MISSING1", caller), CancellationToken.None)
            : await this.start.Handle(new StartDraftCommand("MISSING1", caller), CancellationToken.None);

        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.NotFoundOrWrongPassword);
    }

    [Theory]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.Lobby, true)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomStatus.Lobby, true)]
    [InlineData(DraftRoomRole.Member, DraftRoomStatus.Lobby, false)]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.InProgress, false)]
    public async Task UpdateSettings_WhenActorOrStatusChanges_ShouldEnforceServerPermissions(
        DraftRoomRole role,
        DraftRoomStatus status,
        bool success)
    {
        var (room, caller) = await this.CreateRoomAsync(role: role, status: status);
        var settings = new FactionPickSettingsDto { GameVersions = [GameVersion.BaseGame], PicksPerPlayer = 2, Snake = false };

        var result = await this.updateSettings.Handle(new UpdateDraftSettingsCommand(room.Code, settings, caller), CancellationToken.None);

        result.IsSuccess.Should().Be(success);
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var saved = await context.FactionPickSettings.SingleAsync(item => item.RoomId == room.Id);
        saved.PicksPerPlayer.Should().Be(success ? 2 : 1);
        saved.Snake.Should().Be(!success);
    }

    [Fact]
    public async Task UpdateSettings_WhenNewExpansionPoolIsTooSmall_ShouldRejectWithoutOverwritingOldRules()
    {
        var (room, caller) = await this.CreateRoomAsync();
        this.pool.Setup(provider => provider.GetPoolAsync(
                It.Is<IReadOnlyCollection<GameVersion>>(versions => versions.Count == 1 && versions.Contains(GameVersion.BaseGame)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([FactionName.TheArborec]);

        var result = await this.updateSettings.Handle(
            new UpdateDraftSettingsCommand(room.Code, new FactionPickSettingsDto { GameVersions = [GameVersion.BaseGame] }, caller),
            CancellationToken.None);

        result.Errors.OfType<DraftRoomError>().Single().ValidationCodes.Should().Contain("InsufficientFactionPool");
        await this.AssertUnchangedAsync(room);
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.FactionPickSettings.SingleAsync(item => item.RoomId == room.Id)).GameVersions.Should().HaveCount(3);
    }

    [Fact]
    public async Task Claim_WhenFormerOccupantReclaimsOwnReleasedSeat_ShouldSucceedAndInvokeHookForEachChange()
    {
        var (room, caller) = await this.CreateRoomAsync();
        await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None);
        var seat = room.Seats.First();

        (await this.unclaim.Handle(new UnclaimSeatCommand(room.Code, seat.Id, caller), CancellationToken.None)).IsSuccess.Should().BeTrue();
        (await this.claim.Handle(new ClaimSeatCommand(room.Code, seat.Id, caller), CancellationToken.None)).IsSuccess.Should().BeTrue();

        this.occupantHook.Verify(
            hook => hook.HandleAsync(
                It.IsAny<DraftRoomScope>(),
                It.Is<DraftRoomSeat>(item => item.Id == seat.Id),
                It.IsAny<Guid?>(),
                It.IsAny<CancellationToken>()),
            Times.Exactly(2));
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.DraftRoomSeats.SingleAsync(item => item.Id == seat.Id)).ParticipantId.Should().Be(caller.GuestParticipantId);
    }

    [Fact]
    public async Task Claim_WhenAnotherParticipantReplacesThenReleases_ShouldPermanentlyRevokePreviousOccupantsReclaim()
    {
        var (room, caller) = await this.CreateRoomAsync();
        var replacement = await this.AddSpectatorAsync(room);
        await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None);
        var seat = room.Seats.First();
        await this.unclaim.Handle(new UnclaimSeatCommand(room.Code, seat.Id, caller), CancellationToken.None);

        (await this.claim.Handle(new ClaimSeatCommand(room.Code, seat.Id, replacement), CancellationToken.None)).IsSuccess.Should().BeTrue();
        (await this.unclaim.Handle(new UnclaimSeatCommand(room.Code, seat.Id, replacement), CancellationToken.None)).IsSuccess.Should().BeTrue();
        var result = await this.claim.Handle(new ClaimSeatCommand(room.Code, seat.Id, caller), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var original = await context.DraftRoomParticipants.SingleAsync(item => item.Id == caller.GuestParticipantId);
        original.DraftSeatId.Should().Be(seat.Id);
        original.CanReclaimDraftSeat.Should().BeFalse();
    }

    [Fact]
    public async Task Claim_WhenFormerOccupantTriesDifferentReleasedSeat_ShouldRefuse()
    {
        var (room, caller) = await this.CreateRoomAsync();
        await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None);
        var own = room.Seats.First();
        var other = room.Seats.Last();
        await this.unclaim.Handle(new UnclaimSeatCommand(room.Code, own.Id, caller), CancellationToken.None);
        await this.unclaim.Handle(new UnclaimSeatCommand(room.Code, other.Id, caller), CancellationToken.None);

        var result = await this.claim.Handle(new ClaimSeatCommand(room.Code, other.Id, caller), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        (await this.claim.Handle(new ClaimSeatCommand(room.Code, own.Id, caller), CancellationToken.None)).IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Assign_WhenManagerAssignsNewParticipantToReleasedSeat_ShouldSucceedButMemberCannotAssign()
    {
        var (room, caller) = await this.CreateRoomAsync();
        var replacement = await this.AddSpectatorAsync(room);
        await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None);
        var seat = room.Seats.Last();
        await this.unclaim.Handle(new UnclaimSeatCommand(room.Code, seat.Id, caller), CancellationToken.None);

        (await this.assign.Handle(
            new AssignSeatCommand(room.Code, seat.Id, replacement.GuestParticipantId!.Value, replacement),
            CancellationToken.None)).IsFailed.Should().BeTrue();
        (await this.assign.Handle(
            new AssignSeatCommand(room.Code, seat.Id, replacement.GuestParticipantId.Value, caller),
            CancellationToken.None)).IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Claim_WhenTwoReplacementsRace_ShouldAcceptExactlyOne()
    {
        var (room, caller) = await this.CreateRoomAsync();
        var first = await this.AddSpectatorAsync(room);
        var second = await this.AddSpectatorAsync(room);
        await this.start.Handle(new StartDraftCommand(room.Code, caller), CancellationToken.None);
        var seat = room.Seats.Last();
        await this.unclaim.Handle(new UnclaimSeatCommand(room.Code, seat.Id, caller), CancellationToken.None);

        var results = await Task.WhenAll(
            this.claim.Handle(new ClaimSeatCommand(room.Code, seat.Id, first), CancellationToken.None),
            this.claim.Handle(new ClaimSeatCommand(room.Code, seat.Id, second), CancellationToken.None));

        results.Count(result => result.IsSuccess).Should().Be(1);
        results.Count(result => result.IsFailed).Should().Be(1);
    }

    private async Task<FactionPickViewDto> GetEngineViewAsync(DraftRoom room, DraftRoomCaller caller)
    {
        var stored = await new DraftRoomRepository(this.fixture.Database.ContextFactory).GetRoomForViewByCodeAsync(room.Code, CancellationToken.None);
        var actor = this.roomAuthorizer.Resolve(stored!, caller);
        return (FactionPickViewDto)(await this.views.GetAsync(actor!, CancellationToken.None)).Draft!;
    }

    private async Task<DraftRoomCaller> AddSpectatorAsync(DraftRoom room)
    {
        var participant = new DraftRoomParticipant
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            DisplayName = Guid.NewGuid().ToString("N"),
            DisplayNameKey = Guid.NewGuid().ToString("N"),
            SecretHash = new GuestSecretService().Hash("spectator-secret"),
            JoinedAtUtc = room.CreatedAtUtc,
            LastSeenUtc = room.CreatedAtUtc,
        };
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.DraftRoomParticipants.Add(participant);
        await context.SaveChangesAsync();
        return new DraftRoomCaller(null, null, participant.Id, "spectator-secret");
    }

    private async Task<(DraftRoom Room, DraftRoomCaller Caller)> CreateRoomAsync(
        DraftRoomRole role = DraftRoomRole.Host,
        DraftRoomStatus status = DraftRoomStatus.Lobby,
        FactionPickBansMode mode = FactionPickBansMode.None,
        bool occupied = true,
        int seatCount = 2)
    {
        const string secret = "test-secret";
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant(),
            Name = "Lifecycle test",
            PasswordHash = "test-hash",
            Status = status,
            CreatedAtUtc = DateTime.UtcNow,
            LastActivityUtc = DateTime.UtcNow,
        };
        for (var index = 1; index <= seatCount; index++)
        {
            var participant = new DraftRoomParticipant
            {
                Id = Guid.NewGuid(),
                RoomId = room.Id,
                Role = index == 1 ? role : DraftRoomRole.Member,
                DisplayName = $"Player {index}",
                DisplayNameKey = $"PLAYER {index}",
                Status = DraftParticipantStatus.Active,
                SecretHash = new GuestSecretService().Hash(secret),
                JoinedAtUtc = room.CreatedAtUtc,
                LastSeenUtc = room.CreatedAtUtc,
            };
            room.Participants.Add(participant);
            room.Seats.Add(new DraftRoomSeat
            {
                Id = Guid.NewGuid(),
                RoomId = room.Id,
                Index = index,
                ParticipantId = occupied || index == 1 ? participant.Id : null,
            });
        }

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.DraftRooms.Add(room);
        context.FactionPickSettings.Add(new FactionPickSettings { RoomId = room.Id, BansMode = mode });
        await context.SaveChangesAsync();
        return (room, new DraftRoomCaller(null, null, room.Participants.First().Id, secret));
    }

    private async Task AssertUnchangedAsync(DraftRoom room)
    {
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var saved = await context.DraftRooms.SingleAsync(item => item.Id == room.Id);
        saved.Status.Should().Be(room.Status);
        saved.Version.Should().Be(0);
        (await context.FactionPickStates.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await context.FactionPickTurnOrder.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await context.DraftRoomLogEntries.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
    }
}
