using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Helpers.Numbers;
using TwilightImperiumUltimate.Web.Models.Async;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class FactionStatistics
{
    private bool _isDataLoaded;
    private FactionStatisticsFilter _selectedFactionStatisticsFilter = FactionStatisticsFilter.Official;
    private FactionStatisticsVpFilter _selectedFactionVpStatisticsFilter = FactionStatisticsVpFilter.All;
    private FactionStatisticsSubstatsFilter _selectedFactionStatisticsSubstatsFilter = FactionStatisticsSubstatsFilter.All;
    private int _row;
    private AsyncFactionsSummaryStatsDto _factionsSummaryStats = new AsyncFactionsSummaryStatsDto();
    private List<FactionStatisticsSubstatsFilter> _excludedValues = new List<FactionStatisticsSubstatsFilter>() { FactionStatisticsSubstatsFilter.MinVp, FactionStatisticsSubstatsFilter.MaxVp };

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    public IReadOnlyCollection<AsyncFactionsStatsDto> FactionsStats => Filter switch
    {
        PlayerStatisticsType.All => _factionsSummaryStats.All,
        PlayerStatisticsType.Tigl => _factionsSummaryStats.Tigl,
        PlayerStatisticsType.Custom => _factionsSummaryStats.Custom,
        _ => _factionsSummaryStats.All,
    };

    public IReadOnlyCollection<AsyncFactionsStatsDto> FactionsForDisplay { get; set; } = new List<AsyncFactionsStatsDto>();

    [Inject]
    private IAsyncStatsProvider AsyncStatsProvider { get; set; } = default!;

    [Inject]
    private IAsyncFactionMinMaxStatsProvider AsyncFactionMinMaxStatsProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _isDataLoaded = false;
        _factionsSummaryStats = await AsyncStatsProvider.GetFactionsStatistics();
        _isDataLoaded = true;
    }

    protected override void OnParametersSet()
    {
        if (FactionsStats is not null)
            FactionsForDisplay = GetFilteredFactionStats(FactionsStats);
    }

    private void OnFactionStatisticsFilterChanged(FactionStatisticsFilter filter)
    {
        _selectedFactionStatisticsFilter = filter;
        if (FactionsStats is not null)
            FactionsForDisplay = GetFilteredFactionStats(FactionsStats);
    }

    private void OnFactionStatisticsVpFilterChanged(FactionStatisticsVpFilter filter)
    {
        _selectedFactionVpStatisticsFilter = filter;
        StateHasChanged();
    }

    private void OnFactionStatisticsSubstatsFilterChanged(FactionStatisticsSubstatsFilter filter)
    {
        _selectedFactionStatisticsSubstatsFilter = filter;
        StateHasChanged();
    }

    private List<AsyncFactionsStatsDto> GetFilteredFactionStats(IReadOnlyCollection<AsyncFactionsStatsDto> factionStats)
    {
        return _selectedFactionStatisticsFilter switch
        {
            FactionStatisticsFilter.DiscordantStars => factionStats.Skip(32).Take(40).ToList(),
            FactionStatisticsFilter.Others => factionStats.Skip(72).Where(x => x.FactionName != AsyncFactionName.Unknown && x.FactionName != AsyncFactionName.TwilightsFall).ToList(),
            _ => factionStats.Take(32).ToList(),
        };
    }

    private AsyncFactionStatsByGameVpDto GetCorrectFactionStatsByVp(AsyncFactionsStatsDto factionStats)
    {
        return _selectedFactionVpStatisticsFilter switch
        {
            FactionStatisticsVpFilter.All => factionStats.All,
            FactionStatisticsVpFilter.TenVp => factionStats.TenVp,
            FactionStatisticsVpFilter.TwelveVp => factionStats.TwelveVp,
            FactionStatisticsVpFilter.FourteenVp => factionStats.FourteenVp,
            _ => factionStats.All,
        };
    }

    private AsyncFactionsMaxStatValues GetAllCorrectMaxFactionStatsByVp()
    {
        var correctVpFactionStats = FactionsForDisplay
            .Select(x => GetCorrectFactionStatsByVp(x))
            .ToList();

        var maxGames = correctVpFactionStats.Max(x => x.Games);
        var maxWins = correctVpFactionStats.Max(x => x.Wins);
        var maxWinrate = correctVpFactionStats.Max(x => x.Games == 0 ? 0 : ((float)x.Wins / x.Games) * 100);
        var maxEliminations = correctVpFactionStats.Max(x => x.Eliminations);
        var averageVp = correctVpFactionStats.Max(x => x.Games == 0 ? 0 : (float)x.Vp / x.Games);
        var averageVpPercentage = correctVpFactionStats.Max(x => x.VpPercentage);

        return new AsyncFactionsMaxStatValues()
        {
            Games = maxGames,
            Wins = maxWins,
            WinPercentage = maxWinrate,
            Eliminations = maxEliminations,
            AverageVp = averageVp,
            AverageVpPrecentage = averageVpPercentage,
        };
    }

    private AsyncPlayerFactionMinMaxValues GetMinMaxStatsForFaction(AsyncFactionStatsByGameVpDto factionStats)
    {
        var minMaxValues = AsyncFactionMinMaxStatsProvider.GetCorrectStatValues(GetAllCorrectMaxFactionStatsByVp(), factionStats, _selectedFactionStatisticsSubstatsFilter).GetAwaiter().GetResult();
        return new AsyncPlayerFactionMinMaxValues(minMaxValues.Min, minMaxValues.Value, minMaxValues.Max);
    }

    private string GetCorrectLabelText()
    {
        return _selectedFactionStatisticsSubstatsFilter switch
        {
            FactionStatisticsSubstatsFilter.Games => Strings.FactionStats_Games,
            FactionStatisticsSubstatsFilter.Wins => Strings.FactionStats_Wins,
            FactionStatisticsSubstatsFilter.WinPercentage => Strings.FactionStats_WinPercentage,
            FactionStatisticsSubstatsFilter.Eliminations => Strings.FactionStats_Eliminations,
            FactionStatisticsSubstatsFilter.MaxVp => Strings.FactionStats_MaxVp,
            FactionStatisticsSubstatsFilter.AverageVp => Strings.FactionStats_AverageVp,
            FactionStatisticsSubstatsFilter.MinVp => Strings.FactionStats_MinVp,
            FactionStatisticsSubstatsFilter.AverageVpPercentage => Strings.FactionStats_AverageVpPercentage,
            _ => Strings.FactionStats_All,
        };
    }

    private TextColor GetCorrectLabelColor()
    {
        return TextColor.White;
    }

    private string ToFormatedString(float value)
    {
        return _selectedFactionStatisticsSubstatsFilter switch
        {
            FactionStatisticsSubstatsFilter.AverageVpPercentage or
            FactionStatisticsSubstatsFilter.WinPercentage
            => value.ToStringWithPrecisionAndPercentage(2),
            FactionStatisticsSubstatsFilter.AverageVp => value.ToStringWithPrecision(2),
            _ => value.ToStringWithPrecision(0),
        };
    }

    private TextColor GetSubstatsCorrectColor(float value)
    {
        return _selectedFactionStatisticsSubstatsFilter switch
        {
            FactionStatisticsSubstatsFilter.Games => TextColor.Yellow,
            FactionStatisticsSubstatsFilter.Wins => GetWinColor((int)value),
            FactionStatisticsSubstatsFilter.WinPercentage => value.GetWinrateColor(),
            FactionStatisticsSubstatsFilter.Eliminations => TextColor.Red,
            FactionStatisticsSubstatsFilter.AverageVpPercentage => value.GetAverageVpPercentageColor(),
            FactionStatisticsSubstatsFilter.AverageVp => value.GetAverageVpColor(),
            _ => TextColor.Yellow,
        };
    }

    private TextColor GetCorrectProgressBarColor(float value)
    {
        return _selectedFactionStatisticsSubstatsFilter switch
        {
            FactionStatisticsSubstatsFilter.Games => TextColor.Green,
            FactionStatisticsSubstatsFilter.Wins => TextColor.Green,
            FactionStatisticsSubstatsFilter.WinPercentage => value.GetWinrateColor(),
            FactionStatisticsSubstatsFilter.Eliminations => TextColor.Red,
            FactionStatisticsSubstatsFilter.MinVp => TextColor.Red,
            FactionStatisticsSubstatsFilter.AverageVp => value.GetAverageVpColor(),
            FactionStatisticsSubstatsFilter.MaxVp => TextColor.Green,
            FactionStatisticsSubstatsFilter.AverageVpPercentage => value.GetAverageVpPercentageColor(),
            _ => TextColor.Green,
        };
    }

    private bool IsWinrateEnabled() => true;

    private bool ShowSubstatsValue()
    {
        if ((_selectedFactionStatisticsSubstatsFilter == FactionStatisticsSubstatsFilter.WinPercentage
        || _selectedFactionStatisticsSubstatsFilter == FactionStatisticsSubstatsFilter.Wins)
        && !IsWinrateEnabled())
            return false;

        return true;
    }

    private TextColor GetWinColor(int wins)
    {
        return IsWinrateEnabled() && wins > 0 ? TextColor.Green : TextColor.Red;
    }

    private IReadOnlyCollection<AsyncFactionsStatsDto> GetSortedFactionsForDisplayByStatistics()
    {
        return _selectedFactionStatisticsSubstatsFilter switch
        {
            FactionStatisticsSubstatsFilter.Games => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).Games).ToList(),
            FactionStatisticsSubstatsFilter.Wins => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).Wins).ToList(),
            FactionStatisticsSubstatsFilter.WinPercentage => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).WinsPercentage).ToList(),
            FactionStatisticsSubstatsFilter.Eliminations => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).Eliminations).ToList(),
            FactionStatisticsSubstatsFilter.AverageVp => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).Games == 0 ? 0 : (float)GetCorrectFactionStatsByVp(x).Vp / GetCorrectFactionStatsByVp(x).Games).ToList(),
            FactionStatisticsSubstatsFilter.AverageVpPercentage => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).VpPercentage).ToList(),
            _ => FactionsForDisplay,
        };
    }
}