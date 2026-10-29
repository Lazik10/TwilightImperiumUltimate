using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Components.DraftRooms;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Models.Users;
using TwilightImperiumUltimate.Web.Services.Authentication;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.ComponentTests.Components.DraftRooms;

public sealed class MyRoomsListTests : BunitContext
{
    private readonly Mock<ICurrentUserState> _currentUserState = new();
    private readonly Mock<IDraftRoomApiService> _draftRoomApiService = new();
    private readonly Mock<IGuestIdentityStore> _guestIdentityStore = new();
    private readonly Mock<IRoomTabOpener> _roomTabOpener = new();

    public MyRoomsListTests()
    {
        _currentUserState.Setup(state => state.InitializeAsync()).Returns(Task.CompletedTask);
        _currentUserState.SetupGet(state => state.User).Returns((TwilightImperiumUser?)null);
        _guestIdentityStore
            .Setup(store => store.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<GuestIdentity>());

        Services.AddSingleton(_currentUserState.Object);
        Services.AddSingleton(_draftRoomApiService.Object);
        Services.AddSingleton(_guestIdentityStore.Object);
        Services.AddSingleton(_roomTabOpener.Object);
        Services.AddSingleton(Mock.Of<IPathProvider>());
        Services.AddSingleton(Mock.Of<ITwilightImperiumApiHttpClient>());
        Services.AddSingleton(Mock.Of<ILocalStorageService>());
        Services.AddSingleton(Mock.Of<IMapper>());
    }

    [Fact]
    public void MyRoomsList_WhenAccountHasRooms_ShouldRenderApiRoomDetails()
    {
        _currentUserState
            .SetupGet(state => state.User)
            .Returns(new TwilightImperiumUser { Id = "account-id", UserName = "Account" });
        MyDraftRoomDto[] rooms =
        [
            new()
            {
                Code = "ABC234",
                Name = "Friday TI",
                Status = DraftRoomStatus.InProgress,
                Role = DraftRoomRole.Member,
                SeatIndex = 0,
            },
        ];
        _draftRoomApiService
            .Setup(service => service.GetMyRoomsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<IReadOnlyList<MyDraftRoomDto>>(
                DraftRoomApiResultKind.Success,
                rooms));

        var component = Render<MyRoomsList>();

        component.Markup.Should().Contain("Friday TI");
        component.Markup.Should().Contain("ABC234");
        component.Markup.Should().Contain("Status: InProgress");
        component.Markup.Should().Contain("Role: Member");
        component.Markup.Should().Contain("Seat 1");
        component.FindAll("button").Should().ContainSingle();
        _guestIdentityStore.Verify(
            store => store.ListAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void MyRoomsList_WhenGuestHasSavedRoom_ShouldRenderRoomAndOpenItInNewTab()
    {
        var identity = CreateIdentity("ABC234", "Guest Draft");
        _guestIdentityStore
            .Setup(store => store.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([identity]);
        _roomTabOpener
            .Setup(opener => opener.OpenAsync("/tools/draft-rooms/ABC234"))
            .ReturnsAsync(true);

        var component = Render<MyRoomsList>();

        component.Markup.Should().Contain("Guest Draft");
        component.Markup.Should().Contain("ABC234");
        component.Find("button[aria-label='Enter Guest Draft']").Click();

        _roomTabOpener.Verify(opener => opener.OpenAsync("/tools/draft-rooms/ABC234"), Times.Once);
        _draftRoomApiService.Verify(
            service => service.GetMyRoomsAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void MyRoomsList_WhenGuestForgetsRoom_ShouldRemoveItFromStorageAndList()
    {
        var identity = CreateIdentity("ABC234", "Guest Draft");
        _guestIdentityStore
            .Setup(store => store.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([identity]);

        var component = Render<MyRoomsList>();

        component.Find("button[aria-label='Forget Guest Draft']").Click();

        _guestIdentityStore.Verify(
            store => store.RemoveAsync("ABC234", It.IsAny<CancellationToken>()),
            Times.Once);
        component.FindAll("li").Should().BeEmpty();
    }

    [Fact]
    public void MyRoomsList_WhenAccountRequestFails_ShouldShowAccessibleErrorAndRetry()
    {
        _currentUserState
            .SetupGet(state => state.User)
            .Returns(new TwilightImperiumUser { Id = "account-id", UserName = "Account" });
        _draftRoomApiService
            .Setup(service => service.GetMyRoomsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<IReadOnlyList<MyDraftRoomDto>>(
                DraftRoomApiResultKind.Failed,
                null));

        var component = Render<MyRoomsList>();

        component.Find("[role='alert']").TextContent.Should().Contain("could not be loaded");
        component.Find("button").TextContent.Should().Be("Retry");
    }

    private static GuestIdentity CreateIdentity(string code, string roomName) =>
        new(Guid.NewGuid(), code, Guid.NewGuid(), "guest-secret", "Guest Player", DateTimeOffset.UtcNow)
        {
            RoomName = roomName,
        };
}
