namespace TwilightImperiumUltimate.Web.Components.MiltyDraft;

public partial class MiltyDraftGrid
{
    private MiltyDraftMenuItem _selectedMenuItem = MiltyDraftMenuItem.Draft;

    private void OnMenuItemClick(MiltyDraftMenuItem menuItem)
    {
        _selectedMenuItem = menuItem;
    }
}