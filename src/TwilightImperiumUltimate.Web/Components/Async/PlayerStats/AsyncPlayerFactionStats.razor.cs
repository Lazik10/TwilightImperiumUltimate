using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.FactionStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;
using TwilightImperiumUltimate.Web.Helpers.Numbers;
using TwilightImperiumUltimate.Web.Models.Async;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.PlayerStats;

public partial class AsyncPlayerFactionStats
{
    private FactionStatisticsFilter _selectedFactionStatisticsFilter = FactionStatisticsFilter.Official;

    private FactionStatisticsVpFilter _selectedFactionVpStatisticsFilter = FactionStatisticsVpFilter.All;

    private FactionStatisticsSubstatsFilter _selectedFactionStatisticsSubstatsFilter = FactionStatisticsSubstatsFilter.All;

    private int _row;

    [CascadingParameter(Name = "AsyncPlayerProfile")]
    public AsyncPlayerProfileSummaryStatsDto AsyncPlayerProfile { get; set; } = default!;

    [CascadingParameter(Name = "AsyncPlayerStatisticsType")]
    public PlayerStatisticsType StatType { get; set; }

    public IReadOnlyCollection<AsyncPlayerFactionStatsDto>? FactionStats => StatType switch
    {
        PlayerStatisticsType.All => AsyncPlayerProfile?.FactionStats?.All,
        PlayerStatisticsType.Tigl => AsyncPlayerProfile?.FactionStats?.Tigl,
        PlayerStatisticsType.Custom => AsyncPlayerProfile?.FactionStats?.Custom,
        _ => AsyncPlayerProfile?.FactionStats?.All,
    };

    public IReadOnlyCollection<AsyncPlayerFactionStatsDto> FactionsForDisplay { get; set; } = new List<AsyncPlayerFactionStatsDto>();

    [Inject]
    private IAsyncPlayerMinMaxStatsProvider AsyncPlayerMinMaxStatsProvider { get; set; } = default!;

    protected override void OnParametersSet()
    {
        if (FactionStats is not null)
            FactionsForDisplay = GetFilteredFactionStats(FactionStats);
    }

    private void OnFactionStatisticsFilterChanged(FactionStatisticsFilter filter)
    {
        _selectedFactionStatisticsFilter = filter;
        if (FactionStats is not null)
            FactionsForDisplay = GetFilteredFactionStats(FactionStats);
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

    private List<AsyncPlayerFactionStatsDto> GetFilteredFactionStats(IReadOnlyCollection<AsyncPlayerFactionStatsDto> factionStats)
    {
        return _selectedFactionStatisticsFilter switch
        {
            FactionStatisticsFilter.DiscordantStars => factionStats.Skip(27).Take(34).ToList(),
            FactionStatisticsFilter.Others => factionStats.Skip(61).ToList(),
            _ => factionStats.Take(27).ToList(),
        };
    }

    private AsyncPlayerFactionStatsByGameVp GetCorrectFactionStatsByVp(AsyncPlayerFactionStatsDto factionStats)
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

    private AsyncPlayerMaxStatValues GetAllCorrectMaxFactionStatsByVp()
    {
        var correctVpFactionStats = FactionsForDisplay
            .Select(x => GetCorrectFactionStatsByVp(x))
            .ToList();

        var maxGames = correctVpFactionStats.Max(x => x.Games);
        var maxWins = correctVpFactionStats.Max(x => x.Wins);
        var maxWinrate = correctVpFactionStats.Max(x => x.Games == 0 ? 0 : ((float)x.Wins / x.Games) * 100);
        var maxEliminations = correctVpFactionStats.Max(x => x.Eliminations);
        var maxMinVp = correctVpFactionStats.Max(x => x.MinVp);
        var maxMaxVp = correctVpFactionStats.Max(x => x.MaxVp);
        var maxAverageVp = correctVpFactionStats.Max(x => x.AverageVp);
        var maxAverageVpPercentage = correctVpFactionStats.Max(x => x.AverageVpPercentage);

        return new AsyncPlayerMaxStatValues()
        {
            Games = maxGames,
            Wins = maxWins,
            WinPercentage = maxWinrate,
            Eliminations = maxEliminations,
            MinVp = maxMinVp,
            AverageVp = maxAverageVp,
            MaxVp = maxMaxVp,
            AverageVpPrecentage = maxAverageVpPercentage,
        };
    }

    private AsyncPlayerFactionMinMaxValues GetMinMaxStatsForFaction(AsyncPlayerFactionStatsByGameVp factionStats)
    {
        var minMaxValues = AsyncPlayerMinMaxStatsProvider.GetCorrectStatValues(GetAllCorrectMaxFactionStatsByVp(), factionStats, _selectedFactionStatisticsSubstatsFilter).GetAwaiter().GetResult();
        return new AsyncPlayerFactionMinMaxValues(minMaxValues.Min, minMaxValues.Value, minMaxValues.Max);
    }

    private string GetCorrectLabelText()
    {
        if (!AsyncPlayerProfile.Settings.ShowFactionStats)
            return Strings.AsyncPlayer_Statistics_Private;

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
        return AsyncPlayerProfile.Settings.ShowFactionStats ? TextColor.White : TextColor.Red;
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

    private bool IsWinrateEnabled() => AsyncPlayerProfile.Settings.ShowWinRates;

    private bool ShowSubstatsWinrateValue()
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

    private IReadOnlyCollection<AsyncPlayerFactionStatsDto> GetSortedFactionsForDisplayByStatistics()
    {
        return _selectedFactionStatisticsSubstatsFilter switch
        {
            FactionStatisticsSubstatsFilter.Games => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).Games).ToList(),
            FactionStatisticsSubstatsFilter.Wins => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).Wins).ThenByDescending(x => GetCorrectFactionStatsByVp(x).Games).ToList(),
            FactionStatisticsSubstatsFilter.WinPercentage => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).WinRate).ThenByDescending(x => GetCorrectFactionStatsByVp(x).Games).ToList(),
            FactionStatisticsSubstatsFilter.Eliminations => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).Eliminations).ThenByDescending(x => GetCorrectFactionStatsByVp(x).Games).ToList(),
            FactionStatisticsSubstatsFilter.MinVp => FactionsForDisplay.OrderBy(x => GetCorrectFactionStatsByVp(x).Games > 0 ? GetCorrectFactionStatsByVp(x).MinVp : int.MaxValue).ThenByDescending(x => GetCorrectFactionStatsByVp(x).Games).ToList(),
            FactionStatisticsSubstatsFilter.AverageVp => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).AverageVp).ThenByDescending(x => GetCorrectFactionStatsByVp(x).Games).ToList(),
            FactionStatisticsSubstatsFilter.MaxVp => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).MaxVp).ThenByDescending(x => GetCorrectFactionStatsByVp(x).Games).ToList(),
            FactionStatisticsSubstatsFilter.AverageVpPercentage => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).AverageVpPercentage).ThenByDescending(x => GetCorrectFactionStatsByVp(x).Games).ToList(),
            _ => FactionsForDisplay,
        };
    }
}