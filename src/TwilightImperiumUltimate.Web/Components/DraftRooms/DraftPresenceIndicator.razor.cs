using System.Globalization;
using System.Resources;
using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms;

public partial class DraftPresenceIndicator
{
    /// <summary>
    /// Gets or sets whether the participant is currently online.
    /// </summary>
    [Parameter]
    public bool IsOnline { get; set; }

    private string StatusText => GetResourceText(
        this.IsOnline ? "DraftRooms_PresenceOnline" : "DraftRooms_PresenceOffline");

    private static string GetResourceText(string resourceKey) =>
        Strings.ResourceManager.GetString(resourceKey, CultureInfo.CurrentUICulture)
        ?? throw new MissingManifestResourceException($"The resource '{resourceKey}' is missing.");
}
