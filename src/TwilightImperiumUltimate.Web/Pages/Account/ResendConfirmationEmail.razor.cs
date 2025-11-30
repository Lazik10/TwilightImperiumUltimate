using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.Account;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;

namespace TwilightImperiumUltimate.Web.Pages.Account;

public partial class ResendConfirmationEmail
{
    private bool _confirmationEmailSendSuccess;
    private bool _confirmationEmailSend;
    private bool _disableButton;

    private ResendConfirmationEmailCodeModel ConfirmationEmailModel { get; set; } = new ResendConfirmationEmailCodeModel();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    private async Task ResendConfirmationEmailCode()
    {
        if (_disableButton || string.IsNullOrEmpty(ConfirmationEmailModel.Email))
            return;

        _confirmationEmailSend = false;
        _disableButton = true;

        var request = new ResendConfirmEmailRequest { Email = ConfirmationEmailModel.Email, };
        var result = await HttpClient.PostAsync<ResendConfirmEmailRequest, ResendConfirmEmailResponse>(Paths.ApiPath_ResendConfirmationEmail, request, default);
        var statusCode = result.StatusCode;

        if (statusCode == System.Net.HttpStatusCode.OK)
        {
            _confirmationEmailSendSuccess = true;
            _confirmationEmailSend = true;
            ConfirmationEmailModel = new ResendConfirmationEmailCodeModel();
            StateHasChanged();
        }
        else
        {
            _confirmationEmailSendSuccess = false;
        }

        _confirmationEmailSend = true;
        _disableButton = false;
    }

    private bool GetDisabledState()
    {
        return _disableButton;
    }
}