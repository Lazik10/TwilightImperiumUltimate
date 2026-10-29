using System.Text.Json;
using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Components.DraftRooms;
using TwilightImperiumUltimate.Web.Components.Shared.Controls;
using TwilightImperiumUltimate.Web.Pages.Tools;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.ComponentTests.Components.DraftRooms;

public sealed class DraftResultViewTests : BunitContext
{
    private readonly Mock<IDraftRoomApiService> _api = new(MockBehavior.Strict);
    private readonly Mock<IDraftRoomSession> _session = new();

    public DraftResultViewTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(_api.Object);
        Services.AddSingleton<IDraftTypeUiRegistry, DraftTypeUiRegistry>();
        Services.AddSingleton(Mock.Of<IPathProvider>());
        Services.AddSingleton(Mock.Of<ITwilightImperiumApiHttpClient>());
        Services.AddSingleton(Mock.Of<ILocalStorageService>());
        Services.AddSingleton(Mock.Of<IMapper>());
        _session.Setup(session => session.ExecuteAsync(It.IsAny<Func<Task<DraftRoomApiCommandResult>>>(), It.IsAny<CancellationToken>()))
            .Returns((Func<Task<DraftRoomApiCommandResult>> command, CancellationToken _) => command());
    }

    [Fact]
    public void ResultView_WhenPublic_ShouldRenderHumanFactionNamesAttributionShareAndBothCopyActions()
    {
        var result = CreateResult();

        var view = Render<DraftRoomResultView>(parameters => parameters.Add(component => component.Result, result));

        view.Find("h2").TextContent.Should().Be("Draft results");
        view.Markup.Should().Contain("Original guest").And.Contain("Arborec").And.Contain("decided by Host");
        view.Markup.Should().NotContain("TwilightImperiumUltimate.Contracts.Enums");
        var copy = view.FindComponents<CopyButton>();
        copy.Should().HaveCount(2);
        copy[0].Instance.TextToCopy.Should().Be(result.Text);
        copy[1].Instance.TextToCopy.Should().Be($"http://localhost/tools/draft-rooms/results/{result.ShareToken}");
        view.Find("a[target=_blank]").GetAttribute("rel").Should().Contain("noopener");
        view.FindAll("button").Should().NotContain(button => button.TextContent == "Delete results");
    }

    [Theory]
    [InlineData(DraftRoomApiResultKind.NotFound, "unavailable or have been deleted")]
    [InlineData(DraftRoomApiResultKind.Failed, "could not be loaded")]
    public void PublicPage_WhenResultCannotBeRead_ShouldShowNotFoundOrRetryableError(DraftRoomApiResultKind kind, string message)
    {
        _api.Setup(service => service.GetResultAsync("token", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftResultDto>(kind, null));

        var page = Render<DraftRoomResult>(parameters => parameters.Add(component => component.Token, "token"));

        page.Markup.Should().Contain(message);
        page.FindAll("[role=alert]").Should().HaveCount(kind == DraftRoomApiResultKind.Failed ? 1 : 0);
        page.FindAll("button").Should().HaveCount(kind == DraftRoomApiResultKind.Failed ? 1 : 0);
    }

    [Fact]
    public void PublicPage_WhenLoaded_ShouldShowSnapshotWithoutRoomSession()
    {
        _api.Setup(service => service.GetResultAsync("token", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftResultDto>(DraftRoomApiResultKind.Success, CreateResult()));

        var page = Render<DraftRoomResult>(parameters => parameters.Add(component => component.Token, "token"));

        page.FindComponent<DraftRoomResultView>().Should().NotBeNull();
        page.Find("h1").TextContent.Should().Contain("Draft results");
    }

    [Fact]
    public async Task PublicPage_WhenRouteChangesBeforeResponse_ShouldDiscardStaleResult()
    {
        var pending = new TaskCompletionSource<DraftRoomApiResult<DraftResultDto>>(TaskCreationOptions.RunContinuationsAsynchronously);
        _api.Setup(service => service.GetResultAsync("first", It.IsAny<CancellationToken>())).Returns(pending.Task);
        _api.Setup(service => service.GetResultAsync("second", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftResultDto>(DraftRoomApiResultKind.NotFound, null));
        var page = Render<DraftRoomResult>(parameters => parameters.Add(component => component.Token, "first"));
        page.Find("[role=status]").TextContent.Should().Contain("Loading");

        page.Render(parameters => parameters.Add(component => component.Token, "second"));
        await page.InvokeAsync(() => pending.SetResult(new DraftRoomApiResult<DraftResultDto>(DraftRoomApiResultKind.Success, CreateResult())));

        page.WaitForAssertion(() => page.Markup.Should().Contain("unavailable or have been deleted"));
        page.FindComponents<DraftRoomResultView>().Should().BeEmpty();
    }

    [Theory]
    [InlineData(DraftRoomRole.Host, true)]
    [InlineData(DraftRoomRole.CoHost, true)]
    [InlineData(DraftRoomRole.Member, false)]
    public void FinishedRoom_WhenViewed_ShouldUseImmutableResultAndExposeDeletionOnlyToManagers(DraftRoomRole role, bool canDelete)
    {
        var room = CreateRoom(role);
        _session.SetupGet(session => session.View).Returns(room);
        _api.Setup(service => service.GetRoomResultAsync(room.Code, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftResultDto>(DraftRoomApiResultKind.Success, CreateResult()));

        var root = Render<CascadingValue<IDraftRoomSession>>(parameters => parameters
            .Add(component => component.Value, _session.Object).AddChildContent<DraftRoomDraftPanel>());

        root.FindComponent<DraftRoomResultView>().Should().NotBeNull();
        root.FindAll("fieldset").Should().BeEmpty();
        root.FindAll("button").Count(button => button.TextContent.Trim() == "Delete results").Should().Be(canDelete ? 1 : 0);
    }

    [Fact]
    public void Delete_WhenConfirmed_ShouldRemoveSnapshotAndRestoreFocusToRemainingStatus()
    {
        var room = CreateRoom(DraftRoomRole.Host);
        var result = CreateResult();
        _api.Setup(service => service.GetRoomResultAsync(room.Code, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftResultDto>(DraftRoomApiResultKind.Success, result));
        _api.Setup(service => service.DeleteResultAsync(result.Id, room.Code, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Success));
        var root = Render<CascadingValue<IDraftRoomSession>>(parameters => parameters
            .Add(component => component.Value, _session.Object)
            .AddChildContent<DraftRoomResultPanel>(child => child.Add(component => component.Room, room)));

        root.FindAll("button").Single(button => button.TextContent.Trim() == "Delete results").Click();
        _api.Verify(service => service.DeleteResultAsync(It.IsAny<Guid>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Never);
        root.FindAll("dialog button").Single(button => button.TextContent.Trim() == "Delete results").Click();

        root.WaitForAssertion(() => root.FindComponents<DraftRoomResultView>().Should().BeEmpty());
        root.Find("[role=status]").TextContent.Should().Contain("deleted");
        JSInterop.Invocations.Should().Contain(invocation => invocation.Identifier == "Blazor._internal.domWrapper.focus");
    }

    [Fact]
    public void ResultPanel_WhenRoomVersionChangesAfterOtherManagerDeletion_ShouldReloadAndRemoveStaleShareLink()
    {
        var room = CreateRoom(DraftRoomRole.Member);
        _api.SetupSequence(service => service.GetRoomResultAsync(room.Code, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftResultDto>(DraftRoomApiResultKind.Success, CreateResult()))
            .ReturnsAsync(new DraftRoomApiResult<DraftResultDto>(DraftRoomApiResultKind.NotFound, null));
        var panel = Render<DraftRoomResultPanel>(parameters => parameters.Add(component => component.Room, room));
        panel.Find("a").Should().NotBeNull();

        room.Version++;
        panel.Render(parameters => parameters.Add(component => component.Room, room));

        panel.FindComponents<DraftRoomResultView>().Should().BeEmpty();
        panel.Find("[role=status]").TextContent.Should().Contain("deleted");
    }

    private static DraftRoomViewDto CreateRoom(DraftRoomRole role) => new()
    {
        Code = "ABC234",
        Status = DraftRoomStatus.Finished,
        DraftType = DraftTypeId.FactionPick,
        Me = new DraftRoomViewMeDto { Role = role },
    };

    private static DraftResultDto CreateResult() => new(
        Guid.NewGuid(),
        "aBcDeF0123456789_-abcdE",
        DraftTypeId.FactionPick,
        "Finished Friday",
        DateTime.UnixEpoch,
        1,
        JsonSerializer.SerializeToElement(new FactionPickResultDto
        {
            GameVersions = [GameVersion.BaseGame],
            PicksPerPlayer = 1,
            SeatOrder = [1],
            Seats = [new FactionPickResultSeatDto(1, "Original guest", [new FactionPickResultPickDto(FactionName.TheArborec, 0, "Host")])],
        }),
        "Seat 1 - Original guest: TheArborec (decided by Host)");
}
