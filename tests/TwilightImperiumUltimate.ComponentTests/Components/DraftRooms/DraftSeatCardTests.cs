using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Components.DraftRooms;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.ComponentTests.Components.DraftRooms;

public sealed class DraftSeatCardTests : BunitContext
{
    public DraftSeatCardTests()
    {
        Services.AddSingleton(Mock.Of<IPathProvider>());
        Services.AddSingleton(Mock.Of<ITwilightImperiumApiHttpClient>());
        Services.AddSingleton(Mock.Of<ILocalStorageService>());
        Services.AddSingleton(Mock.Of<IMapper>());
    }

    [Fact]
    public void DraftSeatCard_WhenRunningSeatReleasedAndReclaimed_ShouldShowExplicitActions()
    {
        var participant = Guid.NewGuid();
        var seat = new DraftRoomViewSeatDto { Id = Guid.NewGuid(), Index = 1, ParticipantId = participant, CanUnclaim = true };
        var room = new DraftRoomViewDto
        {
            Status = DraftRoomStatus.InProgress,
            Me = new DraftRoomViewMeDto { ParticipantId = participant, Role = DraftRoomRole.Member },
            Seats = [seat],
        };
        var component = Render<DraftSeatCard>(parameters => parameters
            .Add(parameter => parameter.Room, room)
            .Add(parameter => parameter.Seat, seat));
        component.Find("button").TextContent.Should().Be("Release seat");

        seat.ParticipantId = null;
        seat.CanUnclaim = false;
        seat.CanClaim = true;
        seat.CanReclaim = true;
        component.Render();

        component.Find("button").TextContent.Should().Be("Reclaim seat");
    }

    [Fact]
    public void DraftSeatCard_WhenOccupantIsOffline_ShouldShowOfflinePresenceText()
    {
        var participant = new DraftRoomViewParticipantDto
        {
            Id = Guid.NewGuid(),
            DisplayName = "Player",
            IsOnline = false,
        };
        var room = new DraftRoomViewDto
        {
            Participants = [participant],
            Seats = [new DraftRoomViewSeatDto { Id = Guid.NewGuid(), Index = 1, ParticipantId = participant.Id }],
        };
        var component = Render<DraftSeatCard>(parameters => parameters
            .Add(parameter => parameter.Room, room)
            .Add(parameter => parameter.Seat, room.Seats[0])
            .Add(parameter => parameter.Occupant, participant));

        component.Markup.Should().Contain("Offline");
        component.Find(".draft-presence-indicator__dot")
            .GetAttribute("class")
            .Should().Contain("draft-presence-indicator__dot--offline");
    }
}
