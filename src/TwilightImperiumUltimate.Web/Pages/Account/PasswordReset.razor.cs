using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.Account;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;

namespace TwilightImperiumUltimate.Web.Pages.Account;

public partial class PasswordReset
{
    private bool _passwordResetFailed;
    private bool _resetingPassword;
    private string _errorMessage = string.Empty;

    [Parameter]
    public string? Code { get; set; }

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private ResetPasswordModel ResetPasswordModel { get; set; } = new ResetPasswordModel();

    protected override void OnInitialized()
    {
        ResetPasswordModel.ResetCode = Code;
    }

    private async Task ResetPassword()
    {
        _errorMessage = string.Empty;
        _passwordResetFailed = false;
        _resetingPassword = true;

        var passwordRequest = new ResetPasswordRequest
        {
            Email = ResetPasswordModel.Email,
            NewPassword = ResetPasswordModel.NewPassword,
            ResetCode = ResetPasswordModel.ResetCode,
        };

        var (_, statusCode) = await HttpClient.PostAsync<ResetPasswordRequest, PasswordResetResponse>(Paths.ApiPath_ResetPassword, passwordRequest, default);

        if (statusCode == System.Net.HttpStatusCode.OK)
        {
            _passwordResetFailed = false;
            ResetPasswordModel = new ResetPasswordModel();
            NavigationManager.NavigateTo(Pages.Index);
            StateHasChanged();
        }
        else
        {
            _errorMessage = Strings.PasswordReset_FailedUnknown;
            _passwordResetFailed = true;
        }

        _resetingPassword = false;
    }

    private void ResetPasswordFailed()
    {
        _errorMessage = Strings.PasswordReset_CredentialsInvalid;
        _passwordResetFailed = true;
        StateHasChanged();
    }

    private bool GetDisabledState()
    {
        return _resetingPassword;
    }
}