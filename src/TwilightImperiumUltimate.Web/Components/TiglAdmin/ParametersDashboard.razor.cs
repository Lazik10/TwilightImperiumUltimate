using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Parameters;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Components.TiglAdmin;

public partial class ParametersDashboard
{
    private bool _loading;
    private List<TiglParameterDto> _parameters = new();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadParametersAsync();
    }

    private async Task LoadParametersAsync()
    {
        _loading = true;
        var (resp, status) = await HttpClient.GetAsync<ApiResponse<ItemListDto<TiglParameterDto>>>(Paths.ApiPath_TiglParameters);
        if (status == HttpStatusCode.OK && resp?.Data?.Items is not null)
        {
            _parameters = resp.Data.Items.ToList();
        }
        else
        {
            _parameters = new List<TiglParameterDto>();
        }

        _loading = false;
        StateHasChanged();
    }

    private async Task ToggleParameterAsync(TiglParameterDto p)
    {
        var request = new UpdateTiglParameterRequest { Name = p.Name, Enabled = !p.Enabled };
        var (resp, status) = await HttpClient.PostAsync<UpdateTiglParameterRequest, ApiResponse<UpdateTiglParameterResponse>>(Paths.ApiPath_UpdateTiglParameter, request);
        if (status == HttpStatusCode.OK && resp?.Data?.Success == true)
        {
            p.Enabled = !p.Enabled;
            StateHasChanged();
        }
        else
        {
            // optional error handling
        }
    }
}
