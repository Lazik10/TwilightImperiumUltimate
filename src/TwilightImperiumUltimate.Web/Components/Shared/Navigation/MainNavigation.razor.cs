using TwilightImperiumUltimate.Web.Services.Authentication;
using TwilightImperiumUltimate.Web.Services.Navigation;
using PageRoutes = TwilightImperiumUltimate.Web.Pages.Pages;

namespace TwilightImperiumUltimate.Web.Components.Shared.Navigation;

public partial class MainNavigation : IDisposable
{
    private bool _disposed;

    private bool _isMobileMenuVisible;
    private bool _isAccountSectionVisible;
    private bool _isGameSectionVisible;
    private bool _isCommunitySectionVisible;
    private bool _isTiglSectionVisible;
    private bool _isToolsSectionVisible;
    private bool _isRulesSectionVisible;

    private IReadOnlyList<NavLinkItem> _gameLinks = [];
    private IReadOnlyList<NavLinkItem> _communityLinks = [];
    private IReadOnlyList<NavLinkItem> _tiglLinks = [];
    private IReadOnlyList<NavLinkItem> _toolsLinks = [];
    private IReadOnlyList<NavLinkItem> _rulesLinks = [];

    [Inject]
    private ICurrentUserState CurrentUserState { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IMenuSelectionState MenuSelectionState { get; set; } = default!;

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
            CurrentUserState.UserChanged -= OnUserChanged;

        _disposed = true;
    }

    protected override async Task OnInitializedAsync()
    {
        _gameLinks =
        [
            new NavLinkItem(PageRoutes.Factions, "factions", Strings.Page_Factions),
            new NavLinkItem(PageRoutes.Technologies, "technologies", Strings.Page_Technologies),
            new NavLinkItem(PageRoutes.Cards, "cards", Strings.Page_Cards),
            new NavLinkItem(PageRoutes.SystemTiles, "systemtiles", Strings.Page_SystemTiles),
            new NavLinkItem(PageRoutes.Planets, "planets", Strings.Page_Planets),
        ];

        _communityLinks =
        [
            new NavLinkItem(PageRoutes.GalaxyMap, "galaxymap", Strings.Page_GalaxyMap),
            new NavLinkItem(PageRoutes.MapsArchive, "mapsarchive", Strings.Page_MapArchive),
            new NavLinkItem(PageRoutes.SlicesArchive, "slicesarchive", Strings.Page_SlicesArchive),
            new NavLinkItem(PageRoutes.Discord, "discord", Strings.Page_Discord),
            new NavLinkItem(PageRoutes.Async, "async", Strings.Page_Async),
            new NavLinkItem(PageRoutes.Websites, "websites", Strings.Page_OtherWebsites),
        ];

        _tiglLinks =
        [
            new NavLinkItem(PageRoutes.Tigl, "tiglinfo", Strings.Page_TiglInfo),
            new NavLinkItem(PageRoutes.TiglRegister, "tiglregister", Strings.Page_TiglRegister),
            new NavLinkItem(PageRoutes.TiglReportGame, "tiglreport", Strings.Page_TiglReportGame),
            new NavLinkItem(PageRoutes.TiglLeaderboard, "tiglleaderboard", Strings.Page_TiglLeaderboard),
            new NavLinkItem(PageRoutes.TiglStatistics, "tiglstatistics", Strings.Page_TiglStatistics),
            new NavLinkItem(PageRoutes.TiglPlayers, "tiglplayers", Strings.Page_TiglPlayers),
            new NavLinkItem(PageRoutes.TiglGames, "tiglgames", Strings.Page_TiglGameReports),
            new NavLinkItem(PageRoutes.TiglRankings, "tiglranks", Strings.Page_TiglRanks),
            new NavLinkItem(PageRoutes.TiglLeaders, "tiglleaders", Strings.Page_TiglLeaders),
            new NavLinkItem(PageRoutes.TiglAchievements, "tiglachievements", Strings.Page_TiglAchievements),
        ];

        _toolsLinks =
        [
            new NavLinkItem(PageRoutes.GameTracker, "gametracker", Strings.Page_GameTracker, Target: "_blank"),
            new NavLinkItem(PageRoutes.ColorPicker, "colorpicker", Strings.Page_ColorPicker),
            new NavLinkItem(PageRoutes.FactionDraft, "factiondraft", Strings.Page_FactionDraft),
            new NavLinkItem(PageRoutes.MiltyDraft, "miltydraft", Strings.Page_MiltyDraft),
            new NavLinkItem(PageRoutes.SliceGenerator, "slicegenerator", Strings.Page_SliceGenerator),
            new NavLinkItem(PageRoutes.MapGenerator, "mapgenerator", Strings.Page_MapGenerator),
            // new NavLinkItem(PageRoutes.CardGenerator, "cardgenerator", Strings.Page_CardGenerator),
        ];

        _rulesLinks =
        [
            new NavLinkItem(PageRoutes.Rules, "rulespage", Strings.Page_Rules),
            // new NavLinkItem(PageRoutes.Faq, "faq", Strings.Page_Faq),
            new NavLinkItem(PageRoutes.Resources, "resources", Strings.Page_Resources),
        ];

        var relativePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        if (string.IsNullOrWhiteSpace(relativePath) && MenuSelectionState.ActiveMenuKey is null)
            MenuSelectionState.SelectMenu(MainMenuKey.News);

        CurrentUserState.UserChanged += OnUserChanged;
        await CurrentUserState.InitializeAsync();
    }

