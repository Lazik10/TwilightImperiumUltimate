using AutoMapper;
using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Web.Components.DraftRooms;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Models.Users;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.Authentication;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;
using TwilightImperiumUltimate.Web.Validators.DraftRooms;

namespace TwilightImperiumUltimate.ComponentTests.Components.DraftRooms;

public sealed class JoinDraftRoomFormTests : BunitContext
{
    private readonly Mock<ICurrentUserState> _currentUserState = new();
    private readonly Mock<IDraftRoomApiService> _draftRoomApiService = new();
    private readonly Mock<IGuestIdentityStore> _guestIdentityStore = new();
    private readonly Mock<IRoomTabOpener> _roomTabOpener = new();

    public JoinDraftRoomFormTests()
    {
        _currentUserState.Setup(state => state.InitializeAsync()).Returns(Task.CompletedTask);
        _currentUserState.SetupGet(state => state.User).Returns((TwilightImperiumUser?)null);
        _guestIdentityStore
            .Setup(store => store.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GuestIdentity?)null);

        Services.AddSingleton(Mock.Of<IPathProvider>());
        Services.AddSingleton(Mock.Of<ITwilightImperiumApiHttpClient>());
        Services.AddSingleton(Mock.Of<ILocalStorageService>());
        Services.AddSingleton(Mock.Of<IMapper>());
        Services.AddSingleton(_currentUserState.Object);
        Services.AddSingleton(_draftRoomApiService.Object);
        Services.AddSingleton(_guestIdentityStore.Object);
        Services.AddSingleton(_roomTabOpener.Object);
        Services.AddSingleton<IValidator<JoinDraftRoomFormModel>, JoinDraftRoomFormModelValidator>();
    }

    [Fact]
    public void JoinDraftRoomForm_WhenRenderedForGuest_ShouldShowCodePasswordAndDisplayName()
    {
        var form = Render<JoinDraftRoomForm>();

        form.Find("#draft-room-code").Should().NotBeNull();
        form.Find("#draft-room-join-password").Should().NotBeNull();
        form.Find("#draft-room-join-display-name").Should().NotBeNull();
        foreach (var input in form.FindAll("input"))
        {
            var id = input.GetAttribute("id");
            form.Find($"label[for='{id}']").Should().NotBeNull();
            input.GetAttribute("aria-required").Should().Be("true");
        }
    }

    [Fact]
    public void JoinDraftRoomForm_WhenRenderedForAuthenticatedUser_ShouldHideDisplayName()
    {
        _currentUserState.SetupGet(state => state.User).Returns(new TwilightImperiumUser { UserName = "account-user" });

        var form = Render<JoinDraftRoomForm>();

        form.Find("#draft-room-code").Should().NotBeNull();
        form.Find("#draft-room-join-password").Should().NotBeNull();
        form.FindAll("#draft-room-join-display-name").Should().BeEmpty();
    }

    [Fact]
    public void JoinDraftRoomForm_WhenSubmittedWithMissingValues_ShouldShowValidationMessages()
    {
        var form = Render<JoinDraftRoomForm>();

        form.Find("button[type='submit']").Click();

        form.FindAll(".responsive-form-field-error")
            .Select(message => message.TextContent)
            .Should()
            .Contain([
                ValidationMessages.DraftRooms_CodeRequired,
                ValidationMessages.DraftRooms_PasswordRequired,
                ValidationMessages.DraftRooms_DisplayNameRequired,
            ]);
        form.Find(".responsive-form-summary[role='alert']").Should().NotBeNull();
    }

    [Fact]
    public void JoinDraftRoomForm_WhenGuestJoinsRoom_ShouldSaveIdentityAndOpenRoom()
    {
        var room = CreateGuestRoom();
        _draftRoomApiService
            .Setup(service => service.JoinAsync(
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(DraftRoomApiResultKind.Success, room));
        _roomTabOpener.Setup(opener => opener.OpenAsync("/tools/draft-rooms/ABC234")).ReturnsAsync(true);
        var form = Render<JoinDraftRoomForm>();
        FillGuestForm(form, "ABC234");

        form.Find("button[type='submit']").Click();

        _guestIdentityStore.Verify(
            store => store.SaveAsync(
                It.Is<GuestIdentity>(identity =>
                    identity.RoomId == room.RoomId
                    && identity.Code == "ABC234"
                    && identity.ParticipantId == room.ParticipantId
                    && identity.Secret == room.GuestSecret
                    && identity.RoomName == room.RoomName),
                It.IsAny<CancellationToken>()),
            Times.Once);
        _roomTabOpener.Verify(opener => opener.OpenAsync("/tools/draft-rooms/ABC234"), Times.Once);
        form.Find("[role='status']").TextContent.Should().Contain(Strings.DraftRooms_JoinSuccess);
    }

    [Fact]
    public void JoinDraftRoomForm_WhenForcedCodeIsSet_ShouldLockCodeAndNotifyParentWithoutOpeningTab()
    {
        var room = CreateGuestRoom();
        _draftRoomApiService
            .Setup(service => service.JoinAsync(
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(DraftRoomApiResultKind.Success, room));
        DraftRoomEntryDto? joinedRoom = null;
        var form = Render<JoinDraftRoomForm>(parameters => parameters
            .Add(component => component.ForcedCode, "abc234")
            .Add(component => component.OpenInNewTab, false)
            .Add(component => component.OnJoined, entry => joinedRoom = entry));
        var roomCode = form.Find("#draft-room-code");
        roomCode.GetAttribute("readonly").Should().NotBeNull();
        roomCode.GetAttribute("disabled").Should().BeNull();
        roomCode.GetAttribute("value").Should().Be("ABC234");
        form.Find("#draft-room-join-password").Change("secret123");
        form.Find("#draft-room-join-display-name").Change("Guest Host");

        form.Find("button[type='submit']").Click();

        joinedRoom.Should().BeSameAs(room);
        _roomTabOpener.Verify(opener => opener.OpenAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void JoinDraftRoomForm_WhenGuestIdentityIsSaved_ShouldHidePasswordAndDisplayNameFields()
    {
        _guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GuestIdentity(
                Guid.NewGuid(),
                "ABC234",
                Guid.NewGuid(),
                "guest-secret",
                "Guest Host",
                DateTimeOffset.UtcNow));

        var form = Render<JoinDraftRoomForm>(parameters => parameters.Add(component => component.ForcedCode, "ABC234"));

        form.FindAll("#draft-room-join-password").Should().BeEmpty();
        form.FindAll("#draft-room-join-display-name").Should().BeEmpty();
        form.Find("[role='status']").TextContent.Should().Contain(Strings.DraftRooms_StoredGuestIdentity);
    }

    [Fact]
    public void JoinDraftRoomForm_WhenAccountHasSavedGuestIdentity_ShouldStillRequireRoomPassword()
    {
        _currentUserState.SetupGet(state => state.User).Returns(new TwilightImperiumUser { UserName = "account-user" });
        _guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GuestIdentity(
                Guid.NewGuid(),
                "ABC234",
                Guid.NewGuid(),
                "guest-secret",
                "Guest Host",
                DateTimeOffset.UtcNow));

        var form = Render<JoinDraftRoomForm>(parameters => parameters.Add(component => component.ForcedCode, "ABC234"));

        form.Find("#draft-room-join-password").Should().NotBeNull();
        form.FindAll("#draft-room-join-display-name").Should().BeEmpty();
        form.FindAll("[role='status']").Should().BeEmpty();
    }

    [Fact]
    public void JoinDraftRoomForm_WhenStoredGuestIdentityIsRejected_ShouldRemoveItAndShowCredentialFields()
    {
        _guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GuestIdentity(
                Guid.NewGuid(),
                "ABC234",
                Guid.NewGuid(),
                "guest-secret",
                "Guest Host",
                DateTimeOffset.UtcNow));
        _draftRoomApiService
            .Setup(service => service.JoinAsync(
                "ABC234",
                null,
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(DraftRoomApiResultKind.NotFoundOrWrongPassword, null));
        var form = Render<JoinDraftRoomForm>(parameters => parameters.Add(component => component.ForcedCode, "ABC234"));

        form.Find("button[type='submit']").Click();

        _guestIdentityStore.Verify(store => store.RemoveAsync("ABC234", It.IsAny<CancellationToken>()), Times.Once);
        form.Find("[role='alert']").TextContent.Should().Be(Strings.DraftRooms_JoinNotFoundOrWrongPassword);
        form.Find("#draft-room-join-password").Should().NotBeNull();
        form.Find("#draft-room-join-display-name").Should().NotBeNull();
    }

    [Fact]
    public void JoinDraftRoomForm_WhenRoomCredentialsAreRejected_ShouldShowGenericMessage()
    {
        _draftRoomApiService
            .Setup(service => service.JoinAsync(
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(DraftRoomApiResultKind.NotFoundOrWrongPassword, null));
        var form = Render<JoinDraftRoomForm>();
        FillGuestForm(form, "ABC234");

        form.Find("button[type='submit']").Click();

        form.Find("[role='alert']").TextContent.Should().Be(Strings.DraftRooms_JoinNotFoundOrWrongPassword);
    }

    [Fact]
    public void JoinDraftRoomForm_WhenRequestIsThrottled_ShouldShowThrottleMessage()
    {
        _draftRoomApiService
            .Setup(service => service.JoinAsync(
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(DraftRoomApiResultKind.Throttled, null));
        var form = Render<JoinDraftRoomForm>();
        FillGuestForm(form, "ABC234");

        form.Find("button[type='submit']").Click();

        form.Find("[role='alert']").TextContent.Should().Be(Strings.DraftRooms_JoinThrottled);
    }

    [Fact]
    public void JoinDraftRoomForm_WhenRequestFails_ShouldShowFailureMessage()
    {
        _draftRoomApiService
            .Setup(service => service.JoinAsync(
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(DraftRoomApiResultKind.Failed, null));
        var form = Render<JoinDraftRoomForm>();
        FillGuestForm(form, "ABC234");

        form.Find("button[type='submit']").Click();

        form.Find("[role='alert']").TextContent.Should().Be(Strings.DraftRooms_JoinFailed);
    }

    private static DraftRoomEntryDto CreateGuestRoom() => new()
    {
        RoomId = Guid.NewGuid(),
        Code = "ABC234",
        RoomName = "Friday TI",
        ParticipantId = Guid.NewGuid(),
        DisplayName = "Guest Host",
        IsGuest = true,
        GuestSecret = "guest-secret",
    };

    private static void FillGuestForm(IRenderedComponent<JoinDraftRoomForm> form, string code = "ABC234")
    {
        form.Find("#draft-room-code").Change(code);
        form.Find("#draft-room-join-password").Change("secret123");
        form.Find("#draft-room-join-display-name").Change("Guest Host");
    }
}
