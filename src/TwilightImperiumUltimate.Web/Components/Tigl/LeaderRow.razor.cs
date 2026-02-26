using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Web.Helpers.Time;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class LeaderRow
{
    [Parameter]
    public required RankingsLeaderDto Leader { get; set; }

    private long GetCurrentDurationMilliseconds()
    {
        if (Leader.LastUpdate is null || Leader.LastUpdate == 0 || Leader.ChangeCount == 0)
            return 0;

        var startDate = DateTimeOffset.FromUnixTimeMilliseconds(Leader.LastUpdate.Value);
        var endDate = DateTimeOffset.Now;
        var duration = endDate - startDate;

        return (long)duration.TotalMilliseconds;
    }

    private string GetDurationTime(long? timestamp)
    {
        if (timestamp is null || timestamp == 0 || Leader.ChangeCount == 0)
            return string.Empty;

        var startDate = DateTimeOffset.FromUnixTimeMilliseconds(timestamp.Value);
        var endDate = DateTimeOffset.Now;

        var duration = endDate - startDate;

        return duration.FormaToDays();
    }

    private string GetShortestDurationTime()
    {
        if (Leader.ShortestDuration is null || Leader.ShortestDuration.Value == 0)
        {
            return Leader.LastUpdate is null || Leader.LastUpdate.Value == 0
                ? string.Empty
                : GetDurationTime(Leader.LastUpdate);
        }

        return TimeSpan.FromMilliseconds(Leader.ShortestDuration.Value).FormaToDays();
    }

    private string GetShortestHolderName()
    {
        if (!string.IsNullOrWhiteSpace(Leader.ShortestHolderName))
            return Leader.ShortestHolderName;

        return Leader.ShortestDuration is null || Leader.ShortestDuration.Value == 0
            ? Leader.UserName
            : string.Empty;
    }

    private string GetLongestDurationTime()
    {
        var currentDuration = GetCurrentDurationMilliseconds();
        var longestDuration = Leader.LongestDuration ?? 0;

        if (currentDuration > longestDuration)
            return TimeSpan.FromMilliseconds(currentDuration).FormaToDays();

        if (longestDuration == 0)
        {
            return Leader.LastUpdate is null || Leader.LastUpdate.Value == 0
                ? string.Empty
                : GetDurationTime(Leader.LastUpdate);
        }

        return TimeSpan.FromMilliseconds(longestDuration).FormaToDays();
    }

    private string GetLongestHolderName()
    {
        var currentDuration = GetCurrentDurationMilliseconds();
        var longestDuration = Leader.LongestDuration ?? 0;

        if (currentDuration > longestDuration)
            return Leader.UserName;

        if (!string.IsNullOrWhiteSpace(Leader.LongestHolderName))
            return Leader.LongestHolderName;

        return longestDuration == 0 ? Leader.UserName : string.Empty;
    }
}