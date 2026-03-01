using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Components.TiglAdmin;

public partial class RegisterDashboard
{
    private bool _loading;
    private bool _manualRegisterEnabled;
    private bool _manualRegistering;
    private bool _manualRegisterSuccess;
    private string _manualRegisterError = string.Empty;
    private NewTiglUserRequest _newUser = new();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadTiglParametersAsync();
    }

    private async Task LoadTiglParametersAsync()
    {
        _loading = true;

        var (resp, status) = await HttpClient.GetAsync<ApiResponse<ItemListDto<TiglParameterDto>>>(Paths.ApiPath_TiglParameters);
        if (status == HttpStatusCode.OK && resp?.Data?.Items is not null)
        {
            var testParam = resp.Data.Items.FirstOrDefault(p => p.Name == TiglParameterName.TiglTestUserRegistrations);
            _manualRegisterEnabled = testParam?.Enabled == true;
        }
        else
        {
            _manualRegisterEnabled = false;
        }

        _loading = false;
        StateHasChanged();
    }

    private async Task ManualRegisterUser()
    {
        if (_manualRegistering)
            return;

        _manualRegistering = true;
        _manualRegisterError = string.Empty;
        _manualRegisterSuccess = false;
        StateHasChanged();

        if (_newUser.DiscordId <= 0 || string.IsNullOrWhiteSpace(_newUser.TiglUserName))
        {
            _manualRegisterError = "Please provide a valid Discord ID and TIGL Username.";
            _manualRegistering = false;
            StateHasChanged();
            return;
        }

        var (resp, status) = await HttpClient.PostAsync<NewTiglUserRequest, ApiResponse<NewTiglUserResponse>>(Paths.ApiPath_TiglRegisterUser, _newUser);
        if (status == HttpStatusCode.OK && resp?.Success == true && resp.Data?.Success == true)
        {
            _manualRegisterSuccess = true;
            _newUser = new();
        }
        else
        {
            _manualRegisterError = resp?.ProblemDetails?.Detail ?? "Registration failed.";
        }

        _manualRegistering = false;
        StateHasChanged();
    }
}
