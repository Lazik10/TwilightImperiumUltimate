using System.Net;
using TwilightImperiumUltimate.Web.Models.Account;
using TwilightImperiumUltimate.Web.Services.User;

namespace TwilightImperiumUltimate.Web.Services.Authentication;

public class LoginService(IUserService userService, TwilightImperiumAuthenticationStateProvider stateProvider) : ILoginService
{
    private readonly IUserService _userService = userService;

    public async Task<(bool LoginSuccess, HttpStatusCode StatusCode)> LoginAsync(LoginModel loginModel, CancellationToken ct)
    {
        return await _userService.TryLoginUserAsync(loginModel, ct);
    }

    public async Task<bool> TryAutomaticLoginAsync(CancellationToken ct)
    {
        var loginSuccess = await _userService.TryLoginUserWithAccessTokenAsync(ct);

        if (!loginSuccess)
        {
            loginSuccess = await _userService.TryLoginUserWithRefreshTokenAsync(ct);
            if (loginSuccess)
            {
                await stateProvider.NotifyUserLogin();
            }
        }
        else
        {
            await stateProvider.NotifyUserLogin();
        }

        return loginSuccess;
    }

    public async Task LogoutAsync(CancellationToken ct = default)
    {
        await _userService.LogoutUserAsync(ct);
        stateProvider.NotifyUserLogout();
    }
}
