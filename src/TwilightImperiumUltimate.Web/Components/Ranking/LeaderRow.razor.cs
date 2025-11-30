using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Web.Helpers.Time;

namespace TwilightImperiumUltimate.Web.Components.Ranking;

public partial class LeaderRow
{
    [Parameter]
    public required RankingsLeaderDto Leader { get; set; }

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

    private string GetLongestDurationTime()
    {
        if (Leader.LongestDuration is null || Leader.LongestDuration.Value == 0)
        {
            return Leader.LastUpdate is null || Leader.LastUpdate.Value == 0
                ? string.Empty
                : GetDurationTime(Leader.LastUpdate);
        }

        return TimeSpan.FromMilliseconds(Leader.LongestDuration.Value).FormaToDays();
    }
}