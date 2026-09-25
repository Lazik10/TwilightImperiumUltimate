using TwilightImperiumUltimate.Contracts.ApiContracts.AsyncTI4;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;

namespace TwilightImperiumUltimate.Web.Pages.Community;

public partial class AsyncPlayerProfile
{
    [Parameter]
    [SupplyParameterFromQuery(Name = "discordId")]
    public string DiscordId { get; set; } = string.Empty;

    [Parameter]
    [SupplyParameterFromQuery(Name = "name")]
    public string Name { get; set; } = string.Empty;

    [Parameter]
    [SupplyParameterFromQuery(Name = "playerId")]
    public int PlayerId { get; set; }

    private AsyncPlayerProfileSummaryStatsDto PlayerProfile { get; set; } = default!;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        await GetPlayerProfileAsync();
    }

    private async Task GetPlayerProfileAsync()
    {
        var parseSuccess = long.TryParse(DiscordId, out var parsedDiscordId);
        var discordId = parseSuccess ? parsedDiscordId : 0;
        var name = Name is null ? string.Empty : Name;
        var request = new AsyncPlayerProfileRequest(discordId, name, PlayerId);
        var response = await HttpClient.PostAsync<AsyncPlayerProfileRequest, ApiResponse<AsyncPlayerProfileSummaryStatsDto>>(Paths.ApiPath_AsyncPlayerProfile, request);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            PlayerProfile = response!.Response!.Data!;
        }
    }

}
