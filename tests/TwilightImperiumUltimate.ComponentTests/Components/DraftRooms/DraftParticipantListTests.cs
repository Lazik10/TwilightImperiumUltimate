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

public sealed class DraftParticipantListTests : BunitContext
{
    private static readonly Guid HostParticipantId = Guid.Parse("f0c901ef-fb0f-4c9e-9b93-c9d3d7315a33");
    private static readonly Guid MemberParticipantId = Guid.Parse("d8f0f1b4-4c19-49dc-bc60-fc8a95705b83");
    private static readonly Guid CoHostParticipantId = Guid.Parse("b0f66b28-c387-4eb1-8d38-9e6227b0df80");

    private readonly Mock<IDraftRoomSession> _session = new();
    private readonly Mock<IDraftRoomApiService> _apiService = new();

    public DraftParticipantListTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var dialogModule = JSInterop.SetupModule("./Components/Shared/Controls/ResponsiveConfirmDialog.razor.js");
        dialogModule.SetupVoid("showModal");
        dialogModule.SetupVoid("closeModal");
        _session
            .Setup(session => session.ExecuteAsync(
                It.IsAny<Func<Task<DraftRoomApiCommandResult>>>(),
                It.IsAny<CancellationToken>()))
            .Returns((Func<Task<DraftRoomApiCommandResult>> command, CancellationToken _) => command());
        _apiService
            .Setup(service => service.KickAsync("ABC234", It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Success));
        _apiService
            .Setup(service => service.SetRoleAsync(
                "ABC234",
                It.IsAny<Guid>(),
                It.IsAny<DraftRoomRole>(),
                It.IsAny<CancellationToken>()))
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
    public void DraftParticipantList_WhenParticipantsAreGuestsAndSeated_ShouldShowIdentityRoleAndSeat()
    {
        var hostSeatId = Guid.NewGuid();
        var host = CreateParticipant(HostParticipantId, "Room host", DraftRoomRole.Host, seatId: hostSeatId);
        var guest = CreateParticipant(MemberParticipantId, "Guest player", DraftRoomRole.Member, isGuest: true);
        var page = RenderParticipantList(
            CreateRoomView(DraftRoomRole.Host, HostParticipantId, true, true, host, guest));

        page.Markup.Should().Contain("Room host");
        page.Markup.Should().Contain("Host");
        page.Markup.Should().Contain("Guest player");
        page.Markup.Should().Contain("Guest");
        page.Markup.Should().Contain("Seat 1");
        page.Markup.Should().Contain("No seat");
    }

    [Fact]
    public void DraftParticipantList_WhenParticipantIsOnline_ShouldShowPresenceText()
    {
        var onlineParticipant = CreateParticipant(
            MemberParticipantId,
            "Online player",
            DraftRoomRole.Member,
            isOnline: true);
        var page = RenderParticipantList(
            CreateRoomView(
                DraftRoomRole.Host,
                HostParticipantId,
                canKick: false,
                canChangeRoles: false,
                onlineParticipant));

        var card = FindParticipantCard(page, "Online player");
        card.TextContent.Should().Contain("Online");
        card.QuerySelectorAll(".draft-presence-indicator__dot")
            .Should().ContainSingle()
            .Which.GetAttribute("class")
            .Should().Contain("draft-presence-indicator__dot--online");
    }

