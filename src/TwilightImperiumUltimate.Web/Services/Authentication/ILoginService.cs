using System.Net;
using TwilightImperiumUltimate.Web.Models.Account;

namespace TwilightImperiumUltimate.Web.Services.Authentication;

public interface ILoginService
{
    Task<(bool LoginSuccess, HttpStatusCode StatusCode)> LoginAsync(LoginModel loginModel, CancellationToken ct);

    Task<bool> TryAutomaticLoginAsync(CancellationToken ct);

    Task LogoutAsync(CancellationToken ct = default);
}