using System.Net.Http;
using System.Threading;
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
    private string _errorMessage = string.Empty;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private RegistrationModel RegistrationUserModel { get; set; } = new RegistrationModel();

    private async Task RegisterUser()
    {
        _showRegistrationFailed = false;
        _registrationSuceeded = false;
        _errorMessage = string.Empty;

        if (!_registeringUser)
        {
            _registeringUser = true;
            StateHasChanged();

            var (_, statusCode) = await HttpClient.PostAsync<RegistrationModel, RegistrationResponse>(Paths.ApiPath_AccountRegister, RegistrationUserModel, default);

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

        _registeringUser = false;
        StateHasChanged();
    }

    private async Task AddDefaultUserRole()
    {
        var accountInfoRequest = new AccountInfoRequest { Email = RegistrationUserModel.Email };
        var (response, statusCode) = await HttpClient.PostAsync<AccountInfoRequest, ApiResponse<TwilightImperiumUserDto>>(Paths.ApiPath_UserByEmail, accountInfoRequest);
        if (statusCode == HttpStatusCode.OK)
        {
            var roleRequest = new AddRoleToUserRequest() { RoleName = "User", UserId = response!.Data!.Id };
            var (newResponse, newStatusCode) =await HttpClient.PostAsync<AddRoleToUserRequest, AddRoleToUserResponse>(Paths.ApiPath_AddRole, roleRequest);

            var newResponseTwo = newResponse;
            var newStatusCodeTwo = newStatusCode;
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