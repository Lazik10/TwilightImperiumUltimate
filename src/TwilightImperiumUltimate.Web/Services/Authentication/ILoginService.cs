using TwilightImperiumUltimate.Web.Models.Account;

namespace TwilightImperiumUltimate.Web.Services.Authentication;

public interface ILoginService
{
    Task<(bool LoginSuccess, HttpStatusCode StatusCode)> LoginAsync(LoginModel loginModel, CancellationToken cancellationToken);

    Task<bool> TryAutomaticLoginAsync(CancellationToken cancellationToken);

    Task LogoutAsync(CancellationToken cancellationToken = default);
}