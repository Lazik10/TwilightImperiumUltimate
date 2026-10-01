using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Pages.Account;

public partial class Admin
{
    private AdminTab _activeTab = AdminTab.Roles;

    private void SelectTab(AdminTab tab) => _activeTab = tab;
}
