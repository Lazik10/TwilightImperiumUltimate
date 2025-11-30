using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;
using TwilightImperiumUltimate.Web.Models.TiglAdmin;

namespace TwilightImperiumUltimate.Web.Components.TiglAdmin;

public partial class SeasonAdminDashboard : ComponentBase
{
    private bool _loading;
    private bool _sendingRequest;
    private List<SeasonEditModel> _seasonEdits = new();

    private int _newSeasonNumber;
    private string _newSeasonName = string.Empty;
    private DateTime _newSeasonStartValue = DateTime.UtcNow.Date;
    private DateTime _newSeasonEndValue = DateTime.UtcNow.Date;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadSeasonsAsync();
    }

    private async Task LoadSeasonsAsync()
    {
        _loading = true;
        var (resp, status) = await HttpClient.GetAsync<ApiResponse<ItemListDto<SeasonDto>>>(Paths.ApiPath_Seasons);
        if (status == HttpStatusCode.OK && resp?.Data?.Items is not null)
        {
            _seasonEdits = resp.Data.Items
                .OrderBy(x => x.SeasonNumber)
                .Select(s => new SeasonEditModel(s))
                .ToList();
        }
        else
        {
            _seasonEdits = new List<SeasonEditModel>();
        }

        _loading = false;
    }

    private static void BeginEdit(SeasonEditModel model) => model.IsEditing = true;

    private static void CancelEdit(SeasonEditModel model)
    {
        model.Reset();
        model.IsEditing = false;
    }

    private async Task SaveSeasonAsync(SeasonEditModel model)
    {
        _sendingRequest = true;

        var req = new UpdateSeasonRequest
        {
            SeasonNumber = model.SeasonNumber,
            SeasonName = model.SeasonName,
            StartDate = DateOnly.FromDateTime(model.StartDateValue),
            EndDate = DateOnly.FromDateTime(model.EndDateValue),
        };

        var (resp, status) = await HttpClient.PostAsync<UpdateSeasonRequest, ApiResponse<UpdateSeasonResponse>>(Paths.ApiPath_SeasonUpdate, req);
        if (status == HttpStatusCode.OK && resp?.Data?.Success == true)
        {
            model.IsEditing = false;
            await LoadSeasonsAsync();
        }

        _sendingRequest = false;
    }

    private async Task SetActiveAsync(int seasonNumber)
    {
        _sendingRequest = true;

        var req = new SetActiveSeasonRequest { SeasonNumber = seasonNumber };
        var (resp, status) = await HttpClient.PostAsync<SetActiveSeasonRequest, ApiResponse<SetActiveSeasonResponse>>(Paths.ApiPath_SetActiveSeason, req);
        if (status == HttpStatusCode.OK && resp?.Data?.Success == true)
        {
            await LoadSeasonsAsync();
        }

        _sendingRequest = false;
    }

    private async Task EndSeasonAsync()
    {
        _sendingRequest = true;

        var (resp, status) = await HttpClient.GetAsync<ApiResponse<EndSeasonResponse>>(Paths.ApiPath_EndSeason);
        if (status == HttpStatusCode.OK && resp?.Success == true)
        {
            await LoadSeasonsAsync();
        }

        _sendingRequest = false;
    }

    private async Task AddSeasonAsync()
    {
        if (_newSeasonNumber <= 0 || string.IsNullOrWhiteSpace(_newSeasonName))
            return;

        _sendingRequest = true;

        var addReq = new AddSeasonRequest
        {
            SeasonNumber = _newSeasonNumber,
            SeasonName = _newSeasonName,
        };

        var (addResp, addStatus) = await HttpClient.PostAsync<AddSeasonRequest, ApiResponse<AddSeasonResponse>>(Paths.ApiPath_AddSeason, addReq);
        if (addStatus == HttpStatusCode.OK && addResp?.Data?.Success == true)
        {
            var updateReq = new UpdateSeasonRequest
            {
                SeasonNumber = _newSeasonNumber,
                SeasonName = _newSeasonName,
                StartDate = DateOnly.FromDateTime(_newSeasonStartValue),
                EndDate = DateOnly.FromDateTime(_newSeasonEndValue),
            };

            await HttpClient.PostAsync<UpdateSeasonRequest, ApiResponse<UpdateSeasonResponse>>(Paths.ApiPath_UpdateSeason, updateReq);
            _newSeasonNumber = 0;
            _newSeasonName = string.Empty;
            _newSeasonStartValue = DateTime.UtcNow.Date;
            _newSeasonEndValue = DateTime.UtcNow.Date;

            await LoadSeasonsAsync();
        }

        _sendingRequest = false;
    }

    private string GetActiveSeasonNumber() => $"Active season: {_seasonEdits.FirstOrDefault(x => x.IsActive)?.SeasonName.ToString() ?? string.Empty}";
}
