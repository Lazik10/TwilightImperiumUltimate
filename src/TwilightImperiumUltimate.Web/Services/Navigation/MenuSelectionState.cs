using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Services.Navigation;

public class MenuSelectionState : IMenuSelectionState
{
    public event Action? Changed;

    public MainMenuKey? ActiveMenuKey { get; private set; }

    public string? ActiveSubMenuKey { get; private set; }

    public MainMenuKey? HoveredMenuKey { get; private set; }

    public void SelectMenu(MainMenuKey menuKey)
    {
        ActiveMenuKey = menuKey;
        ActiveSubMenuKey = null;
        Changed?.Invoke();
    }

    public void SelectSubMenu(MainMenuKey menuKey, string subMenuKey)
    {
        ActiveMenuKey = menuKey;
        ActiveSubMenuKey = subMenuKey;
        Changed?.Invoke();
    }

    public void ClearSelection()
    {
        ActiveMenuKey = null;
        ActiveSubMenuKey = null;
        Changed?.Invoke();
    }

    public void SetHoveredMenu(MainMenuKey menuKey)
    {
        if (HoveredMenuKey == menuKey)
            return;

        HoveredMenuKey = menuKey;
        Changed?.Invoke();
    }

    public void ClearHoveredMenu(MainMenuKey menuKey)
    {
        if (HoveredMenuKey != menuKey)
            return;

        HoveredMenuKey = null;
        Changed?.Invoke();
    }
}
