using TwilightImperiumUltimate.Web.Resources;
using PageRoutes = TwilightImperiumUltimate.Web.Pages.Pages;

namespace TwilightImperiumUltimate.Web.Components.Shared.Navigation;

/// <summary>
/// Single source of truth for the links shown in the logged-in account menu, shared by the
/// desktop <c>AccountMenu</c> dropdown and the mobile navigation's account section so both stay
/// in sync without duplicating href/text/role data in two places.
/// </summary>
public static class AccountLinks
{
    /// <summary>
    /// Gets the links shown once a user is signed in (excludes Logout, which is always a distinct
    /// action button rendered by the caller).
    /// </summary>
    public static IReadOnlyList<AccountLinkItem> LoggedIn =>
    [
        new(PageRoutes.AccountInfo, "accountinfo", Strings.Page_AccountInfo),
        new(PageRoutes.Admin, "admin", Strings.Page_Admin, Roles: "Admin"),
        new(PageRoutes.TiglAdmin, "tigladmin", Strings.Page_TiglAdmin, Roles: "TiglAdmin"),
        new(PageRoutes.ResendPasswordRecoveryEmail, "passwordreset", Strings.Page_AccountPasswordReset),
    ];
}
