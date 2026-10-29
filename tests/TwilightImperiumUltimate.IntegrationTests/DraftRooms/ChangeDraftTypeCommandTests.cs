using Microsoft.EntityFrameworkCore;
using Moq;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
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
public sealed class ChangeDraftTypeCommandTests : IClassFixture<DraftRoomsModelTestsFixture>
{
    private readonly DraftRoomsModelTestsFixture fixture;
    private readonly ChangeDraftTypeCommandHandler handler;
    private readonly Mock<IDraftRoomDraftPersistence> persistence = new();

    public ChangeDraftTypeCommandTests(DraftRoomsModelTestsFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        this.fixture = fixture;
        this.handler = new ChangeDraftTypeCommandHandler(
            new DraftRoomUnitOfWork(fixture.Database.ContextFactory, new NullRoomChangeNotifier()),
            new DraftRoomAuthorizer(new GuestSecretService()),
            new DraftTypeRegistry(
            [
                new DraftTypeModule<TwilightImperiumUltimate.Contracts.DTOs.DraftRooms.FactionPickSettingsDto, FactionPickDraftState>(
                    new FactionPickDraftType()),
            ]),
            this.persistence.Object,
            new DraftRoomLog());
    }

    [Theory]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.Lobby, true)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomStatus.Lobby, true)]
    [InlineData(DraftRoomRole.Member, DraftRoomStatus.Lobby, false)]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.InProgress, false)]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.Finished, false)]
    public async Task Handle_WhenSelectingCurrentType_ShouldRespectRoleAndLifecycleWithoutResetting(
        DraftRoomRole role,
        DraftRoomStatus status,
        bool success)
    {
        var (room, caller) = await this.CreateRoomAsync(role, status);

        var result = await this.handler.Handle(
            new ChangeDraftTypeCommand(room.Code, DraftTypeId.FactionPick, caller), CancellationToken.None);

        result.IsSuccess.Should().Be(success);
        this.persistence.VerifyNoOtherCalls();
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.DraftRooms.SingleAsync(item => item.Id == room.Id)).Version.Should().Be(0);
        (await context.DraftRoomLogEntries.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenTypeIsUnknown_ShouldReturnInvalidDraftTypeWithoutChangingStorage()
    {
        var (room, caller) = await this.CreateRoomAsync(DraftRoomRole.Host, DraftRoomStatus.Lobby);

        var result = await this.handler.Handle(
            new ChangeDraftTypeCommand(room.Code, (DraftTypeId)999, caller), CancellationToken.None);

        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.InvalidDraftType);
        this.persistence.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_WhenCallerHasWrongSecret_ShouldRefuseWithoutChangingStorage()
    {
        var (room, caller) = await this.CreateRoomAsync(DraftRoomRole.Host, DraftRoomStatus.Lobby);

        var result = await this.handler.Handle(
            new ChangeDraftTypeCommand(room.Code, DraftTypeId.FactionPick, caller with { GuestSecret = "wrong" }),
            CancellationToken.None);

        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.NotFoundOrWrongPassword);
        this.persistence.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_WhenTypeChanges_ShouldUsePersistenceAdapterAndSaveDefaultsWithOneLogAndVersion()
    {
        var (room, caller) = await this.CreateRoomAsync(DraftRoomRole.Host, DraftRoomStatus.Lobby, "PreviousTestType");
        var repository = new FactionPickRepository();
        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.FactionPickSettings.Add(new FactionPickSettings { RoomId = room.Id, PicksPerPlayer = 5 });
            context.FactionPickStates.Add(new FactionPickState { RoomId = room.Id });
            await context.SaveChangesAsync();
        }

        this.persistence
            .Setup(store => store.DeleteAllAsync(It.IsAny<DraftRoomScope>(), CancellationToken.None))
            .Returns<DraftRoomScope, CancellationToken>(repository.DeleteAllAsync);
        this.persistence
            .Setup(store => store.InitializeDefaults(It.IsAny<DraftRoomScope>(), DraftTypeId.FactionPick))
            .Callback<DraftRoomScope, DraftTypeId>((scope, type) =>
                new DraftRoomDraftPersistence(repository, Mock.Of<IFactionPickPoolProvider>()).InitializeDefaults(scope, type));

        var result = await this.handler.Handle(
            new ChangeDraftTypeCommand(room.Code, DraftTypeId.FactionPick, caller), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await using var verification = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var saved = await verification.DraftRooms.SingleAsync(item => item.Id == room.Id);
        saved.DraftType.Should().Be(nameof(DraftTypeId.FactionPick));
        saved.Version.Should().Be(1);
        (await verification.FactionPickSettings.SingleAsync(item => item.RoomId == room.Id)).PicksPerPlayer.Should().Be(1);
        (await verification.FactionPickStates.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
        (await verification.DraftRoomLogEntries.SingleAsync(item => item.RoomId == room.Id))
            .Type.Should().Be(DraftLogEntryType.DraftTypeChanged);
    }

    private async Task<(DraftRoom Room, DraftRoomCaller Caller)> CreateRoomAsync(
        DraftRoomRole role,
        DraftRoomStatus status,
        string draftType = nameof(DraftTypeId.FactionPick))
    {
        const string secret = "test-secret";
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant(),
            Name = "Draft type test",
            PasswordHash = "test-hash",
            DraftType = draftType,
            Status = status,
            CreatedAtUtc = DateTime.UtcNow,
            LastActivityUtc = DateTime.UtcNow,
        };
        var participant = new DraftRoomParticipant
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            Role = role,
            DisplayName = "Manager",
            DisplayNameKey = "MANAGER",
            SecretHash = new GuestSecretService().Hash(secret),
            Status = DraftParticipantStatus.Active,
            JoinedAtUtc = room.CreatedAtUtc,
            LastSeenUtc = room.CreatedAtUtc,
        };
        room.Participants.Add(participant);
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.DraftRooms.Add(room);
        await context.SaveChangesAsync();
        return (room, new DraftRoomCaller(null, null, participant.Id, secret));
    }
}
