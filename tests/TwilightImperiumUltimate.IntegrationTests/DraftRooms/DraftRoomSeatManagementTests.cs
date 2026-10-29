using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomSeatManagementTests : IClassFixture<DraftRoomsModelTestsFixture>
{
    private readonly DraftRoomsModelTestsFixture fixture;
    private readonly IDraftRoomUnitOfWork unitOfWork;
    private readonly IDraftRoomAuthorizer authorizer;

    public DraftRoomSeatManagementTests(DraftRoomsModelTestsFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        this.fixture = fixture;
        this.unitOfWork = new DraftRoomUnitOfWork(
            fixture.Database.ContextFactory,
            new NullRoomChangeNotifier());
        this.authorizer = new DraftRoomAuthorizer(new GuestSecretService());
    }

    [Fact]
    public async Task AddSeatCommand_WhenAtMaximum_ShouldRejectWithoutChangingRoomVersion()
    {
        var (room, hostCaller) = CreateRoom(DraftRoomLimits.MaxSeatCount);
        await SaveRoomAsync(room);

        var result = await new AddSeatCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(new AddSeatCommand(room.Code, hostCaller), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.SeatLimitReached);
        (await GetRoomVersionAsync(room.Id)).Should().Be(room.Version);
    }

    [Fact]
    public async Task AddSeatCommand_WhenBelowMaximum_ShouldAppendNextSeat()
    {
        var (room, hostCaller) = CreateRoom(DraftRoomLimits.MinSeatCount);
        await SaveRoomAsync(room);

        var result = await new AddSeatCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(new AddSeatCommand(room.Code, hostCaller), CancellationToken.None);

        result.IsSuccess.Should().BeTrue(string.Join(", ", result.Errors.Select(error => error.Message)));

        var seats = await GetSeatsAsync(room.Id);
        seats.Select(seat => seat.Index).Should().Equal(1, 2, 3);
        seats[2].ParticipantId.Should().BeNull();
        (await GetRoomVersionAsync(room.Id)).Should().Be(room.Version + 1);
        await DraftRoomLogAssertions.AssertEntriesAsync(
            this.fixture.Database.ContextFactory,
            room.Id,
            DraftLogEntryType.SeatAdded);
    }

    [Fact]
    public async Task RemoveSeatCommand_WhenAtMinimum_ShouldRejectRemoval()
    {
        var (room, hostCaller) = CreateRoom(DraftRoomLimits.MinSeatCount);
        await SaveRoomAsync(room);

        var seatToRemove = room.Seats.Single(seat => seat.Index == 2);
        var result = await new RemoveSeatCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(new RemoveSeatCommand(room.Code, seatToRemove.Id, hostCaller), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should()
            .Be(DraftRoomErrorCode.MinimumSeatCountReached);
        (await GetSeatsAsync(room.Id)).Should().HaveCount(DraftRoomLimits.MinSeatCount);
    }

    [Fact]
    public async Task RemoveSeatCommand_WhenSeatIsOccupied_ShouldRejectRemoval()
    {
        var (room, hostCaller) = CreateRoom(DraftRoomLimits.MinSeatCount + 1);
        var (member, _) = AddGuestParticipant(room, DraftRoomRole.Member);
        var occupiedSeat = room.Seats.Single(seat => seat.Index == 2);
        occupiedSeat.ParticipantId = member.Id;
        occupiedSeat.Participant = member;
        occupiedSeat.ClaimedAtUtc = DateTime.UtcNow;
        await SaveRoomAsync(room);

        var result = await new RemoveSeatCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(new RemoveSeatCommand(room.Code, occupiedSeat.Id, hostCaller), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.SeatOccupied);
        (await GetSeatsAsync(room.Id)).Should().HaveCount(DraftRoomLimits.MinSeatCount + 1);
    }

    [Fact]
    public async Task AddSeatCommand_WhenCallerIsNotRoomManager_ShouldRejectWithoutChangingRoomVersion()
    {
        var (room, _) = CreateRoom(DraftRoomLimits.DefaultSeatCount);
        var (_, memberCaller) = AddGuestParticipant(room, DraftRoomRole.Member);
        await SaveRoomAsync(room);

        var result = await new AddSeatCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(new AddSeatCommand(room.Code, memberCaller), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.Forbidden);
        (await GetRoomVersionAsync(room.Id)).Should().Be(room.Version);
    }

    [Fact]
    public async Task RemoveSeatCommand_WhenFreeSeatIsRemoved_ShouldRenumberRemainingSeatsInOrder()
    {
        var (room, hostCaller) = CreateRoom(DraftRoomLimits.MinSeatCount + 2);
        await SaveRoomAsync(room);

        var seatToRemove = room.Seats.Single(seat => seat.Index == 2);
        var remainingIds = room.Seats
            .Where(seat => seat.Id != seatToRemove.Id)
            .OrderBy(seat => seat.Index)
            .Select(seat => seat.Id)
            .ToArray();
        var result = await new RemoveSeatCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(new RemoveSeatCommand(room.Code, seatToRemove.Id, hostCaller), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var seats = await GetSeatsAsync(room.Id);
        seats.Select(seat => seat.Id).Should().Equal(remainingIds);
        seats.Select(seat => seat.Index).Should().Equal(Enumerable.Range(1, remainingIds.Length));
        (await GetRoomVersionAsync(room.Id)).Should().Be(room.Version + 1);
        await DraftRoomLogAssertions.AssertEntriesAsync(
            this.fixture.Database.ContextFactory,
            room.Id,
            DraftLogEntryType.SeatRemoved);
    }

    [Fact]
    public async Task ClaimSeatCommand_WhenParticipantMovesToFreeSeat_ShouldReleasePreviousSeat()
    {
        var (room, hostCaller) = CreateRoom(DraftRoomLimits.DefaultSeatCount);
        await SaveRoomAsync(room);

        var previousSeat = room.Seats.Single(seat => seat.Index == 1);
        var targetSeat = room.Seats.Single(seat => seat.Index == 2);
        var result = await new ClaimSeatCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(new ClaimSeatCommand(room.Code, targetSeat.Id, hostCaller), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var seats = await GetSeatsAsync(room.Id);
        seats.Single(seat => seat.Id == previousSeat.Id).ParticipantId.Should().BeNull();
        seats.Single(seat => seat.Id == targetSeat.Id).ParticipantId.Should().Be(hostCaller.GuestParticipantId);
        seats.Single(seat => seat.Id == previousSeat.Id).ClaimedAtUtc.Should().BeNull();
        seats.Single(seat => seat.Id == targetSeat.Id).ClaimedAtUtc.Should().NotBeNull();
        (await GetRoomVersionAsync(room.Id)).Should().Be(room.Version + 1);
        await DraftRoomLogAssertions.AssertEntriesAsync(
            this.fixture.Database.ContextFactory,
            room.Id,
            DraftLogEntryType.SeatUnclaimed,
            DraftLogEntryType.SeatClaimed);
    }

    [Fact]
    public async Task ClaimSeatCommand_WhenTargetSeatIsOccupied_ShouldRejectClaim()
    {
        var (room, hostCaller) = CreateRoom(DraftRoomLimits.MinSeatCount + 1);
        var (member, _) = AddGuestParticipant(room, DraftRoomRole.Member);
        SetSeatParticipant(room, 2, member);
        await SaveRoomAsync(room);

        var targetSeat = room.Seats.Single(seat => seat.Index == 2);
        var result = await new ClaimSeatCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(new ClaimSeatCommand(room.Code, targetSeat.Id, hostCaller), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.SeatOccupied);
    }

    [Fact]
    public async Task ClaimSeatCommand_WhenRoomIsNotInLobby_ShouldRejectClaim()
    {
        var (room, hostCaller) = CreateRoom(DraftRoomLimits.DefaultSeatCount);
        room.Status = DraftRoomStatus.InProgress;
        await SaveRoomAsync(room);

        var targetSeat = room.Seats.Single(seat => seat.Index == 2);
        var result = await new ClaimSeatCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(new ClaimSeatCommand(room.Code, targetSeat.Id, hostCaller), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.RoomChanged);
        (await GetRoomVersionAsync(room.Id)).Should().Be(room.Version);
    }

    [Fact]
    public async Task ClaimSeatCommand_WhenEightParticipantsClaimTheSameSeatInParallel_ShouldAllowOneClaim()
    {
        var (room, hostCaller) = CreateRoom(DraftRoomLimits.MaxSeatCount);
        var callers = new List<DraftRoomCaller> { hostCaller };
        for (var participantNumber = 1; participantNumber < DraftRoomLimits.MaxSeatCount; participantNumber++)
        {
            var (_, caller) = AddGuestParticipant(
                room,
                DraftRoomRole.Member,
                $"Player {participantNumber}");
            callers.Add(caller);
        }

        await SaveRoomAsync(room);

        var targetSeat = room.Seats.Single(seat => seat.Index == 2);
        var results = await Task.WhenAll(callers.Select(caller =>
            new ClaimSeatCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
                .Handle(new ClaimSeatCommand(room.Code, targetSeat.Id, caller), CancellationToken.None)));

        results.Count(result => result.IsSuccess).Should().Be(1);
        results.Count(result =>
            result.IsFailed
                && result.Errors.OfType<DraftRoomError>().Single().Code == DraftRoomErrorCode.SeatOccupied)
            .Should()
            .Be(DraftRoomLimits.MaxSeatCount - 1);
        (await GetSeatsAsync(room.Id))
            .Single(seat => seat.Id == targetSeat.Id)
            .ParticipantId.Should().NotBeNull();
        (await GetRoomVersionAsync(room.Id)).Should().Be(room.Version + 1);
    }

    [Fact]
    public async Task UnclaimSeatCommand_WhenParticipantUnclaimsOwnSeatInLobby_ShouldFreeSeat()
    {
        var (room, _) = CreateRoom(DraftRoomLimits.DefaultSeatCount);
        var (member, memberCaller) = AddGuestParticipant(room, DraftRoomRole.Member, "Player");
        SetSeatParticipant(room, 2, member);
        await SaveRoomAsync(room);

        var seat = room.Seats.Single(item => item.Index == 2);
        var result = await new UnclaimSeatCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(new UnclaimSeatCommand(room.Code, seat.Id, memberCaller), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var persistedSeat = (await GetSeatsAsync(room.Id)).Single(item => item.Id == seat.Id);
        persistedSeat.ParticipantId.Should().BeNull();
        persistedSeat.ClaimedAtUtc.Should().BeNull();
        await DraftRoomLogAssertions.AssertEntriesAsync(
            this.fixture.Database.ContextFactory,
            room.Id,
            DraftLogEntryType.SeatUnclaimed);
        (await GetRoomVersionAsync(room.Id)).Should().Be(room.Version + 1);
    }

    [Fact]
    public async Task UnclaimSeatCommand_WhenManagerUnclaimsInProgress_ShouldFreeSeat()
    {
        var (room, hostCaller) = CreateRoom(DraftRoomLimits.DefaultSeatCount);
        var (member, _) = AddGuestParticipant(room, DraftRoomRole.Member, "Player");
        SetSeatParticipant(room, 2, member);
        room.Status = DraftRoomStatus.InProgress;
        await SaveRoomAsync(room);

        var seat = room.Seats.Single(item => item.Index == 2);
        var result = await new UnclaimSeatCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(new UnclaimSeatCommand(room.Code, seat.Id, hostCaller), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        (await GetSeatsAsync(room.Id)).Single(item => item.Id == seat.Id).ParticipantId.Should().BeNull();
    }

    [Fact]
    public async Task UnclaimSeatCommand_WhenParticipantUnclaimsAnotherSeat_ShouldReject()
    {
        var (room, _) = CreateRoom(DraftRoomLimits.DefaultSeatCount);
        var (seatOwner, _) = AddGuestParticipant(room, DraftRoomRole.Member, "Seat owner");
        var (_, caller) = AddGuestParticipant(room, DraftRoomRole.Member, "Other player");
        SetSeatParticipant(room, 2, seatOwner);
        await SaveRoomAsync(room);

        var seat = room.Seats.Single(item => item.Index == 2);
        var result = await new UnclaimSeatCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(new UnclaimSeatCommand(room.Code, seat.Id, caller), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.Forbidden);
    }

    [Fact]
    public async Task UnclaimSeatCommand_WhenParticipantUnclaimsOwnSeatWhileRunning_ShouldRelease()
    {
        var (room, _) = CreateRoom(DraftRoomLimits.DefaultSeatCount);
        var (member, memberCaller) = AddGuestParticipant(room, DraftRoomRole.Member, "Player");
        SetSeatParticipant(room, 2, member);
        room.Status = DraftRoomStatus.InProgress;
        await SaveRoomAsync(room);

        var seat = room.Seats.Single(item => item.Index == 2);
        var result = await new UnclaimSeatCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(new UnclaimSeatCommand(room.Code, seat.Id, memberCaller), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        var persistedSeat = (await GetSeatsAsync(room.Id)).Single(item => item.Id == seat.Id);
        persistedSeat.ParticipantId.Should().BeNull();
        persistedSeat.HasBeenReleased.Should().BeTrue();
    }

    [Fact]
    public async Task UnclaimSeatCommand_WhenSeatIsAlreadyFree_ShouldReject()
    {
        var (room, hostCaller) = CreateRoom(DraftRoomLimits.DefaultSeatCount);
        await SaveRoomAsync(room);

        var seat = room.Seats.Single(item => item.Index == 2);
        var result = await new UnclaimSeatCommandHandler(this.unitOfWork, this.authorizer, new DraftRoomLog())
            .Handle(new UnclaimSeatCommand(room.Code, seat.Id, hostCaller), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.SeatNotOccupied);
        (await GetRoomVersionAsync(room.Id)).Should().Be(room.Version);
    }

    private static (DraftRoom Room, DraftRoomCaller HostCaller) CreateRoom(int seatCount)
    {
        var now = DateTime.UtcNow;
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant(),
            Name = "Seat management test room",
            PasswordHash = "test-hash",
            Status = DraftRoomStatus.Lobby,
            DraftType = "test",
            Version = 1,
            CreatedAtUtc = now,
            LastActivityUtc = now,
        };

        var (host, hostCaller) = AddGuestParticipant(room, DraftRoomRole.Host);
        for (var index = 1; index <= seatCount; index++)
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
        string? displayName = null)
    {
        var secret = $"test-secret-{Guid.NewGuid():N}";
        var participantName = displayName ?? role.ToString();
        var participant = new DraftRoomParticipant
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            SecretHash = new GuestSecretService().Hash(secret),
            DisplayName = participantName,
            DisplayNameKey = participantName.ToUpperInvariant(),
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
        ArgumentNullException.ThrowIfNull(room);
        ArgumentNullException.ThrowIfNull(participant);

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

    private async Task<List<DraftRoomSeat>> GetSeatsAsync(Guid roomId)
    {
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();

        return await context.DraftRoomSeats
            .Where(seat => seat.RoomId == roomId)
            .OrderBy(seat => seat.Index)
            .ToListAsync();
    }

    private async Task<int> GetRoomVersionAsync(Guid roomId)
    {
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();

        return await context.DraftRooms
            .Where(room => room.Id == roomId)
            .Select(room => room.Version)
            .SingleAsync();
    }
}
