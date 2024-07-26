using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using TwilightImperiumUltimate.Web.Models.Account;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.Authentication;

namespace TwilightImperiumUltimate.Web.Pages.Account;

public partial class Login
{
    private bool _showLoginFailed;
    private bool _loggingIn;
    private string _reasonWhyLoginFailed = string.Empty;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private ILoginService LoginService { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    private LoginModel LoginModel { get; set; } = new LoginModel();

    private async Task LoginUser(CancellationToken ct)
    {
        if (_loggingIn)
            return;

        _showLoginFailed = false;
        _loggingIn = true;
        _reasonWhyLoginFailed = string.Empty;

        var (loginSuccess, statusCode) = await LoginService.LoginAsync(LoginModel, ct);

        if (loginSuccess)
        {
            var authStateProvider = AuthenticationStateProvider as TwilightImperiumAuthenticationStateProvider;
            if (authStateProvider is not null)
            {
                await authStateProvider.NotifyUserLogin();
            }

            NavigationManager.NavigateTo(Pages.Index);
        }
        else
        {
            _reasonWhyLoginFailed = statusCode switch
            {
                HttpStatusCode.Unauthorized => Strings.Login_FailedUnauthorized,
                _ => Strings.Login_FailedUknown,
            };
            _showLoginFailed = true;
        }

        _loggingIn = false;
    }

    private bool GetDisabledState()
    {
        return _loggingIn;
    }
}