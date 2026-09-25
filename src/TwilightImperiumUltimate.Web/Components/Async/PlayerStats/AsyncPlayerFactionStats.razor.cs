using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.FactionStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;
using TwilightImperiumUltimate.Web.Components.Charts;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Helpers.Numbers;

namespace TwilightImperiumUltimate.Web.Components.Async.PlayerStats;

public partial class AsyncPlayerFactionStats
{
    private FactionStatisticsFilter _selectedFactionStatisticsFilter = FactionStatisticsFilter.Official;

    private FactionStatisticsVpFilter _selectedFactionVpStatisticsFilter = FactionStatisticsVpFilter.All;

    private FactionStatisticsSubstatsFilter _selectedFactionStatisticsSubstatsFilter = FactionStatisticsSubstatsFilter.All;

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

    protected override void OnParametersSet()
    {
        if (FactionStats is not null)
            FactionsForDisplay = GetFilteredFactionStats(FactionStats);
    }

    private static string FormatPercentageValue(double value) => $"{((float)value).ToStringWithPrecision(1)} %";

    private static string FormatAverageVpValue(double value) => ((float)value).ToStringWithPrecision(2);

    private static string RemoveTrailingColon(string value) => value.TrimEnd(':');

    private static List<KeyValuePair<FactionStatisticsFilter, string>> GetFactionFilterOptions() => Enum.GetValues<FactionStatisticsFilter>()
        .Select(value => new KeyValuePair<FactionStatisticsFilter, string>(value, value.GetDisplayName()))
        .ToList();

    private static List<KeyValuePair<FactionStatisticsVpFilter, string>> GetFactionVpFilterOptions() => Enum.GetValues<FactionStatisticsVpFilter>()
        .Select(value => new KeyValuePair<FactionStatisticsVpFilter, string>(value, value.GetDisplayName()))
        .ToList();

    private static List<KeyValuePair<FactionStatisticsSubstatsFilter, string>> GetFactionSubstatsFilterOptions() => Enum.GetValues<FactionStatisticsSubstatsFilter>()
        .Select(value => new KeyValuePair<FactionStatisticsSubstatsFilter, string>(value, value.GetDisplayName()))
        .ToList();

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
            FactionStatisticsFilter.DiscordantStars => factionStats.Skip(32).Take(40).ToList(),
            FactionStatisticsFilter.Others => factionStats.Skip(72).Where(x => x.FactionName != AsyncFactionName.Unknown && x.FactionName != AsyncFactionName.TwilightsFall).ToList(),
            _ => factionStats.Take(32).ToList(),
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

    private bool IsWinrateEnabled() => AsyncPlayerProfile.Settings.ShowWinRates;

    private static TextColor GetWinsColor(int wins) => wins > 0 ? TextColor.Green : TextColor.Red;

    private IReadOnlyCollection<RankingBarPoint> GetGamesData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).Games)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetCorrectFactionStatsByVp(faction).Games,
            TextColor.Green.GetChartFillColor(),
            null))
        .ToList();

    private IReadOnlyCollection<RankingBarPoint> GetWinsData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).Wins)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            IsWinrateEnabled() ? GetCorrectFactionStatsByVp(faction).Wins : 0,
            TextColor.Green.GetChartFillColor(),
            null))
        .ToList();

    private IReadOnlyCollection<RankingBarPoint> GetWinPercentageData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).WinRate)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            IsWinrateEnabled() ? GetCorrectFactionStatsByVp(faction).WinRate : 0,
            GetCorrectFactionStatsByVp(faction).WinRate.GetWinrateColor().GetChartFillColor(),
            null))
        .ToList();

    private IReadOnlyCollection<RankingBarPoint> GetEliminationsData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).Eliminations)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetCorrectFactionStatsByVp(faction).Eliminations,
            TextColor.Red.GetChartFillColor(),
            null))
        .ToList();

    private IReadOnlyCollection<RankingBarPoint> GetMinVpData() => FactionsForDisplay
        .OrderBy(faction => GetCorrectFactionStatsByVp(faction).Games > 0 ? GetCorrectFactionStatsByVp(faction).MinVp : int.MaxValue)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetCorrectFactionStatsByVp(faction).MinVp,
            TextColor.Red.GetChartFillColor(),
            null))
        .ToList();

    private IReadOnlyCollection<RankingBarPoint> GetAverageVpData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).AverageVp)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetCorrectFactionStatsByVp(faction).AverageVp,
            TextColor.Green.GetChartFillColor(),
            null))
        .ToList();

    private IReadOnlyCollection<RankingBarPoint> GetMaxVpData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).MaxVp)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetCorrectFactionStatsByVp(faction).MaxVp,
            TextColor.Green.GetChartFillColor(),
            null))
        .ToList();

    private IReadOnlyCollection<RankingBarPoint> GetAverageVpPercentageData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).AverageVpPercentage)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetCorrectFactionStatsByVp(faction).AverageVpPercentage,
            GetCorrectFactionStatsByVp(faction).AverageVpPercentage.GetAverageVpPercentageColor().GetChartFillColor(),
            null))
        .ToList();
}
