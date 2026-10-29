using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class FactionPickRepositoryTests : IClassFixture<DraftRoomsModelTestsFixture>
{
    private readonly DraftRoomsModelTestsFixture fixture;
    private readonly DraftRoomUnitOfWork unitOfWork;
    private readonly FactionPickRepository repository = new();

    public FactionPickRepositoryTests(DraftRoomsModelTestsFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        this.fixture = fixture;
        this.unitOfWork = new DraftRoomUnitOfWork(fixture.Database.ContextFactory, new NullRoomChangeNotifier());
    }

    [Fact]
    public async Task SaveSelectionsAsync_WhenChoicesChange_ShouldPersistLocksAndRemoveDiscardedChoices()
    {
        var room = await this.CreateRoomAsync();
        var seat = room.Seats.First();
        var faction = Enum.GetValues<FactionName>()[0];
        await this.unitOfWork.ExecuteAsync(
            room.Code,
            async scope =>
            {
                await this.repository.SaveSelectionsAsync(
                    scope,
                    [new FactionPickSelection { RoomId = room.Id, SeatId = seat.Id, Faction = faction }],
                    CancellationToken.None);
                return true;
            },
            CancellationToken.None);
        await this.unitOfWork.ExecuteAsync(
            room.Code,
            async scope =>
            {
                await this.repository.SaveSelectionsAsync(
                    scope,
                    [new FactionPickSelection { RoomId = room.Id, SeatId = seat.Id, Faction = faction, State = FactionPickSelectionState.Locked }],
                    CancellationToken.None);
                return true;
            },
            CancellationToken.None);
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var choice = await context.FactionPickSelections.AsNoTracking().SingleAsync(item => item.RoomId == room.Id);
        choice.LockedAtUtc.Should().NotBeNull();
        choice.CreatedAtUtc.Should().BeOnOrBefore(choice.LockedAtUtc!.Value);

        await this.unitOfWork.ExecuteAsync(
            room.Code,
            async scope =>
            {
                await this.repository.DeleteStateAsync(scope, CancellationToken.None);
                return true;
            },
            CancellationToken.None);

        (await context.FactionPickSelections.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task SaveChangesAsync_WhenChoiceIsDuplicated_ShouldRejectSeatAndHostDuplicates(bool hostBan)
    {
        var room = await this.CreateRoomAsync();
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        for (var index = 0; index < 2; index++)
        {
            context.FactionPickSelections.Add(new FactionPickSelection
            {
                Id = Guid.NewGuid(),
                RoomId = room.Id,
                SeatId = hostBan ? null : room.Seats.First().Id,
                Faction = Enum.GetValues<FactionName>()[0],
                CreatedAtUtc = DateTime.UtcNow,
            });
        }

        var save = () => context.SaveChangesAsync();

        await save.Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task DeleteRoomAsync_WhenChoicesExist_ShouldCascadeAndKeepOtherRooms()
    {
        var room = await this.CreateRoomAsync();
        var otherRoom = await this.CreateRoomAsync();
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.FactionPickSelections.Add(new FactionPickSelection
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            SeatId = room.Seats.First().Id,
            CreatedAtUtc = DateTime.UtcNow,
        });
        await context.SaveChangesAsync();

        await context.DraftRooms.Where(item => item.Id == room.Id).ExecuteDeleteAsync();

        (await context.FactionPickSelections.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await context.DraftRooms.AnyAsync(item => item.Id == otherRoom.Id)).Should().BeTrue();
    }

    [Fact]
    public async Task SaveSettingsAsync_WhenDefaultsAreSaved_ShouldMatchContractAndPersistVersionCollection()
    {
        var room = await this.CreateRoomAsync();

        await this.unitOfWork.ExecuteAsync(
            room.Code,
            async scope =>
            {
                await this.repository.SaveSettingsAsync(scope, new FactionPickSettings { RoomId = room.Id }, CancellationToken.None);
                return true;
            },
            CancellationToken.None);

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var settings = await context.FactionPickSettings.SingleAsync(item => item.RoomId == room.Id);
        var defaults = new FactionPickSettingsDto();
        settings.GameVersions.Should().Equal(defaults.GameVersions);
        settings.BansMode.Should().Be(defaults.BansMode);
        settings.BansPerPlayer.Should().Be(defaults.BansPerPlayer);
        settings.HostBanCount.Should().Be(defaults.HostBanCount);
        settings.PickOrderMode.Should().Be(defaults.PickOrderMode);
        settings.PicksPerPlayer.Should().Be(defaults.PicksPerPlayer);
        settings.Snake.Should().Be(defaults.Snake);
    }

    [Fact]
    public async Task SaveSettingsAsync_WhenExistingSettingsChange_ShouldUpdateInsteadOfInsert()
    {
        var room = await this.CreateRoomAsync();
        await this.AddDraftAsync(room);

        await this.unitOfWork.ExecuteAsync(
            room.Code,
            async scope =>
            {
                await this.repository.SaveSettingsAsync(
                    scope,
                    new FactionPickSettings
                    {
                        RoomId = room.Id,
                        GameVersions = [GameVersion.BaseGame],
                        BansMode = FactionPickBansMode.HostBans,
                        HostBanCount = 2,
                        PicksPerPlayer = 3,
                        Snake = false,
                    },
                    CancellationToken.None);
                await this.repository.SaveStateAsync(
                    scope,
                    new FactionPickState
                    {
                        RoomId = room.Id,
                        Phase = FactionPickPhase.Picks,
                        TurnIndex = 1,
                        BansRevealed = true,
                    },
                    CancellationToken.None);
                return true;
            },
            CancellationToken.None);

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var settings = await context.FactionPickSettings.SingleAsync(item => item.RoomId == room.Id);
        settings.GameVersions.Should().Equal(GameVersion.BaseGame);
        settings.BansMode.Should().Be(FactionPickBansMode.HostBans);
        settings.HostBanCount.Should().Be(2);
        settings.PicksPerPlayer.Should().Be(3);
        settings.Snake.Should().BeFalse();
        var state = await context.FactionPickStates.SingleAsync(item => item.RoomId == room.Id);
        state.Phase.Should().Be(FactionPickPhase.Picks);
        state.TurnIndex.Should().Be(1);
        state.BansRevealed.Should().BeTrue();
    }

    [Fact]
    public async Task SaveSettingsAsync_WhenWorkThrows_ShouldRollBackWithoutPartialSave()
    {
        var room = await this.CreateRoomAsync();
        var operation = () => this.unitOfWork.ExecuteAsync<bool>(
            room.Code,
            async scope =>
            {
                await this.repository.SaveSettingsAsync(scope, new FactionPickSettings { RoomId = room.Id }, CancellationToken.None);
                await scope.Context.SaveChangesAsync();
                throw new InvalidOperationException("Abort test operation.");
            },
            CancellationToken.None);

        await operation.Should().ThrowAsync<InvalidOperationException>();

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.FactionPickSettings.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await context.DraftRooms.SingleAsync(item => item.Id == room.Id)).Version.Should().Be(0);
    }

    [Fact]
    public async Task DeleteStateAsync_WhenResetting_ShouldKeepSettingsParticipantsSeatsAndChat()
    {
        var room = await this.CreateRoomAsync();
        await this.AddDraftAsync(room);
        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRoomParticipants.Add(new DraftRoomParticipant
            {
                Id = Guid.NewGuid(),
                RoomId = room.Id,
                DisplayName = "Spectator",
                DisplayNameKey = "SPECTATOR",
                JoinedAtUtc = room.CreatedAtUtc,
                LastSeenUtc = room.CreatedAtUtc,
            });
            context.DraftRoomChatMessages.Add(new DraftRoomChatMessage
            {
                RoomId = room.Id,
                Text = "Keep this chat",
                CreatedAtUtc = room.CreatedAtUtc,
            });
            await context.SaveChangesAsync();
        }

        await this.unitOfWork.ExecuteAsync(
            room.Code,
            async scope =>
            {
                var order = await this.repository.LoadTurnOrderAsync(scope, CancellationToken.None);
                order.Select(entry => entry.SeatId).Should().Equal(room.Seats.OrderBy(seat => seat.Index).Select(seat => seat.Id));
                await this.repository.DeleteStateAsync(scope, CancellationToken.None);
                return true;
            },
            CancellationToken.None);

        await using var verification = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await verification.FactionPickStates.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await verification.FactionPickTurnOrder.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await verification.FactionPickSettings.AnyAsync(item => item.RoomId == room.Id)).Should().BeTrue();
        (await verification.DraftRoomParticipants.CountAsync(item => item.RoomId == room.Id)).Should().Be(1);
        (await verification.DraftRoomSeats.CountAsync(item => item.RoomId == room.Id)).Should().Be(2);
        (await verification.DraftRoomChatMessages.CountAsync(item => item.RoomId == room.Id)).Should().Be(1);
    }

    [Fact]
    public async Task DeleteAllAsync_WhenChangingType_ShouldRemoveSettingsAndRuntimeRows()
    {
        var room = await this.CreateRoomAsync();
        await this.AddDraftAsync(room);

        await this.unitOfWork.ExecuteAsync(
            room.Code,
            async scope =>
            {
                await this.repository.DeleteAllAsync(scope, CancellationToken.None);
                return true;
            },
            CancellationToken.None);

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.FactionPickSettings.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await context.FactionPickStates.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await context.FactionPickTurnOrder.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await context.DraftRoomSeats.CountAsync(item => item.RoomId == room.Id)).Should().Be(2);
    }

    [Fact]
    public async Task DeleteRoom_WhenDraftRowsAreNotLoaded_ShouldCascadeAllDraftTables()
    {
        var room = await this.CreateRoomAsync();
        await this.AddDraftAsync(room);

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            var persisted = await context.DraftRooms.SingleAsync(item => item.Id == room.Id);
            context.DraftRooms.Remove(persisted);
            await context.SaveChangesAsync();
        }

        await using var verification = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await verification.FactionPickSettings.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await verification.FactionPickStates.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await verification.FactionPickTurnOrder.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
    }

    [Fact]
    public async Task SaveChanges_WhenTurnReferencesSeatInAnotherRoom_ShouldRejectCrossRoomReference()
    {
        var room = await this.CreateRoomAsync();
        var otherRoom = await this.CreateRoomAsync();
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.FactionPickTurnOrder.Add(new FactionPickTurnEntry
        {
            RoomId = room.Id,
            SeatId = otherRoom.Seats.First().Id,
        });

        var save = () => context.SaveChangesAsync();

        await save.Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task SaveChanges_WhenSeatHasTwoTurnPositions_ShouldRejectDuplicateSeat()
    {
        var room = await this.CreateRoomAsync();
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var seatId = room.Seats.First().Id;
        context.FactionPickTurnOrder.AddRange(
            new FactionPickTurnEntry { RoomId = room.Id, Position = 0, SeatId = seatId },
            new FactionPickTurnEntry { RoomId = room.Id, Position = 1, SeatId = seatId });

        var save = () => context.SaveChangesAsync();

        await save.Should().ThrowAsync<DbUpdateException>();
    }

    private async Task<DraftRoom> CreateRoomAsync()
    {
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant(),
            Name = "Faction Pick persistence test",
            PasswordHash = "test-hash",
            CreatedAtUtc = DateTime.UtcNow,
            LastActivityUtc = DateTime.UtcNow,
        };
        room.Seats.Add(new DraftRoomSeat { Id = Guid.NewGuid(), RoomId = room.Id, Index = 1 });
        room.Seats.Add(new DraftRoomSeat { Id = Guid.NewGuid(), RoomId = room.Id, Index = 2 });
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.DraftRooms.Add(room);
        await context.SaveChangesAsync();
        return room;
    }

    private Task<bool> AddDraftAsync(DraftRoom room)
    {
        return this.unitOfWork.ExecuteAsync(
            room.Code,
            async scope =>
            {
                await this.repository.SaveSettingsAsync(scope, new FactionPickSettings { RoomId = room.Id }, CancellationToken.None);
                await this.repository.SaveStateAsync(
                    scope,
                    new FactionPickState
                    {
                        RoomId = room.Id,
                        Phase = FactionPickPhase.Bans,
                    },
                    CancellationToken.None);
                this.repository.AddTurnOrder(scope, scope.Room.Seats.OrderBy(seat => seat.Index).Select(seat => seat.Id).ToArray());
                return true;
            },
            CancellationToken.None);
    }
}
