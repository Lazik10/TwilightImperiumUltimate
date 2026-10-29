using AutoMapper;
using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Web.Components.DraftRooms;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;
using TwilightImperiumUltimate.Web.Validators.DraftRooms;

namespace TwilightImperiumUltimate.ComponentTests.Components.DraftRooms;

public sealed class DraftRoomHostSettingsTests : BunitContext
{
    private readonly Mock<IDraftRoomSession> _session = new();
    private readonly Mock<IDraftRoomApiService> _apiService = new();

    public DraftRoomHostSettingsTests()
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
            .Setup(service => service.UpdateSettingsAsync(
                "ABC234",
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Success));
        _apiService
            .Setup(service => service.ChangePasswordAsync(
                "ABC234",
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Success));
        Services.AddSingleton(Mock.Of<IPathProvider>());
        Services.AddSingleton(Mock.Of<ITwilightImperiumApiHttpClient>());
        Services.AddSingleton(Mock.Of<ILocalStorageService>());
        Services.AddSingleton(Mock.Of<IMapper>());
        Services.AddSingleton(_apiService.Object);
        Services.AddSingleton<IValidator<UpdateDraftRoomNameFormModel>, UpdateDraftRoomNameFormModelValidator>();
        Services.AddSingleton<IValidator<ChangeDraftRoomPasswordFormModel>, ChangeDraftRoomPasswordFormModelValidator>();
    }

    [Fact]
    public void DraftRoomHostSettings_WhenParticipantCannotChangeSettings_ShouldHideSettings()
    {
        var page = RenderSettings(canChangeSettings: false);

        page.Markup.Should().BeEmpty();
    }

    [Fact]
    public void DraftRoomHostSettings_WhenParticipantCanChangeSettings_ShouldShowNameAndAccessibleLockSwitch()
    {
        var page = RenderSettings(canChangeSettings: true);

        page.Find("label[for='draft-room-settings-name']")
            .TextContent.Should()
            .Contain(Strings.DraftRooms_RoomName);
        page.Find($"[aria-label='{Strings.DraftRooms_LockRoomAriaLabel}']").Should().NotBeNull();
        page.Markup.Should().Contain(Strings.DraftRooms_SettingsHeading);
    }

    [Fact]
    public void DraftRoomHostSettings_WhenRoomNameIsInvalid_ShouldShowValidationMessageWithoutSaving()
    {
        var page = RenderSettings(canChangeSettings: true);
        page.Find("#draft-room-settings-name").Change(" ");

        page.Find("button[type='submit']").Click();

        page.FindAll(".responsive-form-field-error")
            .Select(message => message.TextContent)
            .Should()
            .Contain(ValidationMessages.DraftRooms_RoomNameRequired);
        _apiService.Verify(
            service => service.UpdateSettingsAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void DraftRoomHostSettings_WhenRoomNameIsValid_ShouldSaveTrimmedNameAndKeepLockState()
    {
        var page = RenderSettings(canChangeSettings: true, isLocked: true);
        page.Find("#draft-room-settings-name").Change("  Updated room  ");

        page.Find("button[type='submit']").Click();

        _apiService.Verify(
            service => service.UpdateSettingsAsync(
                "ABC234",
                "Updated room",
                true,
                It.IsAny<CancellationToken>()),
            Times.Once);
        _session.Verify(
            session => session.ExecuteAsync(
                It.IsAny<Func<Task<DraftRoomApiCommandResult>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void DraftRoomHostSettings_WhenLockSwitchChanges_ShouldUpdateRoomLockState()
    {
        var page = RenderSettings(canChangeSettings: true);
        page.Find($"[aria-label='{Strings.DraftRooms_LockRoomAriaLabel}']").Click();

        _apiService.Verify(
            service => service.UpdateSettingsAsync(
                "ABC234",
                "Test room",
                true,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void DraftRoomHostSettings_WhenPasswordsDoNotMatch_ShouldShowValidationMessageWithoutChangingPassword()
    {
        var page = RenderSettings(canChangeSettings: true);
        OpenPasswordDialog(page);
        page.Find("#draft-room-new-password").Change("secret123");
        page.Find("#draft-room-confirm-password").Change("different123");

        page.Find("[role='dialog'] button[type='submit']").Click();

        page.FindAll(".responsive-form-field-error")
            .Select(message => message.TextContent)
            .Should()
            .Contain(ValidationMessages.ResetPassword__ConfirmPasswordMatch);
        _apiService.Verify(
            service => service.ChangePasswordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void DraftRoomHostSettings_WhenPasswordChangeSucceeds_ShouldCloseDialog()
    {
        var page = RenderSettings(canChangeSettings: true);
        OpenPasswordDialog(page);
        page.Find("#draft-room-new-password").Change("secret123");
        page.Find("#draft-room-confirm-password").Change("secret123");

        page.Find("[role='dialog'] button[type='submit']").Click();

        _apiService.Verify(
            service => service.ChangePasswordAsync("ABC234", "secret123", It.IsAny<CancellationToken>()),
            Times.Once);
        page.FindAll("[role='dialog']").Should().BeEmpty();
    }

    [Fact]
    public void DraftRoomHostSettings_WhenPasswordDialogReceivesEscape_ShouldCloseAndRestoreNormalState()
    {
        var page = RenderSettings(canChangeSettings: true);
        OpenPasswordDialog(page);

        page.Find("[role='dialog']").TriggerEvent("onkeydown", new KeyboardEventArgs { Key = "Escape" });

        page.FindAll("[role='dialog']").Should().BeEmpty();
        _apiService.Verify(
            service => service.ChangePasswordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void DraftRoomHostSettings_WhenPasswordChangeFails_ShouldKeepDialogOpenAndShowError()
    {
        _apiService
            .Setup(service => service.ChangePasswordAsync(
                "ABC234",
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Failed));
        var page = RenderSettings(canChangeSettings: true);
        OpenPasswordDialog(page);
        page.Find("#draft-room-new-password").Change("secret123");
        page.Find("#draft-room-confirm-password").Change("secret123");

        page.Find("[role='dialog'] button[type='submit']").Click();

        page.Find("[role='dialog']").Should().NotBeNull();
        page.Find("[role='dialog'] [role='alert']").TextContent.Should().Be(Strings.DraftRooms_SettingsActionFailed);
    }

    private static void OpenPasswordDialog(IRenderedComponent<DraftRoomHostSettings> page)
    {
        page.FindAll("button")
            .Single(button => button.TextContent.Trim() == Strings.DraftRooms_ChangePassword)
            .Click();
        page.Find("[role='dialog']").Should().NotBeNull();
    }

    private static DraftRoomViewDto CreateRoomView(bool canChangeSettings, bool isLocked) => new()
    {
        Code = "ABC234",
        Name = "Test room",
        IsLocked = isLocked,
        Me = new DraftRoomViewMeDto
        {
            CanChangeSettings = canChangeSettings,
        },
    };

    private IRenderedComponent<DraftRoomHostSettings> RenderSettings(bool canChangeSettings, bool isLocked = false)
    {
        _session.SetupGet(session => session.View).Returns(CreateRoomView(canChangeSettings, isLocked));
        var host = Render<CascadingValue<IDraftRoomSession>>(parameters => parameters
            .Add(parameter => parameter.Value, _session.Object)
            .AddChildContent<DraftRoomHostSettings>());

        return host.FindComponent<DraftRoomHostSettings>();
    }
}
