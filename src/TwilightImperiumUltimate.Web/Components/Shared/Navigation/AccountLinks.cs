using TwilightImperiumUltimate.Web.Models.Users;
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
    public static IReadOnlyList<AccountLinkItem> LoggedIn(TwilightImperiumUser? user)
    {
        var links = new List<AccountLinkItem>
        {
            new(PageRoutes.AccountInfo, "accountinfo", Strings.Page_AccountInfo),
        };

        if (user?.AsyncPlayerProfileId is int asyncPlayerProfileId && asyncPlayerProfileId > 0)
        {
            links.Add(new(
                $"{PageRoutes.AsyncProfile}?playerId={asyncPlayerProfileId}",
                "asyncprofile",
                Strings.Account_AsyncProfile));
        }

        if (user?.TiglPlayerProfileId is int tiglPlayerProfileId && tiglPlayerProfileId > 0)
        {
            links.Add(new(
                $"{PageRoutes.TiglPlayerProfile}?playerId={tiglPlayerProfileId}",
                "tiglprofile",
                Strings.Account_TiglProfile));
        }

        links.Add(new(PageRoutes.Admin, "admin", Strings.Page_Admin, Roles: "Admin"));
        links.Add(new(PageRoutes.TiglAdmin, "tigladmin", Strings.Page_TiglAdmin, Roles: "TiglAdmin"));
        links.Add(new(PageRoutes.ResendPasswordRecoveryEmail, "passwordreset", Strings.Page_AccountPasswordReset));

        return links;
    }
}