    [Fact]
    public void DraftParticipantList_WhenKickIsCancelled_ShouldNotExecuteKick()
    {
        var target = CreateParticipant(MemberParticipantId, "Member player", DraftRoomRole.Member);
        var page = RenderParticipantList(
            CreateRoomView(
                DraftRoomRole.Host,
                HostParticipantId,
                canKick: true,
                canChangeRoles: false,
                CreateParticipant(HostParticipantId, "Room host", DraftRoomRole.Host),
                target));

        FindButton(FindParticipantCard(page, "Member player"), "Kick").Click();
        var dialog = page.Find("[role='alertdialog']");
        dialog.TextContent.Should().Contain("Kick Member player from this draft room?");
        dialog.QuerySelectorAll("button")[0].Click();

        page.FindAll("[role='alertdialog']").Should().BeEmpty();
        _apiService.Verify(
            service => service.KickAsync("ABC234", target.Id, It.IsAny<CancellationToken>()),
            Times.Never);
        _session.Verify(
            session => session.ExecuteAsync(
                It.IsAny<Func<Task<DraftRoomApiCommandResult>>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void DraftParticipantList_WhenKickIsConfirmed_ShouldKickParticipantThroughSession()
    {
        var target = CreateParticipant(MemberParticipantId, "Member player", DraftRoomRole.Member);
        var page = RenderParticipantList(
            CreateRoomView(
                DraftRoomRole.Host,
                HostParticipantId,
                canKick: true,
                canChangeRoles: false,
                CreateParticipant(HostParticipantId, "Room host", DraftRoomRole.Host),
                target));

        FindButton(FindParticipantCard(page, "Member player"), "Kick").Click();
        page.Find("[role='alertdialog']").QuerySelectorAll("button")[1].Click();

        _apiService.Verify(
            service => service.KickAsync("ABC234", target.Id, It.IsAny<CancellationToken>()),
            Times.Once);
        _session.Verify(
            session => session.ExecuteAsync(
                It.IsAny<Func<Task<DraftRoomApiCommandResult>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void DraftParticipantList_WhenHostPromotesMember_ShouldSetCoHostRole()
    {
        var target = CreateParticipant(MemberParticipantId, "Member player", DraftRoomRole.Member);
        var page = RenderParticipantList(
            CreateRoomView(
                DraftRoomRole.Host,
                HostParticipantId,
                canKick: true,
                canChangeRoles: true,
                CreateParticipant(HostParticipantId, "Room host", DraftRoomRole.Host),
                target));

        FindButton(FindParticipantCard(page, "Member player"), "Make co-host").Click();

        _apiService.Verify(
            service => service.SetRoleAsync(
                "ABC234",
                target.Id,
                DraftRoomRole.CoHost,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void DraftParticipantList_WhenHostRemovesCoHostRole_ShouldSetMemberRole()
    {
        var target = CreateParticipant(CoHostParticipantId, "Co-host player", DraftRoomRole.CoHost);
        var page = RenderParticipantList(
            CreateRoomView(
                DraftRoomRole.Host,
                HostParticipantId,
                canKick: true,
                canChangeRoles: true,
                CreateParticipant(HostParticipantId, "Room host", DraftRoomRole.Host),
                target));

        FindButton(FindParticipantCard(page, "Co-host player"), "Remove co-host").Click();

        _apiService.Verify(
            service => service.SetRoleAsync(
                "ABC234",
                target.Id,
                DraftRoomRole.Member,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void DraftParticipantList_WhenViewerIsCoHost_ShouldNotOfferRoleChanges()
    {
        var member = CreateParticipant(MemberParticipantId, "Member player", DraftRoomRole.Member);
        var coHost = CreateParticipant(CoHostParticipantId, "Co-host player", DraftRoomRole.CoHost);
        var page = RenderParticipantList(
            CreateRoomView(
                DraftRoomRole.CoHost,
                CoHostParticipantId,
                canKick: true,
                canChangeRoles: false,
                CreateParticipant(HostParticipantId, "Room host", DraftRoomRole.Host),
                member,
                coHost));

        page.Markup.Should().NotContain("Make co-host");
        page.Markup.Should().NotContain("Remove co-host");
    }

    [Fact]
    public void DraftParticipantList_WhenParticipantIsHost_ShouldNotOfferKick()
    {
        var host = CreateParticipant(HostParticipantId, "Room host", DraftRoomRole.Host);
        var member = CreateParticipant(MemberParticipantId, "Member player", DraftRoomRole.Member);
        var page = RenderParticipantList(
            CreateRoomView(
                DraftRoomRole.CoHost,
                CoHostParticipantId,
                canKick: true,
                canChangeRoles: false,
                host,
                CreateParticipant(CoHostParticipantId, "Current co-host", DraftRoomRole.CoHost),
                member));

        var hostCard = FindParticipantCard(page, "Room host");

        hostCard.QuerySelectorAll("button")
            .Should()
            .NotContain(button => button.TextContent.Trim() == "Kick");
        FindButton(FindParticipantCard(page, "Member player"), "Kick").Should().NotBeNull();
    }

    [Fact]
    public void DraftParticipantList_WhenManagerUnclaimsParticipantSeat_ShouldExecuteUnclaimThroughSession()
    {
        var seatId = Guid.NewGuid();
        var target = CreateParticipant(MemberParticipantId, "Member player", DraftRoomRole.Member, seatId: seatId);
        var page = RenderParticipantList(
            CreateRoomView(
                DraftRoomRole.CoHost,
                CoHostParticipantId,
                canKick: true,
                canChangeRoles: false,
                CreateParticipant(HostParticipantId, "Room host", DraftRoomRole.Host),
                CreateParticipant(CoHostParticipantId, "Current co-host", DraftRoomRole.CoHost),
                target));

        FindButton(FindParticipantCard(page, "Member player"), "Unclaim seat").Click();

        _apiService.Verify(
            service => service.UnclaimSeatAsync("ABC234", seatId, It.IsAny<CancellationToken>()),
            Times.Once);
        _session.Verify(
            session => session.ExecuteAsync(
                It.IsAny<Func<Task<DraftRoomApiCommandResult>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static AngleSharp.Dom.IElement FindParticipantCard(
        IRenderedComponent<DraftParticipantList> page,
        string name) =>
        page.FindAll("article").Single(
            article => article.QuerySelector("h3")?.TextContent == name);

    private static AngleSharp.Dom.IElement FindButton(AngleSharp.Dom.IElement container, string label) =>
        container.QuerySelectorAll("button").Single(button => button.TextContent.Trim() == label);

    private static DraftRoomViewParticipantDto CreateParticipant(
        Guid id,
        string displayName,
        DraftRoomRole role,
        bool isGuest = false,
        Guid? seatId = null,
        bool isOnline = false) => new()
        {
            Id = id,
            DisplayName = displayName,
            Role = role,
            IsGuest = isGuest,
            SeatId = seatId,
            IsOnline = isOnline,
        };

    private static DraftRoomViewDto CreateRoomView(
        DraftRoomRole role,
        Guid currentParticipantId,
        bool canKick,
        bool canChangeRoles,
        params DraftRoomViewParticipantDto[] participants)
    {
        var seats = new List<DraftRoomViewSeatDto>();
        foreach (var participant in participants)
        {
            if (participant.SeatId is not Guid seatId)
            {
                continue;
            }

            seats.Add(new DraftRoomViewSeatDto
            {
                Id = seatId,
                Index = seats.Count + 1,
                ParticipantId = participant.Id,
            });
        }

        return new DraftRoomViewDto
        {
            Code = "ABC234",
            Name = "Test room",
            Participants = participants,
            Seats = seats,
            Me = new DraftRoomViewMeDto
            {
                ParticipantId = currentParticipantId,
                Role = role,
                CanKick = canKick,
                CanChangeRoles = canChangeRoles,
            },
        };
    }

    private IRenderedComponent<DraftParticipantList> RenderParticipantList(DraftRoomViewDto room)
    {
        _session.SetupGet(session => session.View).Returns(room);

        var host = Render<CascadingValue<IDraftRoomSession>>(parameters => parameters
            .Add(parameter => parameter.Value, _session.Object)
            .AddChildContent<DraftParticipantList>());

        return host.FindComponent<DraftParticipantList>();
    }
}
