using Blazored.LocalStorage;
using System.Net;
using System.Security.Claims;
using TwilightImperiumUltimate.Contracts.ApiContracts.Account;
using TwilightImperiumUltimate.Web.Models.Account;
using TwilightImperiumUltimate.Web.Models.Users;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;

namespace TwilightImperiumUltimate.Web.Services.User;

public class UserService(
    ITwilightImperiumApiHttpClient httpClient,
    ILocalStorageService localStorge)
    : IUserService
{
    private readonly ITwilightImperiumApiHttpClient _httpClient = httpClient;
    private readonly ILocalStorageService _localStorage = localStorge;
    private TwilightImperiumUser? _currentUser;

    public Task SetCurrentUserAsync(TwilightImperiumUser? user)
    {
        _currentUser = user;
        return Task.CompletedTask;
    }

    public Task<TwilightImperiumUser?> GetCurrentUserAsync() => Task.FromResult(_currentUser);

    public Task<ClaimsPrincipal> GetCurrentUserClaimsPrincipalAsync()
    {
        if (_currentUser is not null)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, _currentUser.UserName),
                new(ClaimTypes.Email, _currentUser.Email),
                new(ClaimTypes.Role, "Admin"),
                new(ClaimTypes.Role, "User"),
            };

            var identity = new ClaimsIdentity(claims, "CustomAuth");
            var principal = new ClaimsPrincipal(identity);

            return Task.FromResult(principal);
        }

        return Task.FromResult(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async Task<(bool LoginSuccess, HttpStatusCode StatusCode)> TryLoginUserAsync(LoginModel loginModel, CancellationToken ct)
    {
        (LoginResponse response, HttpStatusCode statusCode) = await _httpClient.PostAsync<LoginModel, LoginResponse>(Paths.ApiPath_AccountLogin, loginModel, ct);
        if (statusCode == HttpStatusCode.OK)
        {
            await _localStorage.SetItemAsync("authentication", response, ct);
            await _httpClient.SetAuthorizationHeaderAsync(ct, response.AccessToken);

            if (await TryLoginUserWithAccessTokenAsync(ct))
            {
                return (true, statusCode);
            }
        }

        return (false, statusCode);
    }

    public async Task<bool> TryLoginUserWithAccessTokenAsync(CancellationToken ct)
    {
        var authenticationStore = await _localStorage.GetItemAsync<LoginResponse>("authentication", ct);

        if (authenticationStore is not null)
        {
            await _httpClient.SetAuthorizationHeaderAsync(ct, authenticationStore.AccessToken);

            (string email, HttpStatusCode statusCode) = await GetLoginUserManageInfoAsync(ct);
            if (statusCode == HttpStatusCode.OK)
            {
                await SetCurrentUserAsync(await GetLoginUserAsync(email, ct));
                return true;
            }
        }

        return false;
    }

    public async Task<bool> TryLoginUserWithRefreshTokenAsync(CancellationToken ct)
    {
        var refreshTokensSuccess = await TryUpdateStorageTokensFromRefreshTokenAsync(ct);

        if (refreshTokensSuccess)
        {
            var (email, statusCode) = await GetLoginUserManageInfoAsync(ct);

            if (statusCode == HttpStatusCode.OK)
            {
                await SetCurrentUserAsync(await GetLoginUserAsync(email, ct));
                return true;
            }
        }

        return false;
    }

    public async Task UpdateUserInfoAsync(TwilightImperiumUser? user, CancellationToken ct)
    {
        if (user is not null)
        {
            var (response, statusCode) = await _httpClient.PutAsync<TwilightImperiumUser, TwilightImperiumUser>(Paths.ApiPath_UserUpdate, user, ct);
            if (statusCode == HttpStatusCode.OK)
                await SetCurrentUserAsync(response);
        }
    }

    public async Task LogoutUserAsync(CancellationToken ct)
    {
        await SetCurrentUserAsync(null);
        await _localStorage.SetItemAsync<LoginResponse>("authentication", new LoginResponse(), ct);
        await _httpClient.SetAuthorizationHeaderAsync(ct, string.Empty);
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string code, CancellationToken ct = default)
    {
        var query = $"?userId={userId}&code={code}";
        var success = await _httpClient.GetAsync(query, Paths.ApiPath_AccountConfirmEmail, ct);

        return success;
    }

    private async Task<TwilightImperiumUser?> GetLoginUserAsync(string email, CancellationToken ct)
    {
        var accountInfoRequest = new AccountInfoRequest { Email = email };

        var (response, statusCode) = await _httpClient.PostAsync<AccountInfoRequest, TwilightImperiumUser>(Paths.ApiPath_UserByEmail, accountInfoRequest, ct);
        if (statusCode == HttpStatusCode.OK)
            return response;

        return null;
    }

    private async Task<(string Email, HttpStatusCode StatusCode)> GetLoginUserManageInfoAsync(CancellationToken ct)
    {
        var (response, statusCode) = await _httpClient.GetAsync<ManageInfoResponse>(Paths.ApiPath_ManageInfo, ct);
        if (statusCode == HttpStatusCode.OK)
            return (response.Email, statusCode);

        return (string.Empty, statusCode);
    }

    private async Task<bool> TryUpdateStorageTokensFromRefreshTokenAsync(CancellationToken ct)
    {
        var authenticationStore = await _localStorage.GetItemAsync<LoginResponse>("authentication", ct);

        if (authenticationStore is not null)
        {
            var refreshTokenRequest = new RefreshTokenRequest { RefreshToken = authenticationStore.RefreshToken ?? string.Empty };
            var (response, statusCode) = await _httpClient.PostAsync<RefreshTokenRequest, LoginResponse>(Paths.ApiPath_AccountRefreshToken, refreshTokenRequest, ct);
            if (statusCode == HttpStatusCode.OK)
            {
                await _localStorage.SetItemAsync("authentication", response, ct);
                await _httpClient.SetAuthorizationHeaderAsync(ct, response.AccessToken!);
                return true;
            }
        }

        return false;
    }
}
