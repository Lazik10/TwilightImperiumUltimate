namespace TwilightImperiumUltimate.Web.Services.Navigation;

public interface IMenuSelectionState
{
    string? ActiveMenuKey { get; }

    string? ActiveSubMenuKey { get; }

    void SelectMenu(string menuKey);

    void SelectSubMenu(string menuKey, string subMenuKey);
}
