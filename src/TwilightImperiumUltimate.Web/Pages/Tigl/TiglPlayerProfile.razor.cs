using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Pages.Tigl;

public partial class TiglPlayerProfile
{
    private bool _loading = true;

    [Parameter]
    [SupplyParameterFromQuery(Name = "playerId")]
    public int PlayerId { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "returnUrl")]
    public string? ReturnUrl { get; set; }

    private TiglPlayerProfileDto PlayerProfile { get; set; } = new();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        _loading = true;
        await GetPlayerProfileAsync();
        _loading = false;
    }

    private async Task GetPlayerProfileAsync()
    {
        var response = await HttpClient.GetAsync<ApiResponse<TiglPlayerProfileDto>>(
            Paths.ApiPath_TiglPlayerProfile + PlayerId);

        if (response.StatusCode == HttpStatusCode.OK && response.Response?.Data is not null)
        {
            PlayerProfile = response.Response.Data;
        }
    }

    private void RedirectBack()
    {
        NavigationManager.NavigateTo(!string.IsNullOrEmpty(ReturnUrl) ? ReturnUrl : Pages.TiglPlayers);
    }
}