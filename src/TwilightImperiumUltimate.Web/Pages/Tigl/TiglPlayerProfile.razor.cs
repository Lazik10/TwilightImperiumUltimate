using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Pages.Tigl;

public partial class TiglPlayerProfile
{
    private bool _loading = true;

    [Parameter]
    [SupplyParameterFromQuery(Name = "playerId")]
    public int PlayerId { get; set; }

    private TiglPlayerProfileDto PlayerProfile { get; set; } = new();

    private string PageTitle => Strings.Page_TiglPlayerProfile_PageTitle.FormatWith(PlayerProfile.TiglUserName);

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        _loading = true;
        PlayerProfile = new TiglPlayerProfileDto
        {
            TiglUserId = PlayerId,
            TiglUserName = $"Player #{PlayerId}",
        };
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

}
