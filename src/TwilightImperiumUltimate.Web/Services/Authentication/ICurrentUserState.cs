using TwilightImperiumUltimate.Web.Models.Users;

namespace TwilightImperiumUltimate.Web.Services.Authentication;

/// <summary>
/// Tracks the currently signed-in user for the duration of the app session, so components that
/// need to react to login/logout (for example navigation and account menus) don't each have to
/// bootstrap and subscribe to authentication state independently.
/// </summary>
public interface ICurrentUserState
{
    /// <summary>
    /// Raised whenever <see cref="User"/> changes (login, logout, or an external
    /// authentication state change). Subscribers should call StateHasChanged.
    /// </summary>
    event Action? UserChanged;

    /// <summary>
    /// Gets the currently signed-in user, or null when no user is signed in.
    /// </summary>
    TwilightImperiumUser? User { get; }

    /// <summary>
    /// Loads the current user (attempting an automatic login if needed). Safe to call from
    /// multiple components; the underlying bootstrap work only runs once.
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    /// Logs the current user out and notifies subscribers.
    /// </summary>
    Task LogoutAsync();
}
