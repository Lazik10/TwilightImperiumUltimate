using System.Globalization;
using System.Resources;
using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms;

public partial class DraftRoomConnectionStatus
{
    /// <summary>
    /// Gets or sets the current room hub connection state.
    /// </summary>
    [Parameter]
    public DraftRoomConnectionState State { get; set; }

    private string StatusClass =>
        $"draft-room-connection-status draft-room-connection-status--{this.State.ToString().ToLowerInvariant()}";

    private string StatusText => GetResourceText(this.State switch
    {
        DraftRoomConnectionState.Connected => "DraftRooms_ConnectionConnected",
        DraftRoomConnectionState.Reconnecting => "DraftRooms_ConnectionReconnecting",
        _ => "DraftRooms_ConnectionOffline",
    });

    private static string GetResourceText(string resourceKey) =>
        Strings.ResourceManager.GetString(resourceKey, CultureInfo.CurrentUICulture)
        ?? throw new MissingManifestResourceException($"The resource '{resourceKey}' is missing.");
}
