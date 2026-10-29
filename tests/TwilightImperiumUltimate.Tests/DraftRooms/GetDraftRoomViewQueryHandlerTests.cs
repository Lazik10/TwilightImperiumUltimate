using FluentAssertions;
using Moq;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.Repositories;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class GetDraftRoomViewQueryHandlerTests
{
    private const string GuestSecret = "guest-secret";

    [Fact]
    public async Task Handle_WhenHostRequestsRoom_ShouldMapRoomParticipantsSeatsAndPermissions()
    {
        var fixture = CreateRoomFixture();
        var presenceTracker = new Mock<IPresenceTracker>(MockBehavior.Strict);
        presenceTracker
            .Setup(tracker => tracker.IsOnline(fixture.Room.Id, It.IsAny<Guid>()))
            .Returns((Guid _, Guid participantId) => participantId == fixture.Host.Id);
        var (handler, repository) = CreateHandler(fixture.Room, presenceTracker.Object);

        var result = await handler.Handle(
            new GetDraftRoomViewQuery(
                $" {fixture.Room.Code.ToLowerInvariant()} ",
                new DraftRoomCaller("host-user", "Host", null, null)),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.RoomId.Should().Be(fixture.Room.Id);
        result.Value.Code.Should().Be(fixture.Room.Code);
        result.Value.Name.Should().Be(fixture.Room.Name);
        result.Value.Status.Should().Be(DraftRoomStatus.Lobby);
        result.Value.IsLocked.Should().BeTrue();
        result.Value.Version.Should().Be(4);
        result.Value.DraftType.Should().Be(DraftTypeId.FactionPick);
        result.Value.Settings.Should().BeOfType<FactionPickSettingsDto>();
        result.Value.StartBlockers.Should().Contain("SeatsNotFull");
        result.Value.Draft.Should().BeNull();
        result.Value.Participants.Should().HaveCount(2);
        result.Value.Participants.Should().ContainSingle(participant => participant.Id == fixture.Host.Id)
            .Which.Should().Match<DraftRoomViewParticipantDto>(participant =>
                !participant.IsGuest && participant.IsOnline);
        result.Value.Participants.Should().ContainSingle(participant => participant.Id == fixture.Guest.Id)
            .Which.Should().Match<DraftRoomViewParticipantDto>(participant =>
                participant.SeatId == fixture.GuestSeat.Id && !participant.IsOnline);
        result.Value.Seats.Should().HaveCount(3);
        result.Value.Seats.Select(seat => seat.Index).Should().Equal(1, 2, 3);
        result.Value.Seats.Single(seat => seat.Id == fixture.OpenSeat.Id).ParticipantId.Should().BeNull();
        result.Value.Me.ParticipantId.Should().Be(fixture.Host.Id);
        result.Value.Me.Role.Should().Be(DraftRoomRole.Host);
        result.Value.Me.SeatId.Should().Be(fixture.HostSeat.Id);
        result.Value.Me.CanManageSeats.Should().BeTrue();
        result.Value.Me.CanChangeSettings.Should().BeTrue();
        result.Value.Me.CanKick.Should().BeTrue();
        result.Value.Me.CanChangeRoles.Should().BeTrue();
        result.Value.Me.CanClose.Should().BeTrue();
        result.Value.Me.CanClaimSeat.Should().BeTrue();
        repository.Verify(
            item => item.GetRoomForViewByCodeAsync(fixture.Room.Code, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenGuestMemberRequestsRoom_ShouldReturnTheirSeatAndLimitedPermissions()
    {
        var fixture = CreateRoomFixture();
        var (handler, _) = CreateHandler(fixture.Room);

        var result = await handler.Handle(
            new GetDraftRoomViewQuery(
                fixture.Room.Code,
                new DraftRoomCaller(null, null, fixture.Guest.Id, GuestSecret)),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Me.ParticipantId.Should().Be(fixture.Guest.Id);
        result.Value.Me.Role.Should().Be(DraftRoomRole.Member);
        result.Value.Me.SeatId.Should().Be(fixture.GuestSeat.Id);
        result.Value.Me.CanManageSeats.Should().BeFalse();
        result.Value.Me.CanChangeSettings.Should().BeFalse();
        result.Value.Me.CanKick.Should().BeFalse();
        result.Value.Me.CanChangeRoles.Should().BeFalse();
        result.Value.Me.CanClose.Should().BeFalse();
        result.Value.Me.CanClaimSeat.Should().BeTrue();
        result.Value.Participants.Should().ContainSingle(participant => participant.Id == fixture.Guest.Id)
            .Which.IsGuest.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenCallerIsNotAParticipant_ShouldReturnNotFound()
    {
        var fixture = CreateRoomFixture();
        var (handler, _) = CreateHandler(fixture.Room);

        var result = await handler.Handle(
            new GetDraftRoomViewQuery(
                fixture.Room.Code,
                new DraftRoomCaller("unknown-user", "Stranger", null, null)),
            CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<DraftRoomError>()
            .Which.Code.Should().Be(DraftRoomErrorCode.NotFoundOrWrongPassword);
    }

    [Fact]
    public async Task Handle_WhenRoomCodeIsBlank_ShouldReturnNotFoundWithoutLookingUpRoom()
    {
        var room = CreateRoomFixture().Room;
        var (handler, repository) = CreateHandler(room);

        var result = await handler.Handle(
            new GetDraftRoomViewQuery("  ", new DraftRoomCaller("host-user", "Host", null, null)),
            CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<DraftRoomError>()
            .Which.Code.Should().Be(DraftRoomErrorCode.NotFoundOrWrongPassword);
        repository.Verify(
            item => item.GetRoomForViewByCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void DraftRoomViewDtos_ShouldNotExposeSecretHashOrPasswordProperties()
    {
        var dtoTypes = new[]
        {
            typeof(DraftRoomViewDto),
            typeof(DraftRoomViewMeDto),
            typeof(DraftRoomViewParticipantDto),
            typeof(DraftRoomViewSeatDto),
        };

        var sensitiveProperties = dtoTypes
            .SelectMany(type => type.GetProperties())
            .Select(property => property.Name)
            .Where(name =>
                name.Contains("Secret", StringComparison.OrdinalIgnoreCase)
                || name.Contains("Hash", StringComparison.OrdinalIgnoreCase)
                || name.Contains("Password", StringComparison.OrdinalIgnoreCase));

        sensitiveProperties.Should().BeEmpty();
    }

    private static (GetDraftRoomViewQueryHandler Handler, Mock<IDraftRoomRepository> Repository) CreateHandler(
        DraftRoom room,
        IPresenceTracker? presenceTracker = null)
    {
        var repository = new Mock<IDraftRoomRepository>(MockBehavior.Strict);
        repository
            .Setup(item => item.GetRoomForViewByCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);

        var handler = new GetDraftRoomViewQueryHandler(
            repository.Object,
            new DraftRoomAuthorizer(new GuestSecretService()),
            presenceTracker ?? Mock.Of<IPresenceTracker>(),
            CreateDraftViewProvider());

        return (handler, repository);
    }

    private static IDraftRoomDraftViewProvider CreateDraftViewProvider()
    {
        var provider = new Mock<IDraftRoomDraftViewProvider>();
        provider.Setup(item => item.GetAsync(It.IsAny<DraftRoomActor>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomDraftView(DraftTypeId.FactionPick, new FactionPickSettingsDto(), null, false, ["SeatsNotFull"], false));
        return provider.Object;
    }

    private static RoomFixture CreateRoomFixture()
    {
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = "ABC234",
            Name = "Test Room",
            PasswordHash = "room-password-hash",
            DraftType = "faction-pick",
            Status = DraftRoomStatus.Lobby,
            IsLocked = true,
            Version = 4,
            CreatedAtUtc = DateTime.UtcNow,
            LastActivityUtc = DateTime.UtcNow,
        };
        var host = CreateParticipant(room, "Host", "HOST", "host-user", DraftRoomRole.Host);
        var guest = CreateParticipant(room, "Guest", "GUEST", null, DraftRoomRole.Member);
        var leftParticipant = CreateParticipant(
            room,
            "Former member",
            "FORMER MEMBER",
            null,
            DraftRoomRole.Member,
            DraftParticipantStatus.Left);

        room.Participants.Add(host);
        room.Participants.Add(guest);
        room.Participants.Add(leftParticipant);

        var hostSeat = CreateSeat(room, 1, host.Id);
        var guestSeat = CreateSeat(room, 2, guest.Id);
        var openSeat = CreateSeat(room, 3, null);
        room.Seats.Add(hostSeat);
        room.Seats.Add(guestSeat);
        room.Seats.Add(openSeat);

        return new RoomFixture(room, host, guest, hostSeat, guestSeat, openSeat);
    }

    private static DraftRoomParticipant CreateParticipant(
        DraftRoom room,
        string displayName,
        string displayNameKey,
        string? userId,
        DraftRoomRole role,
        DraftParticipantStatus status = DraftParticipantStatus.Active)
    {
        return new DraftRoomParticipant
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            UserId = userId,
            SecretHash = userId is null ? new GuestSecretService().Hash(GuestSecret) : null,
            DisplayName = displayName,
            DisplayNameKey = displayNameKey,
            Role = role,
            Status = status,
            JoinedAtUtc = DateTime.UtcNow,
            LastSeenUtc = DateTime.UtcNow,
        };
    }

    private static DraftRoomSeat CreateSeat(DraftRoom room, int index, Guid? participantId)
    {
        return new DraftRoomSeat
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            Index = index,
            ParticipantId = participantId,
            ClaimedAtUtc = participantId.HasValue ? DateTime.UtcNow : null,
        };
    }

    private sealed record RoomFixture(
        DraftRoom Room,
        DraftRoomParticipant Host,
        DraftRoomParticipant Guest,
        DraftRoomSeat HostSeat,
        DraftRoomSeat GuestSeat,
        DraftRoomSeat OpenSeat);
}
