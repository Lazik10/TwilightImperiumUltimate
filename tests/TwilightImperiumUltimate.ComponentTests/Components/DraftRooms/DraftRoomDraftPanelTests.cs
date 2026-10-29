using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Components.DraftRooms;
using TwilightImperiumUltimate.Web.Components.DraftRooms.FactionPick;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.ComponentTests.Components.DraftRooms;

public sealed class DraftRoomDraftPanelTests : BunitContext
{
    private readonly Mock<IDraftRoomSession> _session = new();
    private readonly Mock<IDraftRoomApiService> _api = new();

    public DraftRoomDraftPanelTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(Mock.Of<IPathProvider>());
        Services.AddSingleton(Mock.Of<ITwilightImperiumApiHttpClient>());
        Services.AddSingleton(Mock.Of<ILocalStorageService>());
        Services.AddSingleton(Mock.Of<IMapper>());
        Services.AddSingleton(_api.Object);
        Services.AddSingleton<IDraftTypeUiRegistry, DraftTypeUiRegistry>();
        _session.Setup(session => session.ExecuteAsync(
                It.IsAny<Func<Task<DraftRoomApiCommandResult>>>(),
                It.IsAny<CancellationToken>()))
            .Returns((Func<Task<DraftRoomApiCommandResult>> action, CancellationToken _) => action());
    }

    [Theory]
    [InlineData(DraftRoomStatus.Lobby)]
    [InlineData(DraftRoomStatus.InProgress)]
    public void DraftPanel_WhenNotEditable_ShouldShowDisabledRulesWithoutSave(DraftRoomStatus status)
    {
        var view = CreateView();
        view.Status = status;
        var panel = RenderPanel(view);

        panel.Find("fieldset").HasAttribute("disabled").Should().BeTrue();
        panel.FindAll("button").Should().NotContain(button => button.TextContent.Contains("Save draft settings", StringComparison.Ordinal));
        panel.Markup.Should().Contain(status == DraftRoomStatus.Lobby ? "Only the host" : "locked after start");
    }

    [Fact]
    public void DraftPanel_WhenStartBlocked_ShouldExplainEveryBlockerAndDisableStart()
    {
        var view = CreateView(true);
        view.StartBlockers = ["SeatsNotFull", "NotEnoughSeats", "SettingsInvalid"];
        var panel = RenderPanel(view);

        panel.FindAll("#draft-start-blockers li").Should().HaveCount(3);
        var start = panel.FindAll("button").Single(button => button.TextContent.Trim() == "Start draft");
        start.HasAttribute("disabled").Should().BeTrue();
        start.GetAttribute("aria-describedby").Should().Be("draft-start-blockers");
    }

    [Fact]
    public void DraftPanel_WhenStartFails_ShouldShowErrorAndAllowRetry()
    {
        var view = CreateView(true);
        view.CanStart = true;
        _api.Setup(api => api.StartDraftAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Conflict));
        var panel = RenderPanel(view);

        panel.FindAll("button").Single(button => button.TextContent.Trim() == "Start draft").Click();

        panel.Find("[role=alert]").TextContent.Should().Contain("rejected");
        _api.Verify(api => api.StartDraftAsync("ABC234", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void DraftPanel_WhenResetClicked_ShouldRequireConfirmation()
    {
        var view = CreateView(true);
        view.Status = DraftRoomStatus.InProgress;
        view.Me.CanChangeSettings = false;
        view.CanReset = true;
        _api.Setup(api => api.ResetDraftAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Success));
        var panel = RenderPanel(view);

        panel.FindAll("button").Single(button => button.TextContent.Trim() == "Reset draft").Click();
        _api.Verify(api => api.ResetDraftAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        panel.FindAll("dialog button").Single(button => button.TextContent.Trim() == "Reset draft").Click();

        _api.Verify(api => api.ResetDraftAsync("ABC234", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void FactionSettings_WhenSaved_ShouldCopyParametersAndRetainEditsAcrossUnrelatedRenders()
    {
        var settings = new FactionPickSettingsDto();
        DraftSettingsDto? saved = null;
        var panel = Render<FactionPickSettingsEditor>(parameters => parameters
            .Add(component => component.Settings, settings)
            .Add(component => component.CanEdit, true)
            .Add(component => component.OnSave, value => saved = value));
        panel.FindAll("input[type=checkbox]")[0].Change(false);
        panel.Render(parameters => parameters
            .Add(component => component.Settings, settings)
            .Add(component => component.CanEdit, true));
        panel.FindAll("button").Single(button => button.TextContent.Trim() == "Save draft settings").Click();

        settings.GameVersions.Should().Contain(GameVersion.BaseGame);
        saved.Should().BeOfType<FactionPickSettingsDto>().Which.GameVersions.Should().NotContain(GameVersion.BaseGame);
        panel.Find("option[value=HostDefined]").HasAttribute("disabled").Should().BeFalse();
        panel.Markup.Should().Contain("No bans");
        panel.Markup.Should().NotContain("TwilightImperiumUltimate.Contracts.Enums.FactionPickBansMode");
    }

    [Fact]
    public void FactionSettings_WhenOrderMovedWithButtons_ShouldSaveIndependentConfiguredOrder()
    {
        var seats = new[] { new DraftRoomViewSeatDto { Id = Guid.NewGuid(), Index = 1 }, new DraftRoomViewSeatDto { Id = Guid.NewGuid(), Index = 2 } };
        var settings = new FactionPickSettingsDto { PickOrderMode = FactionPickOrderMode.HostDefined, SeatOrder = seats.Select(seat => seat.Id).ToArray() };
        DraftSettingsDto? saved = null;
        var editor = Render<FactionPickSettingsEditor>(parameters => parameters
            .Add(component => component.Settings, settings)
            .Add(component => component.Seats, seats)
            .Add(component => component.CanEdit, true)
            .Add(component => component.OnSave, value => saved = value));

        editor.Find("button[aria-label='Move up: Seat 2']").Click();
        editor.FindAll("button").Single(button => button.TextContent.Trim() == "Save draft settings").Click();

        ((FactionPickSettingsDto)saved!).SeatOrder.Should().Equal(seats.Reverse().Select(seat => seat.Id));
        settings.SeatOrder.Should().Equal(seats.Select(seat => seat.Id));
        editor.Find("button[aria-label='Move up: Seat 2']").HasAttribute("disabled").Should().BeTrue();
    }

    private static DraftRoomViewDto CreateView(bool manager = false) => new()
    {
        Code = "ABC234",
        Settings = new FactionPickSettingsDto(),
        DraftType = DraftTypeId.FactionPick,
        Me = new DraftRoomViewMeDto
        {
            Role = manager ? DraftRoomRole.Host : DraftRoomRole.Member,
            CanChangeSettings = manager,
        },
    };

    private IRenderedComponent<DraftRoomDraftPanel> RenderPanel(DraftRoomViewDto view)
    {
        _session.SetupGet(session => session.View).Returns(view);
        var root = Render<CascadingValue<IDraftRoomSession>>(parameters => parameters
            .Add(component => component.Value, _session.Object)
            .AddChildContent<DraftRoomDraftPanel>());
        return root.FindComponent<DraftRoomDraftPanel>();
    }
}
