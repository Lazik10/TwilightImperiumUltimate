using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglPlayersGrid
{
    private bool _loading = true;
    private IReadOnlyCollection<TiglUserLiteDto> _users = new List<TiglUserLiteDto>();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadUsersAsync();
    }

    private async Task LoadUsersAsync()
    {
        _loading = true;

        var (response, status) = await HttpClient.GetAsync<ApiResponse<ItemListDto<TiglUserLiteDto>>>(Paths.ApiPath_TiglUsers);
        if (status == HttpStatusCode.OK && response?.Data?.Items is not null)
        {
            _users = response.Data.Items;
        }

        _loading = false;
    }
}