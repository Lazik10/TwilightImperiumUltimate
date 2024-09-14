using TwilightImperiumUltimate.Core.Entities.Statistics;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class WebsiteStatisticsData
{
    internal static List<WebsiteStatistics> WebsiteStatistics => new List<WebsiteStatistics>
    {
        new WebsiteStatistics
        {
            Id = 1,
            Visitors = 0,
            MapsGenerated = 0,
            SlicesGenerated = 0,
            MapsArchived = 0,
            SlicesArchived = 0,
            GamesPlayed = 0,
            FactionsDrafted = 0,
            ColorsDrafted = 0,
        },
    };
}
