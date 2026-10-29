using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using TwilightImperiumUltimate.Web.Helpers;
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

    [SupplyParameterFromQuery(Name = "returnUrl")]
    private string? ReturnUrl { get; set; }

    private LoginModel LoginModel { get; set; } = new LoginModel();

    private async Task LoginUser(EditContext editContext)
    {
        if (_loggingIn)
            return;

        _showLoginFailed = false;
        _loggingIn = true;
        _reasonWhyLoginFailed = string.Empty;

        var (loginSuccess, statusCode) = await LoginService.LoginAsync(LoginModel, CancellationToken.None);

        if (loginSuccess)
        {
            var authStateProvider = AuthenticationStateProvider as TwilightImperiumAuthenticationStateProvider;
            if (authStateProvider is not null)
            {
                await authStateProvider.NotifyUserLogin();
            }

            var destination = ReturnUrl is { } returnUrl && LocalUrlHelper.IsLocalPath(returnUrl)
                ? returnUrl
                : Pages.Index;
            NavigationManager.NavigateTo(destination);
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
}
