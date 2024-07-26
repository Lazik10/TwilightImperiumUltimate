using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TwilightImperiumUltimate.Web.Services.User;

namespace TwilightImperiumUltimate.Web.Services.Authentication;

public class TwilightImperiumAuthenticationStateProvider(IUserService userService)
    : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
    private readonly IUserService _userService = userService;

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var principal = await _userService.GetCurrentUserClaimsPrincipalAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        return new AuthenticationState(principal);
    }

    public async Task NotifyUserLogin()
    {
        var principal = await _userService.GetCurrentUserClaimsPrincipalAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public void NotifyUserLogout()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }
}
