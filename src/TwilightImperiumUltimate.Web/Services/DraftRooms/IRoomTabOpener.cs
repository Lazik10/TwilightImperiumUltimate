namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <summary>
/// Opens a draft room in a separate browser tab.
/// </summary>
public interface IRoomTabOpener
{
    /// <summary>
    /// Opens the supplied URL in a new tab, returning whether the browser allowed the tab.
    /// </summary>
    Task<bool> OpenAsync(string url);
}
