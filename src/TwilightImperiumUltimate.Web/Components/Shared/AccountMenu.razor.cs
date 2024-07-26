using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TwilightImperiumUltimate.Web.Models.Users;
using TwilightImperiumUltimate.Web.Services.Authentication;
using TwilightImperiumUltimate.Web.Services.User;

namespace TwilightImperiumUltimate.Web.Components.Shared;

public partial class AccountMenu : IDisposable
{
    private TwilightImperiumUser? _user;
    private bool _disposed;

    ~AccountMenu()
    {
        Dispose(false);
    }

    [Inject]
    private IUserService UserService { get; set; } = default!;

    [Inject]
    private ILoginService LoginService { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                AuthenticationStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
            }

            _user = null;
            _disposed = true;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        _user = await UserService.GetCurrentUserAsync();

        if (_user is null)
        {
            var loginSuccess = await LoginService.TryAutomaticLoginAsync(CancellationToken.None);

            if (loginSuccess)
                _user = await UserService.GetCurrentUserAsync();
        }

        AuthenticationStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
    }

    private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        _user = await UserService.GetCurrentUserAsync();
        StateHasChanged();
    }

    private async Task Logout()
    {
        _user = null;
        await LoginService.LogoutAsync();
        var twilightImperiumAuthStateProvider = AuthenticationStateProvider as TwilightImperiumAuthenticationStateProvider;
        twilightImperiumAuthStateProvider?.NotifyUserLogout();
        StateHasChanged();
    }
}