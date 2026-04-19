using Microsoft.AspNetCore.Components.Authorization;
using TwilightImperiumUltimate.Web.Models.Users;
using TwilightImperiumUltimate.Web.Services.Authentication;
using TwilightImperiumUltimate.Web.Services.Navigation;
using TwilightImperiumUltimate.Web.Services.User;

namespace TwilightImperiumUltimate.Web.Components.Shared;

public partial class MainNavigation : IDisposable
{
    private TwilightImperiumUser? _user;
    private bool _disposed;

    private bool _isMobileMenuVisible;
    private bool _isAccountSectionVisible;
    private bool _isGameSectionVisible;
    private bool _isCommunitySectionVisible;
    private bool _isTiglSectionVisible;
    private bool _isToolsSectionVisible;
    private bool _isRulesSectionVisible;
    private string? _hoveredMainMenuKey;
    private string? _hoveredSubMenuKey;

    [Inject]
    private IUserService UserService { get; set; } = default!;

    [Inject]
    private ILoginService LoginService { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IMenuSelectionState MenuSelectionState { get; set; } = default!;

    ~MainNavigation()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            AuthenticationStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;

        _disposed = true;
    }

    protected override async Task OnInitializedAsync()
    {
        var relativePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        if (string.IsNullOrWhiteSpace(relativePath) && string.IsNullOrWhiteSpace(MenuSelectionState.ActiveMenuKey))
            MenuSelectionState.SelectMenu("news");

        _user = await UserService.GetCurrentUserAsync();

        if (_user is null)
        {
            var loginSuccess = await LoginService.TryAutomaticLoginAsync(CancellationToken.None);
            if (loginSuccess)
                _user = await UserService.GetCurrentUserAsync();
        }

        AuthenticationStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
    }

    private async void OnAuthenticationStateChanged(Task<AuthenticationState> _)
    {
        _user = await UserService.GetCurrentUserAsync();
        StateHasChanged();
    }

    private string GetAccountHeaderText() => _user?.UserName ?? Strings.Page_AccountLogin;

    private bool IsActiveMenu(string menuKey) => string.Equals(MenuSelectionState.ActiveMenuKey, menuKey, StringComparison.OrdinalIgnoreCase);

    private bool IsActiveSubMenu(string subMenuKey) => string.Equals(MenuSelectionState.ActiveSubMenuKey, subMenuKey, StringComparison.OrdinalIgnoreCase);

    private string GetMainMenuClass(string menuKey) => IsActiveMenu(menuKey) ? "menu-link-active" : string.Empty;

    private string GetSubMenuClass(string subMenuKey) => IsActiveSubMenu(subMenuKey) ? "submenu-link-active" : string.Empty;

    private string GetDesktopMainMenuClass(string menuKey)
    {
        if (!string.IsNullOrWhiteSpace(_hoveredMainMenuKey))
        {
            return string.Equals(_hoveredMainMenuKey, menuKey, StringComparison.OrdinalIgnoreCase)
                ? "menu-link-active"
                : string.Empty;
        }

        return GetMainMenuClass(menuKey);
    }

    private string GetDesktopSubMenuClass(string subMenuKey)
    {
        if (!string.IsNullOrWhiteSpace(_hoveredSubMenuKey))
        {
            return string.Equals(_hoveredSubMenuKey, subMenuKey, StringComparison.OrdinalIgnoreCase)
                ? "submenu-link-active"
                : string.Empty;
        }

        return GetSubMenuClass(subMenuKey);
    }

    private void OnMainMenuHoverStart(string menuKey)
    {
        _hoveredMainMenuKey = menuKey;
        _hoveredSubMenuKey = null;
    }

    private void OnSubMenuHoverStart(string menuKey, string subMenuKey)
    {
        _hoveredMainMenuKey = menuKey;
        _hoveredSubMenuKey = subMenuKey;
    }

    private void OnDesktopHoverEnd()
    {
        _hoveredMainMenuKey = null;
        _hoveredSubMenuKey = null;
    }

    private void SelectMenu(string menuKey)
    {
        MenuSelectionState.SelectMenu(menuKey);

        if (_isMobileMenuVisible)
            CollapseAllSections();
    }

    private void SelectSubMenu(string menuKey, string subMenuKey)
    {
        MenuSelectionState.SelectSubMenu(menuKey, subMenuKey);

        if (_isMobileMenuVisible)
            CollapseAllSections();
    }

    private void SelectMenuAndClose(string menuKey)
    {
        SelectMenu(menuKey);
        _isMobileMenuVisible = false;
    }

    private void SelectSubMenuAndClose(string menuKey, string subMenuKey)
    {
        SelectSubMenu(menuKey, subMenuKey);
        _isMobileMenuVisible = false;
    }

    private void ToggleMobileMenu()
    {
        _isMobileMenuVisible = !_isMobileMenuVisible;

        if (!_isMobileMenuVisible)
            CollapseAllSections();
    }

    private void ToggleAccountSection()
    {
        var nextState = !_isAccountSectionVisible;
        CollapseAllSections();
        MenuSelectionState.SelectMenu("account");
        _isAccountSectionVisible = nextState;
    }

    private void ToggleGameSection()
    {
        var nextState = !_isGameSectionVisible;
        CollapseAllSections();
        MenuSelectionState.SelectMenu("game");
        _isGameSectionVisible = nextState;
    }

    private void ToggleCommunitySection()
    {
        var nextState = !_isCommunitySectionVisible;
        CollapseAllSections();
        MenuSelectionState.SelectMenu("community");
        _isCommunitySectionVisible = nextState;
    }

    private void ToggleTiglSection()
    {
        var nextState = !_isTiglSectionVisible;
        CollapseAllSections();
        MenuSelectionState.SelectMenu("tigl");
        _isTiglSectionVisible = nextState;
    }

    private void ToggleToolsSection()
    {
        var nextState = !_isToolsSectionVisible;
        CollapseAllSections();
        MenuSelectionState.SelectMenu("tools");
        _isToolsSectionVisible = nextState;
    }

    private void ToggleRulesSection()
    {
        var nextState = !_isRulesSectionVisible;
        CollapseAllSections();
        MenuSelectionState.SelectMenu("rules");
        _isRulesSectionVisible = nextState;
    }

    private async Task Logout()
    {
        _user = null;
        await LoginService.LogoutAsync();
        var twilightImperiumAuthStateProvider = AuthenticationStateProvider as TwilightImperiumAuthenticationStateProvider;
        twilightImperiumAuthStateProvider?.NotifyUserLogout();
        StateHasChanged();
    }

    private void CollapseAllSections()
    {
        _isAccountSectionVisible = false;
        _isGameSectionVisible = false;
        _isCommunitySectionVisible = false;
        _isTiglSectionVisible = false;
        _isToolsSectionVisible = false;
        _isRulesSectionVisible = false;
    }
}
