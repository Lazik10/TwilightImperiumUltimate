using TwilightImperiumUltimate.Contracts.DTOs.Async.Games;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Games;

public partial class AsyncGamesYearMonthMenu
{
    [Parameter]
    public AsyncGameDatesDto GameDates { get; set; } = new(new List<AsyncGameYearMonthDto>());

    [Parameter]
    public EventCallback<(int Year, int Month)> OnYearMonthSelected { get; set; }

    [Parameter]
    public int SelectedYear { get; set; }

    [Parameter]
    public int SelectedMonth { get; set; }

    [Parameter]
    public int Width { get; set; } = 100;

    [Inject]
    private IAsyncGamesProvider AsyncGameProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        if (GameDates.GameDates.Count > 0)
            await OnMonthSelected(GameDates.GameDates.First().Year, GameDates.GameDates.First().Months.First());
    }

    private List<AsyncGameYearMonthDto> SortedGameDates()
    {
        if (GameDates.GameDates.Count == 0)
        {
            return new List<AsyncGameYearMonthDto>();
        }

        return GameDates.GameDates
            .OrderByDescending(g => g.Year)
            .ToList();
    }

    private async Task OnMonthSelected(int year, int month)
    {
        if (OnYearMonthSelected.HasDelegate)
        {
            await OnYearMonthSelected.InvokeAsync((year, month));
        }
    }

    private TextColor GetYearColor(DateTime dateTime)
    {
        return dateTime.Year == SelectedYear ? TextColor.Green : TextColor.Yellow;
    }

    private string GetMonthColor(DateTime dateTime)
    {
        var color = dateTime.Year == SelectedYear && dateTime.Month == SelectedMonth ? "lawngreen" : "white";
        return $"color: {color}";
    }
}