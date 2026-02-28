using Microsoft.Extensions.Options;
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
                var target = $"{Pages.TiglLeaderboard}";
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

    private void RedirectBack() => NavigationManager.NavigateTo(Pages.TiglLeaderboard);

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
}