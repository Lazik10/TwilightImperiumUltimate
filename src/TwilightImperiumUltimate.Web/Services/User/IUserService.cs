using System.Net;
using System.Security.Claims;
using TwilightImperiumUltimate.Web.Models.Account;
using TwilightImperiumUltimate.Web.Models.Users;

namespace TwilightImperiumUltimate.Web.Services.User;

public interface IUserService
{
    Task<bool> TryLoginUserWithAccessTokenAsync(CancellationToken ct);

    Task<bool> TryLoginUserWithRefreshTokenAsync(CancellationToken ct);

    Task<(bool LoginSuccess, HttpStatusCode StatusCode)> TryLoginUserAsync(LoginModel loginModel, CancellationToken ct);

    Task<TwilightImperiumUser?> GetCurrentUserAsync();

    Task SetCurrentUserAsync(TwilightImperiumUser? user);

    Task UpdateUserInfoAsync(TwilightImperiumUser? user, CancellationToken ct);

    Task LogoutUserAsync(CancellationToken ct);

    Task<ClaimsPrincipal> GetCurrentUserClaimsPrincipalAsync();

    Task<bool> ConfirmEmailAsync(string userId, string code, CancellationToken ct = default);
}
