using TwilightImperiumUltimate.Contracts.DTOs.Rankings;

namespace TwilightImperiumUltimate.Web.Components.Ranking;

public partial class RankingsMainGrid
{
    private RankingsMenuItem _selectedSegment = RankingsMenuItem.Ranks;
    private IReadOnlyCollection<RankingsUserDto>? _rankings;
    private IReadOnlyCollection<RankingsLeaderDto>? _leaders;

    [Parameter]
    public RankingsMenuItem Segment { get; set; }

    [CascadingParameter(Name = "Category")]
    public string Category { get; set; } = string.Empty;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var (ranksResponse, ranksStatus) = await HttpClient.GetAsync<ApiResponse<ItemListDto<RankingsUserDto>>>(Paths.ApiPath_Rankings, cancellationToken: default);
        if (ranksStatus == HttpStatusCode.OK && ranksResponse?.Data is not null)
        {
            _rankings = ranksResponse.Data.Items;
        }

        var (leadersResponse, leadersStatus) = await HttpClient.GetAsync<ApiResponse<ItemListDto<RankingsLeaderDto>>>(Paths.ApiPath_RankingsLeaders, cancellationToken: default);
        if (leadersStatus == HttpStatusCode.OK && leadersResponse?.Data is not null)
        {
            _leaders = leadersResponse.Data.Items;
        }

        StateHasChanged();
    }

    protected override void OnParametersSet()
    {
        if (!string.IsNullOrEmpty(Category))
        {
            if (Category == "ranks")
                _selectedSegment = RankingsMenuItem.Ranks;
            else if (Category == "heroes")
                _selectedSegment = RankingsMenuItem.Leaders;
            else if (Category == "achievements")
                _selectedSegment = RankingsMenuItem.Achievements;
        }

        StateHasChanged();
    }

    private void ChangeSegment(RankingsMenuItem segment)
    {
        _selectedSegment = segment;
        StateHasChanged();
    }
}