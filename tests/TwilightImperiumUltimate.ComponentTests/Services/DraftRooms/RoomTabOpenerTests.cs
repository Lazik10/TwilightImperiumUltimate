using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.ComponentTests.Services.DraftRooms;

public sealed class RoomTabOpenerTests : BunitContext
{
    [Fact]
    public async Task OpenAsync_WhenBrowserOpensTab_ShouldReturnTrue()
    {
        var module = JSInterop.SetupModule("./js/draft-room-tab.js");
        module.Setup<bool>("openDraftRoom", _ => true).SetResult(true);
        await using var opener = new RoomTabOpener(JSInterop.JSRuntime);

        var result = await opener.OpenAsync("/tools/draft-rooms/ABC234");

        result.Should().BeTrue();
    }

    [Fact]
    public async Task OpenAsync_WhenBrowserBlocksTab_ShouldReturnFalse()
    {
        var module = JSInterop.SetupModule("./js/draft-room-tab.js");
        module.Setup<bool>("openDraftRoom", _ => true).SetResult(false);
        await using var opener = new RoomTabOpener(JSInterop.JSRuntime);

        var result = await opener.OpenAsync("/tools/draft-rooms/ABC234");

        result.Should().BeFalse();
    }
}
