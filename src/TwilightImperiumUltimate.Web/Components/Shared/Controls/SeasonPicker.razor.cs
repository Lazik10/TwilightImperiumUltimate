using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class SeasonPicker
{
    private IReadOnlyCollection<SeasonDto> _seasons = Array.Empty<SeasonDto>();

    [Parameter]
    public IReadOnlyCollection<SeasonDto>? Seasons { get; set; }

    [Parameter]
    public int SeasonNumber { get; set; } = -1;

    [Parameter]
    public EventCallback<int> SeasonNumberChanged { get; set; }

    [Parameter]
    public string Title { get; set; } = @Strings.TiglStatistics_Season;

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public int FontSize { get; set; } = 24;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        if (Seasons is { Count: > 0 })
        {
            _seasons = AddAllOption(Seasons);
        }
        else
        {
            var (resp, status) = await HttpClient.GetAsync<ApiResponse<ItemListDto<SeasonDto>>>(Paths.ApiPath_Seasons);
            if (status == HttpStatusCode.OK && resp?.Data?.Items is not null)
            {
                _seasons = AddAllOption(resp.Data.Items);
            }
            else
            {
                _seasons = Array.Empty<SeasonDto>();
            }
        }

        if (!_seasons.Any(s => s.SeasonNumber == SeasonNumber))
        {
            SeasonNumber = -1;
            await SeasonNumberChanged.InvokeAsync(SeasonNumber);
        }
    }

    private static IReadOnlyCollection<SeasonDto> AddAllOption(IReadOnlyCollection<SeasonDto> source)
    {
        var list = source.ToList();
        if (!list.Any(s => s.SeasonNumber == -1))
        {
            list.Add(new SeasonDto { SeasonNumber = -1, Name = "All" });
        }

        return list.OrderByDescending(s => s.SeasonNumber).ToList();
    }

    private string GetDisplayText()
    {
        if (SeasonNumber == -1)
            return "All";

        var s = _seasons.FirstOrDefault(s => s.SeasonNumber == SeasonNumber);
        return s is null || string.IsNullOrWhiteSpace(s.Name) ? SeasonNumber.ToString() : s.Name;
    }

    private async Task DecreaseAsync()
    {
        if (_seasons.Count == 0)
            return;

        var ordered = _seasons.OrderBy(s => s.SeasonNumber).ToList();
        var idx = ordered.FindIndex(s => s.SeasonNumber == SeasonNumber);
        if (idx == -1)
        {
            SeasonNumber = ordered.First().SeasonNumber;
        }
        else
        {
            SeasonNumber = idx > 0 ? ordered[idx - 1].SeasonNumber : ordered.Last().SeasonNumber;
        }

        await SeasonNumberChanged.InvokeAsync(SeasonNumber);
    }

    private async Task IncreaseAsync()
    {
        if (_seasons.Count == 0)
            return;

        var ordered = _seasons.OrderBy(s => s.SeasonNumber).ToList();
        var idx = ordered.FindIndex(s => s.SeasonNumber == SeasonNumber);
        if (idx == -1)
        {
            SeasonNumber = ordered.First().SeasonNumber;
        }
        else
        {
            SeasonNumber = idx < ordered.Count - 1 ? ordered[idx + 1].SeasonNumber : ordered.First().SeasonNumber;
        }

        await SeasonNumberChanged.InvokeAsync(SeasonNumber);
    }
}
