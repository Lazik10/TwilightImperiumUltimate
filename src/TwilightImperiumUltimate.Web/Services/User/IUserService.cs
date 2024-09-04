using System.Security.Claims;
using TwilightImperiumUltimate.Web.Models.Account;
using TwilightImperiumUltimate.Web.Models.Users;

namespace TwilightImperiumUltimate.Web.Services.User;

public interface IUserService
{
    Task<bool> TryLoginUserWithAccessTokenAsync(CancellationToken cancellationToken);

    Task<bool> TryLoginUserWithRefreshTokenAsync(CancellationToken cancellationToken);

    Task<(bool LoginSuccess, HttpStatusCode StatusCode)> TryLoginUserAsync(LoginModel loginModel, CancellationToken cancellationToken);

    Task<TwilightImperiumUser?> GetCurrentUserAsync();

    Task SetCurrentUserAsync(TwilightImperiumUser? user);

    Task<bool> UpdateUserInfoAsync(TwilightImperiumUser? user, CancellationToken cancellationToken);

    Task LogoutUserAsync(CancellationToken cancellationToken);

    Task<ClaimsPrincipal> GetCurrentUserClaimsPrincipalAsync();

    Task<bool> ConfirmEmailAsync(string userId, string code, CancellationToken cancellationToken = default);
}
