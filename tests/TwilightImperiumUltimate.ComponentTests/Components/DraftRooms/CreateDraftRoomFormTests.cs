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

public sealed class CreateDraftRoomFormTests : BunitContext
{
    private readonly Mock<ICurrentUserState> _currentUserState = new();
    private readonly Mock<IDraftRoomApiService> _draftRoomApiService = new();
    private readonly Mock<IGuestIdentityStore> _guestIdentityStore = new();
    private readonly Mock<IRoomTabOpener> _roomTabOpener = new();

    public CreateDraftRoomFormTests()
    {
        _currentUserState.Setup(state => state.InitializeAsync()).Returns(Task.CompletedTask);
        _currentUserState.SetupGet(state => state.User).Returns((TwilightImperiumUser?)null);

        Services.AddSingleton(Mock.Of<IPathProvider>());
        Services.AddSingleton(Mock.Of<ITwilightImperiumApiHttpClient>());
        Services.AddSingleton(Mock.Of<ILocalStorageService>());
        Services.AddSingleton(Mock.Of<IMapper>());
        Services.AddSingleton(_currentUserState.Object);
        Services.AddSingleton(_draftRoomApiService.Object);
        Services.AddSingleton(_guestIdentityStore.Object);
        Services.AddSingleton(_roomTabOpener.Object);
        Services.AddSingleton<IValidator<CreateDraftRoomFormModel>, CreateDraftRoomFormModelValidator>();
    }

    [Fact]
    public void CreateDraftRoomForm_WhenRenderedForGuest_ShouldShowDisplayNameField()
    {
        var form = Render<CreateDraftRoomForm>();

        form.Find("label[for='draft-room-display-name']").TextContent.Should().Contain(Strings.DraftRooms_DisplayName);
        form.Find("#draft-room-display-name").Should().NotBeNull();
        foreach (var input in form.FindAll("input"))
        {
            var id = input.GetAttribute("id");
            form.Find($"label[for='{id}']").Should().NotBeNull();
            input.GetAttribute("aria-required").Should().Be("true");
        }
    }

    [Fact]
    public void CreateDraftRoomForm_WhenRenderedForAuthenticatedUser_ShouldHideDisplayNameField()
    {
        _currentUserState.SetupGet(state => state.User).Returns(new TwilightImperiumUser { UserName = "account-user" });

        var form = Render<CreateDraftRoomForm>();

        form.FindAll("#draft-room-display-name").Should().BeEmpty();
    }

    [Fact]
    public void CreateDraftRoomForm_WhenSubmittedWithMissingValues_ShouldShowValidationMessages()
    {
        var form = Render<CreateDraftRoomForm>();

        form.Find("button[type='submit']").Click();

        form.FindAll(".responsive-form-field-error")
            .Select(message => message.TextContent)
            .Should()
            .Contain([ValidationMessages.DraftRooms_RoomNameRequired, ValidationMessages.DraftRooms_PasswordRequired, ValidationMessages.DraftRooms_DisplayNameRequired]);
        form.Find(".responsive-form-summary[role='alert']").Should().NotBeNull();
    }

    [Fact]
    public void CreateDraftRoomForm_WhenGuestRoomIsCreated_ShouldSaveIdentityAndOpenRoom()
    {
        var roomId = Guid.NewGuid();
        var participantId = Guid.NewGuid();
        _draftRoomApiService
            .Setup(service => service.CreateAsync(It.IsAny<CreateDraftRoomFormModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.Success,
                new DraftRoomEntryDto
                {
                    RoomId = roomId,
                    Code = "abc123",
                    RoomName = "Friday TI",
                    ParticipantId = participantId,
                    DisplayName = "Guest Host",
                    IsGuest = true,
                    GuestSecret = "guest-secret",
                }));
        _roomTabOpener.Setup(opener => opener.OpenAsync("/tools/draft-rooms/ABC123")).ReturnsAsync(true);
        var form = Render<CreateDraftRoomForm>();
        FillValidGuestForm(form);

        form.Find("button[type='submit']").Click();

        _guestIdentityStore.Verify(
            store => store.SaveAsync(
                It.Is<GuestIdentity>(identity =>
                    identity.RoomId == roomId
                    && identity.Code == "ABC123"
                    && identity.ParticipantId == participantId
                    && identity.Secret == "guest-secret"
                    && identity.RoomName == "Friday TI"),
                It.IsAny<CancellationToken>()),
            Times.Once);
        _roomTabOpener.Verify(opener => opener.OpenAsync("/tools/draft-rooms/ABC123"), Times.Once);
        form.Find("[role='status']").TextContent.Should().Contain(Strings.DraftRooms_CreateSuccess);
        form.FindAll("a").Should().BeEmpty();
    }

