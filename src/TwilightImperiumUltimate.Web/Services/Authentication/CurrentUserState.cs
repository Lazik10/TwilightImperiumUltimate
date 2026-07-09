using Microsoft.AspNetCore.Components.Authorization;
using TwilightImperiumUltimate.Web.Models.Users;
using TwilightImperiumUltimate.Web.Services.User;

namespace TwilightImperiumUltimate.Web.Services.Authentication;

/// <inheritdoc cref="ICurrentUserState" />
public sealed class CurrentUserState : ICurrentUserState, IDisposable
{
    private readonly IUserService _userService;
    private readonly ILoginService _loginService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private Task? _initializeTask;
    private bool _disposed;

    public CurrentUserState(
        IUserService userService,
        ILoginService loginService,
        AuthenticationStateProvider authenticationStateProvider)
    {
        _userService = userService;
        _loginService = loginService;
        _authenticationStateProvider = authenticationStateProvider;
        _authenticationStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
    }

    public event Action? UserChanged;

    public TwilightImperiumUser? User { get; private set; }

    public Task InitializeAsync() => _initializeTask ??= InitializeCoreAsync();

    public async Task LogoutAsync()
    {
        User = null;
        await _loginService.LogoutAsync();

        if (_authenticationStateProvider is TwilightImperiumAuthenticationStateProvider twilightImperiumAuthStateProvider)
            twilightImperiumAuthStateProvider.NotifyUserLogout();

        UserChanged?.Invoke();
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _authenticationStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        _disposed = true;
    }

    private async Task InitializeCoreAsync()
    {
        User = await _userService.GetCurrentUserAsync();

        if (User is null)
        {
            var loginSuccess = await _loginService.TryAutomaticLoginAsync(CancellationToken.None);
            if (loginSuccess)
                User = await _userService.GetCurrentUserAsync();
        }
    }

    private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        User = await _userService.GetCurrentUserAsync();
        UserChanged?.Invoke();
    }
}
