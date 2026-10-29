using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Components.DraftRooms;
using TwilightImperiumUltimate.Web.Models.Users;
using TwilightImperiumUltimate.Web.Services.Authentication;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.ComponentTests.Components.DraftRooms;

public sealed class MyDraftsListTests : BunitContext
{
    private readonly Mock<ICurrentUserState> _user = new();
    private readonly Mock<IDraftRoomApiService> _api = new(MockBehavior.Strict);

    public MyDraftsListTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        _user.Setup(state => state.InitializeAsync()).Returns(Task.CompletedTask);
        _user.SetupGet(state => state.User).Returns(new TwilightImperiumUser { Id = "account", UserName = "Account" });
        Services.AddSingleton(_user.Object);
        Services.AddSingleton(_api.Object);
        Services.AddSingleton(Mock.Of<IPathProvider>());
        Services.AddSingleton(Mock.Of<ITwilightImperiumApiHttpClient>());
        Services.AddSingleton(Mock.Of<ILocalStorageService>());
        Services.AddSingleton(Mock.Of<IMapper>());
    }

    [Fact]
    public void MyDrafts_WhenGuest_ShouldNotRenderHistoryOrRequestAccountData()
    {
        _user.SetupGet(state => state.User).Returns((TwilightImperiumUser?)null);

        var list = Render<MyDraftsList>();

        list.Markup.Should().BeEmpty();
        _api.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MyDrafts_WhenAccountHasResults_ShouldShowPublicLinksAndEffectiveDeletePermission(bool canDelete)
    {
        var item = CreateItem(canDelete);
        this.SetupHistory([item]);

        var list = Render<MyDraftsList>();

        list.Find("a").GetAttribute("href").Should().Be($"/tools/draft-rooms/results/{item.ShareToken}");
        list.Find("a").TextContent.Should().Be("Friday draft");
        list.FindAll("button[aria-label='Delete results: Friday draft']").Should().HaveCount(canDelete ? 1 : 0);
        list.FindAll("button").Single(button => button.TextContent.Trim() == "Previous drafts").HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void MyDrafts_WhenNextPageSelected_ShouldFetchRequestedPageAndMoveFocusToHistorySection()
    {
        this.SetupHistory([CreateItem()], hasMore: true);
        _api.Setup(service => service.GetMyResultsAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftResultHistoryDto>(DraftRoomApiResultKind.Success, new DraftResultHistoryDto([], 2, false)));
        var list = Render<MyDraftsList>();

        list.FindAll("button").Single(button => button.TextContent.Trim() == "Next drafts").Click();

        list.FindAll("[role=status]").Should().Contain(element => element.TextContent == "Page 2");
        list.FindAll("button").Single(button => button.TextContent.Trim() == "Next drafts").HasAttribute("disabled").Should().BeTrue();
        JSInterop.Invocations.Should().Contain(invocation => invocation.Identifier == "Blazor._internal.domWrapper.focus");
    }

    [Fact]
    public void MyDrafts_WhenLoadingFails_ShouldOfferRetryAndRecover()
    {
        _api.SetupSequence(service => service.GetMyResultsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftResultHistoryDto>(DraftRoomApiResultKind.Failed, null))
            .ReturnsAsync(new DraftRoomApiResult<DraftResultHistoryDto>(DraftRoomApiResultKind.Success, new DraftResultHistoryDto([], 1, false)));
        var list = Render<MyDraftsList>();
        list.Find("[role=alert]").TextContent.Should().Contain("could not be loaded");

        list.Find("button").Click();

        list.FindAll("[role=alert]").Should().BeEmpty();
        list.Markup.Should().Contain("No finished drafts");
    }

    [Fact]
    public void MyDrafts_WhenDeleteConfirmed_ShouldRemoveResultAndRestoreFocusWithoutDeletingRoom()
    {
        var item = CreateItem(canDelete: true);
        _api.SetupSequence(service => service.GetMyResultsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftResultHistoryDto>(DraftRoomApiResultKind.Success, new DraftResultHistoryDto([item], 1, false)))
            .ReturnsAsync(new DraftRoomApiResult<DraftResultHistoryDto>(DraftRoomApiResultKind.Success, new DraftResultHistoryDto([], 1, false)));
        _api.Setup(service => service.DeleteResultAsync(item.Id, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiCommandResult(DraftRoomApiResultKind.Success));
        var list = Render<MyDraftsList>();

        list.Find("button[aria-label='Delete results: Friday draft']").Click();
        _api.Verify(service => service.DeleteResultAsync(It.IsAny<Guid>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Never);
        list.FindAll("dialog button").Single(button => button.TextContent.Trim() == "Delete results").Click();

        list.FindAll("a").Should().BeEmpty();
        list.Markup.Should().Contain("No finished drafts");
        JSInterop.Invocations.Should().Contain(invocation => invocation.Identifier == "Blazor._internal.domWrapper.focus");
        _api.VerifyAll();
    }

    [Fact]
    public async Task MyDrafts_WhenAccountChangesDuringLoad_ShouldNotRenderPreviousAccountsHistory()
    {
        var pending = new TaskCompletionSource<DraftRoomApiResult<DraftResultHistoryDto>>(TaskCreationOptions.RunContinuationsAsynchronously);
        _api.Setup(service => service.GetMyResultsAsync(1, It.IsAny<CancellationToken>())).Returns(pending.Task);
        var list = Render<MyDraftsList>();
        list.Find("[role=status]").TextContent.Should().Contain("Loading");

        _user.SetupGet(state => state.User).Returns((TwilightImperiumUser?)null);
        await list.InvokeAsync(() => _user.Raise(state => state.UserChanged += null));
        await list.InvokeAsync(() => pending.SetResult(new DraftRoomApiResult<DraftResultHistoryDto>(
            DraftRoomApiResultKind.Success, new DraftResultHistoryDto([CreateItem()], 1, false))));

        list.WaitForAssertion(() => list.Markup.Should().BeEmpty());
    }

    private static DraftResultHistoryItemDto CreateItem(bool canDelete = false) =>
        new(Guid.NewGuid(), "aBcDeF0123456789_-abcd", DraftTypeId.FactionPick, "Friday draft", DateTime.UnixEpoch, canDelete);

    private void SetupHistory(IReadOnlyList<DraftResultHistoryItemDto> items, bool hasMore = false) =>
        _api.Setup(service => service.GetMyResultsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<DraftResultHistoryDto>(DraftRoomApiResultKind.Success, new DraftResultHistoryDto(items, 1, hasMore)));
}
