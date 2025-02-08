using System.Globalization;
using TwilightImperiumUltimate.Contracts.ApiContracts.AsyncTI4;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;

namespace TwilightImperiumUltimate.Web.Pages.Community;

public partial class AsyncPlayerProfile
{
    private char _digitGroup = '1';
    private char _othersGroup = '*';

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

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

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

    private void RedirectBack()
    {
        char redirectCategory = GetPlayerCharCategory(PlayerProfile.PlayerInfo.DiscordUserName.ToUpper(CultureInfo.InvariantCulture)[0]);
        NavigationManager.NavigateTo($"{Pages.Async}?category=players&letter={redirectCategory}");
    }

    private char GetPlayerCharCategory(char firstChar)
    {
        if (char.IsLetter(firstChar))
        {
            return firstChar;
        }
        else if (char.IsDigit(firstChar))
        {
            return _digitGroup;
        }
        else
        {
            return _othersGroup;
        }
    }
}