using TwilightImperiumUltimate.Web.Components.DraftRooms;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.ComponentTests.Components.DraftRooms;

public sealed class DraftRoomConnectionStatusTests : BunitContext
{
    [Theory]
    [InlineData(DraftRoomConnectionState.Connected, "Connected to live updates")]
    [InlineData(DraftRoomConnectionState.Reconnecting, "Reconnecting to live updates")]
    [InlineData(DraftRoomConnectionState.Offline, "Disconnected from live updates")]
    public void DraftRoomConnectionStatus_WhenStateChanges_ShouldShowAccessibleStatus(
        DraftRoomConnectionState state,
        string expectedText)
    {
        var component = Render<DraftRoomConnectionStatus>(parameters =>
            parameters.Add(parameter => parameter.State, state));

        component.Find("[role='status']").TextContent.Should().Contain(expectedText);
    }
}
