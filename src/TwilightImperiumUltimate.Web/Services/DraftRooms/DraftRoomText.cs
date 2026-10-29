using System.Globalization;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

internal static class DraftRoomText
{
    internal static string Get(string key) => Strings.ResourceManager.GetString($"DraftRooms_{key}", CultureInfo.CurrentUICulture)
        ?? throw new InvalidOperationException($"Missing draft-room resource '{key}'.");
}
