using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Components.DraftRooms;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.ComponentTests.Components.DraftRooms;

public sealed class DraftRoomHeaderTests : BunitContext
{
    private readonly Mock<IDraftRoomSession> _session = new();
    private readonly Mock<IDraftRoomApiService> _apiService = new();

    public DraftRoomHeaderTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var dialogModule = JSInterop.SetupModule("./Components/Shared/Controls/ResponsiveConfirmDialog.razor.js");
        dialogModule.SetupVoid("showModal");
        dialogModule.SetupVoid("closeModal");
        _session.SetupGet(session => session.IsLoading).Returns(false);
        _session
            .Setup(session => session.ExecuteAsync(
                It.IsAny<Func<Task<DraftRoomApiCommandResult>>>(),
                It.IsAny<CancellationToken>()))
            .Returns((Func<Task<DraftRoomApiCommandResult>> command, CancellationToken _) => command());
        _session.Setup(session => session.RefreshAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _apiService
            .Setup(service => service.LeaveAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Success));
        _apiService
            .Setup(service => service.CloseAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Success));
        Services.AddSingleton(Mock.Of<IPathProvider>());
        Services.AddSingleton(Mock.Of<ITwilightImperiumApiHttpClient>());
        Services.AddSingleton(Mock.Of<ILocalStorageService>());
        Services.AddSingleton(Mock.Of<IMapper>());
        Services.AddSingleton(_apiService.Object);
    }

    [Fact]
    public void DraftRoomHeader_WhenParticipantIsMember_ShouldOfferLeaveButNotClose()
    {
        var page = RenderHeader(DraftRoomRole.Member, canClose: false);

        page.Markup.Should().Contain("Member");
        page.Markup.Should().Contain("Room code");
        page.Markup.Should().Contain("Room link");
        page.Find("a[href^='http']").GetAttribute("target").Should().Be("_blank");
        FindButton(page, "Leave room").Should().NotBeNull();
        page.Markup.Should().NotContain("Close room");
    }

    [Fact]
    public void DraftRoomHeader_WhenParticipantCanClose_ShouldOfferCloseButNotLeave()
    {
        var page = RenderHeader(DraftRoomRole.CoHost, canClose: true);

        page.Markup.Should().Contain("Co-host");
        FindButton(page, "Close room").Should().NotBeNull();
        page.Markup.Should().NotContain("Leave room");
    }

    [Fact]
    public void DraftRoomHeader_WhenLeaveIsCancelled_ShouldNotExecuteCommand()
    {
        var page = RenderHeader(DraftRoomRole.Member, canClose: false);

        FindButton(page, "Leave room").Click();
        var dialog = page.Find("[role='alertdialog']");
        dialog.QuerySelectorAll("button")[0].Click();

        page.FindAll("[role='alertdialog']").Should().BeEmpty();
        _apiService.Verify(
            service => service.LeaveAsync("ABC234", It.IsAny<CancellationToken>()),
            Times.Never);
        _session.Verify(
            session => session.ExecuteAsync(
                It.IsAny<Func<Task<DraftRoomApiCommandResult>>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void DraftRoomHeader_WhenLeaveIsCancelledWithEscape_ShouldNotExecuteCommand()
    {
        var page = RenderHeader(DraftRoomRole.Member, canClose: false);

        FindButton(page, "Leave room").Click();
        page.Find("[role='alertdialog']").TriggerEvent("onkeydown", new KeyboardEventArgs { Key = "Escape" });

        page.FindAll("[role='alertdialog']").Should().BeEmpty();
        _apiService.Verify(
            service => service.LeaveAsync("ABC234", It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void DraftRoomHeader_WhenLeaveIsConfirmed_ShouldLeaveAndNavigateToDraftRooms()
    {
        var page = RenderHeader(DraftRoomRole.Member, canClose: false);

        FindButton(page, "Leave room").Click();
        page.Find("[role='alertdialog']").QuerySelectorAll("button")[1].Click();

        _apiService.Verify(
            service => service.LeaveAsync("ABC234", It.IsAny<CancellationToken>()),
            Times.Once);
        Services.GetRequiredService<NavigationManager>().Uri.Should().EndWith("/tools/draft-rooms");
    }

    [Fact]
    public void DraftRoomHeader_WhenCloseIsConfirmed_ShouldExecuteCloseCommand()
    {
        var page = RenderHeader(DraftRoomRole.Host, canClose: true);

        FindButton(page, "Close room").Click();
        page.Find("[role='alertdialog']").QuerySelectorAll("button")[1].Click();

        _apiService.Verify(
            service => service.CloseAsync("ABC234", It.IsAny<CancellationToken>()),
            Times.Once);
        _session.Verify(
            session => session.ExecuteAsync(
                It.IsAny<Func<Task<DraftRoomApiCommandResult>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void DraftRoomHeader_WhenConnectionIsOffline_ShouldShowRefreshAndConnectionStatus()
    {
        var page = RenderHeader(DraftRoomRole.Member, canClose: false);

        page.Markup.Should().Contain("Disconnected from live updates");
        FindButton(page, "Refresh").Click();

        _session.Verify(session => session.RefreshAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void DraftRoomHeader_WhenConnectionIsConnected_ShouldHideRefreshButton()
    {
        var page = RenderHeader(
            DraftRoomRole.Member,
            canClose: false,
            DraftRoomConnectionState.Connected);

        page.Markup.Should().Contain("Connected to live updates");
        page.FindAll("button").Should().NotContain(button => button.TextContent.Trim() == "Refresh");
    }

    [Fact]
    public void DraftRoomHeader_WhenConnectionIsReconnecting_ShouldShowBannerWithoutRefresh()
    {
        var page = RenderHeader(
            DraftRoomRole.Member,
            canClose: false,
            DraftRoomConnectionState.Reconnecting);

        page.Find(".draft-room-connection-status--reconnecting").TextContent
            .Should().Contain("Reconnecting to live updates");
        page.FindAll("button").Should().NotContain(button => button.TextContent.Trim() == "Refresh");
    }

    private static AngleSharp.Dom.IElement FindButton(
        IRenderedComponent<DraftRoomHeader> page,
        string label) =>
        page.FindAll("button").Single(button => button.TextContent.Trim() == label);

    private static DraftRoomViewDto CreateRoomView(DraftRoomRole role, bool canClose) => new()
    {
        Code = "ABC234",
        Name = "Test room",
        Me = new DraftRoomViewMeDto
        {
            Role = role,
            CanClose = canClose,
        },
    };

    private IRenderedComponent<DraftRoomHeader> RenderHeader(
        DraftRoomRole role,
        bool canClose,
        DraftRoomConnectionState connectionState = DraftRoomConnectionState.Offline)
    {
        _session.SetupGet(session => session.View).Returns(CreateRoomView(role, canClose));
        _session.SetupGet(session => session.ConnectionState).Returns(connectionState);

        var host = Render<CascadingValue<IDraftRoomSession>>(parameters => parameters
            .Add(parameter => parameter.Value, _session.Object)
            .AddChildContent<DraftRoomHeader>());

        return host.FindComponent<DraftRoomHeader>();
    }
}
