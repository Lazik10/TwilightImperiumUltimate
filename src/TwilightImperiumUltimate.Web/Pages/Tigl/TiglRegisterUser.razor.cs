using Microsoft.Extensions.Options;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Options.Api;

namespace TwilightImperiumUltimate.Web.Pages.Tigl;

public partial class TiglRegisterUser
{
    private bool _redirectScheduled;

    [Parameter]
    [SupplyParameterFromQuery(Name = "playerId")]
    public int PlayerId { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "name")]
    public string Name { get; set; } = string.Empty;

    [Parameter]
    [SupplyParameterFromQuery(Name = "errorMessage")]
    public string ErrorMessage { get; set; } = string.Empty;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IOptions<TwilightImperiumApiOptions> ApiOptions { get; set; } = default!;

    private bool _manualRegisterEnabled;
    private bool _manualRegistering;
    private bool _manualRegisterSuccess;
    private string _manualRegisterError = string.Empty;
    private NewTiglUserRequest _newUser = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadTiglParametersAsync();
    }

    private async Task LoadTiglParametersAsync()
    {
        // Fetch TIGL parameters and find TiglTestUserRegistrations
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

        StateHasChanged();
    }

    protected override void OnParametersSet()
    {
        if (!_redirectScheduled && IsRegistrationSuccessful())
        {
            _redirectScheduled = true;

            // schedule redirect after 5 seconds
            _ = Task.Run(async () =>
            {
                await Task.Delay(5000);

                // navigate to player profile page with query playerId
                var target = $"{Pages.TiglPlayerProfile}?playerId={PlayerId}";
                await InvokeAsync(() => NavigationManager.NavigateTo(target));
            });
        }
        else if (!_redirectScheduled && IsRegistrationFailure())
        {
            _redirectScheduled = true;

            // schedule redirect after 5 seconds
            _ = Task.Run(async () =>
            {
                await Task.Delay(5000);

                // navigate to player profile page with query playerId
                var target = $"{Pages.Tigl}";
                await InvokeAsync(() => NavigationManager.NavigateTo(target));
            });
        }
    }

    private bool IsRegistrationSuccessful()
    {
        return !string.IsNullOrWhiteSpace(Name) && PlayerId > 0;
    }

    private bool IsRegistrationFailure()
    {
        return PlayerId == -1 && !string.IsNullOrEmpty(ErrorMessage);
    }

    private void RedirectBack() => NavigationManager.NavigateTo(Pages.Tigl);

    private void RegisterWithDiscord()
    {
        var currentUrl = NavigationManager.Uri;
        var baseUrl = ApiOptions.Value.BaseUrl.ToString() ?? NavigationManager.BaseUri;

        // We want to send the user to the backend API login endpoint.
        // The backend will later redirect them to Discord.
        var loginUrl = $"{baseUrl}auth/discord/login?returnUrl={Uri.EscapeDataString(currentUrl)}";

        // forceLoad: true = do a full browser navigation (leave the SPA),
        // so the request goes directly to the backend server.
        NavigationManager.NavigateTo(loginUrl, forceLoad: true);
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