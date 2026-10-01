using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Services.Authentication;
using TwilightImperiumUltimate.Web.Services.Navigation;

namespace TwilightImperiumUltimate.Web.Components.Shared.Account;

public partial class AccountMenu : IDisposable
{
    private bool _disposed;
    private bool _isAccountDropdownOpen;

    [Inject]
    private ICurrentUserState CurrentUserState { get; set; } = default!;

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
        CurrentUserState.UserChanged += OnUserChanged;
        await CurrentUserState.InitializeAsync();
    }

    private void OnUserChanged() => InvokeAsync(StateHasChanged);

    private void OpenAccountDropdown() => _isAccountDropdownOpen = true;

    private void CloseAccountDropdown() => _isAccountDropdownOpen = false;

    private void ClearMenuSelection() => MenuSelectionState.ClearSelection();

    private async Task Logout() => await CurrentUserState.LogoutAsync();
}
