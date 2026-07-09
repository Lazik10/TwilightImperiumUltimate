namespace TwilightImperiumUltimate.Web.Components.Shared.Navigation;

/// <summary>
/// A single link shown in the logged-in account menu (desktop <c>AccountMenu</c> dropdown and the
/// mobile navigation's account section).
/// </summary>
/// <param name="Href">The link destination.</param>
/// <param name="Key">A stable identifier used to track active state for this link.</param>
/// <param name="Text">The localized link text.</param>
/// <param name="Roles">
/// An optional comma-separated role list. When set, the link is only rendered for users in one of
/// these roles (via <c>AuthorizeView</c>). When null, the link is shown to any signed-in user.
/// </param>
public sealed record AccountLinkItem(string Href, string Key, string Text, string? Roles = null);
