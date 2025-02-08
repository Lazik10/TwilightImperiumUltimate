using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;

namespace TwilightImperiumUltimate.Web.Pages.Community;

public partial class Async
{
    private IReadOnlyCollection<AsyncPlayerProfileDto> _playerProfileNames = new List<AsyncPlayerProfileDto>();

    [Parameter]
    [SupplyParameterFromQuery(Name = "category")]
    public string Category { get; set; } = string.Empty;

    [Parameter]
    [SupplyParameterFromQuery(Name = "letter")]
    public string Letter { get; set; } = string.Empty;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await GetPlayerProfileNamesAsync();
    }

    private async Task GetPlayerProfileNamesAsync()
    {
        var response = await HttpClient.GetAsync<ApiResponse<AsyncPlayerProfileListDto>>(Paths.ApiPath_AsyncPlayerProfiles);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            _playerProfileNames = response!.Response!.Data!.PlayerProfiles;
        }
    }
}