using FluentAssertions;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomAuthorizerTests
{
    private readonly GuestSecretService guestSecretService = new();
    private readonly DraftRoomAuthorizer authorizer;

    public DraftRoomAuthorizerTests()
    {
        this.authorizer = new DraftRoomAuthorizer(this.guestSecretService);
    }

    [Fact]
    public void Resolve_WhenActiveAccountParticipantHasASeat_ShouldReturnActor()
    {
        var (room, participant) = CreateParticipant(DraftRoomRole.Member, DraftParticipantStatus.Active, seated: true);

        var actor = this.authorizer.Resolve(
            room,
            new DraftRoomCaller(participant.UserId, participant.DisplayName, null, null));

        actor.Should().NotBeNull();
        actor!.Participant.Should().BeSameAs(participant);
        actor.Seat.Should().NotBeNull();
        actor.Role.Should().Be(DraftRoomRole.Member);
    }

    [Fact]
    public void Resolve_WhenGuestSecretIsValid_ShouldReturnGuestActor()
    {
        var (room, participant) = CreateParticipant(
            DraftRoomRole.Host,
            DraftParticipantStatus.Active,
            seated: false,
            guestSecret: "guest-secret");

        var actor = this.authorizer.Resolve(
            room,
            new DraftRoomCaller(null, null, participant.Id, "guest-secret"));

        actor.Should().NotBeNull();
        actor!.Participant.Should().BeSameAs(participant);
        actor.Seat.Should().BeNull();
        actor.Role.Should().Be(DraftRoomRole.Host);
    }

    [Fact]
    public void Resolve_WhenGuestSecretIsInvalid_ShouldReturnForbidden()
    {
        var (room, participant) = CreateParticipant(
            DraftRoomRole.Member,
            DraftParticipantStatus.Active,
            seated: true,
            guestSecret: "correct-secret");

        var actor = this.authorizer.Resolve(
            room,
            new DraftRoomCaller(null, null, participant.Id, "wrong-secret"));

        actor.Should().BeNull();
    }

    [Theory]
    [InlineData(DraftParticipantStatus.Kicked)]
    [InlineData(DraftParticipantStatus.Left)]
    public void Resolve_WhenParticipantIsNotActive_ShouldReturnForbidden(DraftParticipantStatus status)
    {
        var (room, participant) = CreateParticipant(DraftRoomRole.Member, status, seated: true);

        var actor = this.authorizer.Resolve(
            room,
            new DraftRoomCaller(participant.UserId, participant.DisplayName, null, null));

        actor.Should().BeNull();
    }

    [Fact]
    public void Resolve_WhenCallerIsNotAParticipant_ShouldReturnForbidden()
    {
        var (room, _) = CreateParticipant(DraftRoomRole.Host, DraftParticipantStatus.Active, seated: true);

        var actor = this.authorizer.Resolve(
            room,
            new DraftRoomCaller("unrelated-user", "Unrelated", null, null));

        actor.Should().BeNull();
    }

    [Fact]
    public void Resolve_WhenAccountCallerAlsoProvidesGuestCredentials_ShouldReturnForbidden()
    {
        var (room, participant) = CreateParticipant(DraftRoomRole.Host, DraftParticipantStatus.Active, seated: true);

        var actor = this.authorizer.Resolve(
            room,
            new DraftRoomCaller(participant.UserId, participant.DisplayName, Guid.NewGuid(), "guest-secret"));

        actor.Should().BeNull();
    }

    [Theory]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.Lobby, true)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomStatus.Lobby, true)]
    [InlineData(DraftRoomRole.Member, DraftRoomStatus.Lobby, false)]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.InProgress, false)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomStatus.InProgress, false)]
    [InlineData(DraftRoomRole.Member, DraftRoomStatus.InProgress, false)]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.Finished, false)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomStatus.Finished, false)]
    [InlineData(DraftRoomRole.Member, DraftRoomStatus.Finished, false)]
    public void CanManageSeats_WhenRoleAndRoomStateVary_ShouldMatchLobbyPolicy(
        DraftRoomRole role,
        DraftRoomStatus status,
        bool expected)
    {
        var (_, _, actor) = CreateActor(role, status, seated: false);

        this.authorizer.CanManageSeats(actor).Should().Be(expected);
        this.authorizer.CanChangeSettings(actor).Should().Be(expected);
    }

    [Theory]
    [InlineData(DraftRoomRole.Host, true)]
    [InlineData(DraftRoomRole.CoHost, true)]
    [InlineData(DraftRoomRole.Member, false)]
    public void CanClose_WhenRoleVaries_ShouldAllowOnlyHostAndCohost(DraftRoomRole role, bool expected)
    {
        var (_, _, actor) = CreateActor(role, DraftRoomStatus.Finished, seated: false);

        this.authorizer.CanClose(actor).Should().Be(expected);
    }

    [Theory]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.Lobby, true)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomStatus.Lobby, true)]
    [InlineData(DraftRoomRole.Member, DraftRoomStatus.Lobby, true)]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.InProgress, false)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomStatus.InProgress, false)]
    [InlineData(DraftRoomRole.Member, DraftRoomStatus.InProgress, false)]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.Finished, false)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomStatus.Finished, false)]
    [InlineData(DraftRoomRole.Member, DraftRoomStatus.Finished, false)]
    public void CanClaimSeat_WhenRoleAndRoomStateVary_ShouldAllowActiveLobbyParticipants(
        DraftRoomRole role,
        DraftRoomStatus status,
        bool expected)
    {
        var (_, _, actor) = CreateActor(role, status, seated: false);

        this.authorizer.CanClaimSeat(actor).Should().Be(expected);
    }

    [Theory]
    [InlineData(DraftRoomRole.Host, DraftRoomRole.Member, true)]
    [InlineData(DraftRoomRole.Host, DraftRoomRole.CoHost, true)]
    [InlineData(DraftRoomRole.Host, DraftRoomRole.Host, false)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomRole.Member, true)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomRole.CoHost, false)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomRole.Host, false)]
    [InlineData(DraftRoomRole.Member, DraftRoomRole.Member, false)]
    [InlineData(DraftRoomRole.Member, DraftRoomRole.CoHost, false)]
    public void CanKick_WhenActorAndTargetRolesVary_ShouldEnforceHostHierarchy(
        DraftRoomRole actorRole,
        DraftRoomRole targetRole,
        bool expected)
    {
        var (room, _, actor) = CreateActor(actorRole, DraftRoomStatus.Lobby, seated: true);
        var target = AddParticipant(room, targetRole, DraftParticipantStatus.Active);

        this.authorizer.CanKick(actor, target).Should().Be(expected);
    }

    [Theory]
    [InlineData(DraftRoomRole.Host, DraftRoomRole.Member, true)]
    [InlineData(DraftRoomRole.Host, DraftRoomRole.CoHost, true)]
    [InlineData(DraftRoomRole.Host, DraftRoomRole.Host, false)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomRole.Member, false)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomRole.CoHost, false)]
    [InlineData(DraftRoomRole.Member, DraftRoomRole.Member, false)]
    public void CanChangeRoles_WhenActorAndTargetRolesVary_ShouldAllowOnlyHost(
        DraftRoomRole actorRole,
        DraftRoomRole targetRole,
        bool expected)
    {
        var (room, _, actor) = CreateActor(actorRole, DraftRoomStatus.InProgress, seated: true);
        var target = AddParticipant(room, targetRole, DraftParticipantStatus.Active);

        this.authorizer.CanChangeRoles(actor, target).Should().Be(expected);
    }

    [Theory]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.Lobby, false, true)]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.InProgress, false, true)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomStatus.Lobby, false, true)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomStatus.InProgress, false, true)]
    [InlineData(DraftRoomRole.Member, DraftRoomStatus.Lobby, true, true)]
    [InlineData(DraftRoomRole.Member, DraftRoomStatus.InProgress, true, true)]
    [InlineData(DraftRoomRole.Member, DraftRoomStatus.Lobby, false, false)]
    public void CanUnclaimSeat_WhenRoleAndSeatOwnerVary_ShouldEnforceSeatPolicy(
        DraftRoomRole actorRole,
        DraftRoomStatus status,
        bool targetIsActor,
        bool expected)
    {
        var (room, actorParticipant, actor) = CreateActor(actorRole, status, seated: true);
        var target = targetIsActor
            ? actorParticipant
            : AddParticipant(room, DraftRoomRole.Member, DraftParticipantStatus.Active);
        var targetSeat = AddSeat(room, target, index: 2);

        this.authorizer.CanUnclaimSeat(actor, targetSeat).Should().Be(expected);
    }

    [Fact]
    public void CanKick_WhenTargetIsInactiveOrInAnotherRoom_ShouldDenyOperation()
    {
        var (room, _, actor) = CreateActor(DraftRoomRole.Host, DraftRoomStatus.Lobby, seated: true);
        var inactiveTarget = AddParticipant(room, DraftRoomRole.Member, DraftParticipantStatus.Left);
        var otherRoomTarget = CreateParticipant(
            DraftRoomRole.Member,
            DraftParticipantStatus.Active,
            seated: false).Participant;
        var unattachedTarget = new DraftRoomParticipant
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            Role = DraftRoomRole.Member,
            Status = DraftParticipantStatus.Active,
        };

        this.authorizer.CanKick(actor, inactiveTarget).Should().BeFalse();
        this.authorizer.CanKick(actor, otherRoomTarget).Should().BeFalse();
        this.authorizer.CanKick(actor, unattachedTarget).Should().BeFalse();
    }

    [Fact]
    public void CanUnclaimSeat_WhenSeatIsFreeOrBelongsToAnotherRoom_ShouldDenyOperation()
    {
        var (room, _, actor) = CreateActor(DraftRoomRole.Host, DraftRoomStatus.Lobby, seated: true);
        var otherRoom = CreateRoom(DraftRoomStatus.Lobby);
        var otherParticipant = AddParticipant(otherRoom, DraftRoomRole.Member, DraftParticipantStatus.Active);
        var freeSeat = new DraftRoomSeat
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            Index = 2,
            ParticipantId = null,
            Room = room,
        };
        var otherRoomSeat = AddSeat(otherRoom, otherParticipant, index: 1);

        this.authorizer.CanUnclaimSeat(actor, freeSeat).Should().BeFalse();
        this.authorizer.CanUnclaimSeat(actor, otherRoomSeat).Should().BeFalse();
    }

    [Fact]
    public void CanClose_WhenActorBecomesInactive_ShouldDenyOperation()
    {
        var (_, participant, actor) = CreateActor(DraftRoomRole.Host, DraftRoomStatus.Lobby, seated: true);
        participant.Status = DraftParticipantStatus.Kicked;

        this.authorizer.CanClose(actor).Should().BeFalse();
    }

    private static DraftRoom CreateRoom(DraftRoomStatus status = DraftRoomStatus.Lobby)
    {
        var now = DateTime.UtcNow;

        return new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant(),
            Name = "Authorization test room",
            PasswordHash = "test-hash",
            Status = status,
            DraftType = "test",
            Version = 0,
            CreatedAtUtc = now,
            LastActivityUtc = now,
        };
    }

    private static (DraftRoom Room, DraftRoomParticipant Participant, DraftRoomActor Actor) CreateActor(
        DraftRoomRole role,
        DraftRoomStatus status,
        bool seated)
    {
        var room = CreateRoom(status);
        var participant = AddParticipant(room, role, DraftParticipantStatus.Active, "account-user");
        if (seated)
        {
            _ = AddSeat(room, participant, index: 1);
        }

        var authorizer = new DraftRoomAuthorizer(new GuestSecretService());
        var actor = authorizer.Resolve(
            room,
            new DraftRoomCaller(participant.UserId, participant.DisplayName, null, null));

        return (room, participant, actor!);
    }

    private static (DraftRoom Room, DraftRoomParticipant Participant) CreateParticipant(
        DraftRoomRole role,
        DraftParticipantStatus status,
        bool seated,
        string? guestSecret = null)
    {
        var room = CreateRoom();
        var participant = guestSecret is null
            ? AddParticipant(room, role, status, userId: Guid.NewGuid().ToString())
            : AddParticipant(room, role, status, guestSecret: guestSecret);
        if (seated)
        {
            _ = AddSeat(room, participant, index: 1);
        }

        return (room, participant);
    }

    private static DraftRoomParticipant AddParticipant(
        DraftRoom room,
        DraftRoomRole role,
        DraftParticipantStatus status,
        string? userId = null,
        string? guestSecret = null)
    {
        var participantId = Guid.NewGuid();
        var participant = new DraftRoomParticipant
        {
            Id = participantId,
            RoomId = room.Id,
            Room = room,
            UserId = userId,
            SecretHash = userId is null
                ? new GuestSecretService().Hash(guestSecret ?? "guest-secret")
                : null,
            DisplayName = userId ?? $"Guest-{participantId:N}",
            DisplayNameKey = userId ?? $"GUEST-{participantId:N}",
            Role = role,
            Status = status,
            JoinedAtUtc = DateTime.UtcNow,
            LastSeenUtc = DateTime.UtcNow,
        };

        room.Participants.Add(participant);

        return participant;
    }

    private static DraftRoomSeat AddSeat(DraftRoom room, DraftRoomParticipant participant, int index)
    {
        var seat = new DraftRoomSeat
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            Room = room,
            Index = index,
            ParticipantId = participant.Id,
            ClaimedAtUtc = DateTime.UtcNow,
            Participant = participant,
        };

        room.Seats.Add(seat);

        return seat;
    }
}
