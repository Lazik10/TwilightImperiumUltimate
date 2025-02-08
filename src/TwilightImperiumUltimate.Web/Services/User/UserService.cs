using Blazored.LocalStorage;
using System.Security.Claims;
using TwilightImperiumUltimate.Contracts.ApiContracts.Account;
using TwilightImperiumUltimate.Contracts.DTOs.User;
using TwilightImperiumUltimate.Web.Models.Account;
using TwilightImperiumUltimate.Web.Models.Users;

namespace TwilightImperiumUltimate.Web.Services.User;

public class UserService(
    ITwilightImperiumApiHttpClient httpClient,
    ILocalStorageService localStorge,
    IMapper mapper)
    : IUserService
{
    private readonly ITwilightImperiumApiHttpClient _httpClient = httpClient;
    private readonly ILocalStorageService _localStorage = localStorge;
    private readonly IMapper _mapper = mapper;
    private TwilightImperiumUser? _currentUser;

    public async Task SetCurrentUserAsync(TwilightImperiumUser? user)
    {
        _currentUser = user;
        var roles = await GetUserRoles();

        if (roles.Count > 0 && _currentUser is not null)
        {
            _currentUser!.Roles = roles;
        }
    }

    public Task<TwilightImperiumUser?> GetCurrentUserAsync() => Task.FromResult(_currentUser);

    public Task<ClaimsPrincipal> GetCurrentUserClaimsPrincipalAsync()
    {
        if (_currentUser is not null && _currentUser.Roles.Count > 0)
        {
            var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, _currentUser.UserName),
                    new(ClaimTypes.Email, _currentUser.Email),
                };

            foreach (var role in _currentUser.Roles)
            {
                claims.Add(new(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, "CustomAuth");
            var principal = new ClaimsPrincipal(identity);

            return Task.FromResult(principal);
        }

        return Task.FromResult(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async Task<(bool LoginSuccess, HttpStatusCode StatusCode)> TryLoginUserAsync(LoginModel loginModel, CancellationToken cancellationToken)
    {
        (LoginResponse response, HttpStatusCode statusCode) = await _httpClient.PostAsync<LoginModel, LoginResponse>(Paths.ApiPath_AccountLogin, loginModel, cancellationToken);
        if (statusCode == HttpStatusCode.OK)
        {
            await _localStorage.SetItemAsync("authentication", response, cancellationToken);
            await _httpClient.SetAuthorizationHeaderAsync(cancellationToken, response.AccessToken);

            if (await TryLoginUserWithAccessTokenAsync(cancellationToken))
            {
                return (true, statusCode);
            }
        }

        return (false, statusCode);
    }

    public async Task<bool> TryLoginUserWithAccessTokenAsync(CancellationToken cancellationToken)
    {
        var authenticationStore = await _localStorage.GetItemAsync<LoginResponse>("authentication", cancellationToken);

        if (authenticationStore is not null)
        {
            await _httpClient.SetAuthorizationHeaderAsync(cancellationToken, authenticationStore.AccessToken);

            (string email, HttpStatusCode statusCode) = await GetLoginUserManageInfoAsync(cancellationToken);
            if (statusCode == HttpStatusCode.OK)
            {
                var user = await GetLoginUserAsync(email, cancellationToken);
                await SetCurrentUserAsync(user);
                return true;
            }
        }

        return false;
    }

    public async Task<bool> TryLoginUserWithRefreshTokenAsync(CancellationToken cancellationToken)
    {
        var refreshTokensSuccess = await TryUpdateStorageTokensFromRefreshTokenAsync(cancellationToken);

        if (refreshTokensSuccess)
        {
            var (email, statusCode) = await GetLoginUserManageInfoAsync(cancellationToken);

            if (statusCode == HttpStatusCode.OK)
            {
                var user = await GetLoginUserAsync(email, cancellationToken);
                await SetCurrentUserAsync(user);
                return true;
            }
        }

        return false;
    }

    public async Task<bool> UpdateUserInfoAsync(TwilightImperiumUser? user, CancellationToken cancellationToken)
    {
        if (user is not null)
        {
            var userDto = _mapper.Map<TwilightImperiumUserDto>(user);
            var (response, statusCode) = await _httpClient.PutAsync<TwilightImperiumUserDto, TwilightImperiumUser>(Paths.ApiPath_UserUpdate, userDto, cancellationToken);
            if (statusCode == HttpStatusCode.OK)
            {
                await SetCurrentUserAsync(response);
                return true;
            }
        }

        return false;
    }

    public async Task LogoutUserAsync(CancellationToken cancellationToken)
    {
        await SetCurrentUserAsync(null);
        await _localStorage.SetItemAsync<LoginResponse>("authentication", new LoginResponse(), cancellationToken);
        await _httpClient.SetAuthorizationHeaderAsync(cancellationToken, string.Empty);
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string code, CancellationToken cancellationToken = default)
    {
        var query = $"?userId={userId}&code={code}";
        var success = await _httpClient.GetAsync(query, Paths.ApiPath_AccountConfirmEmail, cancellationToken);

        return success;
    }

    private async Task<TwilightImperiumUser?> GetLoginUserAsync(string email, CancellationToken cancellationToken)
    {
        var accountInfoRequest = new AccountInfoRequest { Email = email };

        var (response, statusCode) = await _httpClient.PostAsync<AccountInfoRequest, ApiResponse<TwilightImperiumUserDto>>(Paths.ApiPath_UserByEmail, accountInfoRequest, cancellationToken);
        if (statusCode == HttpStatusCode.OK)
            return _mapper.Map<TwilightImperiumUser>(response.Data);

        return null;
    }

    private async Task<(string Email, HttpStatusCode StatusCode)> GetLoginUserManageInfoAsync(CancellationToken cancellationToken)
    {
        var (response, statusCode) = await _httpClient.GetAsync<ManageInfoResponse>(Paths.ApiPath_ManageInfo, cancellationToken: cancellationToken);
        if (statusCode == HttpStatusCode.OK)
            return (response!.Email, statusCode);

        return (string.Empty, statusCode);
    }

    private async Task<bool> TryUpdateStorageTokensFromRefreshTokenAsync(CancellationToken cancellationToken)
    {
        var authenticationStore = await _localStorage.GetItemAsync<LoginResponse>("authentication", cancellationToken);

        if (authenticationStore is not null)
        {
            var refreshTokenRequest = new RefreshTokenRequest { RefreshToken = authenticationStore.RefreshToken ?? string.Empty };
            var (response, statusCode) = await _httpClient.PostAsync<RefreshTokenRequest, LoginResponse>(Paths.ApiPath_AccountRefreshToken, refreshTokenRequest, cancellationToken);
            if (statusCode == HttpStatusCode.OK)
            {
                await _localStorage.SetItemAsync("authentication", response, cancellationToken);
                await _httpClient.SetAuthorizationHeaderAsync(cancellationToken, response.AccessToken!);
                return true;
            }
        }

        return false;
    }

    private async Task<List<string>> GetUserRoles(CancellationToken cancellationToken = default)
    {
        if (_currentUser is null)
            return new List<string>();

        var request = _mapper.Map<TwilightImperiumUserDto>(_currentUser);
        var (response, statusCode) = await _httpClient.PostAsync<TwilightImperiumUserDto, ApiResponse<ItemListDto<RoleDto>>>(Paths.ApiPath_UserRoles, request, cancellationToken);

        if (statusCode == HttpStatusCode.OK)
            return response!.Data!.Items.Select(x => x.Name).ToList();

        return new List<string>();
    }
}
