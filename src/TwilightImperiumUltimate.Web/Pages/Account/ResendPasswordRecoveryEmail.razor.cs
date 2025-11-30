using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.Account;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;

namespace TwilightImperiumUltimate.Web.Pages.Account;

public partial class ResendPasswordRecoveryEmail
{
    private bool _passwordRecoveryEmailSendSuccess;
    private bool _recoveryEmailSend;
    private bool _disableButton;

    private ResendPasswordRecoveryEmailModel PasswordRecoveryModel { get; set; } = new ResendPasswordRecoveryEmailModel();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    private async Task ResendPasswordRecoveryEmailCode()
    {
        if (_disableButton || string.IsNullOrEmpty(PasswordRecoveryModel.Email))
            return;

        _disableButton = true;
        _recoveryEmailSend = false;

        var request = new ResendPasswordRecoveryEmailRequest { Email = PasswordRecoveryModel.Email, };
        var result = await HttpClient.PostAsync<ResendPasswordRecoveryEmailRequest, ResendPasswordRecoveryEmailResponse>(Paths.ApiPath_ForgotPassword, request, default);
        var statusCode = result.StatusCode;

        if (statusCode == System.Net.HttpStatusCode.OK)
        {
            _passwordRecoveryEmailSendSuccess = true;
            _recoveryEmailSend = true;
            PasswordRecoveryModel = new ResendPasswordRecoveryEmailModel();
            StateHasChanged();
        }
        else
        {
            _passwordRecoveryEmailSendSuccess = false;
        }

        _recoveryEmailSend = true;
        _disableButton = false;
    }

    private bool GetDisabledState()
    {
        return _disableButton;
    }
}