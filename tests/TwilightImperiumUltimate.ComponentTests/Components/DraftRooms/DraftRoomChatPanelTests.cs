using System.Globalization;
using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Components.DraftRooms;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.ComponentTests.Components.DraftRooms;

public sealed class DraftRoomChatPanelTests : BunitContext
{
    private readonly Mock<IDraftRoomSession> _session = new();
    private BunitJSModuleInterop? _module;

    public DraftRoomChatPanelTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(Mock.Of<IPathProvider>());
        Services.AddSingleton(Mock.Of<ITwilightImperiumApiHttpClient>());
        Services.AddSingleton(Mock.Of<ILocalStorageService>());
        Services.AddSingleton(Mock.Of<IMapper>());
        _session
            .Setup(session => session.GetChatHistoryAsync(
                null,
                DraftRoomLimits.DefaultHistoryPageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<IReadOnlyList<ChatMessageDto>>(
                DraftRoomApiResultKind.Success,
                Array.Empty<ChatMessageDto>()));
    }

    [Fact]
    public void DraftRoomChatPanel_WhenHistoryLoads_ShouldRenderMessagesInPoliteLog()
    {
        var message = CreateMessage(3, "Hello from history");
        var systemMessage = CreateMessage(4, "Guest One joined");
        systemMessage.Kind = ChatMessageKind.Join;
        _session
            .Setup(session => session.GetChatHistoryAsync(
                null,
                DraftRoomLimits.DefaultHistoryPageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<IReadOnlyList<ChatMessageDto>>(
                DraftRoomApiResultKind.Success,
                [message, systemMessage]));

        var panel = RenderPanel();

        panel.Find("[role='log']").GetAttribute("aria-live").Should().Be("polite");
        panel.Find("[role='log']").GetAttribute("aria-relevant").Should().Be("additions");
        panel.Find("[role='log']").GetAttribute("tabindex").Should().Be("0");
        panel.Find("[role='log']").TextContent.Should().Contain("Hello from history");
        panel.Find("[role='log']").TextContent.Should().Contain("System message");
        panel.Find("#draft-room-chat-input").GetAttribute("maxlength")
            .Should().Be(DraftRoomLimits.MaxChatMessageLength.ToString(CultureInfo.InvariantCulture));
        panel.FindAll(".draft-room-chat-panel__empty").Should().BeEmpty();
    }

    [Fact]
    public void DraftRoomChatPanel_WhenHistoryIsPending_ShouldKeepLiveAnnouncementsOffUntilHistoryRenders()
    {
        var history = new TaskCompletionSource<DraftRoomApiResult<IReadOnlyList<ChatMessageDto>>>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        _session
            .Setup(session => session.GetChatHistoryAsync(
                null,
                DraftRoomLimits.DefaultHistoryPageSize,
                It.IsAny<CancellationToken>()))
            .Returns(history.Task);

        var panel = RenderPanel();

        panel.Find("[role='log']").GetAttribute("aria-live").Should().Be("off");
        history.SetResult(new DraftRoomApiResult<IReadOnlyList<ChatMessageDto>>(
            DraftRoomApiResultKind.Success,
            [CreateMessage(3, "History message")]));

        panel.WaitForAssertion(() =>
        {
            panel.Find("[role='log']").GetAttribute("aria-live").Should().Be("polite");
            panel.FindAll("[role='log'] li").Should().ContainSingle()
                .Which.TextContent.Should().Contain("History message");
        });
    }

    [Fact]
    public void DraftRoomChatPanel_WhenLiveMessageArrivesWhileScrolledUp_ShouldAppendWithoutAutoScrolling()
    {
        var initialMessage = CreateMessage(3, "Earlier message");
        _session
            .Setup(session => session.GetChatHistoryAsync(
                null,
                DraftRoomLimits.DefaultHistoryPageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<IReadOnlyList<ChatMessageDto>>(
                DraftRoomApiResultKind.Success,
                [initialMessage]));
        var panel = RenderPanel(isAtBottom: false);

        _session.Raise(session => session.ChatMessageReceived += null!, CreateMessage(4, "New live message"));

        panel.WaitForAssertion(() =>
        {
            panel.Find("[role='log']").TextContent.Should().Contain("New live message");
            this._module!.Invocations
                .Where(invocation => invocation.Identifier == "scrollToBottom")
                .Should()
                .ContainSingle();
        });
    }

    [Fact]
    public void DraftRoomChatPanel_WhenTextIsEmpty_ShouldHideSendButtonAndNotSend()
    {
        var panel = RenderPanel();

        panel.FindAll("button[type='submit']").Should().BeEmpty();
        panel.Find("form").TriggerEvent("onsubmit", new EventArgs());

        _session.Verify(
            session => session.SendChatAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void DraftRoomChatPanel_WhenSendIsThrottled_ShouldShowPoliteFeedback()
    {
        _session
            .Setup(session => session.SendChatAsync("Hello", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<ChatMessageDto>(DraftRoomApiResultKind.Throttled, null));
        var panel = RenderPanel();
        panel.Find("#draft-room-chat-input").Change("  Hello  ");

        panel.Find("form").TriggerEvent("onsubmit", new EventArgs());

        panel.WaitForAssertion(() =>
        {
            var status = panel.Find("#draft-room-chat-status");
            status.GetAttribute("role").Should().Be("status");
            status.GetAttribute("aria-live").Should().Be("polite");
            status.TextContent.Should().Contain("sending messages too quickly");
        });
        _session.Verify(
            session => session.SendChatAsync("Hello", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static ChatMessageDto CreateMessage(long id, string text)
    {
        return new ChatMessageDto
        {
            Id = id,
            DisplayName = "Guest One",
            Kind = ChatMessageKind.User,
            Text = text,
            CreatedAtUtc = DateTime.UtcNow,
        };
    }

    private IRenderedComponent<DraftRoomChatPanel> RenderPanel(bool isAtBottom = true)
    {
        this._module = JSInterop.SetupModule("./Components/DraftRooms/DraftRoomChatPanel.razor.js");
        this._module.Setup<bool>("isAtBottom").SetResult(isAtBottom);
        this._module.SetupVoid("scrollToBottom");

        var host = Render<CascadingValue<IDraftRoomSession>>(parameters => parameters
            .Add(parameter => parameter.Value, this._session.Object)
            .AddChildContent<DraftRoomChatPanel>());

        return host.FindComponent<DraftRoomChatPanel>();
    }
}
