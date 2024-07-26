using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.Account;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;

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

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                _registrationSuceeded = true;
                RegistrationUserModel = new RegistrationModel();
                StateHasChanged();

                await Task.Delay(7000);
                NavigationManager.NavigateTo(Pages.Login);
            }
            else
            {
                if (statusCode == System.Net.HttpStatusCode.BadRequest)
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