    [Fact]
    public void CreateDraftRoomForm_WhenRoomTabIsBlocked_ShouldShowOpenRoomLink()
    {
        _draftRoomApiService
            .Setup(service => service.CreateAsync(It.IsAny<CreateDraftRoomFormModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(
                DraftRoomApiResultKind.Success,
                new DraftRoomEntryDto
                {
                    RoomId = Guid.NewGuid(),
                    Code = "ABC123",
                    ParticipantId = Guid.NewGuid(),
                    DisplayName = "Guest Host",
                    IsGuest = true,
                    GuestSecret = "guest-secret",
                }));
        _roomTabOpener.Setup(opener => opener.OpenAsync(It.IsAny<string>())).ReturnsAsync(false);
        var form = Render<CreateDraftRoomForm>();
        FillValidGuestForm(form);

        form.Find("button[type='submit']").Click();

        var openRoomLink = form.Find("a");
        openRoomLink.TextContent.Should().Be(Strings.DraftRooms_OpenRoom);
        openRoomLink.GetAttribute("target").Should().Be("_blank");
        openRoomLink.GetAttribute("rel").Should().Contain("noopener");
    }

    [Fact]
    public void CreateDraftRoomForm_WhenRequestIsThrottled_ShouldShowThrottleMessage()
    {
        _draftRoomApiService
            .Setup(service => service.CreateAsync(It.IsAny<CreateDraftRoomFormModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(DraftRoomApiResultKind.Throttled, null));
        var form = Render<CreateDraftRoomForm>();
        FillValidGuestForm(form);

        form.Find("button[type='submit']").Click();

        form.Find("[role='alert']").TextContent.Should().Be(Strings.DraftRooms_CreateThrottled);
    }

    [Fact]
    public void CreateDraftRoomForm_WhenServerRejectsValues_ShouldShowValidationMessage()
    {
        _draftRoomApiService
            .Setup(service => service.CreateAsync(It.IsAny<CreateDraftRoomFormModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(DraftRoomApiResultKind.ValidationFailed, null));
        var form = Render<CreateDraftRoomForm>();
        FillValidGuestForm(form);

        form.Find("button[type='submit']").Click();

        form.Find("[role='alert']").TextContent.Should().Be(Strings.DraftRooms_CreateValidationFailed);
    }

    [Fact]
    public void CreateDraftRoomForm_WhenRequestFails_ShouldShowFailureMessage()
    {
        _draftRoomApiService
            .Setup(service => service.CreateAsync(It.IsAny<CreateDraftRoomFormModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftRoomEntryDto>(DraftRoomApiResultKind.Failed, null));
        var form = Render<CreateDraftRoomForm>();
        FillValidGuestForm(form);

        form.Find("button[type='submit']").Click();

        form.Find("[role='alert']").TextContent.Should().Be(Strings.DraftRooms_CreateFailed);
    }

    private static void FillValidGuestForm(IRenderedComponent<CreateDraftRoomForm> form)
    {
        form.Find("#draft-room-name").Change("Test draft");
        form.Find("#draft-room-password").Change("secret123");
        form.Find("#draft-room-display-name").Change("Guest Host");
    }
}
