using System.Globalization;
using AutoMapper;
using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Models.Users;
using TwilightImperiumUltimate.Web.Pages.Tools;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.Authentication;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;
using TwilightImperiumUltimate.Web.Validators.DraftRooms;

namespace TwilightImperiumUltimate.ComponentTests.Pages.Tools;

public sealed class DraftRoomPageTests : BunitContext
{
    private readonly Mock<ICurrentUserState> _currentUserState = new();
    private readonly Mock<IDraftRoomApiService> _draftRoomApiService = new();
    private readonly Mock<IGuestIdentityStore> _guestIdentityStore = new();
    private readonly Mock<IDraftRoomSession> _draftRoomSession = new();
    private readonly Mock<IRoomTabOpener> _roomTabOpener = new();

    public DraftRoomPageTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var chatModule = JSInterop.SetupModule("./Components/DraftRooms/DraftRoomChatPanel.razor.js");
        chatModule.Setup<bool>("isAtBottom").SetResult(true);
        chatModule.SetupVoid("scrollToBottom");
        var confirmDialogModule = JSInterop.SetupModule("./Components/Shared/Controls/ResponsiveConfirmDialog.razor.js");
        confirmDialogModule.SetupVoid("showModal");
        confirmDialogModule.SetupVoid("closeModal");
        _currentUserState.Setup(state => state.InitializeAsync()).Returns(Task.CompletedTask);
        _currentUserState.SetupGet(state => state.User).Returns((TwilightImperiumUser?)null);
        _guestIdentityStore
            .Setup(store => store.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GuestIdentity?)null);
        _draftRoomSession.SetupGet(session => session.View).Returns(CreateGuestRoomView());
        _draftRoomSession.SetupGet(session => session.Error).Returns((DraftRoomApiResultKind?)null);
        _draftRoomSession
            .Setup(session => session.ConnectAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _draftRoomSession
            .Setup(session => session.GetChatHistoryAsync(
                null,
                DraftRoomLimits.DefaultHistoryPageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<IReadOnlyList<ChatMessageDto>>(
                DraftRoomApiResultKind.Success,
                Array.Empty<ChatMessageDto>()));
        _draftRoomSession
            .Setup(session => session.GetLogHistoryAsync(
                null,
                DraftRoomLimits.DefaultHistoryPageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<IReadOnlyList<DraftLogEntryDto>>(
                DraftRoomApiResultKind.Success,
                Array.Empty<DraftLogEntryDto>()));

        Services.AddSingleton(Mock.Of<IPathProvider>());
        Services.AddSingleton(Mock.Of<ITwilightImperiumApiHttpClient>());
        Services.AddSingleton(Mock.Of<ILocalStorageService>());
        Services.AddSingleton(Mock.Of<IMapper>());
        Services.AddSingleton(_currentUserState.Object);
        Services.AddSingleton(_draftRoomApiService.Object);
        Services.AddSingleton<IDraftTypeUiRegistry, DraftTypeUiRegistry>();
        Services.AddSingleton(_guestIdentityStore.Object);
        Services.AddSingleton(_draftRoomSession.Object);
        Services.AddSingleton(_roomTabOpener.Object);
        Services.AddSingleton(Mock.Of<IPageVisibility>());
        Services.AddSingleton(TimeProvider.System);
        Services.AddSingleton<IValidator<JoinDraftRoomFormModel>, JoinDraftRoomFormModelValidator>();
        Services.AddSingleton<IValidator<UpdateDraftRoomNameFormModel>, UpdateDraftRoomNameFormModelValidator>();
        Services.AddSingleton<IValidator<ChangeDraftRoomPasswordFormModel>, ChangeDraftRoomPasswordFormModelValidator>();
    }

    [Fact]
    public void DraftRoom_WhenInitialCodeIsInvalid_ShouldFocusTheTerminalStatusShell()
    {
        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "bad"));

        page.Find("section[tabindex='-1']").TextContent.Should().Contain(Strings.DraftRooms_RoomNotFoundHeading);
        JSInterop.Invocations.Should().Contain(invocation => invocation.Identifier == "Blazor._internal.domWrapper.focus");
    }

    [Fact]
    public void DraftRoom_WhenGuestHasSavedIdentity_ShouldRenderRoomShell()
    {
        _guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGuestIdentity());
        _draftRoomApiService
            .Setup(service => service.JoinAsync("ABC234", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.Success,
                CreateGuestRoom()));

        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "abc234"));

        page.Find("h1").TextContent.Should().Be("Test room");
        page.Markup.Should().Contain("You are Guest Host (Host).");
        page.FindAll("#draft-room-code").Should().BeEmpty();
        _draftRoomApiService.Verify(
            service => service.JoinAsync("ABC234", null, null, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void DraftRoom_WhenAnonymousGuestOpensRoom_ShouldShowLoginAndRegisterLinks()
    {
        _guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGuestIdentity());
        _draftRoomApiService
            .Setup(service => service.JoinAsync("ABC234", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.Success,
                CreateGuestRoom()));

        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "abc234"));

        page.Markup.Should().Contain(Strings.DraftRooms_GuestAccountPrompt);
        page.Find("a[href='/account/login?returnUrl=%2Ftools%2Fdraft-rooms%2FABC234']")
            .TextContent.Should().Be(Strings.Login_Login);
        page.Find("a[href='/account/register']")
            .TextContent.Should().Be(Strings.Page_AccountRegister);
        page.FindAll("[role='alertdialog']").Should().BeEmpty();
    }

    [Fact]
    public void DraftRoom_WhenAccountHasNoGuestIdentity_ShouldNotShowLinkPrompt()
    {
        _currentUserState
            .SetupGet(state => state.User)
            .Returns(new TwilightImperiumUser
            {
                Id = "account-id",
                UserName = "AccountName",
            });
        _draftRoomApiService
            .Setup(service => service.JoinAsync("ABC234", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.Success,
                CreateAccountRoom()));

        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC234"));

        page.FindAll("[role='alertdialog']").Should().BeEmpty();
        page.FindAll("button")
            .Should()
            .NotContain(button => button.TextContent.Trim() == Strings.DraftRooms_UseMyAccount);
    }

    [Fact]
    public void DraftRoom_WhenAccountHasGuestIdentity_ShouldPromptAndAllowDismissal()
    {
        SetupAuthenticatedGuestRoom();

        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC234"));

        page.Find("[role='alertdialog']").TextContent.Should().Contain(Strings.DraftRooms_LinkAccountTitle);
        var keepPlayingButton = page.FindAll("button")
            .Single(button => button.TextContent.Trim() == Strings.DraftRooms_KeepPlayingAsGuest);
        keepPlayingButton.Click();

        page.FindAll("[role='alertdialog']").Should().BeEmpty();
        var useAccountButton = page.FindAll("button")
            .Single(button => button.TextContent.Trim() == Strings.DraftRooms_UseMyAccount);
        useAccountButton.Click();
        page.Find("[role='alertdialog']").Should().NotBeNull();
        _draftRoomApiService.Verify(
            service => service.LinkAccountAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _guestIdentityStore.Verify(
            store => store.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void DraftRoom_WhenAccountConfirmsGuestLink_ShouldLinkAndRefreshRoom()
    {
        SetupAuthenticatedGuestRoom();
        _draftRoomApiService
            .Setup(service => service.LinkAccountAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Success));

        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC234"));
        var confirmButton = page.Find("[role='alertdialog']").QuerySelectorAll("button")[1];

        confirmButton.Click();

        page.FindAll("[role='alertdialog']").Should().BeEmpty();
        _draftRoomApiService.Verify(
            service => service.LinkAccountAsync("ABC234", It.IsAny<CancellationToken>()),
            Times.Once);
        _draftRoomSession.Verify(
            session => session.RefreshAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void DraftRoom_WhenAccountLinkConflicts_ShouldShowConflictAndKeepGuestIdentity()
    {
        SetupAuthenticatedGuestRoom();
        _draftRoomApiService
            .Setup(service => service.LinkAccountAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Conflict));

        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC234"));
        page.Find("[role='alertdialog']").QuerySelectorAll("button")[1].Click();

        page.Find("[role='alert']").TextContent.Should().Contain(Strings.DraftRooms_LinkAccountConflict);
        _draftRoomSession.Verify(
            session => session.RefreshAsync(It.IsAny<CancellationToken>()),
            Times.Never);
        _guestIdentityStore.Verify(
            store => store.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void DraftRoom_WhenHostOpensRoom_ShouldShowMobileDisclosureButtonsAndNamedRegions()
    {
        _guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGuestIdentity());
        _draftRoomApiService
            .Setup(service => service.JoinAsync("ABC234", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.Success,
                CreateGuestRoom()));
        _draftRoomSession.SetupGet(session => session.View).Returns(new DraftRoomViewDto
        {
            Code = "ABC234",
            Name = "Test room",
            Me = new DraftRoomViewMeDto
            {
                CanChangeSettings = true,
            },
        });

        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC234"));

        var tabs = page.FindAll(".draft-room-lobby__mobile-tabs button");
        var chatTabLabel = Strings.ResourceManager.GetString("DraftRooms_ChatTab", CultureInfo.CurrentUICulture);
        var logTabLabel = Strings.ResourceManager.GetString("DraftRooms_LogTab", CultureInfo.CurrentUICulture);
        tabs.Select(tab => tab.TextContent.Trim()).Should().Equal(
            Strings.DraftRooms_SeatsTab,
            Strings.DraftRooms_PeopleTab,
            Strings.DraftRooms_SettingsTab,
            chatTabLabel,
            logTabLabel);
        page.Find("nav.draft-room-lobby__mobile-tabs").GetAttribute("aria-label").Should().Be(Strings.DraftRooms_RoomSections);
        tabs.Select(tab => page.Find($"#{tab.GetAttribute("aria-controls")}")).Should().HaveCount(5);
        page.Find("#draft-room-mobile-seats-tab").GetAttribute("aria-expanded").Should().Be("true");
        page.Find("#draft-room-mobile-seats-tab").GetAttribute("tabindex").Should().BeNull();
        page.Find("#draft-room-people-panel").GetAttribute("role").Should().Be("region");
        page.Find("#draft-room-people-panel").GetAttribute("aria-label").Should().Be(Strings.DraftRooms_PeopleTab);
        page.FindAll("[role='tab'], [role='tabpanel']").Should().BeEmpty();
        page.Find("#draft-room-chat-panel").Should().NotBeNull();
        page.Find("#draft-room-log-panel").Should().NotBeNull();

        page.Find("#draft-room-mobile-people-tab").Click();

        page.Find("#draft-room-mobile-people-tab").GetAttribute("aria-expanded").Should().Be("true");
        page.Find("#draft-room-people-panel").ClassList.Should().Contain("draft-room-lobby__panel--active");
        page.Find("#draft-room-mobile-seats-tab").GetAttribute("aria-expanded").Should().Be("false");
    }

    [Fact]
    public void DraftRoom_WhenMobileControlsRender_ShouldUseNativeKeyboardFocusableButtons()
    {
        _guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGuestIdentity());
        _draftRoomApiService
            .Setup(service => service.JoinAsync("ABC234", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.Success,
                CreateGuestRoom()));
        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC234"));
        page.FindAll(".draft-room-lobby__mobile-tabs button")
            .Should().OnlyContain(button => button.GetAttribute("type") == "button" && !button.HasAttribute("tabindex"));
        page.Find("#draft-room-mobile-seats-tab").GetAttribute("aria-expanded").Should().Be("true");
        page.Find("#draft-room-mobile-people-tab").GetAttribute("aria-expanded").Should().Be("false");
    }

    [Fact]
    public void DraftRoom_WhenMemberOpensRoom_ShouldNotShowSettingsTabOrPanel()
    {
        _guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGuestIdentity());
        _draftRoomApiService
            .Setup(service => service.JoinAsync("ABC234", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.Success,
                CreateGuestRoom()));
        _draftRoomSession.SetupGet(session => session.View).Returns(CreateGuestRoomView());

        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC234"));

        page.FindAll(".draft-room-lobby__mobile-tabs button").Select(tab => tab.TextContent.Trim()).Should().Equal(
            Strings.DraftRooms_SeatsTab,
            Strings.DraftRooms_PeopleTab,
            Strings.ResourceManager.GetString("DraftRooms_ChatTab", CultureInfo.CurrentUICulture),
            Strings.ResourceManager.GetString("DraftRooms_LogTab", CultureInfo.CurrentUICulture));
        page.FindAll("#draft-room-mobile-settings-tab").Should().BeEmpty();
        page.FindAll("#draft-room-settings-panel").Should().BeEmpty();
    }

    [Fact]
    public void DraftRoom_WhenChatAndLogArriveWhileTheirTabsAreInactive_ShouldCountAndClearUnreadEntries()
    {
        _guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGuestIdentity());
        _draftRoomApiService
            .Setup(service => service.JoinAsync("ABC234", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.Success,
                CreateGuestRoom()));
        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC234"));

        _draftRoomSession.Raise(
            session => session.ChatMessageReceived += null!,
            new ChatMessageDto
            {
                Id = 1,
                ParticipantId = Guid.NewGuid(),
                DisplayName = "Guest One",
                Kind = ChatMessageKind.User,
                Text = "Hello",
            });
        _draftRoomSession.Raise(
            session => session.LogEntryAdded += null!,
            new DraftLogEntryDto
            {
                Id = 1,
                ActorParticipantId = Guid.NewGuid(),
                Type = DraftLogEntryType.RoomLocked,
                DataJson = """{"IsLocked":true}""",
            });

        page.WaitForAssertion(() =>
        {
            page.Find("#draft-room-mobile-chat-tab .draft-room-lobby__unread-count").TextContent.Should().Be("1");
            page.Find("#draft-room-mobile-log-tab .draft-room-lobby__unread-count").TextContent.Should().Be("1");
        });

        page.Find("#draft-room-mobile-chat-tab").Click();
        page.FindAll("#draft-room-mobile-chat-tab .draft-room-lobby__unread-count").Should().BeEmpty();
        page.Find("#draft-room-mobile-log-tab .draft-room-lobby__unread-count").TextContent.Should().Be("1");

        page.Find("#draft-room-mobile-log-tab").Click();
        page.FindAll("#draft-room-mobile-log-tab .draft-room-lobby__unread-count").Should().BeEmpty();
    }

    [Fact]
    public void DraftRoom_WhenSettingsPermissionIsRevoked_ShouldReturnToSeatsTab()
    {
        var roomView = new DraftRoomViewDto
        {
            Code = "ABC234",
            Name = "Test room",
            Me = new DraftRoomViewMeDto
            {
                CanChangeSettings = true,
            },
        };
        _guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGuestIdentity());
        _draftRoomApiService
            .Setup(service => service.JoinAsync("ABC234", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.Success,
                CreateGuestRoom()));
        _draftRoomSession.SetupGet(session => session.View).Returns(() => roomView);
        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC234"));

        page.Find("#draft-room-mobile-settings-tab").Click();
        page.Find("#draft-room-mobile-settings-tab").GetAttribute("aria-expanded").Should().Be("true");
        roomView = CreateGuestRoomView();
        _draftRoomSession.Raise(session => session.Changed += null);

        page.WaitForAssertion(() =>
        {
            page.Find("#draft-room-mobile-seats-tab").GetAttribute("aria-expanded").Should().Be("true");
            page.FindAll("#draft-room-mobile-settings-tab").Should().BeEmpty();
        });
    }

    [Fact]
    public void DraftRoom_WhenGuestHasNoSavedIdentity_ShouldShowForcedCodeJoinForm()
    {
        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "abc234"));

        page.Find("h1").TextContent.Should().Be(Strings.DraftRooms_JoinRoomHeading);
        page.Find("#draft-room-code").GetAttribute("readonly").Should().NotBeNull();
        page.Find("#draft-room-code").GetAttribute("value").Should().Be("ABC234");
        _draftRoomApiService.Verify(
            service => service.JoinAsync(
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void DraftRoom_WhenCodeIsInvalid_ShouldShowNotFoundState()
    {
        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC123"));

        page.Find("h1").TextContent.Should().Be(Strings.DraftRooms_RoomNotFoundHeading);
        page.Markup.Should().Contain(Strings.DraftRooms_RoomNotFoundMessage);
        _draftRoomApiService.Verify(
            service => service.JoinAsync(
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void DraftRoom_WhenJoinFormSucceeds_ShouldRenderRoomShellInSameTab()
    {
        var room = CreateGuestRoom();
        _draftRoomApiService
            .Setup(service => service.JoinAsync(
                "ABC234",
                "secret123",
                "Guest Host",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(DraftRoomApiResultKind.Success, room));
        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC234"));
        page.Find("#draft-room-join-password").Change("secret123");
        page.Find("#draft-room-join-display-name").Change("Guest Host");

        page.Find("button[type='submit']").Click();

        page.Find("h1").TextContent.Should().Be("Test room");
        page.Markup.Should().Contain("You are Guest Host (Host).");
        page.Find("section[tabindex='-1']").Should().NotBeNull();
        JSInterop.Invocations.Should().Contain(invocation => invocation.Identifier == "Blazor._internal.domWrapper.focus");
        _roomTabOpener.Verify(opener => opener.OpenAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void DraftRoom_WhenSavedIdentityIsRejected_ShouldRemoveIdentityAndShowJoinForm()
    {
        _guestIdentityStore
            .SetupSequence(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGuestIdentity())
            .ReturnsAsync((GuestIdentity?)null);
        _draftRoomApiService
            .Setup(service => service.JoinAsync("ABC234", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.NotFoundOrWrongPassword,
                null));

        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC234"));

        page.Find("h1").TextContent.Should().Be(Strings.DraftRooms_JoinRoomHeading);
        page.Find("#draft-room-join-password").Should().NotBeNull();
        page.Find("#draft-room-join-display-name").Should().NotBeNull();
        _guestIdentityStore.Verify(store => store.RemoveAsync("ABC234", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void DraftRoom_WhenKnownParticipantIsForbidden_ShouldShowRemovedState()
    {
        _guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGuestIdentity());
        _draftRoomApiService
            .Setup(service => service.JoinAsync("ABC234", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.Forbidden,
                null));

        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC234"));

        page.Find("h1").TextContent.Should().Be(Strings.DraftRooms_RoomRemovedHeading);
        page.Markup.Should().Contain(Strings.DraftRooms_RoomRemovedMessage);
        _draftRoomSession.Verify(session => session.Initialize(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void DraftRoom_WhenRoomViewIsNotFoundAfterJoining_ShouldShowClosedState()
    {
        _guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGuestIdentity());
        _draftRoomApiService
            .Setup(service => service.JoinAsync("ABC234", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.Success,
                CreateGuestRoom()));
        _draftRoomSession.SetupGet(session => session.Error).Returns(DraftRoomApiResultKind.NotFound);

        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC234"));

        page.Find("h1").TextContent.Should().Be(Strings.DraftRooms_RoomClosedHeading);
        page.Markup.Should().Contain(Strings.DraftRooms_RoomClosedMessage);
        _draftRoomSession.Verify(session => session.Initialize("ABC234"), Times.Once);
        _draftRoomSession.Verify(session => session.ConnectAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void DraftRoom_WhenSessionRefreshFails_ShouldShowLoadError()
    {
        _guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGuestIdentity());
        _draftRoomApiService
            .Setup(service => service.JoinAsync("ABC234", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.Success,
                CreateGuestRoom()));
        _draftRoomSession.SetupGet(session => session.Error).Returns(DraftRoomApiResultKind.Failed);
        _draftRoomSession.SetupGet(session => session.View).Returns((DraftRoomViewDto?)null);

        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC234"));

        page.Find("[role='alert']").TextContent.Should().Contain(Strings.DraftRooms_RoomLoadFailed);
    }

    [Fact]
    public void DraftRoom_WhenSessionReportsNotFoundAfterRefresh_ShouldRenderClosedState()
    {
        DraftRoomApiResultKind? sessionError = null;
        DraftRoomViewDto? sessionView = CreateGuestRoomView();
        _guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGuestIdentity());
        _draftRoomApiService
            .Setup(service => service.JoinAsync("ABC234", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.Success,
                CreateGuestRoom()));
        _draftRoomSession.SetupGet(session => session.Error).Returns(() => sessionError);
        _draftRoomSession.SetupGet(session => session.View).Returns(() => sessionView);
        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC234"));

        sessionError = DraftRoomApiResultKind.NotFound;
        sessionView = null;
        _draftRoomSession.Raise(session => session.Changed += null);

        page.WaitForAssertion(() =>
            page.Find("h1").TextContent.Should().Be(Strings.DraftRooms_RoomClosedHeading));
    }

    [Fact]
    public void DraftRoom_WhenSessionRefreshIsPending_ShouldShowLoadingState()
    {
        var connectCompletion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        _guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGuestIdentity());
        _draftRoomApiService
            .Setup(service => service.JoinAsync("ABC234", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.Success,
                CreateGuestRoom()));
        _draftRoomSession
            .Setup(session => session.ConnectAsync(It.IsAny<CancellationToken>()))
            .Returns(connectCompletion.Task);

        var page = Render<DraftRoom>(parameters => parameters.Add(component => component.Code, "ABC234"));

        page.Markup.Should().Contain(Strings.DraftRooms_RoomLoading);
        connectCompletion.SetResult();
        page.WaitForAssertion(() => page.Find("h1").TextContent.Should().Be("Test room"));
    }

    private static GuestIdentity CreateGuestIdentity() => new(
        Guid.NewGuid(),
        "ABC234",
        Guid.NewGuid(),
        "guest-secret",
        "Guest Host",
        DateTimeOffset.UtcNow);

    private static DraftRoomEntryDto CreateGuestRoom() => new()
    {
        RoomId = Guid.NewGuid(),
        Code = "ABC234",
        RoomName = "Test room",
        ParticipantId = Guid.NewGuid(),
        DisplayName = "Guest Host",
        Role = TwilightImperiumUltimate.Contracts.Enums.DraftRoomRole.Host,
        IsGuest = true,
        GuestSecret = "guest-secret",
    };

    private static DraftRoomEntryDto CreateAccountRoom() => new()
    {
        RoomId = Guid.NewGuid(),
        Code = "ABC234",
        RoomName = "Test room",
        ParticipantId = Guid.NewGuid(),
        DisplayName = "AccountName",
        Role = TwilightImperiumUltimate.Contracts.Enums.DraftRoomRole.Host,
        IsGuest = false,
    };

    private static DraftRoomViewDto CreateGuestRoomView() => new()
    {
        Code = "ABC234",
        Name = "Test room",
    };

    private void SetupAuthenticatedGuestRoom()
    {
        _currentUserState
            .SetupGet(state => state.User)
            .Returns(new TwilightImperiumUser
            {
                Id = "account-id",
                UserName = "AccountName",
            });
        _guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateGuestIdentity());
        _draftRoomApiService
            .Setup(service => service.JoinAsync("ABC234", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.Success,
                CreateGuestRoom()));
    }
}