    private void OnUserChanged() => InvokeAsync(StateHasChanged);

    private string GetAccountHeaderText() => CurrentUserState.User?.UserName ?? Strings.Page_AccountLogin;

    private string GetMainMenuClass(MainMenuKey menuKey) =>
        MenuSelectionState.ActiveMenuKey == menuKey ? "menu-link-active" : string.Empty;

    private string GetSubMenuClass(string subMenuKey) =>
        string.Equals(MenuSelectionState.ActiveSubMenuKey, subMenuKey, StringComparison.OrdinalIgnoreCase)
            ? "submenu-link-active"
            : string.Empty;

    private void SelectMenu(MainMenuKey menuKey)
    {
        MenuSelectionState.SelectMenu(menuKey);

        if (_isMobileMenuVisible)
            CollapseAllSections();
    }

    private void SelectSubMenu(MainMenuKey menuKey, string subMenuKey)
    {
        MenuSelectionState.SelectSubMenu(menuKey, subMenuKey);

        if (_isMobileMenuVisible)
            CollapseAllSections();
    }

    private void SelectMenuAndClose(MainMenuKey menuKey)
    {
        SelectMenu(menuKey);
        _isMobileMenuVisible = false;
    }

    private void SelectSubMenuAndClose(MainMenuKey menuKey, string subMenuKey)
    {
        SelectSubMenu(menuKey, subMenuKey);
        _isMobileMenuVisible = false;
    }

    private void ClearMenuSelectionAndClose()
    {
        MenuSelectionState.ClearSelection();

        if (_isMobileMenuVisible)
            CollapseAllSections();

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
        MenuSelectionState.SelectMenu(MainMenuKey.Account);
        _isAccountSectionVisible = nextState;
    }

    private void ToggleGameSection()
    {
        var nextState = !_isGameSectionVisible;
        CollapseAllSections();
        MenuSelectionState.SelectMenu(MainMenuKey.Game);
        _isGameSectionVisible = nextState;
    }

    private void ToggleCommunitySection()
    {
        var nextState = !_isCommunitySectionVisible;
        CollapseAllSections();
        MenuSelectionState.SelectMenu(MainMenuKey.Community);
        _isCommunitySectionVisible = nextState;
    }

    private void ToggleTiglSection()
    {
        var nextState = !_isTiglSectionVisible;
        CollapseAllSections();
        MenuSelectionState.SelectMenu(MainMenuKey.Tigl);
        _isTiglSectionVisible = nextState;
    }

    private void ToggleToolsSection()
    {
        var nextState = !_isToolsSectionVisible;
        CollapseAllSections();
        MenuSelectionState.SelectMenu(MainMenuKey.Tools);
        _isToolsSectionVisible = nextState;
    }

    private void ToggleRulesSection()
    {
        var nextState = !_isRulesSectionVisible;
        CollapseAllSections();
        MenuSelectionState.SelectMenu(MainMenuKey.Rules);
        _isRulesSectionVisible = nextState;
    }

    private async Task Logout() => await CurrentUserState.LogoutAsync();

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
