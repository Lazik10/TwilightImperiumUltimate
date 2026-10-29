using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Components.DraftRooms;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.ComponentTests.Components.DraftRooms;

public sealed class DraftSeatListTests : BunitContext
{
    private static readonly Guid CurrentParticipantId = Guid.Parse("a161e9d6-0235-4f1e-a6fb-f129cf8c4159");

    private readonly Mock<IDraftRoomSession> _session = new();
    private readonly Mock<IDraftRoomApiService> _apiService = new();

    public DraftSeatListTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        _session
            .Setup(session => session.ExecuteAsync(
                It.IsAny<Func<Task<DraftRoomApiCommandResult>>>(),
                It.IsAny<CancellationToken>()))
            .Returns((Func<Task<DraftRoomApiCommandResult>> command, CancellationToken _) => command());
        _apiService
            .Setup(service => service.AddSeatAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Success));
        _apiService
            .Setup(service => service.RemoveSeatAsync("ABC234", It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Success));
        _apiService
            .Setup(service => service.ClaimSeatAsync("ABC234", It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Success));
        _apiService
            .Setup(service => service.UnclaimSeatAsync("ABC234", It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Success));
        Services.AddSingleton(Mock.Of<IPathProvider>());
        Services.AddSingleton(Mock.Of<ITwilightImperiumApiHttpClient>());
        Services.AddSingleton(Mock.Of<ILocalStorageService>());
        Services.AddSingleton(Mock.Of<IMapper>());
        Services.AddSingleton(_apiService.Object);
    }

    [Fact]
    public void DraftSeatList_WhenRunning_ShouldUseSeatSpecificClaimPermission()
    {
        var eligible = new DraftRoomViewSeatDto { Id = Guid.NewGuid(), Index = 1, CanClaim = true };
        var ineligible = new DraftRoomViewSeatDto { Id = Guid.NewGuid(), Index = 2 };
        var view = CreateRoomView(DraftRoomRole.Member, false, true, [eligible, ineligible]);
        view.Status = DraftRoomStatus.InProgress;
        var page = RenderSeatList(view);

        page.FindAll("button").Count(button => button.TextContent.Trim() == "Claim").Should().Be(1);
        FindButton(page, "Claim").Click();

        _apiService.Verify(service => service.ClaimSeatAsync("ABC234", eligible.Id, It.IsAny<CancellationToken>()), Times.Once);
        _apiService.Verify(service => service.ClaimSeatAsync("ABC234", ineligible.Id, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void DraftSeatList_WhenManagerAssigns_ShouldOnlyOfferServerApprovedParticipants()
    {
        var participantId = Guid.NewGuid();
        var seat = new DraftRoomViewSeatDto
        {
            Id = Guid.NewGuid(),
            Index = 1,
            CanAssign = true,
            AssignableParticipantIds = [participantId],
        };
        _apiService.Setup(service => service.AssignSeatAsync("ABC234", seat.Id, participantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Success));
        var participants = new DraftRoomViewParticipantDto[]
        {
            new DraftRoomViewParticipantDto { Id = participantId, DisplayName = "Replacement" },
            new DraftRoomViewParticipantDto { Id = Guid.NewGuid(), DisplayName = "Not eligible" },
        };
        var view = CreateRoomView(DraftRoomRole.Host, false, false, [seat], participants);
        view.Status = DraftRoomStatus.InProgress;
        var page = RenderSeatList(view);

        page.FindAll("option").Should().HaveCount(2);
        page.Find("select").Change(participantId.ToString());

        _apiService.Verify(service => service.AssignSeatAsync("ABC234", seat.Id, participantId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void DraftSeatList_WhenFreeSeatCanBeClaimed_ShouldShowAndExecuteClaim()
    {
        var freeSeat = new DraftRoomViewSeatDto
        {
            Id = Guid.NewGuid(),
            Index = 2,
        };
        var page = RenderSeatList(CreateRoomView(DraftRoomRole.Member, false, true, [freeSeat]));

        page.Markup.Should().Contain("Seat 2");
        page.Markup.Should().Contain("Free");
        FindButton(page, "Claim").Click();

        _apiService.Verify(
            service => service.ClaimSeatAsync("ABC234", freeSeat.Id, It.IsAny<CancellationToken>()),
            Times.Once);
        _session.Verify(
            session => session.ExecuteAsync(
                It.IsAny<Func<Task<DraftRoomApiCommandResult>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void DraftSeatList_WhenMemberOwnsSeat_ShouldOfferLeaveAndUnclaimThroughSession()
    {
        var ownSeat = new DraftRoomViewSeatDto
        {
            Id = Guid.NewGuid(),
            Index = 1,
            ParticipantId = CurrentParticipantId,
        };
        var ownParticipant = new DraftRoomViewParticipantDto
        {
            Id = CurrentParticipantId,
            DisplayName = "Current player",
            Role = DraftRoomRole.Member,
            SeatId = ownSeat.Id,
        };
        var page = RenderSeatList(
            CreateRoomView(DraftRoomRole.Member, false, true, [ownSeat], [ownParticipant]));

        page.Markup.Should().Contain("Current player");
        FindButton(page, "Leave seat").Click();

        _apiService.Verify(
            service => service.UnclaimSeatAsync("ABC234", ownSeat.Id, It.IsAny<CancellationToken>()),
            Times.Once);
        _session.Verify(
            session => session.ExecuteAsync(
                It.IsAny<Func<Task<DraftRoomApiCommandResult>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        page.Markup.Should().NotContain(">Unclaim<");
    }

    [Fact]
    public void DraftSeatList_WhenManagerViewsOccupiedSeat_ShouldOfferUnclaimAndExplainRemovalRestriction()
    {
        var occupantId = Guid.NewGuid();
        var occupiedSeat = new DraftRoomViewSeatDto
        {
            Id = Guid.NewGuid(),
            Index = 2,
            ParticipantId = occupantId,
        };
        var occupant = new DraftRoomViewParticipantDto
        {
            Id = occupantId,
            DisplayName = "Another player",
            Role = DraftRoomRole.Member,
            SeatId = occupiedSeat.Id,
        };
        var page = RenderSeatList(
            CreateRoomView(DraftRoomRole.CoHost, true, false, CreateSeats(3, occupiedSeat), [occupant]));

        page.Markup.Should().Contain("Another player");
        FindButton(page, "Unclaim").Should().NotBeNull();
        var occupiedSeatCard = page.FindAll("article")
            .Single(article => article.TextContent.Contains("Another player", StringComparison.Ordinal));
        var removeSeatButton = occupiedSeatCard.QuerySelectorAll("button")
            .Single(button => button.TextContent.Trim() == "Remove seat");
        removeSeatButton.GetAttribute("disabled").Should().NotBeNull();
        removeSeatButton.GetAttribute("aria-describedby")
            .Should()
            .Be($"draft-room-seat-{occupiedSeat.Id:N}-remove-reason");
        page.Find($"#draft-room-seat-{occupiedSeat.Id:N}-remove-reason")
            .TextContent.Should().Be("Unclaim this seat before removing it.");
        page.Markup.Should().Contain("Unclaim this seat before removing it.");
        FindButton(page, "Unclaim").Click();

        _apiService.Verify(
            service => service.UnclaimSeatAsync("ABC234", occupiedSeat.Id, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void DraftSeatList_WhenSeatCountIsAtMinimum_ShouldDisableRemovalWithReason()
    {
        var room = CreateRoomView(DraftRoomRole.Host, true, false, CreateSeats(2));

        var page = RenderSeatList(room);

        page.FindAll("button")
            .Where(button => button.TextContent.Trim() == "Remove seat")
            .Should()
            .OnlyContain(button => button.HasAttribute("disabled"));
        page.Markup.Should().Contain("Keep at least 2 seats in the room.");
    }

    [Fact]
    public void DraftSeatList_WhenSeatCountIsAtMaximum_ShouldDisableAddingWithReason()
    {
        var page = RenderSeatList(CreateRoomView(DraftRoomRole.Host, true, false, CreateSeats(8)));

        var addSeatButton = FindButton(page, "Add seat");
        addSeatButton.GetAttribute("disabled").Should().NotBeNull();
        addSeatButton.GetAttribute("aria-describedby").Should().Be("draft-room-max-seats-reason");
        page.Markup.Should().Contain("The room already has the maximum of 8 seats.");
        _apiService.Verify(
            service => service.AddSeatAsync("ABC234", It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void DraftSeatList_WhenManagerAddsSeat_ShouldExecuteAddThroughSession()
    {
        var page = RenderSeatList(CreateRoomView(DraftRoomRole.Host, true, false, CreateSeats(2)));

        FindButton(page, "Add seat").Click();

        _apiService.Verify(
            service => service.AddSeatAsync("ABC234", It.IsAny<CancellationToken>()),
            Times.Once);
        _session.Verify(
            session => session.ExecuteAsync(
                It.IsAny<Func<Task<DraftRoomApiCommandResult>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void DraftSeatList_WhenManagerRemovesFreeSeat_ShouldExecuteRemoveThroughSession()
    {
        var room = CreateRoomView(DraftRoomRole.Host, true, false, CreateSeats(3));
        var page = RenderSeatList(room);
        var seat = room.Seats[2];
        var seatCard = page.FindAll("article")
            .Single(article => article.QuerySelector("h3")?.TextContent == "Seat 3");

        seatCard.QuerySelectorAll("button")
            .Single(button => button.TextContent.Trim() == "Remove seat")
            .Click();

        _apiService.Verify(
            service => service.RemoveSeatAsync("ABC234", seat.Id, It.IsAny<CancellationToken>()),
            Times.Once);
        _session.Verify(
            session => session.ExecuteAsync(
                It.IsAny<Func<Task<DraftRoomApiCommandResult>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static AngleSharp.Dom.IElement FindButton(
        IRenderedComponent<DraftSeatList> page,
        string label) =>
        page.FindAll("button").Single(button => button.TextContent.Trim() == label);

    private static DraftRoomViewDto CreateRoomView(
        DraftRoomRole role,
        bool canManageSeats,
        bool canClaimSeat,
        IReadOnlyList<DraftRoomViewSeatDto> seats,
        IReadOnlyList<DraftRoomViewParticipantDto>? participants = null) => new()
        {
            Code = "ABC234",
            Name = "Test room",
            Seats = seats,
            Participants = participants ?? Array.Empty<DraftRoomViewParticipantDto>(),
            Me = new DraftRoomViewMeDto
            {
                ParticipantId = CurrentParticipantId,
                Role = role,
                CanManageSeats = canManageSeats,
                CanClaimSeat = canClaimSeat,
            },
        };

    private static DraftRoomViewSeatDto[] CreateSeats(
        int count,
        DraftRoomViewSeatDto? occupiedSeat = null)
    {
        var seats = Enumerable.Range(1, count)
            .Select(index => new DraftRoomViewSeatDto
            {
                Id = Guid.NewGuid(),
                Index = index,
            })
            .ToArray();

        if (occupiedSeat is not null)
        {
            seats[occupiedSeat.Index - 1] = occupiedSeat;
        }

        return seats;
    }

    private IRenderedComponent<DraftSeatList> RenderSeatList(DraftRoomViewDto room)
    {
        _session.SetupGet(session => session.View).Returns(room);

        var host = Render<CascadingValue<IDraftRoomSession>>(parameters => parameters
            .Add(parameter => parameter.Value, _session.Object)
            .AddChildContent<DraftSeatList>());

        return host.FindComponent<DraftSeatList>();
    }
}
