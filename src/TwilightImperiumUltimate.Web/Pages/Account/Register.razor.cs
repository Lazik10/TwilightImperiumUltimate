using TwilightImperiumUltimate.Contracts.ApiContracts.Account;
using TwilightImperiumUltimate.Contracts.ApiContracts.User;
using TwilightImperiumUltimate.Contracts.DTOs.User;
using TwilightImperiumUltimate.Web.Models.Account;

namespace TwilightImperiumUltimate.Web.Pages.Account;

public partial class Register
{
    private bool _showRegistrationFailed;
    private bool _registeringUser;
    private bool _registrationSuceeded;
    private bool _usernameExists;
    private bool _emailExists;
    private string _errorMessage = string.Empty;
    private string _userName = string.Empty;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private RegistrationModel RegistrationUserModel { get; set; } = new RegistrationModel();

    private async Task RegisterUser()
    {
        _usernameExists = false;
        _emailExists = false;
        _showRegistrationFailed = false;
        _registrationSuceeded = false;
        _errorMessage = string.Empty;

        if (!_registeringUser)
        {
            _registeringUser = true;
            StateHasChanged();

            var precheckRequest = new UserRegisterationPrecheckRequest
            {
                Email = RegistrationUserModel.Email,
                Username = _userName,
            };

            var precheck = await HttpClient.PostAsync<UserRegisterationPrecheckRequest, ApiResponse<UserRegistrationPrecheckResponse>>(Paths.ApiPath_PreRegistrationCheck, precheckRequest);
            var precheckResponse = precheck.Response;
            var precheckStatusCode = precheck.StatusCode;

            if (precheckStatusCode == HttpStatusCode.OK && !precheckResponse.Data!.UserNameNotAvailable && !precheckResponse.Data!.EmailNotAvailable)
            {
                var registerResult = await HttpClient.PostAsync<RegistrationModel, RegistrationResponse>(Paths.ApiPath_AccountRegister, RegistrationUserModel, default);
                var statusCode = registerResult.StatusCode;

                if (statusCode == HttpStatusCode.OK)
                {
                    _registrationSuceeded = true;
                    StateHasChanged();

                    await AddDefaultUserRole();

                    RegistrationUserModel = new RegistrationModel();
                    await Task.Delay(7000);
                    NavigationManager.NavigateTo(Pages.Login);
                }
                else
                {
                    if (statusCode == HttpStatusCode.BadRequest)
                    {
                        _errorMessage = Strings.Register_FailedBadRequest;
                    }
                    else
                    {
                        _errorMessage = Strings.Register_FailedUnknown;
                    }

                    _showRegistrationFailed = true;
                    StateHasChanged();
                }
            }
            else
            {
                _usernameExists = precheckResponse.Data!.UserNameNotAvailable;
                _emailExists = precheckResponse.Data!.EmailNotAvailable;
            }
        }

        _registeringUser = false;
        StateHasChanged();
    }

    private async Task AddDefaultUserRole()
    {
        var accountInfoRequest = new AccountInfoRequest { Email = RegistrationUserModel.Email };
        var userResult = await HttpClient.PostAsync<AccountInfoRequest, ApiResponse<TwilightImperiumUserDto>>(Paths.ApiPath_UserByEmail, accountInfoRequest);
        var response = userResult.Response;
        var statusCode = userResult.StatusCode;
        if (statusCode == HttpStatusCode.OK)
        {
            var roleRequest = new AddRoleToUserRequest() { RoleName = "User", UserId = response!.Data!.Id };
            var roleResult = await HttpClient.PostAsync<AddRoleToUserRequest, AddRoleToUserResponse>(Paths.ApiPath_AddRole, roleRequest);
            var roleStatusCode = roleResult.StatusCode;

            if (roleStatusCode == HttpStatusCode.OK)
            {
                var request = new TwilightImperiumUserDto(
                    string.Empty,
                    _userName,
                    string.Empty,
                    string.Empty,
                    RegistrationUserModel.Email,
                    string.Empty,
                    0,
                    FactionName.None,
                    string.Empty,
                    string.Empty,
                    string.Empty);

                await HttpClient.PutAsync<TwilightImperiumUserDto, ApiResponse<TwilightImperiumUserDto>>(Paths.ApiPath_UpdateUserName, request, default);
            }
        }
    }

    private void RegisterUserFailed()
    {
        _errorMessage = Strings.Register_FailedCredentials;
        _showRegistrationFailed = true;
        StateHasChanged();
    }

    private bool GetDisabledState()
    {
        return _registeringUser;
    }
}