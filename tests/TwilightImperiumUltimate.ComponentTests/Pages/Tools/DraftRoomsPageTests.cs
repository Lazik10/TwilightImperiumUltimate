using AutoMapper;
using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Pages.Tools;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.Authentication;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;
using TwilightImperiumUltimate.Web.Validators.DraftRooms;

namespace TwilightImperiumUltimate.ComponentTests.Pages.Tools;

public sealed class DraftRoomsPageTests : BunitContext
{
    public DraftRoomsPageTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(Mock.Of<IPathProvider>());
        Services.AddSingleton(Mock.Of<ITwilightImperiumApiHttpClient>());
        Services.AddSingleton(Mock.Of<ILocalStorageService>());
        Services.AddSingleton(Mock.Of<IMapper>());
        var currentUserState = Mock.Of<ICurrentUserState>(state => state.User == null);
        Mock.Get(currentUserState).Setup(state => state.InitializeAsync()).Returns(Task.CompletedTask);
        Services.AddSingleton(currentUserState);
        var draftRoomApiService = new Mock<IDraftRoomApiService>();
        draftRoomApiService
            .Setup(service => service.GetMyRoomsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<IReadOnlyList<MyDraftRoomDto>>(
                DraftRoomApiResultKind.Success,
                Array.Empty<MyDraftRoomDto>()));
        Services.AddSingleton(draftRoomApiService.Object);
        var guestIdentityStore = new Mock<IGuestIdentityStore>();
        guestIdentityStore
            .Setup(store => store.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<GuestIdentity>());
        Services.AddSingleton(guestIdentityStore.Object);
        Services.AddSingleton(Mock.Of<IRoomTabOpener>());
        Services.AddSingleton<IValidator<CreateDraftRoomFormModel>, CreateDraftRoomFormModelValidator>();
        Services.AddSingleton<IValidator<JoinDraftRoomFormModel>, JoinDraftRoomFormModelValidator>();
    }

    [Fact]
    public void DraftRooms_WhenRendered_ShouldShowBothActionsWithoutASelection()
    {
        var page = Render<DraftRooms>();

        page.FindAll("button").Should().HaveCount(2);
        page.Find("button[aria-pressed='false']").TextContent.Should().Be("Create Draft");
        page.FindAll("h2").Select(heading => heading.TextContent)
            .Should()
            .ContainSingle()
            .Which.Should().Be(Strings.DraftRooms_MyRoomsHeading);
    }

    [Fact]
    public void DraftRooms_WhenCreateIsSelected_ShouldShowCreateSection()
    {
        var page = Render<DraftRooms>();

        page.FindAll("button")[0].Click();

        page.FindAll("h2").Select(heading => heading.TextContent).Should().Contain("Create Draft");
        page.Find("button[aria-pressed='true']").TextContent.Should().Be("Create Draft");
        JSInterop.Invocations.Should().Contain(invocation => invocation.Identifier == "Blazor._internal.domWrapper.focus");
    }

    [Fact]
    public void DraftRooms_WhenJoinIsSelected_ShouldShowJoinSection()
    {
        var page = Render<DraftRooms>();

        page.FindAll("button")[1].Click();

        page.FindAll("h2").Select(heading => heading.TextContent).Should().Contain("Join Draft");
        page.Find("button[aria-pressed='true']").TextContent.Should().Be("Join Draft");
        JSInterop.Invocations.Should().Contain(invocation => invocation.Identifier == "Blazor._internal.domWrapper.focus");
    }
}
