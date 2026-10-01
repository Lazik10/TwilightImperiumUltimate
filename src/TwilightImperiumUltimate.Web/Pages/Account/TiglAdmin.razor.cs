namespace TwilightImperiumUltimate.Web.Pages.Account;

public partial class TiglAdmin
{
    private TiglAdminTab _activeTab = TiglAdminTab.Seasons;

    private void SelectTab(TiglAdminTab tab) => _activeTab = tab;
}
