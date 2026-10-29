using FluentAssertions;
using Moq;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;
using TwilightImperiumUltimate.Draft.DraftRooms.FactionPick;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomDraftViewProviderTests
{
    [Theory]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.Lobby, true, false)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomStatus.Lobby, true, false)]
    [InlineData(DraftRoomRole.Member, DraftRoomStatus.Lobby, false, false)]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.InProgress, false, true)]
    [InlineData(DraftRoomRole.CoHost, DraftRoomStatus.InProgress, false, true)]
    [InlineData(DraftRoomRole.Member, DraftRoomStatus.InProgress, false, false)]
    [InlineData(DraftRoomRole.Host, DraftRoomStatus.Finished, false, false)]
    public async Task GetAsync_WhenViewerRoleOrStatusChanges_ShouldProjectOnlyPermittedControls(
        DraftRoomRole role,
        DraftRoomStatus status,
        bool canStart,
        bool canReset)
    {
        var (provider, actor, room) = CreateFixture(role, status);

        var view = await provider.GetAsync(actor, CancellationToken.None);

        view.CanStart.Should().Be(canStart);
        view.CanReset.Should().Be(canReset);
        view.Settings.Should().BeOfType<FactionPickSettingsDto>();
        if (status == DraftRoomStatus.Lobby)
        {
            view.Draft.Should().BeNull();
            view.StartBlockers.Should().BeEmpty();
        }
        else
        {
            var draft = view.Draft.Should().BeOfType<FactionPickViewDto>().Which;
            draft.TurnOrder.Should().Equal(room.FactionPickTurnOrder.OrderBy(entry => entry.Position).Select(entry => entry.SeatId));
            view.StartBlockers.Should().Contain("RoomChanged");
        }
    }

    [Fact]
    public async Task GetAsync_WhenSeatIsVacant_ShouldExplainWhyManagerCannotStart()
    {
        var (provider, actor, room) = CreateFixture(DraftRoomRole.Host, DraftRoomStatus.Lobby);
        room.Seats.Last().ParticipantId = null;

        var view = await provider.GetAsync(actor, CancellationToken.None);

        view.CanStart.Should().BeFalse();
        view.StartBlockers.Should().Contain("SeatsNotFull");
    }

    [Fact]
    public async Task GetAsync_WhenSpectatorIsManager_ShouldNotAcquireAnotherSeatIdentity()
    {
        var (provider, _, room) = CreateFixture(DraftRoomRole.Host, DraftRoomStatus.InProgress);
        var spectator = new DraftRoomParticipant { Id = Guid.NewGuid(), UserId = "spectator", Role = DraftRoomRole.CoHost };
        room.Participants.Add(spectator);
        var actor = new DraftRoomAuthorizer(new GuestSecretService())
            .Resolve(room, new DraftRoomCaller("spectator", "Spectator", null, null))!;

        var view = await provider.GetAsync(actor, CancellationToken.None);

        view.CanReset.Should().BeTrue();
        actor.Seat.Should().BeNull();
        view.Draft.Should().BeOfType<FactionPickViewDto>();
    }

    private static (DraftRoomDraftViewProvider Provider, DraftRoomActor Actor, DraftRoom Room) CreateFixture(
        DraftRoomRole role,
        DraftRoomStatus status)
    {
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Status = status,
            FactionPickSettings = new FactionPickSettings(),
            FactionPickState = status == DraftRoomStatus.Lobby ? null : new FactionPickState { Phase = FactionPickPhase.Picks },
        };
        for (var index = 0; index < 2; index++)
        {
            var participant = new DraftRoomParticipant { Id = Guid.NewGuid(), UserId = $"user{index}", Role = index == 0 ? role : DraftRoomRole.Member };
            room.Participants.Add(participant);
            var seat = new DraftRoomSeat { Id = Guid.NewGuid(), RoomId = room.Id, Index = index, ParticipantId = participant.Id };
            room.Seats.Add(seat);
            room.FactionPickTurnOrder.Add(new FactionPickTurnEntry { RoomId = room.Id, Position = index, SeatId = seat.Id });
        }

        var authorizer = new DraftRoomAuthorizer(new GuestSecretService());
        var actor = authorizer.Resolve(room, new DraftRoomCaller("user0", "User", null, null))!;
        var pool = new Mock<IFactionPickPoolProvider>();
        pool.Setup(provider => provider.GetPoolAsync(It.IsAny<IReadOnlyCollection<GameVersion>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([FactionName.TheArborec, FactionName.TheBaronyOfLetnev]);
        var registry = new DraftTypeRegistry(
        [
            new DraftTypeModule<FactionPickSettingsDto, FactionPickDraftState>(new FactionPickDraftType()),
        ]);
        return (new DraftRoomDraftViewProvider(registry, pool.Object, authorizer), actor, room);
    }
}
